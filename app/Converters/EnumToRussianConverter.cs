using System;
using System.Globalization;
using app.Models;
using Avalonia.Data.Converters;

namespace app.Converters;

public class PriorityToRussianConverter : IValueConverter
{
    public static readonly PriorityToRussianConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is TaskPriority p ? p switch
        {
            TaskPriority.High => "Высокий",
            TaskPriority.Medium => "Средний",
            TaskPriority.Low => "Низкий",
            _ => value.ToString()!
        } : value?.ToString() ?? "";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string s ? s switch
        {
            "Высокий" => TaskPriority.High,
            "Средний" => TaskPriority.Medium,
            "Низкий" => TaskPriority.Low,
            _ => TaskPriority.Medium
        } : TaskPriority.Medium;
}

public class StatusToRussianConverter : IValueConverter
{
    public static readonly StatusToRussianConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is TaskStatus s ? s switch
        {
            TaskStatus.Backlog => "Бэклог",
            TaskStatus.Todo => "Новая",
            TaskStatus.InProgress => "В процессе",
            TaskStatus.Done => "Выполнено",
            TaskStatus.Failed => "Провалено",
            _ => value.ToString()!
        } : value?.ToString() ?? "";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => TaskStatus.Todo;
}