namespace WebApi.AutoMapper.Interface;

public interface IUserOrientedEnumerableViewModelMapper<TSource, TDestination>: 
    IUserOrientedViewModelMapper<IEnumerable<TSource>, IEnumerable<TDestination>>
{
    public IEnumerable<TDestination> Map(IEnumerable<TSource> source, int userId);
}