using DesktopApplication.Helpers;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace DesktopApplication;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
