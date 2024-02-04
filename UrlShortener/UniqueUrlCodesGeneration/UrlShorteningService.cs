using Microsoft.EntityFrameworkCore;

namespace UrlShortener.UniqueUrlCodesGeneration;

public sealed class UrlShorteningService
{
    public const int NumberOfCharsInShortLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();
    private readonly IServiceProvider _serviceProvider;

    public UrlShorteningService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharsInShortLink];

        using (var scope = _serviceProvider.CreateScope())
        {
            var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
            //while (true)
            //{
                for (var i = 0; i < NumberOfCharsInShortLink; i++)
                {
                    var randomIndex = _random.Next(Alphabet.Length - 1);

                    codeChars[i] = Alphabet[randomIndex];
                }

                var code = new string(codeChars);

                if (!await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == code))
                {
                    return code;
                }

            throw new Exception($"Code {code} already exists.");
            //}
        }
    }
}
