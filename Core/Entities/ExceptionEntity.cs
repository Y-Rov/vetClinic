namespace Core.Entities
{
    public class ExceptionEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateTime { get; set; }
        public string? StackTrace { get; set; }
        public string? Path { get; set; }
        public ExceptionEntity() {}
        public ExceptionEntity(string name, DateTime dateTime, string stackTrace, string path)
        {
            Name = name;
            DateTime = dateTime;
            StackTrace = stackTrace;
            Path = path;
        }
    }
}
