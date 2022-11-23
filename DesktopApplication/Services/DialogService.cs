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

    public bool IsEnabled { get; set; }

    public ContentDialog Dialog;

    public DialogService()
    {
        _root = MainWindowHelper.GetXamlRoot();
        IsEnabled= false;
        Dialog = new ContentDialog();
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

        Dialog = dialog;

        dialog.IsPrimaryButtonEnabled = false;

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

    //void IDialogService.ButtonEnabled(bool status)
    //{
    //    Dialog.IsPrimaryButtonEnabled = status;
    //}

}
