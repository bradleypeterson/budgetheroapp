using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Views
{

    public sealed partial class ExpensesView : Page
    {
        public ExpensesView()
        {
            this.InitializeComponent();
        }

        private void AddExpense_Btn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Submit_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditExpense_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteExpense_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        System.Collections.ObjectModel.ObservableCollection<Transaction> ExpensesData = new System.Collections.ObjectModel.ObservableCollection<Transaction>();
    }
}
