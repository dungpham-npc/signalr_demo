
namespace DAL.Models
{
    public class NewsArticleViewModel
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string NewsContent { get; set; } = string.Empty;
        public string? NewsSource { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int CreatedById { get; set; }
        public string NewsStatus { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? UpdatedById { get; set; }
        public List<Tag> Tags { get; set; } = [];
        public string AuthorName { get; set; } = string.Empty;
        public string ModifiedByName { get; set; } = string.Empty;
    }
}