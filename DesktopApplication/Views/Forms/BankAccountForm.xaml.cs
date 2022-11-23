using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Common;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class BankAccountForm : Page
{
    public BankAccountFormViewModel ViewModel { get; }
    public BankAccountForm()
    {
        ViewModel = App.GetService<BankAccountFormViewModel>();
        InitializeComponent();
    }

    private void txtAccountName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(txtAccountName.Text == "")
        {
            tbAccountNameEmptyError.Visibility = Visibility.Visible;
        }
        else
        {
            tbAccountNameEmptyError.Visibility = Visibility.Collapsed;
        }
    }

    private void txtAccountBalance_TextChanged(object sender, TextChangedEventArgs e)
    {
        string balance = txtAccountBalance.Text;

        if (balance == "")
        {
            tbAccountBalanceEmptyError.Visibility = Visibility.Visible;
        }
        else
        {
            tbAccountBalanceEmptyError.Visibility = Visibility.Collapsed;
        }

        
        if (balance.Contains("."))
        {
            string[] decimalSplit = balance.Split(".");
            if (decimalSplit[1] is not null)
            {
                if (decimalSplit[1].Length > 2)
                {
                    tbAccountBalanceInvalidError.Visibility = Visibility.Visible;
                }
                else
                {
                    tbAccountBalanceInvalidError.Visibility = Visibility.Collapsed;
                }
            }
        }

    }
}
