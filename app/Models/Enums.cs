namespace app.Models;

public enum ArtifactType
{
    Article,
    Code,
    Dataset,
    Note
}

public enum TaskStatus
{
    Backlog,
    Todo,
    InProgress,
    Done,
    Failed
}

public enum TaskPriority
{
    Low,
    Medium,
    High
}