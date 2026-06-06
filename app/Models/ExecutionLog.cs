using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("execution_logs")]
public class ExecutionLog
{
    [Key]
    public int Id { get; set; }
    public int ArtifactId { get; set; }
    public int DurationMs { get; set; }
    public int ExitCode { get; set; }
    public string Output { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Artifact? Artifact { get; set; }
}