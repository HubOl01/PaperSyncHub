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
    [ObservableProperty] private DateTimeOffset? _taskDueDate;
    [ObservableProperty] private TimeSpan _taskDueTime = new TimeSpan(12, 0, 0);

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
        ActiveTasks = new ObservableCollection<TaskItem>(
            tasks.Where(t => t.Status != TaskStatus.Done && t.Status != TaskStatus.Failed));
        DoneTasks = new ObservableCollection<TaskItem>(
            tasks.Where(t => t.Status == TaskStatus.Done || t.Status == TaskStatus.Failed));
    }

    [RelayCommand]
    private void OpenNewTask()
    {
        EditingTask = null;
        TaskTitle = "";
        TaskDescription = "";
        TaskPriority = TaskPriority.Medium;
        TaskDueDate = DateTimeOffset.Now;
        TaskDueTime = new TimeSpan(12, 0, 0);
        IsEditorVisible = true;
        TaskPriorityString = "Средний";
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
            TaskDueDate = new DateTimeOffset(task.DueDate.Value);
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
            TaskStatus.Backlog => TaskStatus.Todo,
            TaskStatus.Todo => TaskStatus.InProgress,
            TaskStatus.InProgress => TaskStatus.Done,
            TaskStatus.Done => TaskStatus.Backlog,
            _ => TaskStatus.Todo
        };
        task.UpdatedAt = DateTime.UtcNow;
        await _taskRepo.UpdateAsync(task);
        await LoadTasksAsync();
    }

    [RelayCommand]
    private void GoBack()
    {
        _main.CurrentPage = _main.LastProjectPage!;
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
}