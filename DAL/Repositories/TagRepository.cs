using DAL.Models;

namespace DAL.Repositories;

public class TagRepository : ITagRepository
{
    private readonly FunewsmanagementContext _context;

    public TagRepository(FunewsmanagementContext context)
    {
        _context = context;
    }

    public IEnumerable<Tag> GetTags()
    {
        return _context.Tags.ToList();
    }

    public IEnumerable<Tag> GetTagsByNewsArticleId(int newsArticleId)
    {
        return _context.NewsArticles
            .Where(n => n.NewsArticleId == newsArticleId)
            .SelectMany(n => n.Tags)
            .ToList();
    }

    public Tag GetTagById(int id)
    {
        return _context.Tags.Find(id)!;
    }

    public Tag GetTagByName(string tagName)
    {
        return _context.Tags.FirstOrDefault(t => t.TagName == tagName)!;
    }
    public Tag CreateTag(Tag tag)
    {
        _context.Tags.Add(tag);
        _context.SaveChanges();
        return tag;
    }

    public void DeleteTag(int id)
    {
        var tag = _context.Tags.Find(id);
        if (tag == null) return;
        _context.Tags.Remove(tag);
        _context.SaveChanges();
    }

    public Tag? UpdateTag(Tag tag)
    {
        var existing = _context.Tags.Find(tag.TagId);
        if (existing == null) return null;

        _context.Entry(existing).CurrentValues.SetValues(tag);
        _context.SaveChanges();
        return existing;
    }
}