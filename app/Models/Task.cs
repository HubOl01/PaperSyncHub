using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

[Table("tasks")]
public class Task
{
    [Key]
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatus Statuc { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Project? Project { get; set; }
}