using Microsoft.UI.Xaml;
using DesktopApplication.Contracts.Views;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AddCategoryGroupForm : Page, IDialogForm
{
    public CategoryGroupFormViewModel ViewModel { get; }

    private bool isValidCategoryName;

    public AddCategoryGroupForm()
    {
        ViewModel = App.GetService<CategoryGroupFormViewModel>();
        InitializeComponent();
    }

    public void ValidateForm()
    {
        ValidateGroupNameField();
    }

    private void ValidateGroupNameField()
    {
        if(CategoryGroupText.Text == string.Empty)
        {
            GroupNameError.Visibility = Visibility.Visible;
            isValidCategoryName = false;
        }
        else
        {
            GroupNameError.Visibility = Visibility.Collapsed;
            isValidCategoryName = true;
        }
    }

    public bool IsValidForm() => isValidCategoryName;

    public void SetModel(object model)
    {
        ViewModel.BudgetCategoryGroup = (BudgetCategoryGroup)model;
    }
}
