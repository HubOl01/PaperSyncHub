using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using app.Repositories;
using app.Services;
using Avalonia.Markup.Xaml;
using app.ViewModels;
using app.Views;
using Microsoft.EntityFrameworkCore;

namespace app;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var db = new ApplicationDbContext();
        // db.Database.EnsureCreated();
        db.Database.Migrate();
        

        var projectRepo = new ProjectRepository(db);
        var artifactRepo = new ArtifactRepository(db);
        var taskRepo = new TaskRepository(db);
        var bibliographyRepo = new BibliographyRepository(db);
        var gitCommitRepo = new GitCommitRepository(db);
        var artifactDependencyRepo = new ArtifactDependencyRepository(db);
        var executionLogRepo = new ExecutionLogRepository(db);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();

            mainWindow.DataContext = new MainWindowViewModel(
                projectRepo,
                artifactRepo,
                taskRepo,
                bibliographyRepo,
                gitCommitRepo,
                artifactDependencyRepo,
                executionLogRepo
            );

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}