using Core.Entities;
using Core.Interfaces.Services.PDF_Service;
using Core.Paginator;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.GeneratePDF.AnimalMedCard_PDF
{
    public class CreateTableForAnimalMedCardPDF : ICreateTableForPdf<Appointment>
    {
        public DataTable CreateTable(PagedList<Appointment> listOfAppointments)
        {
            var table = new DataTable("Medical Card");

            table.Columns.Add("Data");
            table.Columns.Add("Decese");

            foreach (var x in listOfAppointments)
            {
                table.Rows.Add(new object[] { $"{x.Date}", $"{x.Disease}"});
            }

            return table;
        }
    }
}
