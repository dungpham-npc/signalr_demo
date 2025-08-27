using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PhamAnhDungRazorPages.Hubs;

namespace PhamAnhDungRazorPages.Pages.News
{
    public class TagModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _newsHub;

        public TagModel(INewsArticleService newsArticleService, ITagService tagService, IHubContext<NewsHub> newsHub)
        {
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _newsHub = newsHub;
        }

        private bool CheckIsStaff()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            const string staffRole = "1";
            return userRole == staffRole;
        }

        public async Task<JsonResult> OnPostAddTagAsync(int newsArticleId, int? tagId, string tagName, string note)
        {
            if (!CheckIsStaff())
            {
                return new JsonResult(new { success = false, message = "Access denied" });
            }

            try
            {
                if (tagId.HasValue)
                {
                    _newsArticleService.AddTagToNewsArticle(newsArticleId, tagId.Value);
                }
                else if (!string.IsNullOrEmpty(tagName))
                {
                    var newTag = new Tag { TagName = tagName, Note = note };
                    _tagService.CreateTag(newTag);
                    var createdTag = _tagService.GetTagByName(tagName);
                    _newsArticleService.AddTagToNewsArticle(newsArticleId, createdTag.TagId);
                }

                await _newsHub.Clients.All.SendAsync("NewsUpdated", "tagAdded", new { NewsArticleId = newsArticleId });

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<JsonResult> OnPostRemoveTagAsync(int newsArticleId, int tagId)
        {
            if (!CheckIsStaff())
            {
                return new JsonResult(new { success = false, message = "Access denied" });
            }

            try
            {
                _newsArticleService.RemoveTagFromNewsArticle(newsArticleId, tagId);

                await _newsHub.Clients.All.SendAsync("NewsUpdated", "tagRemoved", new { NewsArticleId = newsArticleId });

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
