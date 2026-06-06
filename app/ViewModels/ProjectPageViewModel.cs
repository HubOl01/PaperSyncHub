using System;
using System.Collections.ObjectModel;
using app.Models;
using app.Repositories;
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

    [ObservableProperty] private string _projectName;
    [ObservableProperty] private ObservableCollection<Artifact> _artifacts = new();
    [ObservableProperty] private ObservableCollection<GitCommit> _commits = new();

    public ProjectPageViewModel(Project project, MainWindowViewModel main,
        ArtifactRepository artifactRepo, GitCommitRepository gitCommitRepo)
    {
        _project = project;
        _main = main;
        _artifactRepo = artifactRepo;
        _gitCommitRepo = gitCommitRepo;
        _projectName = project.Name;
        
        _ = LoadDataAsync();
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
    private void ConfirmRename()
    {
        if (!string.IsNullOrWhiteSpace(NewProjectName))
            ProjectName = NewProjectName;
        IsRenamingProject = false;
    }
    
    [ObservableProperty] 
    private Artifact? _selectedArtifact;

    [ObservableProperty]
    private bool _isCommitsPanelVisible;

    [RelayCommand]
    private void ShowCommits()
    {
        IsCommitsPanelVisible = !IsCommitsPanelVisible;
    }
}