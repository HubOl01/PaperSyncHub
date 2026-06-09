namespace app.Models;

public class ArticleTemplate
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Format { get; set; } = ""; // "blank", "scientific", "academic"
    public string Icon { get; set; } = "";
    public string[] Examples { get; set; } = [];
}