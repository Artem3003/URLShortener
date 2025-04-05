

using System;
using System.ComponentModel.DataAnnotations;

namespace URLShortener.Models;

public class Url
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ShortUrl { get; set; } = string.Empty;

    [Required]
    public string OriginalUrl { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public string CreatedBy { get; set; } = string.Empty;
}