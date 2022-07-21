using Core.Paginator;
using System.Data;

namespace Core.Interfaces.Services.PDF_Service
{
    public interface ICreateTableForPDF<TypeOfListElement>
    {
        DataTable CreateTable(PagedList<TypeOfListElement> list);
    }
}
