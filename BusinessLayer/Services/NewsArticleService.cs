using DAL.Models;
using DAL.Repositories;
using BusinessLayer.DTO;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services;

public class NewsArticleService : INewsArticleService
{
    private readonly INewsArticleRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITagRepository _tagRepository;
    private readonly IAccountService _accountService;
    private readonly ICategoryService _categoryService;


    public NewsArticleService(INewsArticleRepository repository,
                            IHttpContextAccessor httpContextAccessor,
                            ITagRepository tagRepository,
                            IAccountService accountService,
                            ICategoryService categoryService)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _tagRepository = tagRepository;
        _accountService = accountService;
        _categoryService = categoryService;
    }

    public IEnumerable<NewsArticleViewModel> GetNewsArticles()
    {
        var articles = _repository.GetNewsArticles();

        return (from article in articles
            let author = _accountService.GetAccountById(article.CreatedById)
            let modifiedBy = article.UpdatedById.HasValue ? _accountService.GetAccountById(article.UpdatedById.Value) : null
            let category = _categoryService.GetCategoryById(article.CategoryId)
            select new NewsArticleViewModel
            {
                NewsArticleId = article.NewsArticleId,
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryId = article.CategoryId,
                CreatedById = article.CreatedById,
                NewsStatus = article.NewsStatus,
                CreatedDate = article.CreatedDate,
                ModifiedDate = article.ModifiedDate,
                UpdatedById = article.UpdatedById,
                Tags = article.Tags.ToList(),
                AuthorName = author?.AccountName ?? "Unknown",
                Category = category
            }).ToList();
    }

    public NewsArticleViewModel GetNewsArticleViewModelById(int id)
    {
        var article = GetNewsArticleById(id);
        var author = _accountService.GetAccountById(article.CreatedById);
        var modifiedBy = article.UpdatedById.HasValue ? _accountService.GetAccountById(article.UpdatedById.Value) : null;
        var category = _categoryService.GetCategoryById(article.CategoryId);

        return new NewsArticleViewModel
        {
            NewsArticleId = article.NewsArticleId,
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            NewsContent = article.NewsContent,
            NewsSource = article.NewsSource,
            CategoryId = article.CategoryId,
            CreatedById = article.CreatedById,
            NewsStatus = article.NewsStatus,
            CreatedDate = article.CreatedDate,
            ModifiedDate = article.ModifiedDate,
            UpdatedById = article.UpdatedById,
            Tags = article.Tags.ToList(),
            AuthorName = author?.AccountName ?? "Unknown",
            ModifiedByName = modifiedBy?.AccountName ?? "Unknown",
            Category = category
        };
    }

    public NewsArticle GetNewsArticleById(int id)
    {
        return _repository.GetNewsArticleById(id)
            ?? throw new KeyNotFoundException($"News article {id} not found");
    }

    public NewsArticle CreateNewsArticle(NewsArticleDto dto)
    {
        ValidateSession();

        var article = new NewsArticle
        {
            NewsTitle = dto.NewsTitle,
            Headline = dto.Headline,
            NewsContent = dto.NewsContent,
            NewsSource = dto.NewsSource,
            CategoryId = dto.CategoryId,
            CreatedById = GetAccountId()!.Value,
            NewsStatus = "active",
            CreatedDate = DateTime.Now,
            Tags = dto.TagIds.Select(id => _tagRepository.GetTagById(id)).ToList()

        };

        return _repository.CreateNewsArticle(article);
    }

    public NewsArticle UpdateNewsArticle(int id, NewsArticleDto dto)
    {
        ValidateSession();
        var existing = GetNewsArticleById(id);

        existing.NewsTitle = dto.NewsTitle;
        existing.Headline = dto.Headline;
        existing.NewsContent = dto.NewsContent;
        existing.NewsSource = dto.NewsSource;
        existing.CategoryId = dto.CategoryId;
        existing.UpdatedById = GetAccountId();
        existing.ModifiedDate = DateTime.Now;
        existing.Tags = dto.TagIds.Select(tagId => _tagRepository.GetTagById(tagId)).ToList();

        return _repository.UpdateNewsArticle(existing)
            ?? throw new InvalidOperationException("Update failed");
    }

    public void DeleteNewsArticle(int id)
    {
        ValidateSession();
        _repository.DeleteNewsArticle(id);
    }

    private void ValidateSession()
    {
        if (GetAccountId() == null)
        {
            throw new UnauthorizedAccessException("Not logged in or session expired");
        }
    }

    private int? GetAccountId() => _httpContextAccessor.HttpContext?.Session.GetInt32("AccountID");

    public void AddTagToNewsArticle(int newsArticleId, int tagId)
    {
        var article = GetNewsArticleById(newsArticleId);
        var tag = _tagRepository.GetTagById(tagId);

        if (article.Tags.Any(t => t.TagId == tagId)) return;
        article.Tags.Add(tag);
        _repository.UpdateNewsArticle(article);
    }

    public void RemoveTagFromNewsArticle(int newsArticleId, int tagId)
    {
        var article = GetNewsArticleById(newsArticleId);
        var tag = article.Tags.FirstOrDefault(t => t.TagId == tagId);

        if (tag == null) return;
        article.Tags.Remove(tag);
        _repository.UpdateNewsArticle(article);
    }


}