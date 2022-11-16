using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;

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
}
