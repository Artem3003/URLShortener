using Microsoft.AspNetCore.Identity;

namespace URLShortener.Entities;

public class Role : IdentityRole
{
    public string Description { get; set; }
}