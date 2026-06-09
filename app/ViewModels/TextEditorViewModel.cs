

using System;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.IO;
using CommunityToolkit.Mvvm.Input;

namespace app.ViewModels;

public partial class TextEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private TextDocument _textDocument;
    private readonly Action? _onClose;
    
    

    [ObservableProperty]
    private string _markdownText = "";

    private readonly string _filePath;
    //
    // public TextEditorViewModel(string relativePath, Action? onClose = null)
    // {
    //     _onClose = onClose;
    //     _filePath = relativePath;
    //     _textDocument = new TextDocument();
    //
    //     if (File.Exists(relativePath))
    //     {
    //         var content = File.ReadAllText(relativePath);
    //         MarkdownText = content;
    //         _textDocument = new TextDocument(content);
    //     }
    // }
    public TextEditorViewModel(string relativePath, Action? onClose = null)
    {
        _filePath = relativePath;
        _onClose = onClose;
    
        var content = File.Exists(relativePath) 
            ? File.ReadAllText(relativePath) 
            : "";
    
        _textDocument = new TextDocument(content);
    }

    [RelayCommand]
    public void SaveFile()
    {
        File.WriteAllText(_filePath, TextDocument.Text);
    }
    
    [RelayCommand]
    public void CloseFile()
    {
        SaveFile(); // автосохранение при закрытии
        _onClose?.Invoke();
    }
    
}