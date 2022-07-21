using Core.Interfaces.Services.PDF_Service;
using Core.Models.Finance;
using Core.Paginator;
using System.Data;
using System.Text;

namespace Application.Services.FinancialStatement_PDF
{
    public class CreateTableForFinancialStatementPDF : ICreateTableForPDF<FinancialStatement>
    {   
        public DataTable CreateTable(PagedList<FinancialStatement> listOfFinancialStatement)
        {
            var table = new DataTable("Financial Statement");

            //Add column "Month"
            var column = new DataColumn();
            column.ColumnName = "Month";
            table.Columns.Add(column);

            //Add column "Incomes"
            column = new DataColumn();
            column.ColumnName = "Total Incomes";
            table.Columns.Add(column);

            //Add column "Expences"
            column = new DataColumn();
            column.ColumnName = "Total Expences";
            table.Columns.Add(column);

            //Add rows
            string incomesList;
            string expencesList;
            foreach(var statement in listOfFinancialStatement)
            {
                table.Rows.Add(new object[] {statement.Month, statement.TotalIncomes, statement.TotalExpences});
                incomesList = "";
                foreach(var income in statement.IncomesList)
                {
                    incomesList += " AppointmentID: "+ income.AppointmenId.ToString() 
                        + "\tCost: " + income.Cost.ToString() + "\n";
                }
                expencesList = "";
                foreach(var expence in statement.ExpencesList)
                {
                    expencesList+= " " + expence.EmployeeName.ToString() 
                        + "\tPayments:" + (expence.SalaryValue + expence.Premium).ToString() + "\n";
                }
                table.Rows.Add(new object[] {null,incomesList,expencesList});
            }


            return table;
        }
    }
}
