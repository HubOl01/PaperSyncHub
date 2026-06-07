using app.Models;
using app.Repositories;
using app.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace app.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;
    
    
    public readonly ProjectRepository ProjectRepo;
    public readonly ArtifactRepository ArtifactRepo;
    public readonly TaskRepository TaskRepo;
    public readonly BibliographyRepository BibliographyRepo;
    public readonly GitCommitRepository GitCommitRepo;
    public readonly ArtifactDependencyRepository ArtifactDependencyRepo;
    public readonly ExecutionLogRepository ExecutionLogRepo;
    public ProjectPageViewModel? LastProjectPage { get; private set; }
    public MainWindowViewModel(
        ProjectRepository projectRepository,
        ArtifactRepository artifactRepository,
        TaskRepository taskRepository,
        BibliographyRepository bibliographyRepository,
        GitCommitRepository gitCommitRepository,
        ArtifactDependencyRepository artifactDependencyRepository,
        ExecutionLogRepository executionLogRepository)
    {
        ProjectRepo = projectRepository;
        ArtifactRepo = artifactRepository;
        TaskRepo = taskRepository;
        BibliographyRepo = bibliographyRepository;
        GitCommitRepo = gitCommitRepository;
        ArtifactDependencyRepo = artifactDependencyRepository;
        ExecutionLogRepo = executionLogRepository;
        _currentPage = new HomePageViewModel(this);
    }
    
    public void NavigateToProject(Project project)
    {
        var gitService = new GitService(GitCommitRepo, ArtifactRepo);
        var vm = new ProjectPageViewModel(project, this, ArtifactRepo, GitCommitRepo, gitService);
        LastProjectPage = vm;
        CurrentPage = vm;
    }
    
    public void NavigateToTasks(int projectId)
    {
        CurrentPage = new TaskPageViewModel(projectId, TaskRepo, this);
    }
}