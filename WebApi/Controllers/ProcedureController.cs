using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[Route("api/procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly IViewModelMapper<ProcedureViewModelBase, Procedure> _procedureCreateMapper;
    private readonly IViewModelMapper<ProcedureUpdateViewModel, Procedure> _procedureUpdateMapper;
    private readonly IViewModelMapper<Procedure, ProcedureReadViewModel> _procedureViewModelMapper;

    private readonly IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>
        _procedureEnumerableViewModelMapper;

    public ProcedureController(IProcedureService procedureService, 
        IViewModelMapper<ProcedureViewModelBase, Procedure> procedureCreateMapper,
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
    public async Task<ActionResult<IEnumerable<ProcedureReadViewModel>>> GetAsync()
    {
        var procedures = await _procedureService.GetAllProceduresAsync();
        var viewModels = _procedureEnumerableViewModelMapper.Map(procedures);
        return Ok(viewModels);
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ProcedureReadViewModel>> GetAsync([FromRoute]int id)
    {
        var result = await _procedureService.GetByIdAsync(id);
        var viewModel = _procedureViewModelMapper.Map(result);
        return Ok(viewModel);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody]ProcedureViewModelBase procedure)
    {
        var newProcedure = _procedureCreateMapper.Map(procedure);
        await _procedureService.CreateNewProcedureAsync(newProcedure);
        return Ok();
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute]int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody]ProcedureUpdateViewModel newProcedure)
    {
        var updatedProcedure = _procedureUpdateMapper.Map(newProcedure);
        await _procedureService.UpdateProcedureAsync(updatedProcedure);
        await _procedureService.UpdateProcedureSpecializationsAsync(
            newProcedure.Id,
            newProcedure.SpecializationIds);
        return NoContent();
    }
}