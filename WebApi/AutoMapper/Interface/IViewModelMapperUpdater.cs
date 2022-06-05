namespace WebApi.AutoMapper.Interface
{
    public interface IViewModelMapperUpdater<TSource, TDestination> : IViewModelMapper<TSource, TDestination>
    {
        void Map(TSource source, TDestination dest);
    }
}