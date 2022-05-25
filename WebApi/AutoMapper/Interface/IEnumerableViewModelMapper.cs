namespace WebApi.AutoMapper.Interface
{
    public interface IEnumerableViewModelMapper<TSource, TViewModel> : IViewModelMapper<TSource, TViewModel>
    {
        public IEnumerable<TViewModel> Map(IEnumerable<TSource> source)
        {
            return source.Select<TSource, TViewModel>(item => Map(item));
        }
    }
}
