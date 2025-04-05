using System.ComponentModel.DataAnnotations;

namespace URLShortener.VM;

public class VerifyEmailViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string? Email { get; set; }

    
}
