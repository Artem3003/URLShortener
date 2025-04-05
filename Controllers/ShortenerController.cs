using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLShortener.Data;
using URLShortener.Interfaces;
using URLShortener.Models;
using URLShortener.VM;

namespace URLShortener.Controllers;


[Authorize(Policy = "Visitor")]
public class ShortenerController : Controller
{
    private readonly IUrlShorteningService urlShorteningService;
    private readonly DatabaseContext context;

    public ShortenerController(IUrlShorteningService urlShorteningService, DatabaseContext context)
    {
        this.urlShorteningService = urlShorteningService;
        this.context = context;
    }

    [Route("Shortener")]
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var totalCount = await context.ShortenedUrls.CountAsync();
        var urls = await context.ShortenedUrls
            .OrderByDescending(u => u.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return View(urls);
    }   

    [Route("Shortener")]
    [HttpGet("Shorten")]
    public IActionResult Shorten()
    {
        return View();
    }

    [Route("Shortener")]
    [HttpPost("Shorten")]
    public async Task<IActionResult> Shorten(ShortenedUrlRequest shortenedUrlRequest)
    {
        var longUrl = shortenedUrlRequest.Url;

        if (!Uri.TryCreate(longUrl, UriKind.Absolute, out _))
        {
            ModelState.AddModelError("", "Please enter a valid URL.");
            return View();
        }

        try
        {
            if (await context.ShortenedUrls.AnyAsync(u => u.OriginalUrl == longUrl))
            {
                return View("Shortened", context.ShortenedUrls.FirstOrDefault(u => u.OriginalUrl == longUrl));
            }
            var code = await urlShorteningService.GenerateUniqueCode();
            var shortenedUrl = new ShortenedUrl
            {
                OriginalUrl = longUrl,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{code}",
                CreatedDate = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name!
            };

            await context.ShortenedUrls.AddAsync(shortenedUrl);
            await context.SaveChangesAsync();

            return View("Shortened", shortenedUrl);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while shortening the URL. Please try again later.");
            return View(shortenedUrlRequest);
        }
    }

    [HttpGet("{code}")]
    public async Task<IResult> RedirectToOriginalUrl(string code)
    {
        var shortenedUrl = await context.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortUrl == $"{Request.Scheme}://{Request.Host}/{code}");

        if (shortenedUrl == null)
        {
            return Results.NotFound();
        }

        return Results.Redirect(shortenedUrl.OriginalUrl);
    }

    [Route("Shortener")]
    [HttpDelete("Delete/{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var shortenedUrl = await context.ShortenedUrls.FindAsync(id);
        if (shortenedUrl == null)
        {
            return NotFound();
        }

        context.ShortenedUrls.Remove(shortenedUrl);
        await context.SaveChangesAsync();

        return NoContent();
    }
}