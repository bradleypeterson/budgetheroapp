using System.Diagnostics;
using DesktopApplication.Contracts.Data;
using DesktopApplication.Data;
using DesktopApplication.Helpers;
using ModelsLibrary;

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
