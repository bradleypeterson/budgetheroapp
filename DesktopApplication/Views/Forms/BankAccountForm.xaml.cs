using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Common;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class BankAccountForm : Page
{
    public BankAccountFormViewModel ViewModel { get; }
    private bool validInput;
    public BankAccountForm()
    {
        ViewModel = App.GetService<BankAccountFormViewModel>();
        InitializeComponent();
        ViewModel.OnValidForm += EnableButton;
        ViewModel.OnInvalidForm += DisableButton;
        if (txtAccountType.SelectedIndex == -1)
        {
            tbAccountTypeEmptyError.Visibility = Visibility.Visible;
            validInput = false;
            CheckValidInput();
        }
    }


    private void txtAccountName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(txtAccountName.Text == "")
        {
            tbAccountNameEmptyError.Visibility = Visibility.Visible;
            validInput = false;
            CheckValidInput();
        }
        else
        {
            tbAccountNameEmptyError.Visibility = Visibility.Collapsed;
            validInput = true;
            CheckValidInput();
        }
    }

    private void txtAccountBalance_TextChanged(object sender, TextChangedEventArgs e)
    {
        string balance = txtAccountBalance.Text;

        if (balance == "")
        {
            tbAccountBalanceEmptyError.Visibility = Visibility.Visible;
            tbAccountBalanceInvalidError.Visibility = Visibility.Collapsed;
            validInput = false;
            CheckValidInput();
        }
        else
        {
            tbAccountBalanceEmptyError.Visibility = Visibility.Collapsed;
            validInput = true;
            CheckValidInput();
            decimal tempBalance;
            if (decimal.TryParse(balance, out tempBalance))
            {
                tbAccountBalanceInvalidError.Visibility = Visibility.Collapsed;
                validInput = true;
                CheckValidInput();
            }
            else
            {
                tbAccountBalanceInvalidError.Visibility = Visibility.Visible;
                validInput = false;
                CheckValidInput();
            }
        }
    }

    private void txtAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        tbAccountTypeEmptyError.Visibility = Visibility.Collapsed;
        validInput = true;
        CheckValidInput();
    }

    private void CheckValidInput()
    {
        if (validInput)
        {
            //enable save button
            Debug.Write("valid form");
        }
        else
        {
            //disable save button
            Debug.Write("INVALID form");
        }
    }


    //Enable/Disable button event handlers are used to disable the button behind the scenes when the user hasn't touched the form at all (initial launch of dialog)
    private void EnableButton(object? sender, EventArgs e)
    {
        //enable button
    }

    private void DisableButton(object? sender, EventArgs e)
    {
        //disable button
    }
}
