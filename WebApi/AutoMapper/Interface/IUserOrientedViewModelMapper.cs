namespace WebApi.AutoMapper.Interface;

public interface IUserOrientedViewModelMapper<TSource, TDestination>
{
    TDestination Map(TSource source, int userId);
}