using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using app.Models;
using CommunityToolkit.Mvvm.Input;
using Task = System.Threading.Tasks.Task;

namespace app.ViewModels;

public partial class ProjectListViewModel : ViewModelBase
{
    public ObservableCollection<Project> Projects { get; } = new();
    private readonly MainWindowViewModel _main;

    public ProjectListViewModel(List<Project> projects, MainWindowViewModel main)
    {
        _main = main;
        foreach (var p in projects.OrderByDescending(p => p.CreatedAt))
            Projects.Add(p);
    }

    [RelayCommand]
    private void OpenProject(Project project)
    {
        _main.NavigateToProject(project);
    }

    [RelayCommand]
    private async Task DeleteProject(Project project)
    {
        await _main.ProjectRepo.DeleteAsync(project);
        Projects.Remove(project);
    }

    [RelayCommand]
    private void GoBack()
    {
        _main.CurrentPage = new HomePageViewModel(_main);
    }
}