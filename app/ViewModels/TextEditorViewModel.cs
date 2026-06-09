

using AvaloniaEdit;
using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace app.ViewModels;

public partial class TextEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private TextDocument _textDocument;

    public TextEditorViewModel(string relativePath)
    {
        _textDocument = new TextDocument();

        using (var writer = new StreamWriter(relativePath))
        {
            _textDocument.WriteTextTo(writer);
        }

        Debug.WriteLine(_textDocument);
    }
}