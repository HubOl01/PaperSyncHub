using System;
using System.Collections.ObjectModel;
using System.Linq;
using app.Models;
using app.Repositories;
using app.Services;
using app.Views;
using AvaloniaEdit;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Task = System.Threading.Tasks.Task;

namespace app.ViewModels;

public partial class ProjectPageViewModel : ViewModelBase
{
    private readonly Project _project;
    private readonly ArtifactRepository _artifactRepo;
    private readonly GitCommitRepository _gitCommitRepo;
    private readonly MainWindowViewModel _main;
    private readonly GitService _gitService;
    

    [ObservableProperty] private bool _isGitPanelVisible;
    [ObservableProperty] private GitPanelViewModel? _gitPanel;

    [ObservableProperty] private string _projectName;
    [ObservableProperty] private ObservableCollection<Artifact> _artifacts = new();
    [ObservableProperty] private ObservableCollection<GitCommit> _commits = new();

    public ProjectPageViewModel(Project project, MainWindowViewModel main,
        ArtifactRepository artifactRepo, GitCommitRepository gitCommitRepo, GitService gitService )
    {
        _project = project;
        _main = main;
        _artifactRepo = artifactRepo;
        _gitCommitRepo = gitCommitRepo;
        _projectName = project.Name;

        _ = LoadDataAsync();
        
        _gitService = gitService;
        GitPanel = new GitPanelViewModel(
            project.Id, gitService, gitCommitRepo, artifactRepo);
    }

    private async Task LoadDataAsync()
    {
        var artifacts = await _artifactRepo.GetByProjectIdAsync(_project.Id);
        Artifacts = new ObservableCollection<Artifact>(artifacts);
        var commits = await _gitCommitRepo.GetAllAsync(); // потом отфильтруй по проекту
        Commits = new ObservableCollection<GitCommit>(commits);
        
    }

    [RelayCommand]
    private async Task SaveProject()
    {
        _project.Name = ProjectName;
        _project.UpdatedAt = DateTime.UtcNow;
        await _main.ProjectRepo.UpdateAsync(_project);
        // Здесь же создавай GitCommit-снимок
    }

    [RelayCommand]
    private void GoBack()
    {
        _main.CurrentPage = new HomePageViewModel(_main);
    }

    [ObservableProperty] private bool _isRenamingProject;
    [ObservableProperty] private string _newProjectName = "";

    [RelayCommand]
    private void RenameProject()
    {
        NewProjectName = ProjectName;
        IsRenamingProject = true;
    }

    [RelayCommand]
    private void CancelRename()
    {
        IsRenamingProject = false;
    }

    [RelayCommand]
    private async Task ConfirmRename()
    {
        if (!string.IsNullOrWhiteSpace(NewProjectName))
        {
            ProjectName = NewProjectName;
            _project.Name = NewProjectName;
            _project.UpdatedAt = DateTime.UtcNow;
            await _main.ProjectRepo.UpdateAsync(_project);
        }

        IsRenamingProject = false;
    }

    [ObservableProperty] private Artifact? _selectedArtifact;

    [ObservableProperty] private bool _isCommitsPanelVisible;

    [RelayCommand]
    private void ShowTasks()
    {
        _main.NavigateToTasks(_project.Id);
    }
    
    [RelayCommand]
    private void ShowGitPanel()
    {
        var window = new GitPanelWindow
        {
            DataContext = GitPanel
        };
        // Получаем главное окно как owner
        if (Avalonia.Application.Current?.ApplicationLifetime 
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            window.ShowDialog(desktop.MainWindow!);
        }
    }

    [RelayCommand]
    private void SelectArtifact(int id)
    {
        SelectedArtifact = Artifacts.FirstOrDefault(Artifact => Artifact.Id == id);

        //To-do: сделать нормальную обработку для типа, пока только открывать редактор при открытии статьи
        if(SelectedArtifact != null && SelectedArtifact.Type == ArtifactType.Article)
        {
            var TextEditor = new TextEditor();

            TextEditor.Load(SelectedArtifact.RelativePath);
        }
    }
}