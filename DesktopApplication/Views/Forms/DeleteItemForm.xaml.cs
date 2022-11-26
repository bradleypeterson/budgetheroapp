using DesktopApplication.Contracts.Views;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.ViewModels.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DeleteItemForm : Page, IDialogForm
{
    public DeleteItemViewModel ViewModel { get; }

    public DeleteItemForm()
    {
        ViewModel = App.GetService<DeleteItemViewModel>();
        InitializeComponent();
    }

    public void ValidateForm() { }

    public bool IsValidForm() => true;

    public void SetModel(object model)
    {
        if (model is BankAccount)
        {
            BankAccount _model = (BankAccount)model;
            UpdateDeleteMessage(_model.BankName);
        }
        else if (model is Transaction)
        {
            Transaction _model = (Transaction)model;
            UpdateDeleteMessage(_model.TransactionPayee);
        }
    }

    private void UpdateDeleteMessage(string modelProperty)
    {
        ViewModel.Message = $"Are you sure you want to delete {modelProperty}?";
    }
}
