using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TagService(ITagRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public IEnumerable<Tag> GetTags()
    {
        return _repository.GetTags();
    }

    public IEnumerable<Tag> GetTagsByNewsArticleId(int newsArticleId)
    {
        return _repository.GetTagsByNewsArticleId(newsArticleId);
    }

    public Tag GetTagById(int id)
    {
        return _repository.GetTagById(id)
               ?? throw new KeyNotFoundException($"Tag {id} not found");
    }

    public Tag GetTagByName(string tagName)
    {
        return _repository.GetTagByName(tagName);
    }

    public Tag CreateTag(Tag tag)
    {
        ValidateSession();
        var tagToCreate = new Tag
        {
            TagName = tag.TagName,
            Note = tag.Note
        };

        return _repository.CreateTag(tagToCreate);
    }

    public Tag UpdateTag(Tag tag)
    {
        ValidateSession();
        var existing = GetTagById(tag.TagId);

        existing.TagName = tag.TagName;
        existing.Note = tag.Note;

        return _repository.UpdateTag(existing)
               ?? throw new InvalidOperationException("Update failed");
    }

    public void DeleteTag(int id)
    {
        ValidateSession();
        _repository.DeleteTag(id);
    }

    private void ValidateSession()
    {
        if (GetAccountId() == null)
        {
            throw new UnauthorizedAccessException("Not logged in or session expired");
        }
    }

    private int? GetAccountId() => _httpContextAccessor.HttpContext?.Session.GetInt32("AccountID");
}