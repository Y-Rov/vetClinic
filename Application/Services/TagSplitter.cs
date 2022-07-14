namespace Application.Services;

public class TagSplitter
{
    private int _previousTagIndex = 0;
    private ReadOnlyMemory<char> _articleBody;

    public TagSplitter(string articleBody)
    {
        _articleBody = articleBody.ToCharArray();
    }

    public bool TryGetNextTag(out ReadOnlyMemory<char> tag)
    {
        return TryGetNextTag(out tag, out _, out _);
    }

    public bool TryGetNextTag(out ReadOnlyMemory<char> tag, out int startIndex, out int length)
    {
        startIndex = 0;
        length = 0;
        tag = ReadOnlyMemory<char>.Empty;
        
        var tagIndex = _articleBody.Span.Slice(_previousTagIndex).IndexOf("<img", StringComparison.Ordinal);
        if (tagIndex == -1)
        {
            return false;
        }
        _previousTagIndex = tagIndex + 1;

        length = _articleBody.Span.Slice(tagIndex).IndexOf(">", StringComparison.Ordinal) + 1;
        tag = _articleBody.Slice(tagIndex, length);
        startIndex = tagIndex;
        return true;
    }

    public void Reset()
    {
        _previousTagIndex = 0;
    }

    public void Reset(string newBody)
    {
        Reset();
        _articleBody = newBody.ToCharArray();
    }
}