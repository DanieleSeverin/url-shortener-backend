using Microsoft.AspNetCore.Mvc;
using UrlShortener.Entities;
using UrlShortener.Models;
using UrlShortener.UniqueUrlCodesGeneration;

namespace UrlShortener.Controllers;

[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly UniqueUrlCodeProvider _uniqueUrlCodeProvider;
    private readonly UrlCodeUsedEvent _urlCodeUsedEvent;
    private readonly ApplicationDbContext _dbContext;

    public UrlShortenerController(UniqueUrlCodeProvider uniqueUrlCodeProvider,
                                  ApplicationDbContext dbContext,
                                  UrlCodeUsedEvent urlCodeUsedEvent)
    {
        _uniqueUrlCodeProvider = uniqueUrlCodeProvider;
        _dbContext = dbContext;
        _urlCodeUsedEvent = urlCodeUsedEvent;
    }

    [HttpPost]
    [Route("api/shorten")]
    public async Task<IActionResult> Shorten(ShortenUrl request)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return BadRequest("The specified URL is invalid.");
        }

        var code = await _uniqueUrlCodeProvider.GetUrlCode();

        string baseShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        var shortenedUrl = ShortenedUrl.Create(longUrl: request.Url,
                                               baseShortUrl: baseShortUrl,
                                               code: code);

        _dbContext.ShortenedUrls.Add(shortenedUrl);

        await _dbContext.SaveChangesAsync();

        // Run this in the background
        //Task.Run(() => _urlCodeUsedEvent.OnCodeUsed());
        await _urlCodeUsedEvent.OnCodeUsed();

        return Ok(new ShortenUrl() { Url = shortenedUrl.ShortUrl });
    }
}
