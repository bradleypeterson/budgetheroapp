
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;

namespace DesktopApplication.Views.Forms;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AddCategoryGroupForm : Page
{
    public CategoryGroupFormViewModel ViewModel { get; }
    public AddCategoryGroupForm()
    {
        ViewModel = App.GetService<CategoryGroupFormViewModel>();
        InitializeComponent();
    }
}
