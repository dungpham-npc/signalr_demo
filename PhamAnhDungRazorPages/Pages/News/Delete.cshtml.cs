using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PhamAnhDungRazorPages.Hubs;

namespace PhamAnhDungRazorPages.Pages.News;

public class DeleteModel : PageModel
{
    private readonly INewsArticleService _newsArticleService;
    private readonly IHubContext<NewsHub> _newsHub;

    public DeleteModel(INewsArticleService newsArticleService, IHubContext<NewsHub> newsHub)
    {
        _newsArticleService = newsArticleService;
        _newsHub = newsHub;
    }

    [BindProperty]
    public int NewsArticleId { get; set; }

    public IActionResult OnGet(int id)
    {
        NewsArticleId = id;
        return Page();
    }

    private bool CheckIsStaff()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        const string staffRole = "1";
        return userRole == staffRole;
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        if (!CheckIsStaff())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }

        try
        {
            Console.WriteLine($"Received Delete Request for ID: {id}");
            _newsArticleService.DeleteNewsArticle(id);

            Console.WriteLine($"Sending SignalR Update for Deleted Article ID: {id}");
            await _newsHub.Clients.All.SendAsync("NewsUpdated", "deleted", new { NewsArticleId = id });

            Console.WriteLine($"SignalR Update Sent Successfully");
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting article: {ex.Message}");
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

}