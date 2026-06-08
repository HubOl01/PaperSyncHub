using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using app.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskStatus = app.Models.TaskStatus;

namespace app.ViewModels;

public partial class TaskPageViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private readonly TaskRepository _taskRepo;
    private readonly int _projectId;

    [ObservableProperty] private ObservableCollection<TaskItem> _activeTasks = new();
    [ObservableProperty] private ObservableCollection<TaskItem> _doneTasks = new();
    [ObservableProperty] private bool _isEditorVisible;
    [ObservableProperty] private TaskItem? _editingTask;

    // Поля редактора
    [ObservableProperty] private string _taskTitle = "";
    [ObservableProperty] private string _taskDescription = "";
    [ObservableProperty] private TaskPriority _taskPriority = TaskPriority.Medium;
    [ObservableProperty] private DateTime? _taskDueDate;
    [ObservableProperty] private TimeSpan _taskDueTime = new TimeSpan(12, 0, 0);
    [ObservableProperty] private bool _isEditingExisting;
    
    public string EditorTitle => IsEditingExisting ? "Редактировать задачу" : "Новая задача";
    public string SaveButtonText => IsEditingExisting ? "Сохранить" : "Добавить задачу";

    public string[] PriorityOptions => new[] { "Высокий", "Средний", "Низкий" };

    private string _taskPriorityString = "Средний";
    public TaskPageViewModel(int projectId, TaskRepository taskRepo, MainWindowViewModel main)
    {
        _projectId = projectId;
        _taskRepo = taskRepo;
        _main = main;
        _ = LoadTasksAsync();
    }
    private async Task LoadTasksAsync()
    {
        var tasks = await _taskRepo.GetByProjectIdAsync(_projectId);
    
        var active = tasks
            .Where(t => t.Status != TaskStatus.Done && t.Status != TaskStatus.Failed)
            .OrderBy(t => t.Status switch
            {
                TaskStatus.InProgress => 0,
                TaskStatus.Todo => 1,
                _ => 2
            })
            .ThenBy(t => t.Priority switch
            {
                TaskPriority.High => 0,
                TaskPriority.Medium => 1,
                TaskPriority.Low => 2,
                _ => 3
            });

        ActiveTasks = new ObservableCollection<TaskItem>(active);
        DoneTasks = new ObservableCollection<TaskItem>(
            tasks
                .Where(t => t.Status == TaskStatus.Done || t.Status == TaskStatus.Failed)
                .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt));
    }

    [RelayCommand]
    private void OpenNewTask()
    {
        EditingTask = null;
        TaskTitle = "";
        TaskDescription = "";
        TaskPriority = TaskPriority.Medium;
        TaskDueDate = DateTime.Now;
        TaskDueTime = new TimeSpan(12, 0, 0);
        IsEditorVisible = true;
        TaskPriorityString = "Средний";
        IsEditingExisting = false;
    }

    [RelayCommand]
    private void EditTask(TaskItem task)
    {
        EditingTask = task;
        TaskTitle = task.Title;
        TaskDescription = task.Description;
        TaskPriority = task.Priority;
        if (task.DueDate.HasValue)
        {
            TaskDueDate = task.DueDate.Value;
            TaskDueTime = task.DueDate.Value.TimeOfDay;
        }
        IsEditorVisible = true;
        TaskPriorityString = task.Priority switch
        {
            TaskPriority.High => "Высокий",
            TaskPriority.Medium => "Средний",
            TaskPriority.Low => "Низкий",
            _ => "Средний"
        };
        IsEditingExisting = true;
    }

    [RelayCommand]
    private async Task SaveTask()
    {
        if (string.IsNullOrWhiteSpace(TaskTitle)) return;

        DateTime? due = null;
        if (TaskDueDate.HasValue)
            due = TaskDueDate.Value.Date + TaskDueTime;

        if (EditingTask == null)
        {
            var newTask = new TaskItem
            {
                ProjectId = _projectId,
                Title = TaskTitle,
                Description = TaskDescription,
                Priority = TaskPriority,
                Status = TaskStatus.Todo,
                DueDate = due,
                CreatedAt = DateTime.UtcNow
            };
            await _taskRepo.AddAsync(newTask);
        }
        else
        {
            EditingTask.Title = TaskTitle;
            EditingTask.Description = TaskDescription;
            EditingTask.Priority = TaskPriority;
            EditingTask.DueDate = due;
            EditingTask.UpdatedAt = DateTime.UtcNow;
            await _taskRepo.UpdateAsync(EditingTask);
        }

        IsEditorVisible = false;
        await LoadTasksAsync();
    }

    [RelayCommand]
    private void CancelEdit() => IsEditorVisible = false;

    [RelayCommand]
    private async Task DeleteTask(TaskItem task)
    {
        await _taskRepo.DeleteAsync(task);
        await LoadTasksAsync();
    }

    [RelayCommand]
    private async Task CycleStatus(TaskItem task)
    {
        task.Status = task.Status switch
        {
            TaskStatus.Todo => TaskStatus.InProgress,
            TaskStatus.InProgress => TaskStatus.Done,
            _ => TaskStatus.Todo
        };
        task.UpdatedAt = DateTime.UtcNow;
        await _taskRepo.UpdateAsync(task);
        await LoadTasksAsync();
    }

    [RelayCommand]
    private void GoBack()
    {
        _main.LastProjectPage!.CenterContent = null;
    }
    
    public string TaskPriorityString
    {
        get => _taskPriorityString;
        set
        {
            _taskPriorityString = value;
            TaskPriority = value switch
            {
                "Высокий" => TaskPriority.High,
                "Средний" => TaskPriority.Medium,
                "Низкий" => TaskPriority.Low,
                _ => TaskPriority.Medium
            };
            OnPropertyChanged();
        }
    }
    partial void OnIsEditingExistingChanged(bool value)
    {
        OnPropertyChanged(nameof(EditorTitle));
        OnPropertyChanged(nameof(SaveButtonText));
    }
}