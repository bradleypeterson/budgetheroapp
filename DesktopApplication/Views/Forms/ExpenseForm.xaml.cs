using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ExpenseForm : Page
{
    public ExpenseForm()
    {
        this.InitializeComponent();
        ExpenseDate.Date = DateTime.Now;
        DepositDate.Date = DateTime.Now;
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        var radButton = sender as RadioButton;

        if (radButton is not null)
        {
            if (radButton.Content.ToString() == "Expense")
            {
                ExpensePanel.Visibility = Visibility.Visible;
               DepositPanel.Visibility = Visibility.Collapsed;
            }
            else if (radButton.Content.ToString() == "Deposit")
            {
                DepositPanel.Visibility = Visibility.Visible;
                ExpensePanel.Visibility = Visibility.Collapsed;
            }
        }
    }

}
