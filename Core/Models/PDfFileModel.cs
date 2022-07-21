namespace Core.Models
{
    public class PdfFileModel
    {
        public MemoryStream? FileStream { get; set; }
        public string? DefaultFileName { get; set; }
        public string? ContentType { get; set; }
    }
}