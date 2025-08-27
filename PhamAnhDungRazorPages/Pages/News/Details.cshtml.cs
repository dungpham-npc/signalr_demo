using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhamAnhDungRazorPages.Pages.News;

public class DetailsModel : PageModel
{
    private readonly INewsArticleService _newsArticleService;
    private readonly ITagService _tagService;

    public DetailsModel(INewsArticleService newsArticleService, ITagService tagService)
    {
        _newsArticleService = newsArticleService;
        _tagService = tagService;
    }

    public NewsArticleViewModel NewsArticle { get; set; }
    public List<Tag> Tags { get; set; }

    public IActionResult OnGet(int id)
    {
        NewsArticle = _newsArticleService.GetNewsArticleViewModelById(id);
        if (NewsArticle == null) return NotFound();

        Tags = _tagService.GetTags().ToList();
        return Page();
    }
}