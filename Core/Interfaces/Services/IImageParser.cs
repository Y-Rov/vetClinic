namespace Core.Interfaces.Services;

public interface IImageParser
{
    void ParseImgTag(string tag, out bool isBase64, out string base64, out string format, out string link,
        out bool isOuterLink);
    Task<string> UploadImages(string body);
    Task<string> DeleteImages(string body);
}