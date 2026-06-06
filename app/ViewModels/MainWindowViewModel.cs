using app.Repositories;

namespace app.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Hi ^_^ !";
    public string Greeting2 { get; } = "Hi :~) !";
    
    
    private readonly ProjectRepository _projectRepository;
    private readonly ArtifactRepository _artifactRepository;
    private readonly TaskRepository _taskRepository;
    private readonly BibliographyRepository _bibliographyRepository;
    private readonly GitCommitRepository _gitCommitRepository;
    private readonly ArtifactDependencyRepository _artifactDependencyRepository;
    private readonly ExecutionLogRepository _executionLogRepository;
    public MainWindowViewModel(
        ProjectRepository projectRepository,
        ArtifactRepository artifactRepository,
        TaskRepository taskRepository,
        BibliographyRepository bibliographyRepository,
        GitCommitRepository gitCommitRepository,
        ArtifactDependencyRepository artifactDependencyRepository,
        ExecutionLogRepository executionLogRepository)
    {
        _projectRepository = projectRepository;
        _artifactRepository = artifactRepository;
        _taskRepository = taskRepository;
        _bibliographyRepository = bibliographyRepository;
        _gitCommitRepository = gitCommitRepository;
        _artifactDependencyRepository = artifactDependencyRepository;
        _executionLogRepository = executionLogRepository;
    }
}