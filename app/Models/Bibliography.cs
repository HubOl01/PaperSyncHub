using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("bibliography")]
public class Bibliography
{
    [Key]
    public int id { get; set; }
    public int ProjectId { get; set; }
    public string? CitationKey { get; set; }
    public string? authors { get; set; }
    public string? title { get; set; }
    public int? Year { get; set; }
    public int? RawBibtex { get; set; }
    
    public Project? Project { get; set; }
}