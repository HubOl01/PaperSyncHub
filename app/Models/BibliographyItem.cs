using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("bibliography")]
public class BibliographyItem
{
    [Key]
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string? CitationKey { get; set; }
    public string? authors { get; set; }
    public string? title { get; set; }
    public int? Year { get; set; }
    public string? RawBibtex { get; set; }
    
    public Project? Project { get; set; }
}