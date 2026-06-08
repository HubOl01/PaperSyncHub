using System;
using System.Globalization;
using app.Models;
using Avalonia.Data.Converters;
using Avalonia.Media;

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

public class PriorityToColorConverter : IValueConverter
{
    public static readonly PriorityToColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is TaskPriority p ? p switch
        {
            TaskPriority.High => new SolidColorBrush(Color.Parse("#FF4444")),
            TaskPriority.Medium => new SolidColorBrush(Color.Parse("#FFA500")),
            TaskPriority.Low => new SolidColorBrush(Color.Parse("#4CAF50")),
            _ => new SolidColorBrush(Colors.Black)
        } : new SolidColorBrush(Colors.Black);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => TaskPriority.Medium;
}
public class PriorityStringToColorConverter : IValueConverter
{
    public static readonly PriorityStringToColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string s ? s switch
        {
            "Высокий" => new SolidColorBrush(Color.Parse("#E53935")),
            "Средний" => new SolidColorBrush(Color.Parse("#FB8C00")),
            "Низкий" => new SolidColorBrush(Color.Parse("#43A047")),
            _ => new SolidColorBrush(Colors.Black)
        } : new SolidColorBrush(Colors.Black);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => "Средний";
}

public class StatusToColorConverter : IValueConverter
{
    public static readonly StatusToColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is TaskStatus s ? s switch
        {
            TaskStatus.InProgress => new SolidColorBrush(Color.Parse("#4F46E5")),
            TaskStatus.Todo => new SolidColorBrush(Color.Parse("#9E9E9E")),
            TaskStatus.Done => new SolidColorBrush(Color.Parse("#4CAF50")),
            TaskStatus.Failed => new SolidColorBrush(Color.Parse("#F44336")),
            _ => new SolidColorBrush(Colors.Black)
        } : new SolidColorBrush(Colors.Black);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => TaskStatus.Todo;
}