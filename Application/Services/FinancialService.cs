using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Finance;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Application.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly ISalaryRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IProcedureRepository _procedureRepository;
        private readonly ILoggerManager _logger;

        public FinancialService(
            ISalaryRepository repository,
            IUserRepository userRepository,
            ILoggerManager logger,
            IAppointmentRepository appointmentRepository,
            IProcedureRepository procedureRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _logger = logger;
            _appointmentRepository = appointmentRepository;
            _procedureRepository = procedureRepository;
        }

        private async Task GenerateUpdateSalary(Salary salary, decimal wage)
        {
            var result = new Salary();
            result.EmployeeId = salary.EmployeeId;
            result.Employee = salary.Employee;
            result.Date = DateTime.Now;
            result.Value = wage;
            await _repository.InsertAsync(result);
            await _repository.SaveChangesAsync();
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            var result = await _repository.GetById(salary.EmployeeId);

            if((result != null)&&(result.Value !=0))
            {
                _logger.LogWarn($"Employee with Id: { result.EmployeeId } has already had a salary");
                throw new BadRequestException();
            }
            await _repository.InsertAsync(salary);
            await _repository.SaveChangesAsync();
            _logger.LogInfo("Salary added");
        }

        public async Task DeleteSalaryByUserIdAsync(int id)
        {
            var res = await GetSalaryByUserIdAsync(id);
            await GenerateUpdateSalary(res, 0);
            _logger.LogInfo($"Salary with id: {id} deleted");
        }

        public async Task<PagedList<Salary>> GetSalaryAsync(SalaryParametrs parametrs)
        {

            var result = await _repository.GetAsync(parametrs);
            _logger.LogInfo($"salaries were recieved");
            return result;
        }

        public async Task<Salary> GetSalaryByUserIdAsync(int id)
        {
            Salary? salary = await _repository.GetById(id);
            if ((salary == null)||(salary.Value == 0))
            {
                _logger.LogWarn($"Salary with id: {id} not found");
                throw new NotFoundException($"Salary with id: {id} not found");
            }
            _logger.LogInfo($"Salary with id: {id} found");
            return salary;
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            var res = await GetSalaryByUserIdAsync(salary.EmployeeId);
            if(res.Value == salary.Value)
            {
                _logger.LogError($"You try to update Salary with id: {res.EmployeeId} with the same value");
                throw new BadRequestException();
            }

            await GenerateUpdateSalary(res, salary.Value);

            _logger.LogInfo($"Salary with id: {res.EmployeeId} updated");
        }

        public async Task<IEnumerable<User>> GetEmployeesWithoutSalary()
        {
            var parametrs = new SalaryParametrs()
            {
                PageNumber = 1,
                PageSize = 100
            };
            var salaries = await GetSalaryAsync(parametrs);
            var employees = await _userRepository.GetByRolesAsync(new List<int> { 1, 2, 3 });

            var res = from salary in salaries
                      join employee in employees on salary.EmployeeId equals employee.Id
                      select employee;
            var result = from employee in employees
                         where !res.Contains(employee)
                         select employee;

            return result;
        }

        private async Task<IList<SalaryDetail>> GetSalaryDetail(Salary salary, DatePeriod date, int id)
        {
            //Count detail salary

            IList<SalaryDetail> salaryDetail = new List<SalaryDetail>();
            var salaryDate = salary.Date;
            var value = salary.Value;
            while (salaryDate < date.EndDate)
            {
                var salaryUpdate = await _repository.GetByIdForStatement(id, s => s.Date > salaryDate);
                if (salaryUpdate == null)
                {
                    int lastPeriod = 0;
                    if(salaryDate>date.StartDate)
                    {
                        lastPeriod = (date.EndDate - salaryDate).Days;
                    }
                    else
                    {
                        lastPeriod = (date.EndDate - date.StartDate).Days;
                    }
                    salaryDetail.Add(new SalaryDetail(lastPeriod, value));
                    break;
                }
                else if (salaryUpdate.Date >= date.EndDate)
                {
                    var lastPeriod = (date.EndDate - salaryDate).Days;
                    salaryDetail.Add(new SalaryDetail(lastPeriod, value));
                    break;
                }
                var timePeriod = (salaryUpdate.Date - salaryDate).Days;
                salaryDetail.Add(new SalaryDetail(timePeriod, value));
                value = salaryUpdate.Value;
                salaryDate = salaryUpdate.Date;
            }
            return salaryDetail;
        }

        private async Task<Expences> GetExpencesAsync(
            Salary salary, 
            DatePeriod date, 
            User employee, 
            decimal premium)
        {
            var salaryDetail = await GetSalaryDetail(salary, date, employee.Id);

            var period = (date.EndDate - date.StartDate).Days;
            decimal avgSalaryValue = 0;
            decimal forDay = 0;

            foreach (var sal in salaryDetail)
            {
                forDay = sal.Value / period;
                avgSalaryValue += forDay * sal.Days;
            }

            var expence = new Expences()
            {
                EmployeeName = employee.FirstName + " " + employee.LastName,
                SalaryValue = avgSalaryValue,
                Premium = premium
            };
            return expence;
        }
        private async Task<FinancialStatement> GetFinancialStatementOneMonth(DatePeriod date)
        {

            decimal procent = 0.1M;
            decimal allIncome =0;
            decimal allExpence = 0;

            //Dictionary<EmployeeId, EmployeePremiums>
            IDictionary<int, decimal> premiums = new Dictionary<int,decimal>();

            //Get all Appoinments in Period
            var appointments = await _appointmentRepository.GetAsync(x=>
                (x.Date >= date.StartDate)&&
                (x.Date < date.EndDate)&&
                (x.MeetHasOccureding == true),
                null,
                includeProperties: "AppointmentProcedures.Procedure,AppointmentUsers.User,Animal");
            var _incomes = new List<Income>();

            //Dictionary<Appoinment.Id, AppointmentCost>
            IDictionary<int, decimal> appCosts = new Dictionary<int, decimal>();

            foreach(var appointment in appointments)
            {
                appCosts.Add(appointment.Id, 0);
            //Count cost of every Appointment
                foreach(var procedures in appointment.AppointmentProcedures)
                {
                    var procedure = await _procedureRepository.GetById(procedures.ProcedureId);
                    appCosts[appointment.Id] += procedure.Cost;
                }
            //Create Income 
                var income = new Income
                {
                    AppointmenId = appointment.Id,
                    Cost = appCosts[appointment.Id]
                };
            //Add previous to list
                _incomes.Add(income);

                var doctorsCount = appointment.AppointmentUsers.Count;

            //Count all doctors Premiums
                foreach(var doctors in appointment.AppointmentUsers)
                {
                    if(!premiums.ContainsKey(doctors.UserId))
                    {
                        premiums.Add(doctors.UserId, appCosts[appointment.Id]*procent / doctorsCount);
                    }
                    else
                    {
                        premiums[doctors.UserId] += appCosts[appointment.Id]*procent / doctorsCount;
                    }
                }

            }
            
            //Get all Salaries where we need to pay in Period
            var salaries = await _repository.GetAllForStatement(filter:x => x.Date < date.StartDate);

            var _expences = new List<Expences>();

            foreach(var salary in salaries)
            {
                var employee = await _userRepository.GetByIdAsync(salary.EmployeeId);
                if (!premiums.ContainsKey(employee.Id))
                {
                    premiums.Add(employee.Id, 0);
                }

                //Count Avarage Salary Value

                var expence = await GetExpencesAsync(salary, date, employee, premiums[employee.Id]);
                _expences.Add(expence);
            }

            var checkSalaryList = await _repository.GetAllForStatement(filter:x => x.Date < date.EndDate);
            if(checkSalaryList.Count() != salaries.Count())
            {
                var newEmployeeSalary = from c in checkSalaryList
                                            from s in salaries
                                              where c.EmployeeId != s.EmployeeId
                                        select c;
                foreach(var salary in newEmployeeSalary)
                {
                    var employee = await _userRepository.GetByIdAsync(salary.EmployeeId);
                    if (!premiums.ContainsKey(employee.Id))
                    {
                        premiums.Add(employee.Id, 0);
                    }

                    //Count Avarage Salary Value

                    var expence = await GetExpencesAsync(salary, date, employee, premiums[employee.Id]);
                    _expences.Add(expence);
                }
            }

            //Count All Incomes
            foreach(var res in appCosts)
            {
                allIncome += res.Value*(1-procent);
            }    

            //Count All Expences
            foreach(var expence in _expences)
            {
                allExpence += (expence.SalaryValue + expence.Premium);
            }

            //Generate Financial Statement for Month
            var financialStatement = new FinancialStatement()
            { 
                Month = date.StartDate.ToString("MMMM yyyy"),
                ExpencesList = _expences,
                IncomesList = _incomes,
                TotalExpences = allExpence,
                TotalIncomes = allIncome
            };

            return financialStatement;
        }

        private int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }

        public async Task<PagedList<FinancialStatement>> GetFinancialStatement(
            DatePeriod date, 
            FinancialStatementParameters parametrs)
        {
            var countMonth = GetMonthsBetween(date.StartDate, date.EndDate);
            var finStatList = new List<FinancialStatement>();
            var monthDate = new DatePeriod();


            for (int i = (parametrs.PageNumber-1)*parametrs.PageSize; i < parametrs.PageNumber*parametrs.PageSize; i++)
            {
                if(monthDate.EndDate == date.EndDate)
                {
                    break;
                }

                monthDate.StartDate = date.StartDate.AddMonths(i);
                monthDate.EndDate = date.StartDate.AddMonths(i+1);

                finStatList.Add(await GetFinancialStatementOneMonth(monthDate));
            }

            //return finStatList;
            return new PagedList<FinancialStatement>(finStatList, countMonth, parametrs.PageNumber, parametrs.PageSize);
        }
    }
    
}
