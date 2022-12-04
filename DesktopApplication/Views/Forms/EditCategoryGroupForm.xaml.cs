using CommunityToolkit.Common;
using DesktopApplication.Contracts.Views;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

namespace DesktopApplication.Views.Forms;

public sealed partial class EditCategoryGroupForm : Page, IDialogForm
{
    public EditCategoryGroupFormViewModel ViewModel { get; }

    private bool isValidEditGroup;
    private bool isValidEditName;
    private bool isValidAddItemName = true;
    private bool isValidAddItemAmount = true;
    private bool isValidRemoveItemCombo = true;
    private bool isValidEditItemcombo = true;
    private bool isValidEditItemName = true;
    private bool isValidEditItemAmount = true;

    public EditCategoryGroupForm()
    {
        ViewModel = App.GetService<EditCategoryGroupFormViewModel>();
        InitializeComponent();        
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
            decimal parsedVal;

            if (Decimal.TryParse(textBox.Text, out parsedVal))
            {
                ViewModel.CategoryItemBudgetAmt = Convert.ToDecimal(textBox.Text);
            }
            else
            {
                return;
            }
        }
    }

    public void ValidateForm()
    {
        ValidateEditGroupCombo();
        ValidateCatGroupName();
        ValidateAddItemName();
        ValidateAddItemAmount();
        ValidateRemoveItemCombo();
        ValidateEditItemCombo();
        ValidateEditItemName();
        ValidateEditItemAmount();
    }

    private void ValidateEditGroupCombo()
    {
        if(EditGroupCombo.SelectedIndex == -1)
        {
            EditCatGroupComboError.Visibility = Visibility.Visible;
            isValidEditGroup = false;
        }
        else
        {
            EditCatGroupComboError.Visibility= Visibility.Collapsed;
            isValidEditGroup = true;
        }
    }
    private void ValidateCatGroupName()
    {
        if(GroupNameText.Text == string.Empty)
        {
            EditCatGroupNameError.Visibility = Visibility.Visible;
            isValidEditName = false;
        }
        else
        {
            EditCatGroupNameError.Visibility= Visibility.Collapsed;
            isValidEditName = true;
        }
    }
    private void ValidateAddItemName()
    {
        if(AddCatRadio.IsChecked == true)
        {
            if(CatItemText.Text == string.Empty)
            {
                AddItemNameError.Visibility = Visibility.Visible;
                isValidAddItemName = false;
            }
            else
            {
                AddItemNameError.Visibility= Visibility.Collapsed;
                isValidAddItemName = true;
            }
        }
        else
        {
            isValidAddItemName = true;
        }
    }
    private void ValidateAddItemAmount()
    {
        if (AddCatRadio.IsChecked == true)
        {
            decimal parsedVal;

            if (CatAmountText.Text == string.Empty)
            {
                AddItemAmountError.Visibility = Visibility.Visible;
                AddItemAmountError.Text = "Amount Cannot be Empty";
                isValidAddItemAmount = false;
            }
            else if(Decimal.TryParse(CatAmountText.Text, out parsedVal))
            {
                AddItemAmountError.Visibility = Visibility.Collapsed;
                isValidAddItemAmount = true;
            }
            else
            {
                AddItemAmountError.Visibility = Visibility.Visible;
                AddItemAmountError.Text = "Must be a Number";
                isValidAddItemAmount = false;
            }
        }
        else
        {
            isValidAddItemAmount = true;
        }
    }
    private void ValidateRemoveItemCombo()
    {
        if(RemoveCatRadio.IsChecked == true)
        {
            if(RemoveItemCombo.SelectedIndex == -1)
            {
                RemoveItemError.Visibility= Visibility.Visible;
                isValidRemoveItemCombo = false;
            }
            else
            {
                RemoveItemError.Visibility= Visibility.Collapsed;
                isValidRemoveItemCombo = true;
            }
        }
        else
        {
            isValidRemoveItemCombo = true;
        }
    }
    private void ValidateEditItemCombo()
    {
        if(EditCatItemRadio.IsChecked == true)
        {
            if (EditItemCombo.SelectedIndex == -1)
            {
                EditItemComboError.Visibility= Visibility.Visible;
                isValidEditItemcombo= false;
            }
            else
            {
                EditItemComboError.Visibility= Visibility.Collapsed;
                isValidEditItemcombo = true;
            }
        }
        else
        {
            isValidEditItemcombo= true;
        }
    }
    private void ValidateEditItemName()
    {
        if (EditCatItemRadio.IsChecked == true)
        {
            if (EditCatItemText.Text == string.Empty)
            {
                EditItemNameError.Visibility= Visibility.Visible;
                isValidEditItemName = false;
            }
            else
            {
                EditItemNameError.Visibility= Visibility.Collapsed;
                isValidEditItemName = true;
            }
        }
        else
        {
            isValidEditItemName= true;
        }
    }
    private void ValidateEditItemAmount()
    {
        if (EditCatItemRadio.IsChecked == true)
        {
            decimal parsedVal;

            if (EditCatItemAmt.Text == string.Empty)
            {
                EditItemAmountError.Visibility = Visibility.Visible;
                EditItemAmountError.Text = "Amount Cannot be Empty";
                isValidEditItemAmount = false;
            }
            else if (Decimal.TryParse(EditCatItemAmt.Text, out parsedVal))
            {
                EditItemAmountError.Visibility = Visibility.Collapsed;
                isValidEditItemAmount = true;
            }
            else
            {
                EditItemAmountError.Visibility = Visibility.Visible;
                EditItemAmountError.Text = "Must be a Number";
                isValidEditItemAmount = false;
            }
        }
        else
        {
            isValidEditItemAmount = true;
        }
    }

    public bool IsValidForm() => isValidEditGroup && isValidEditName && isValidAddItemName && isValidAddItemAmount && 
        isValidRemoveItemCombo && isValidEditItemcombo && isValidEditItemName && isValidEditItemAmount;

    public void SetModel(object model)
    {
        ViewModel.SelectedCategoryGroup = (BudgetCategoryGroup)model;
    }
}
