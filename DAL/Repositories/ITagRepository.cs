using DAL.Models;

namespace DAL.Repositories;

public interface ITagRepository
{
    public IEnumerable<Tag> GetTags();

    public IEnumerable<Tag> GetTagsByNewsArticleId(int newsArticleId);

    public Tag GetTagById(int id);

    public Tag CreateTag(Tag tag);

    public void DeleteTag(int id);

    public Tag? UpdateTag(Tag tag);
    Tag GetTagByName(string tagName);
}