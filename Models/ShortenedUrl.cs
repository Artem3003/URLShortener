

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace URLShortener.Models;

[Index(nameof(ShortUrl), IsUnique = true)]
public class ShortenedUrl
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string ShortUrl { get; set; } = string.Empty;

    [Required]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public string CreatedBy { get; set; } = string.Empty;
}