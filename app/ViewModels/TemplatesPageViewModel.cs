using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using app.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace app.ViewModels;

public partial class TemplatesPageViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;

    [ObservableProperty] private string _searchTerm = "";
    [ObservableProperty] private string _activeFilter = "all"; // all, blank, scientific, academic
    [ObservableProperty] private ArticleTemplate? _selectedTemplate;

    public ObservableCollection<ArticleTemplate> AllTemplates { get; } = new(new[]
    {
        new ArticleTemplate { Id=1, Title="ГОСТ Р 7.0.11-2011", Description="Диссертации и авторефераты", Format="blank", Icon="📘", Examples=["Диссертация","Автореферат"] },
        new ArticleTemplate { Id=2, Title="ГОСТ 7.32-2017",     Description="Отчёт о НИР",              Format="blank", Icon="📊", Examples=["НИР","Технический отчёт"] },
        new ArticleTemplate { Id=3, Title="APA 7th Edition",    Description="Психология, соц.науки",     Format="scientific", Icon="🧪", Examples=["Research Paper","Journal Article"] },
        new ArticleTemplate { Id=4, Title="IEEE Conference",    Description="Технические конференции",  Format="scientific", Icon="⚙️", Examples=["Conference Proceedings"] },
        new ArticleTemplate { Id=5, Title="Vancouver Style",    Description="Медицина и биология",      Format="scientific", Icon="🧬", Examples=["Medical Journal","Case Study"] },
        new ArticleTemplate { Id=6, Title="MLA 9th Edition",    Description="Гуманитарные науки",       Format="academic",   Icon="📖", Examples=["Essay","Literature Review"] },
        new ArticleTemplate { Id=7, Title="Harvard Style",      Description="Университетские работы",   Format="academic",   Icon="🎓", Examples=["Bachelor Thesis","Master Thesis"] },
        new ArticleTemplate { Id=8, Title="Chicago Style",      Description="История и социология",     Format="academic",   Icon="📜", Examples=["Book Chapter","Historical Paper"] },
    });

    public IEnumerable<ArticleTemplate> FilteredTemplates => AllTemplates
        .Where(t => (ActiveFilter == "all" || t.Format == ActiveFilter)
                 && (string.IsNullOrWhiteSpace(SearchTerm)
                     || t.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                     || t.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)));

    public TemplatesPageViewModel(MainWindowViewModel main) => _main = main;

    [RelayCommand] private void SetFilter(string filter) { ActiveFilter = filter; OnPropertyChanged(nameof(FilteredTemplates)); }
    [RelayCommand] private void SelectTemplate(ArticleTemplate t) => SelectedTemplate = t;
    [RelayCommand] private void ApplyTemplate(ArticleTemplate t) { /* TODO: применить к проекту */ }
    [RelayCommand] private void GoBack() => _main.CurrentPage = _main.LastProjectPage!;

    partial void OnSearchTermChanged(string _) => OnPropertyChanged(nameof(FilteredTemplates));
}