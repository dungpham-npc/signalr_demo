using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.SignalR;
using PhamAnhDungRazorPages.Hubs;

namespace PhamAnhDungRazorPages.Pages.News
{
    public class StatisticsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _configuration;

        public StatisticsModel(
            INewsArticleService newsArticleService,
            ICategoryService categoryService,
            IConfiguration configuration)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1);

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; } = DateTime.Now;

        public List<StatisticsItem> Statistics { get; set; } = [];

        private bool CheckIsAdmin()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            const string adminRole = "3";
            return userRole == adminRole;
        }

        public IActionResult OnGet()
        {
            if (!CheckIsAdmin())
            {
                return new JsonResult(new { success = false, message = "Access denied" });
            }

            var articles = _newsArticleService.GetNewsArticles();

            articles = articles.Where(a => a.CreatedDate >= StartDate && a.CreatedDate <= EndDate).ToList();

            Statistics = articles
                .GroupBy(a => new { Year = a.CreatedDate.Year, Month = a.CreatedDate.Month, Category = a.Category.CategoryName })
                .Select(g => new StatisticsItem
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    CategoryName = g.Key.Category,
                    ArticleCount = g.Count()
                })
                .OrderByDescending(s => s.Period)
                .ToList();

            return Page();
        }

        public class StatisticsItem
        {
            public string Period { get; set; }
            public string CategoryName { get; set; }
            public int ArticleCount { get; set; }
        }
    }
}
