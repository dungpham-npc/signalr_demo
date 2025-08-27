using DAL.Models;
using BusinessLayer.DTO;

namespace BusinessLayer.Services;
public interface INewsArticleService
{
    public IEnumerable<NewsArticleViewModel> GetNewsArticles();

    public NewsArticle GetNewsArticleById(int id);

    public NewsArticleViewModel GetNewsArticleViewModelById(int id);

    public NewsArticle CreateNewsArticle(NewsArticleDto dto);

    public NewsArticle UpdateNewsArticle(int id, NewsArticleDto dto);

    public void DeleteNewsArticle(int id);

    void AddTagToNewsArticle(int newsArticleId, int tagId);

    void RemoveTagFromNewsArticle(int newsArticleId, int tagId);

}