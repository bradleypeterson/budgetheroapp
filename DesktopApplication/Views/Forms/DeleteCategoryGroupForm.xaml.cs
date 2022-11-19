using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DeleteCategoryGroupForm : Page
{
    public DeleteCategoryGroupFormViewModel ViewModel { get; }

    public DeleteCategoryGroupForm()
    {
        ViewModel = App.GetService<DeleteCategoryGroupFormViewModel>();
        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
}
