using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("artifact_dependencies")]
public class ArtifactDependency
{
    [Key]
    public int Id { get; set; }
    public int SourceArtifactId { get; set; }
    public int TargetArtifactId { get; set; }
    public string? RelationType { get; set; }
    public bool IsDeprecated { get; set; }
    
    public Artifact? SourceArtifact { get; set; }
    public Artifact? TargetArtifact { get; set; }
}