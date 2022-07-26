using Core.Interfaces.Services.PDF_Service;
using Core.Models.Finance;
using Core.Paginator;
using System.Data;
using System.Text;

namespace Application.Services.GeneratePDF.FinancialStatement_PDF
{
    public class CreateTableForFinancialStatementPdf : ICreateTableForPdf<FinancialStatement>
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
            StringBuilder incomesList ;
            StringBuilder expencesList;
            foreach(var statement in listOfFinancialStatement)
            {
                table.Rows.Add(new object[] 
                {
                    statement.Month, 
                    statement.TotalIncomes.ToString("F2"), 
                    statement.TotalExpences.ToString("F2") 
                });

                incomesList = new StringBuilder();
                foreach(var income in statement.IncomesList)
                {
                    incomesList.AppendLine( " AppointmentID: "+ income.AppointmenId.ToString() 
                        + "\tCost: " + income.Cost.ToString("F2"));
                }
                expencesList = new StringBuilder();
                foreach(var expence in statement.ExpencesList)
                {
                    expencesList.AppendLine(" " + expence.EmployeeName.ToString() 
                        + "\tPayments:" + (expence.SalaryValue + expence.Premium).ToString("F2"));
                }
                table.Rows.Add(new object[] {"",incomesList,expencesList});
            }


            return table;
        }
    }
}
