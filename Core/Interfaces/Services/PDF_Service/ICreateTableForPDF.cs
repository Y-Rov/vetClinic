using Core.Paginator;
using System.Data;

namespace Core.Interfaces.Services.PDF_Service
{
    public interface ICreateTableForPdf<TypeOfListElement>
    {
        DataTable CreateTable(PagedList<TypeOfListElement> list);
    }
}
