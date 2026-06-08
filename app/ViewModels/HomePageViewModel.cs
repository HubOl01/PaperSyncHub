using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using CommunityToolkit.Mvvm.Input;

namespace app.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    public ObservableCollection<Project> Projects { get; } = new();
    
    // Свойство для первого проекта
    public Project? FirstProject => Projects.OrderByDescending(p => p.CreatedAt).FirstOrDefault();
    
    public HomePageViewModel(MainWindowViewModel main)
    {
        _main = main;
        LoadProjects();
    }
    private async void LoadProjects()
    {
        var projects = await _main.ProjectRepo.GetAllAsync();
        foreach (var p in projects.OrderByDescending(p => p.CreatedAt))
            Projects.Add(p);
    }
    [RelayCommand]
    private void OpenProject(Project project)
    {
        _main.NavigateToProject(project);
    }

    [RelayCommand]
    private async Task CreateProject()
    {
        var project = new Project
        {
            Name = "Новый проект",
            LocalPath = "",
            GitCommitHash = "",
            CreatedAt = DateTime.UtcNow
        };
        await _main.ProjectRepo.AddAsync(project);
        _main.NavigateToProject(project);
    }

    [RelayCommand]
    private async Task OpenProjects()
    {
        var projects = await _main.ProjectRepo.GetAllAsync();
        _main.CurrentPage = new ProjectListViewModel(projects, _main);
    }
}