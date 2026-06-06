using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("projects")]
public class Project
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string LocalPath { get; set; }
    public string GitCommitHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<Artifact>? Artifacts { get; set; } = new List<Artifact>();
    public ICollection<Task>? Tasks { get; set; } = new List<Task>();
    public ICollection<Bibliography>? Bibliographies { get; set; } = new List<Bibliography>();
    public ICollection<GitCommit>? GitCommits { get; set; } = new List<GitCommit>();
    
}