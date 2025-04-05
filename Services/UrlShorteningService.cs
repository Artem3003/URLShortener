

using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Interfaces;

namespace URLShortener.Services;

public class UrlShorteningService : IUrlShorteningService
{
    private const int CodeLength = 6;
    private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly Random random = new Random();
    private readonly DatabaseContext context;

    public UrlShorteningService(DatabaseContext context)
    {
        this.context = context;
    }

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[CodeLength];

        for (int i = 0; i < CodeLength; i++)
        {
            var randomIndex = random.Next(Characters.Length - 1);

            codeChars[i] = Characters[randomIndex];
        }

        var code = new string(codeChars);
        var existingUrl = await context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortUrl == code);

        return existingUrl != null ? await GenerateUniqueCode() : code;
    }
}