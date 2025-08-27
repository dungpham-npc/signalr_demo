namespace BusinessLayer.DTO;

public class NewsArticleDto
{
    public int NewsArticleId { get; set; }
    public string NewsTitle { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string NewsContent { get; set; } = string.Empty;
    public string? NewsSource { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public List<int> TagIds { get; set; } = [];

}