namespace WebApi.AutoMapper.Interface;

public interface IViewModelMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
    
}