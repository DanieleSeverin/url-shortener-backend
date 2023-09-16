namespace UrlShortener.Entities;

public class ShortenedUrl
{
    public Guid Id { get; set; }

    public string LongUrl { get; set; } = string.Empty;

    public string ShortUrl { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public DateTime CreatedOnUtc { get; set; }

    private ShortenedUrl() { }

    private ShortenedUrl(string longUrl, string baseShortUrl, string code)
    {
        Id = Guid.NewGuid();
        LongUrl = longUrl;
        Code = code;
        ShortUrl = $"{baseShortUrl}/{code}";
        CreatedOnUtc = DateTime.Now;
    }

    public static ShortenedUrl Create(string longUrl, string baseShortUrl, string code)
    {
        var shortenedUrl = new ShortenedUrl(longUrl, baseShortUrl, code);

        //shortenedUrl.RaiseDomainEvent(new ShortenedUrlCreatedDomainEvent() { ShortenedUrlId = shortenedUrl.Id });

        return shortenedUrl;
    }


}
