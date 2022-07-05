namespace Core.Paginator.Parameters
{
    public class UserParameters : ElementParameters
    {
        public string? FilterParam { get; init; }
        public string? OrderByParam { get; set; }
    }
}
