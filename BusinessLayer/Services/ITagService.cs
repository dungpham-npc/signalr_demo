using DAL.Models;

namespace BusinessLayer.Services;

public interface ITagService
{
    public IEnumerable<Tag> GetTags();

    public IEnumerable<Tag> GetTagsByNewsArticleId(int newsArticleId);

    public Tag GetTagByName(string tagName);

    public Tag GetTagById(int id);

    public Tag CreateTag(Tag tag);

    public void DeleteTag(int id);

    public Tag UpdateTag(Tag tag);
}