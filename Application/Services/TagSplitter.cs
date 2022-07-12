namespace Application.Services;

public class TagSplitter
{
    private int _previousTagIndex = 0;
    private string _articleBody;
    
    public TagSplitter(string articleBody)
    {
        _articleBody = articleBody;
    }

    public bool TryGetNextTag(out string tag)
    {
        tag = string.Empty;
        var tagIndex = _articleBody.IndexOf("<img", _previousTagIndex, StringComparison.Ordinal);
        if (tagIndex == -1)
        {
            return false;
        }

        _previousTagIndex = tagIndex + 1;
            
        var closingQuoteIndex = _articleBody.IndexOf('>', tagIndex);
        tag = _articleBody.Substring(tagIndex, closingQuoteIndex - tagIndex + 1);
        return true;
    }    
    public bool TryGetNextTag(out string tag, out int startIndex, out int length)
    {
        startIndex = 0;
        length = 0;
        tag = string.Empty;
        var tagIndex = _articleBody.IndexOf("<img", _previousTagIndex, StringComparison.Ordinal);
        if (tagIndex == -1)
        {
            return false;
        }

        _previousTagIndex = tagIndex + 1;
            
        var closingQuoteIndex = _articleBody.IndexOf('>', tagIndex);
        tag = _articleBody.Substring(tagIndex, closingQuoteIndex - tagIndex + 1);
        startIndex = tagIndex;
        length = closingQuoteIndex - tagIndex;
        return true;
    }

    public void Reset()
    {
        _previousTagIndex = 0;
    }

    public void Reset(string newBody)
    {
        Reset();
        _articleBody = newBody;
    }
}