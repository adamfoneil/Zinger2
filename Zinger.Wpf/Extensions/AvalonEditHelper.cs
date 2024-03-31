using ICSharpCode.AvalonEdit;
using System.Windows;

namespace Zinger.Wpf.Extensions;

public static class AvalonEditHelper
{
    public static string GetText(DependencyObject obj)
    {
        return (string)obj.GetValue(TextProperty);
    }

    public static void SetText(DependencyObject obj, string value)
    {
        obj.SetValue(TextProperty, value);
    }

    // Using a DependencyProperty as the backing store for Text.
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(AvalonEditHelper),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextEditor editor)
        {
            if (e.NewValue != null)
            {
                editor.Document.Text = e.NewValue.ToString();
            }
            else
            {
                editor.Document.Text = "";
            }
        }
    }
}
