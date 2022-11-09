using System.Diagnostics;
using DesktopApplication.Contracts.Services;
using DesktopApplication.CustomEventArgs;
using DesktopApplication.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Services;
public class DialogService : IDialogService
{
    private readonly XamlRoot _root;

    public event EventHandler<DialogServiceEventArgs>? OnSaved;

    public DialogService()
    {
        _root = MainWindowHelper.GetXamlRoot();
    }

    public async Task ShowDialogAsync<TForm>(string dialogTitle, object? model = null, bool isDeleting = false)
    {
        Page? dialogContent;

        if (model is not null)
        {
            dialogContent = DialogFormHelper.GetFormWithData(typeof(TForm), model, isDeleting);
        }
        else
        {
            dialogContent = (Page?)Activator.CreateInstance(typeof(TForm));
        }

        var dialog = BuildContentDialog(dialogTitle, dialogContent);

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary && dialogContent is not null)
        {
            OnSaved?.Invoke(this, new DialogServiceEventArgs(dialogContent));
        }
    }

    private ContentDialog BuildContentDialog(string dialogTitle, Page? dialogContent)
    {
        ContentDialog contentDialog = new()
        {
            XamlRoot = _root,
            Style = App.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = dialogTitle,
            PrimaryButtonText = "Save",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = dialogContent
        };

        return contentDialog;
    }
}
