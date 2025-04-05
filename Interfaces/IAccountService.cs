
using URLShortener.Models;
using URLShortener.VM;

namespace URLShortener.Interfaces;

public interface IAccountService
{
    Task<bool> RegisterUserAsync(User user, string password);
    Task<bool> LoginUserAsync(string email, string password, bool rememberMe);
    Task<bool> LogoutUserAsync();
    Task<bool> ChangePasswordAsync(string email, string newPassword);
    Task<bool> VerifyEmailAsync(string email);
}