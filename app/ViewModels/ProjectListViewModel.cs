using System.Collections.Generic;
using app.Models;

namespace app.ViewModels;

public partial class ProjectListViewModel : ViewModelBase
{
    public List<Project> Projects { get; }
    private readonly MainWindowViewModel _main;

    public ProjectListViewModel(List<Project> projects, MainWindowViewModel main)
    {
        Projects = projects;
        _main = main;
    }
}