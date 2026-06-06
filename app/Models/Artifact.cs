using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("artifacts")]
public class Artifact
{
    [Key]
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public ArtifactType Type { get; set; }
    public string Title { get; set; }
    public string Context { get; set; }
    public string RelativePath { get; set; }
    
    public Project? Project { get; set; }
    
    public ICollection<ExecutionLog> ExecutionLogs { get; set; } = new List<ExecutionLog>();
}