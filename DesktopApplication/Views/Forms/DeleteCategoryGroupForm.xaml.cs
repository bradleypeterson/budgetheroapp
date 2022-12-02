using DesktopApplication.Contracts.Views;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DeleteCategoryGroupForm : Page, IDialogForm
{
    public DeleteCategoryGroupFormViewModel ViewModel { get; }

    private bool isValidSelectedGroup;

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
        ValidateGroupToDelete();
    }

    private void ValidateGroupToDelete()
    {
        int sel = DeleteCategoryGroupCombo.SelectedIndex;

        if(sel == -1)
        {
            DeleteCategoryGroupError.Visibility = Visibility.Visible;
            isValidSelectedGroup = false;
        }
        else
        {
            DeleteCategoryGroupError.Visibility = Visibility.Collapsed;
            isValidSelectedGroup= true;
        }
    }

    public bool IsValidForm() => isValidSelectedGroup;


    public void SetModel(object model)
    {
        ViewModel.SelectedCategoryGroup = (BudgetCategoryGroup)model;
    }
}
