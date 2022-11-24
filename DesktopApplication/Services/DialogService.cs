using DesktopApplication.Contracts.Services;
using DesktopApplication.Contracts.Views;
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

    public async Task ShowDialogAsync<TForm>(string dialogTitle, object? model = null)
    {
        IDialogForm? content;
        ContentDialog dialog;

        try
        {
            content = (IDialogForm?)Activator.CreateInstance(typeof(TForm));
        }
        catch (InvalidCastException ex)
        {
            throw new Exception($"The dialog form you are trying to show does not implement IDialogForm >>> {ex.Message}");
        }

        if (content is not null)
        {
            if (model is not null)
                content.SetModel(model);

            dialog = BuildContentDialog(dialogTitle, content);
            await ShowDialogAsync(dialog, content);
        }
    }

    private async Task ShowDialogAsync(ContentDialog dialog, IDialogForm content)
    {
        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary && content is not null)
        {
            content.ValidateForm();

            if (content.IsValidForm())
                OnSaved?.Invoke(this, new DialogServiceEventArgs(content));
            else
                await ShowDialogAsync(dialog, content);
        }
    }

    private ContentDialog BuildContentDialog(string dialogTitle, IDialogForm dialogContent)
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
