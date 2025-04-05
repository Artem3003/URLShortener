using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using URLShortener.Interfaces;
using URLShortener.Models;
using URLShortener.VM;

namespace URLShortener.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
     private readonly ILogger<AccountService> logger;

    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountService> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.logger = logger;
    }

    public async Task<bool> ChangePasswordAsync(string email, string newPassword)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            logger.LogError($"User with email {email} not found.");
            return false;
        }

        var removeResult = await userManager.RemovePasswordAsync(user);
        if (!removeResult.Succeeded)
        {
            logger.LogError("Failed to remove the old password.");
            return false;
        }

        var addResult = await userManager.AddPasswordAsync(user, newPassword);
        if (!addResult.Succeeded)
        {
            logger.LogError("Failed to add the new password.");
            return false;
        }

        logger.LogInformation($"Password changed successfully for {email}.");
        return true;
    }

    public async Task<bool> LoginUserAsync(string email, string password, bool rememberMe)
    {
        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);

        if (result.Succeeded)
        {
            logger.LogInformation("User logged in.");
            return true;
        }
        
        logger.LogError("Error logging in user.");
        return false;
    }

    public async Task<bool> LogoutUserAsync()
    {
        try
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error logging out: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterUserAsync(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Visitor");
            logger.LogInformation("User created a new account with password.");
            return true;
        }

        logger.LogError("Error creating user account.");
        return false;
    }

    public async Task<bool> VerifyEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            logger.LogError($"Email not found: {email}");
            return false;
        }

        logger.LogInformation($"Email verified: {email}");
        return true;
    }
}