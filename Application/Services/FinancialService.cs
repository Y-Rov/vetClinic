using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Finance;
using System.Linq.Expressions;

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

        public async Task<IEnumerable<Salary>> GetSalaryAsync(Expression<Func<Salary, bool>>? filter)
        {
            _logger.LogInfo($"salaries were recieved");
            var allSalary = await _repository.GetAsync(filter, x => x.OrderBy(y => y.EmployeeId).ThenByDescending(y => y.Date));
            var result = new List<Salary>();
            int? id = null;

            foreach (var salary in allSalary)
            {
                if((id!=salary.EmployeeId))
                {
                    id = salary.EmployeeId;
                    if(salary.Value !=0)
                    {
                        result.Add(salary);
                    }
                }
            }
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

        public async Task CleanOldSalariesAsync()
        {
            var salaries = await _repository.GetAsync(x => x.Date.Year >= (DateTime.Now.Year - 2));
            foreach(var salary in salaries)
            {
                _repository.Delete(salary);
            }
        }

        public async Task<IEnumerable<User>> GetEmployeesWithoutSalary()
        {
            var salaries = await GetSalaryAsync(null);
            var employeesId = await _repository.GetEmployees();
            var employees = await _userRepository.GetAllAsync(filter: x=> employeesId.Contains(x.Id));

            var res = from salary in salaries
                      join employee in employees on salary.EmployeeId equals employee.Id
                      select employee;
            var result = from employee in employees
                         where !res.Contains(employee)
                         select employee;

            return result;
        }

        private async Task<FinancialStatement> GetFinancialStatementOneMonth(Date date)
        {
            decimal procent = 0.1M;
            decimal allIncome =0;
            decimal allExpence = 0;

            //Dictionary<EmployeeId, EmployeePremiums>
            IDictionary<int, decimal> premiums = new Dictionary<int,decimal>();

            //Get all Appoinments in Period
            var appointments = await _appointmentRepository.GetAsync(x=>
                (x.Date >= date.startDate)&&
                (x.Date < date.endDate)&&
                (x.MeetHasOccureding == true),
                null,
                includeProperties: "AppointmentProcedures.Procedure,AppointmentUsers.User,Animal");
            var _incomes = new List<Income>();

            //Dictionary<Appoinment.Id, AppointmentCost>
            IDictionary<int, decimal> appCosts = new Dictionary<int, decimal>();

            var listProcedures = new List<Procedure>();

            foreach(var appointment in appointments)
            {
                appCosts.Add(appointment.Id, 0);
            //Count cost of every Appointment
                foreach(var procedures in appointment.AppointmentProcedures)
                {
                    var procedure = await _procedureRepository.GetById(procedures.ProcedureId);
                    listProcedures.Add(procedure);
                    appCosts[appointment.Id] += procedure.Cost;
                }
            //Create Income 
                var income = new Income
                {
                    AppointmenId = appointment.Id,
                    ListOfProcedures = listProcedures,
                    Cost = appCosts[appointment.Id]
                };
            //Add previous to list
                _incomes.Add(income);

                var doctorsCount = appointment.AppointmentUsers.Count();

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
            var salaries = await GetSalaryAsync(x => x.Date < date.startDate);

            var _expences = new List<Expences>();
            foreach(var salary in salaries)
            {
                var employee = await _userRepository.GetByIdAsync(salary.EmployeeId);
                if (!premiums.ContainsKey(employee.Id))
                {
                    premiums.Add(employee.Id, 0);
                }
                    var expence = new Expences()
                {
                    EmployeeName = employee.FirstName + " " + employee.LastName,
                    SalaryValue = salary.Value,
                    Premium = premiums[employee.Id]
                };
                _expences.Add(expence);
            }

            foreach(var res in appCosts)
            {
                allIncome += res.Value*(1-procent);
            }    

            foreach(var expence in _expences)
            {
                allExpence += (expence.SalaryValue + expence.Premium);
            }

            var financialStatement = new FinancialStatement()
            { 
                Month = date.startDate.ToString("MMMM yyyy"),
                //Period = date,
                expences = _expences,
                incomes = _incomes,
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

        public async Task<FinancialStatementList> GetFinancialStatement(Date date)
        {
            var countMonth = GetMonthsBetween(date.startDate, date.endDate);
            var finStatList = new List<FinancialStatement>();
            var monthDate = new Date();

            for(int i = 0; i < countMonth; i++)
            {
                monthDate.startDate = date.startDate.AddMonths(i);
                monthDate.endDate = date.startDate.AddMonths(i+1);

                finStatList.Add(await GetFinancialStatementOneMonth(monthDate));
            }
            
            var financialStatementList = new FinancialStatementList()
            {
                Period = date,
                StatementsForEachMonth = finStatList
            };
            return financialStatementList;
        }
    }
    
}
