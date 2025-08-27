using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.SignalR;
using PhamAnhDungRazorPages.Hubs;

namespace PhamAnhDungRazorPages.Pages.News
{
    public class HistoryModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NewsHub> _newsHub;

        public HistoryModel(
            INewsArticleService newsArticleService,
            ICategoryService categoryService,
            ITagService tagService,
            IConfiguration configuration,
            IHubContext<NewsHub> newsHub)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _configuration = configuration;
            _newsHub = newsHub;
        }

        public List<NewsArticleViewModel> NewsArticles { get; set; } = [];
        public List<DAL.Models.Category> Categories { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
        public string ErrorMessage { get; set; }

        private bool CheckIsStaff()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            const string staffRole = "1";
            return userRole == staffRole;
        }

        public IActionResult OnGet()
        {
            if (!CheckIsStaff())
            {
                return new JsonResult(new { success = false, message = "Access denied" });
            }

            var accountId = HttpContext.Session.GetInt32("AccountID");
            NewsArticles = _newsArticleService.GetNewsArticles()
                .Where(a => a.CreatedById == accountId).ToList();
            Categories = _categoryService.GetCategories().ToList();
            Tags = _tagService.GetTags().ToList();
            return Page();
        }
    }
}
