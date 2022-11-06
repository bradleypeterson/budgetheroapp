using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ABI.Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EditCategoryGroupForm : Page
{
    public EditCategoryGroupForm()
    {
        this.InitializeComponent();
        GroupNameText.IsReadOnly = true;
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
                //TODO: Clear all fields in remove item panel
            }
            else if (radButton.Content.ToString() == "Remove Category Item")
            {
                RemoveItemPanel.Visibility = Visibility.Visible;
                AddItemPanel.Visibility = Visibility.Collapsed;
                //TODO: clear all fields in add item panel
            }
        }
    }

    private void EditCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var combo = sender as ComboBox;

        

        if (combo != null)
        {
           GroupNameText.IsReadOnly = false;
           GroupNameText.Text = (string)combo.SelectedItem;
        }
    }
}
