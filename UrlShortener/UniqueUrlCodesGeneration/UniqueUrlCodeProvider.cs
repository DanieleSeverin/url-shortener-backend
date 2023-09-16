namespace UrlShortener.UniqueUrlCodesGeneration
{
    public sealed class UniqueUrlCodeProvider
    {
        private readonly UrlShorteningService _urlShorteningService;
        private readonly UniqueUrlCodesPool _uniqueUrlCodePool;

        public UniqueUrlCodeProvider(UrlShorteningService urlShorteningService, UniqueUrlCodesPool uniqueUrlCodePool)
        {
            _urlShorteningService = urlShorteningService;
            _uniqueUrlCodePool = uniqueUrlCodePool;
        }

        public async Task<string> GetUrlCode()
        {
            string? code = _uniqueUrlCodePool.Dequeue();
            if(string.IsNullOrEmpty(code))
            {
                code = await _urlShorteningService.GenerateUniqueCode();
            }

            return code;
        }
    }
}
