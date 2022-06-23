using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[Route("api/procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly IViewModelMapper<ProcedureCreateViewModel, Procedure> _procedureCreateMapper;
    private readonly IViewModelMapper<ProcedureUpdateViewModel, Procedure> _procedureUpdateMapper;
    private readonly IViewModelMapper<Procedure, ProcedureReadViewModel> _procedureViewModelMapper;

    private readonly IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>
        _procedureEnumerableViewModelMapper;

    public ProcedureController(IProcedureService procedureService, 
        IViewModelMapper<ProcedureCreateViewModel, Procedure> procedureCreateMapper,
        IViewModelMapper<ProcedureUpdateViewModel, Procedure> procedureUpdateMapper,
        IViewModelMapper<Procedure, ProcedureReadViewModel> procedureViewModelMapper, 
        IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> procedureEnumerableViewModelMapper)
    {
        _procedureService = procedureService;
        _procedureCreateMapper = procedureCreateMapper;
        _procedureUpdateMapper = procedureUpdateMapper;
        _procedureViewModelMapper = procedureViewModelMapper;
        _procedureEnumerableViewModelMapper = procedureEnumerableViewModelMapper;
    }

    [HttpGet]
    public async Task<IEnumerable<ProcedureReadViewModel>> GetAsync()
    {
        var procedures = await _procedureService.GetAllProceduresAsync();
        var viewModels = _procedureEnumerableViewModelMapper.Map(procedures);
        return viewModels;
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ProcedureReadViewModel> GetAsync([FromRoute]int id)
    {
        var result = await _procedureService.GetByIdAsync(id);
        var viewModel = _procedureViewModelMapper.Map(result);
        return viewModel;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task CreateAsync([FromBody]ProcedureCreateViewModel procedure)
    {
        var newProcedure = _procedureCreateMapper.Map(procedure);        
        await _procedureService.CreateNewProcedureAsync(newProcedure, procedure.SpecializationIds);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task DeleteAsync([FromRoute]int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task UpdateAsync([FromBody]ProcedureUpdateViewModel newProcedure)
    {
        var updatedProcedure = _procedureUpdateMapper.Map(newProcedure);
        await _procedureService.UpdateProcedureAsync(updatedProcedure);
        await _procedureService.UpdateProcedureSpecializationsAsync(
            newProcedure.Id,
            newProcedure.SpecializationIds);
    }
}