using DesktopApplication.Contracts.Views;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DeleteCategoryGroupForm : Page, IDialogForm
{
    public DeleteCategoryGroupFormViewModel ViewModel { get; }

    public DeleteCategoryGroupForm()
    {
        ViewModel = App.GetService<DeleteCategoryGroupFormViewModel>();
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadAsync();
    }

    public void ValidateForm()
    {
        // Refer to BankAccountForm.xaml.cs on how to implement this. - RO
    }

    public bool IsValidForm()
    {
        // Refer to BankAccountForm.xaml.cs on how to implement this. - RO
        return true;
    }

    public void SetModel(object model)
    {
        // Refer to BankAccountForm.xaml.cs on how to implement this. - RO
    }
}
