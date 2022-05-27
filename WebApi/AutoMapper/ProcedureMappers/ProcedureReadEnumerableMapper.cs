using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureReadEnumerableMapper : 
    IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>
{
    private readonly IViewModelMapper<Procedure, ProcedureReadViewModel> _readMapper;

    public ProcedureReadEnumerableMapper(IViewModelMapper<Procedure, ProcedureReadViewModel> readMapper)
    {
        _readMapper = readMapper;
    }
    public IEnumerable<ProcedureReadViewModel> Map(IEnumerable<Procedure> source)
    {
        var readViewModels = source.Select(pr => _readMapper.Map(pr));
        return readViewModels;
    }
}