using System.ComponentModel.DataAnnotations;

namespace URLShortener.VM;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}