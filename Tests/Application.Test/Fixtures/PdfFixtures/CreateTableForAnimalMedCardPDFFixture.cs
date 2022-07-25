using Application.Services.GeneratePDF.AnimalMedCard_PDF;
using Core.Entities;
using Core.Paginator;
using System.Data;

namespace Application.Test.Fixtures.PdfFixtures
{
    public class CreateTableForAnimalMedCardPDFFixture
    {
        public CreateTableForAnimalMedCardPDFFixture()
        {
            ListOfAppointments = GetListOfAppointments();
            EmpyListOfAppointments = GetEmptyPagedMedCard();
            ExpectedTable = GetTable(ListOfAppointments);
            ExpectedEmptyTable = GetEmptyTable();

            MockAnimalMedCardTableCreater = new CreateTableForAnimalMedCardPDF();
        }
        public CreateTableForAnimalMedCardPDF MockAnimalMedCardTableCreater { get; }
        public DataTable ExpectedTable { get; }
        public DataTable ExpectedEmptyTable { get; }
        public PagedList<Appointment> ListOfAppointments { get; }
        public PagedList<Appointment> EmpyListOfAppointments { get; }

        private DataTable GetTable(PagedList<Appointment> listOfAppointments)
        {
            var table = new DataTable("Medical Card");

            table.Columns.Add("Data");
            table.Columns.Add("Decese");

            foreach (var x in listOfAppointments)
            {
                table.Rows.Add(new object[] { $"{x.Date}", $"{x.Disease}" });
            }

            return table;
        }

        private DataTable GetEmptyTable()
        {
            var table = new DataTable("Medical Card");

            table.Columns.Add("Data");
            table.Columns.Add("Decese");

            return table;
        }

        private PagedList<Appointment> GetListOfAppointments()
        {
            var appointments = GetAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private PagedList<Appointment> GetEmptyPagedMedCard()
        {
            var appointments = GetEmptyAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private List<Appointment> GetAppointmentList()
        {
            var appointments = new List<Appointment>()
            {

                new Appointment()
                {
                    Id = 1,
                    AnimalId = 1,
                    Date = new DateTime(2022, 06, 20),
                    Disease = "string",
                    MeetHasOccureding = true
                },
                new Appointment()
                {
                    Id = 2,
                    AnimalId = 2,
                    Date = new DateTime(2022, 06, 21),
                    Disease = "string",
                    MeetHasOccureding = true
                }
            };

            return appointments;
        }

        private List<Appointment> GetEmptyAppointmentList()
        {
            return new List<Appointment>();
        }
    }
}
