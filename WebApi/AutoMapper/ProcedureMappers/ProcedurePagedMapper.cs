using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.ProcedureViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedurePagedMapper : IViewModelMapper<PagedList<Procedure>, PagedReadViewModel<ProcedureReadViewModel>>
{
    private readonly IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>
        _enumMapper;

    public ProcedurePagedMapper(IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> enumMapper)
    {
        _enumMapper = enumMapper;
    }


    public PagedReadViewModel<ProcedureReadViewModel> Map(PagedList<Procedure> source)
    {
        return new PagedReadViewModel<ProcedureReadViewModel>()
        {
            CurrentPage = source.CurrentPage,
            PageSize = source.PageSize,
            TotalCount = source.TotalCount,
            HasPrevious = source.HasPrevious,
            HasNext = source.HasNext,
            TotalPages = source.TotalPages,
            Entities = _enumMapper.Map(source.AsEnumerable())
        };
    }
}
