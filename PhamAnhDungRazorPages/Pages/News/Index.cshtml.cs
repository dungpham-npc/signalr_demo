using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.SignalR;
using PhamAnhDungRazorPages.Hubs;

namespace PhamAnhDungRazorPages.Pages.News
{
    public class NewsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NewsHub> _newsHub;

        public NewsModel(
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
            const string staffRole = "2";
            return userRole == staffRole;
        }

        public IActionResult OnGet()
        {
            NewsArticles = _newsArticleService.GetNewsArticles().ToList();
            Categories = _categoryService.GetCategories().ToList();
            Tags = _tagService.GetTags().ToList();
            return Page();
        }

        public IActionResult OnGetStatistics(DateTime? startDate, DateTime? endDate)
        {
            var adminRole = _configuration["AdminRole"] ?? "3";
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != adminRole) return Unauthorized();

            var articles = _newsArticleService.GetNewsArticles();

            if (startDate.HasValue)
                articles = articles.Where(a => a.CreatedDate >= startDate.Value).ToList();

            if (endDate.HasValue)
                articles = articles.Where(a => a.CreatedDate <= endDate.Value).ToList();

            var statistics = articles
                .GroupBy(a => new { Year = a.CreatedDate.Year, Month = a.CreatedDate.Month, Category = a.Category.CategoryName })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    CategoryName = g.Key.Category,
                    ArticleCount = g.Count(),
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}"
                })
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToList();

            return new JsonResult(new { success = true, statistics });
        }
    }
}
