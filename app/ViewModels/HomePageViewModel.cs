using System;
using System.Threading.Tasks;
using app.Models;
using CommunityToolkit.Mvvm.Input;

namespace app.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    
    public HomePageViewModel(MainWindowViewModel main)
    {
        _main = main;
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
    private async Task OpenProject()
    {
        var projects = await _main.ProjectRepo.GetAllAsync();
        _main.CurrentPage = new ProjectListViewModel(projects, _main);
    }
}