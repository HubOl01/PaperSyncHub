using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("git_commits")]
public class GitCommit
{
    [Key]
    public string Id { get; set; }
    public int ProjectId { get; set; }
    public string CommitHash { get; set; }
    public string Message { get; set; }
    public string AuthorName { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Project? Project { get; set; }
}