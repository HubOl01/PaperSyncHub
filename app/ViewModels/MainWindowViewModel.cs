using app.Models;
using app.Repositories;
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
        CurrentPage = new ProjectPageViewModel(project, this, ArtifactRepo, GitCommitRepo);
    }
}