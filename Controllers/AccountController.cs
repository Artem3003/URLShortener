using URLShortener.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using URLShortener.VM;
using URLShortener.Interfaces;

namespace URLShortener.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly IAccountService accountService;

    public AccountController(UserManager<User> userManager, IMapper mapper, IAccountService accountService)
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.accountService = accountService;
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var loginSuccess = await accountService.LoginUserAsync(loginViewModel.Email!, loginViewModel.Password!, loginViewModel.RememberMe);

            if (loginSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email or password is incorrect.");
        }
        return View(loginViewModel);
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = mapper.Map<User>(registerViewModel); 
            var success = await accountService.RegisterUserAsync(user, registerViewModel.Password!);

            if (success)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Registration failed. Please try again.");
        }
        
        return View(registerViewModel);
    }

    [HttpGet("VerifyEmail")]
    public IActionResult VerifyEmail()
    {
        return View();
    }

    [HttpPost("VerifyEmail")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel verifyEmailViewModel)
    {
        if (ModelState.IsValid)
        {
            var success = await accountService.VerifyEmailAsync(verifyEmailViewModel.Email!);

            if (!success)
            {
                ModelState.AddModelError("", "Email not found.");
                return View(verifyEmailViewModel);
            }

            return RedirectToAction("ChangePassword", "Account", new { username = verifyEmailViewModel.Email });
        }

        return View(verifyEmailViewModel);
    }

    [HttpGet("ChangePassword")]
    public IActionResult ChangePassword(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("VerifyEmail", "Account");
        }

        return View(new ChangePasswordViewModel { Email = username });
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        if (ModelState.IsValid)
        {
            var success = await accountService.ChangePasswordAsync(changePasswordViewModel.Email!, changePasswordViewModel.NewPassword!);

            if (success)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Failed to change password. Please check your inputs.");
        }
        else
        {
            ModelState.AddModelError("", "Something went wrong. Please try again.");
        }

        return View(changePasswordViewModel);
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        var success = await accountService.LogoutUserAsync();

        if (success)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Failed to log out. Please try again.");
        return View();
    }
}