

namespace URLShortener.Interfaces;

public interface IUrlShorteningService
{
    Task<string> GenerateUniqueCode();
}