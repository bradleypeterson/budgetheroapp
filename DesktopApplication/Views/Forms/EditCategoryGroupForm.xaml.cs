using DesktopApplication.Contracts.Views;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EditCategoryGroupForm : Page, IDialogForm
{
    public EditCategoryGroupFormViewModel ViewModel { get; }

    public EditCategoryGroupForm()
    {
        ViewModel = App.GetService<EditCategoryGroupFormViewModel>();
        this.InitializeComponent();        
        GroupNameText.IsReadOnly = true;
        EditCatItemText.IsReadOnly = true;
        EditCatItemAmt.IsReadOnly = true;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadAsync();
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        var radButton = sender as RadioButton;

        if (radButton is not null)
        {
            if (radButton.Content.ToString() == "Add Category Item")
            {
                AddItemPanel.Visibility = Visibility.Visible;
                RemoveItemPanel.Visibility = Visibility.Collapsed;
                EditItemPanel.Visibility = Visibility.Collapsed;
                //Clear all fields in other panels
                EditCatItemText.Text = string.Empty;
                EditCatItemAmt.Text = string.Empty;
                SetCategoryRadStatus(radButton.Content.ToString());
            }
            else if (radButton.Content.ToString() == "Remove Category Item")
            {
                RemoveItemPanel.Visibility = Visibility.Visible;
                AddItemPanel.Visibility = Visibility.Collapsed;
                EditItemPanel.Visibility = Visibility.Collapsed;
                //clear all fields in other panels
                EditItemCombo.SelectedItem = null;
                CatAmountText.Text = string.Empty;
                CatItemText.Text = string.Empty;
                EditCatItemText.Text = string.Empty;
                EditCatItemAmt.Text = string.Empty;
                SetCategoryRadStatus(radButton.Content.ToString());
            }
            else if (radButton.Content.ToString() == "Edit Category Item")
            {
                EditItemPanel.Visibility = Visibility.Visible;
                AddItemPanel.Visibility = Visibility.Collapsed;
                RemoveItemPanel.Visibility= Visibility.Collapsed;
                CatAmountText.Text = string.Empty;
                CatItemText.Text = string.Empty;
                SetCategoryRadStatus(radButton.Content.ToString());
            }
        }
    }

    public void SetCategoryRadStatus(string status)
    {
        ViewModel.CategoryGroupRadStatus = status;
    }

    private void EditCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var combo = sender as ComboBox;

        if (combo != null)
        {
            GroupNameText.IsReadOnly = false;

            if (combo.SelectedItem != null)
            {
                GroupNameText.Text = ViewModel.SelectedCategoryGroup.CategoryGroupDesc.ToString();
            }
        }

        //Call viewmodel function to populate the appropriate items
        ViewModel.SetCategoriesToShow();
    }

    private void EditItemCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var combo = sender as ComboBox;

        if (combo != null)
        {
            EditCatItemText.IsReadOnly = false;
            EditCatItemAmt.IsReadOnly = false;
            EditCatItemText.Text = ViewModel.SelectedCategoryItem.CategoryName.ToString();
            EditCatItemAmt.Text = ViewModel.SelectedCategoryItem.CategoryAmount.ToString();
        }
    }

    private void GroupNameText_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.CategoryGroupDescText = GroupNameText.Text;
    }

    private void CatItemText_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;

        if (textBox != null)
        {
            if (textBox.Name == "CatItemText")
            {
                ViewModel.CategoryItemName = CatItemText.Text;
            }
            else if (textBox.Name == "EditCatItemText")
            {
                ViewModel.CategoryItemName = EditCatItemText.Text;
            }
        }
    }

    private void CatAmountText_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;

        if (textBox != null)
        {
            if (textBox.Name == "CatAmountText")
            {
                ViewModel.CategoryItemBudgetAmt = CatAmountText.Text;
            }
            else if (textBox.Name == "EditCatItemAmt")
            {
                ViewModel.CategoryItemBudgetAmt = EditCatItemAmt.Text;
            }
        }

        ViewModel.CategoryItemBudgetAmt = CatAmountText.Text;
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
