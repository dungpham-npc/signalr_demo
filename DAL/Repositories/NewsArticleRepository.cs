using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace DAL.Repositories;

public class NewsArticleRepository : INewsArticleRepository
{
    private readonly FunewsmanagementContext _context;

    public NewsArticleRepository(FunewsmanagementContext context)
    {
        _context = context;
    }

    public IEnumerable<NewsArticle> GetNewsArticles()
    {
        return _context.NewsArticles
            .Where(n => n.NewsStatus == "active")
            .Include(n => n.Category)
            .Include(n => n.Tags)
            .ToList();
    }

    public NewsArticle GetNewsArticleById(int id)
    {
        return _context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.Tags)
            .FirstOrDefault(n => n.NewsArticleId == id)!;
    }

    public NewsArticle CreateNewsArticle(NewsArticle newsArticle)
    {
        _context.NewsArticles.Add(newsArticle);
        _context.SaveChanges();
        return newsArticle;
    }

    public void DeleteNewsArticle(int id)
    {
        var article = _context.NewsArticles.Find(id);
        if (article == null) return;
        article.NewsStatus = "inactive";
        article.ModifiedDate = DateTime.Now;
        _context.SaveChanges();
    }

    public NewsArticle? UpdateNewsArticle(NewsArticle newsArticle)
    {
        var existing = _context.NewsArticles.Find(newsArticle.NewsArticleId);
        if (existing == null) return null;

        _context.Entry(existing).CurrentValues.SetValues(newsArticle);
        _context.SaveChanges();
        return existing;
    }
}