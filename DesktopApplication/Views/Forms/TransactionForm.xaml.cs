using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TransactionForm : Page
{

    public TransactionFormViewModel ViewModel{ get; }

    public TransactionForm()
    {
        ViewModel = App.GetService<TransactionFormViewModel>();
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }

    private void addInvalidNumberError()
    {
        tbInvalidNumberError.Visibility = Visibility.Visible;
    }
    private void removeInvalidNumberError()
    {
        tbInvalidNumberError.Visibility = Visibility.Collapsed;
    }

    private void addInvalidAccountError()
    {
        tbInvalidAccountError.Visibility = Visibility.Visible;
    }
    private void removeInvalidAccountError()
    {
        tbInvalidAccountError.Visibility = Visibility.Collapsed;
    }

    private void addInvalidCategoryError()
    {
        tbInvalidCategoryError.Visibility = Visibility.Visible;
    }
    private void removeInvalidCategoryError()
    {
        tbInvalidCategoryError.Visibility = Visibility.Collapsed;
    }
    private void addInvalidPayeeError()
    {
        tbInvalidPayeeError.Visibility = Visibility.Visible;
    }
    private void removeInvalidPayeeError()
    {
        tbInvalidPayeeError.Visibility = Visibility.Collapsed;
    }


    //Validation
    private void TransactionPayeeTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (TransactionPayeeTextBox.Text.Equals(""))
        {
            addInvalidPayeeError();
        }
        else
        {
            removeInvalidPayeeError();
            validData();
        }
    }

    private void ExpenseAmount_TextChanged(object sender, TextChangedEventArgs e)
    {
        
        try
        {
            if (!Double.IsNaN(Decimal.ToDouble(Decimal.Parse(ExpenseAmount.Text))))
            {
                if (ExpenseAmount.Text.Split(".")[1].Length > 3)
                {
                    addInvalidNumberError();
                }
                else
                {
                    removeInvalidNumberError();
                    validData();
                }
            }
        }
        catch(Exception ex)
        {
            addInvalidNumberError();
        }
        
    }

    private void BankAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BankAccountComboBox.SelectedItem.ToString() == null)
        {
            addInvalidAccountError();
        }
        else
        {
            removeInvalidAccountError();
            validData();
        }
    }

    private void ExpenseCategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ExpenseCategoryCombo.SelectedItem.ToString() == null)
        {
            addInvalidCategoryError();
        }
        else
        {
            removeInvalidCategoryError();
            validData();
        }
    }

    private void validData()
    {
       if (TransactionPayeeTextBox.Text.Equals("")) return;
        try
        {
            if (Double.IsNaN(Decimal.ToDouble(Decimal.Parse(ExpenseAmount.Text))))
            {
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
        if (BankAccountComboBox.SelectedItem == null) return;
        if (ExpenseCategoryCombo.SelectedItem == null) return;

        //Enable button here
    }
}
