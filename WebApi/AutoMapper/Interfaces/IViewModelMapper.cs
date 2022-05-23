namespace WebApi.AutoMapper.Interfaces;

public interface IViewModelMapper<TSource, TDest>
{
    public TDest Map(TSource source);
}