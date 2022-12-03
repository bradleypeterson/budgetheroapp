using DesktopApplication.Contracts.Views;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ModelsLibrary;

namespace DesktopApplication.Views.Forms
{
    public sealed partial class AddHouseholdBudgetItemForm : Page, IDialogForm
    {
        public AddHouseholdBudgetItemFormViewModel ViewModel { get; }

        private bool isValidItemName;
        private bool isValidItemAmount;
        private bool isValidSelectedUsers;
        
        public AddHouseholdBudgetItemForm()
        {
            ViewModel = App.GetService<AddHouseholdBudgetItemFormViewModel>();
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadAsync();
        }

        private void CatItemText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox != null)
            {
                    ViewModel.CatItemName = CatItemText.Text;
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
                    ViewModel.CatItemAmount = Convert.ToDecimal(textBox.Text);
                }
                else
                {
                    return;
                }
            }
        }
        
        private void UserSelectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;

            if (listView != null && listView.SelectedItems.Count > 0)
            {
                foreach ( var user in listView.SelectedItems )
                {
                    ViewModel.SelectedUsers.Add((User)user);
                }
            }
        }
        
        public void ValidateForm()
        {
            ValidateItemName();
            ValidateItemAmount();
            ValidateSelectedUsers();
        }

        private void ValidateItemName() 
        {
            if (CatItemText.Text == string.Empty)
            {
                AddItemNameError.Visibility = Visibility.Visible;
                isValidItemName = false;
            }
            else
            {
                AddItemNameError.Visibility = Visibility.Collapsed;
                isValidItemName = true;
            }

        }

        private void ValidateItemAmount() 
        {
            decimal parsedVal;

            if (CatAmountText.Text == string.Empty)
            {
                AddItemAmountError.Visibility = Visibility.Visible;
                AddItemAmountError.Text = "Amount Cannot be Empty";
                isValidItemAmount = false;
            }
            else if (Decimal.TryParse(CatAmountText.Text, out parsedVal))
            {
                AddItemAmountError.Visibility = Visibility.Collapsed;
                isValidItemAmount = true;
            }
            else
            {
                AddItemAmountError.Visibility = Visibility.Visible;
                AddItemAmountError.Text = "Must be a Number";
                isValidItemAmount = false;
            }

        }

        private void ValidateSelectedUsers() 
        {
            if(UserSelectList.SelectedItems.Count == 0)
            {
                SelectedUserListError.Visibility = Visibility.Visible;
                isValidSelectedUsers = false;
            }
            else
            {
                SelectedUserListError.Visibility = Visibility.Collapsed;
                isValidSelectedUsers = true;
            }
        }

        public bool IsValidForm() => isValidItemName && isValidItemAmount && isValidSelectedUsers;

        //TODO: Finish this method
        public void SetModel(object model)
        {
            throw new NotImplementedException();
        }

    }
}
