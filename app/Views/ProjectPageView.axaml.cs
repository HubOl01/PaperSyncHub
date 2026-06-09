using app.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace app.Views;

public partial class ProjectPageView : UserControl
{
    public ProjectPageView()
    {
        InitializeComponent();
    }
    private void ListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is ProjectPageViewModel vm && vm.SelectedArtifact != null)
            vm.SelectArtifactCommand.Execute(vm.SelectedArtifact.Id);
    }
}