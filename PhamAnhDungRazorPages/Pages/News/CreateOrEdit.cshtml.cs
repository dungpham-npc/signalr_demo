using BusinessLayer.DTO;
using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PhamAnhDungRazorPages.Hubs;

namespace PhamAnhDungRazorPages.Pages.News;

public class CreateOrEditModel : PageModel
{
    private readonly INewsArticleService _newsArticleService;
    private readonly ICategoryService _categoryService;
    private readonly IHubContext<NewsHub> _newsHub;

    public CreateOrEditModel(
        INewsArticleService newsArticleService,
        ICategoryService categoryService,
        IHubContext<NewsHub> newsHub)
    {
        _newsArticleService = newsArticleService;
        _categoryService = categoryService;
        _newsHub = newsHub;
    }

    [BindProperty]
    public NewsArticleDto NewsArticle { get; set; }
    public List<DAL.Models.Category> Categories { get; set; }

    private bool CheckIsStaff()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        const string staffRole = "1";
        return userRole == staffRole;
    }

    public IActionResult OnGet(int? id)
    {
        if (!CheckIsStaff())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }

        Categories = _categoryService.GetCategories().ToList();

        if (id.HasValue && id.Value > 0)
        {
            var article = _newsArticleService.GetNewsArticleById(id.Value);
            if (article == null)
            {
                return NotFound($"News article {id.Value} not found");
            }

            NewsArticle = new NewsArticleDto
            {
                NewsArticleId = article.NewsArticleId,
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryId = article.CategoryId
            };
        }
        else
        {
            NewsArticle = new NewsArticleDto();
        }

        return Page();
    }


    public async Task<JsonResult> OnPostCreateOrUpdate()
    {
        if (!CheckIsStaff())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }
        try
        {
            Console.WriteLine("OnPostCreateOrUpdate() was called!");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Validation failed: " + string.Join(", ", errors));
                return new JsonResult(new { success = false, message = "Validation failed", errors }) { StatusCode = 400 };
            }

            if (NewsArticle == null)
            {
                Console.WriteLine("NewsArticle object is NULL!");
                return new JsonResult(new { success = false, message = "Invalid news article data." }) { StatusCode = 400 };
            }

            if (NewsArticle.NewsArticleId == 0)
            {
                Console.WriteLine($"Creating new news article: {NewsArticle.NewsTitle}");
                var createdArticle = _newsArticleService.CreateNewsArticle(NewsArticle);
                NewsArticle.NewsArticleId = createdArticle.NewsArticleId;

                Console.WriteLine($" Sending SignalR Update for Created Article ID: {NewsArticle.NewsArticleId}");
                await _newsHub.Clients.All.SendAsync("NewsUpdated", "created", NewsArticle);
            }
            else
            {
                Console.WriteLine($"Updating news article {NewsArticle.NewsArticleId}: {NewsArticle.NewsTitle}");
                _newsArticleService.UpdateNewsArticle(NewsArticle.NewsArticleId, NewsArticle);
                Console.WriteLine($"Sending SignalR Update for Updated Article ID: {NewsArticle.NewsArticleId}");
                await _newsHub.Clients.All.SendAsync("NewsUpdated", "updated", NewsArticle);
            }

            Console.WriteLine("Successfully processed request.");
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception in OnPostCreateOrUpdate: " + ex.Message);
            return new JsonResult(new { success = false, message = "An unexpected error occurred.", error = ex.Message }) { StatusCode = 500 };
        }
    }


}