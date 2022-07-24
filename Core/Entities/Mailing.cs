namespace Core.Entities
{
    public class Mailing
    {
        public IList<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
