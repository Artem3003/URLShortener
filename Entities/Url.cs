

using System;
using System.ComponentModel.DataAnnotations;

namespace URLShortener.Entities;

public class Url
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ShortUrl { get; set; } = string.Empty;

    [Required]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string UserId { get; set; } = string.Empty;
}