namespace WebApi.AutoMapper.Interface;

public interface IViewModelMapperAsync<TSource, TDestination>
{
    Task<TDestination> MapAsync(TSource source);
}