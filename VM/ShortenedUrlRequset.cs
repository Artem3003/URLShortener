using System.ComponentModel.DataAnnotations;

namespace URLShortener.VM;

public class ShortenedUrlRequest
{
    [Required(ErrorMessage = "Url is required.")]
    [Url(ErrorMessage = "Please enter a valid URL.")]
    public string Url { get; set; } = string.Empty;
}