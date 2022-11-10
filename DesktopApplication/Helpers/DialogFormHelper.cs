using DesktopApplication.Models;
using DesktopApplication.ViewModels.Forms;
using DesktopApplication.Views.Forms;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

namespace DesktopApplication.Helpers;
public class DialogFormHelper
{
    public static Page GetFormWithData(Type formType, object model, bool isDeleting = false)
    {
        var form = new Page();

        if (formType == typeof(BankAccountForm))
        {
            var _model = (BankAccount)model;

            if (isDeleting)
            {
                var tempForm = new DeleteItemForm();
                tempForm.ViewModel.Message = $"Are you sure you want to delete '{_model.BankName}'";
                form = tempForm;
            }
            else
            {
                var tempForm = new BankAccountForm();
                tempForm.ViewModel.BankAccount = _model;
                form = tempForm;
            }
        } else if (formType == typeof(TransactionForm))
        {
            var _model = (Transaction)model;

            if (isDeleting)
            {
                var tempForm = new DeleteItemForm();
                tempForm.ViewModel.Message = $"Are you sure you want to delete '{_model.TransactionMemo}'";
                form = tempForm;
            }
            else
            {
                var tempForm = new TransactionForm();
                tempForm.ViewModel.ObservableTransaction = new ObservableTransaction(_model);
                form = tempForm;
            }
        }

        return form;
    }
}
