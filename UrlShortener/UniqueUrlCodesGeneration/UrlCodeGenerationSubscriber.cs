namespace UrlShortener.UniqueUrlCodesGeneration;

public sealed class UrlCodeGenerationSubscriber
{
    private const int THRESHOLD = 10;
    private readonly UrlShorteningService _UrlShorteningService;

    public UrlCodeGenerationSubscriber(UrlCodeUsedEvent codeUsedEvent, 
                                       UniqueUrlCodesPool uniqueUrlCodePool,
                                       UrlShorteningService urlShorteningService)
    {
        _UrlShorteningService = urlShorteningService;
        codeUsedEvent.CodeUsed += async (sender, e) => await GenerateCodesAsync(uniqueUrlCodePool);
        //codeUsedEvent.OnCodeUsed();
    }

    private async Task GenerateCodesAsync(UniqueUrlCodesPool uniqueUrlCodePool)
    {
        for(int i = uniqueUrlCodePool.Count; i < THRESHOLD; i++)
        {
            string newCode = await _UrlShorteningService.GenerateUniqueCode();
            uniqueUrlCodePool.Enqueue(newCode);
        }
    }
}
