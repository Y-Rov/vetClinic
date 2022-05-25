using Core.Entities;
using Core.ViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ExceptionMapper;

public class ExceptionsMapper : IViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>
{

    public IEnumerable<ExceptionEntityReadViewModel> Map(IEnumerable<ExceptionEntity> source)
    {
        List<ExceptionEntityReadViewModel> result = new();
        foreach (var item in source)
        {
            result.Add(new ExceptionEntityReadViewModel()
            {

                Id = item.Id,
                Name = item.Name,
                DateTime = item.DateTime,
                Path = item.Path,

            });
        }
            return result;
    }
}


