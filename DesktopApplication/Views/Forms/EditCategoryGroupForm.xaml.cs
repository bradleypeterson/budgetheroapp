using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EditCategoryGroupForm : Page
{
    public EditCategoryGroupFormViewModel ViewModel { get; }

    public EditCategoryGroupForm()
    {
        ViewModel = App.GetService<EditCategoryGroupFormViewModel>();
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
           GroupNameText.Text = ViewModel.SelectedCategoryGroup.CategoryGroupDesc.ToString();
        }
    }

    public void GetCategoryDescTextBox()
    {
        ViewModel.CategoryGroupDescText = GroupNameText.Text;
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }

    private void GroupNameText_TextChanged(object sender, TextChangedEventArgs e)
    {
        GetCategoryDescTextBox();
    }
}
