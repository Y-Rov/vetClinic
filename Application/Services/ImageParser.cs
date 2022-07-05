using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ImageParser : IImageParser
{
    private readonly IConfiguration _configuration;
    private readonly IImageRepository _imageRepository;

    public ImageParser(
        IConfiguration configuration,
        IImageRepository imageRepository)
    {
        _configuration = configuration;
        _imageRepository = imageRepository;
    }
    public void ParseImgTag(string tag, out bool isBase64, out string base64, out string format, out string link, out bool isOuterLink)
    {
        format = "";
        link = "";
        base64 = "";
        isOuterLink = false;
        int srcOffset = tag.IndexOf("src", StringComparison.Ordinal) - 5;

        isBase64 = tag.Substring(10 + srcOffset, 4) == "data";
        if (isBase64)
        {
            var base64StartIndex = tag.IndexOf(",", srcOffset, StringComparison.Ordinal);
            var base64EndIndex = tag.IndexOf('"', base64StartIndex);
            base64 = tag.Substring(
                startIndex: base64StartIndex + 1, //+1 for separating comma: ...base64,iVBORw0KG...
                length: base64EndIndex - base64StartIndex - 1); //-2 for the closing " of the tag - 1 for length not position
            format = tag.Substring(
                startIndex: 21 + srcOffset, // 21 for <img src="data:image/ length
                length: tag.IndexOf(';') - 21 - srcOffset);
            return;
        }
        
        int possibleQueryIndex = tag.IndexOf('?', 11 + srcOffset);
        int closingQuoteIndex = tag.IndexOf('"', 11+ srcOffset);
        int linkEndingIndex = possibleQueryIndex > 0 && possibleQueryIndex < closingQuoteIndex
            ? possibleQueryIndex
            : closingQuoteIndex;
        link = tag.Substring(10 + srcOffset, linkEndingIndex - 10 - srcOffset);
        isOuterLink = link.Substring(0, _configuration["Azure:ContainerLink"].Length) != _configuration["Azure:ContainerLink"];
    }

    public async Task<string> UploadImages(string body)
    {
        int previousTagIndex = 0;
        while (true)
        {
            var tagIndex = body.IndexOf("<img", previousTagIndex, StringComparison.Ordinal);
            if (tagIndex == -1)
            {
                break;
            }

            previousTagIndex = tagIndex + 1;
            
            var closingQuoteIndex = body.IndexOf('>', tagIndex);
            var tag = body.Substring(tagIndex, closingQuoteIndex - tagIndex + 1);

            ParseImgTag(tag, out bool isBase64, out var base64, out var format, out var link, out var isOuterLink);
            if (isBase64)
            {
                var fileName = await _imageRepository.UploadFromBase64Async(
                    base64: base64,
                    folder: "articles",
                    imageFormat: format);
                var newLink = _configuration["Azure:ContainerLink"] + "/" + _configuration["Azure:ContainerName"] + "/" + fileName;
                
                body = body.Remove(
                    startIndex: tagIndex ,
                    count: closingQuoteIndex - tagIndex);
                body = body.Insert(tagIndex, "<img src=\"" + newLink + '"');
            }

            if (isOuterLink)
            {
                //avoid writing
                body = body.Remove(
                    startIndex: tagIndex ,
                    count: closingQuoteIndex - tagIndex);
                body = body.Insert(tagIndex, "<img src=\"" +link + '"');
            }
        }
        return body;
    }
    
    
    public async Task<string> DeleteImages(string body)
    {
        while (true)
        {
            var tagIndex = body.IndexOf("<img", StringComparison.Ordinal);
            if (tagIndex == -1)
            {
                break;
            }
            var closingQuoteIndex = body.IndexOf('>', tagIndex);
            var tag = body.Substring(tagIndex, closingQuoteIndex - tagIndex + 1);

            ParseImgTag(tag, out _, out _, out _, out var link, out var isOuterLink);

            if (!isOuterLink)
            {
                int nameIndex = link.LastIndexOf('/');
                var name = link.Substring(nameIndex + 1);
                await _imageRepository.DeleteAsync(
                    imageName: name,
                    folder: "articles");
            }

            body = body.Remove(
                startIndex: tagIndex ,
                count: closingQuoteIndex - tagIndex + 1);        
        }

        return body;
    }
}