using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;
using URLShortener.Services;

namespace URLShortener.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly UserManager<User> userManager;
    private readonly AlgorithmDescriptionService descriptionService;

    public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, AlgorithmDescriptionService descriptionService)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.descriptionService = descriptionService;
    }


    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("About")]
    public async Task<IActionResult> About()
    {
        var user = await userManager.GetUserAsync(User);
        var isAdmin = user != null && await userManager.IsInRoleAsync(user, "Admin");

        ViewBag.Description = descriptionService.GetDescription();
        
        ViewBag.IsAdmin = isAdmin;
        return View();
    }

    [HttpPost("About")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> About(string description)
    {
        Console.WriteLine($"Received description: {description}"); // Debug output
        
        var user = await userManager.GetUserAsync(User);
        bool isAdmin = user != null && await userManager.IsInRoleAsync(user, "Admin");
        
        if (!isAdmin)
        {
            Console.WriteLine("Non-admin user tried to save"); // Debug
            return Forbid();
        }

        descriptionService.SetDescription(description);
        Console.WriteLine("Description saved successfully"); // Debug
        return RedirectToAction("About");
    }
}