using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using app.Repositories;
using app.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace app.ViewModels;

public partial class GitPanelViewModel : ViewModelBase
{
    private readonly int _projectId;
    private readonly GitService _gitService;
    private readonly GitCommitRepository _commitRepo;
    private readonly ArtifactRepository _artifactRepo;

    [ObservableProperty] private ObservableCollection<GitCommit> _commits = new();
    [ObservableProperty] private string _commitMessage = "";
    [ObservableProperty] private bool _isCommitDialogVisible;
    [ObservableProperty] private GitCommit? _selectedCommit;

    public GitPanelViewModel(int projectId, GitService gitService,
        GitCommitRepository commitRepo, ArtifactRepository artifactRepo)
    {
        _projectId = projectId;
        _gitService = gitService;
        _commitRepo = commitRepo;
        _artifactRepo = artifactRepo;
        _ = LoadCommitsAsync();
    }

    private async Task LoadCommitsAsync()
    {
        var all = await _commitRepo.GetAllAsync();
        var filtered = all.Where(c => c.ProjectId == _projectId)
            .OrderByDescending(c => c.CreatedAt)
            .ToList();
        Commits = new ObservableCollection<GitCommit>(filtered);
    }

    [RelayCommand]
    private void OpenCommitDialog()
    {
        CommitMessage = "";
        IsCommitDialogVisible = true;
    }

    [RelayCommand]
    private async Task ConfirmCommit()
    {
        if (string.IsNullOrWhiteSpace(CommitMessage)) return;
        await _gitService.CreateCommitAsync(_projectId, CommitMessage, "User");
        IsCommitDialogVisible = false;
        await LoadCommitsAsync();
    }

    [RelayCommand]
    private void CancelCommit() => IsCommitDialogVisible = false;

    // Откат — пока логируем, реальный rollback зависит от структуры данных
    [RelayCommand]
    private void RollbackTo(GitCommit commit)
    {
        SelectedCommit = commit;
        // TODO: восстановить снапшот артефактов из commit
    }
}