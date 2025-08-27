using DAL.Models;

namespace DAL.Repositories;

public interface INewsArticleRepository
{
    public IEnumerable<NewsArticle> GetNewsArticles();

    public NewsArticle GetNewsArticleById(int id);

    public NewsArticle CreateNewsArticle(NewsArticle newsArticle);

    public void DeleteNewsArticle(int id);

    public NewsArticle? UpdateNewsArticle(NewsArticle newsArticle);


}