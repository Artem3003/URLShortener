using Microsoft.AspNetCore.Identity;

namespace URLShortener.Models;

public class Role : IdentityRole
{
    public string Description { get; set; }
}