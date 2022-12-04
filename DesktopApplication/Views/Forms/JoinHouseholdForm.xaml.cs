using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using DesktopApplication.Contracts.Views;
using ModelsLibrary;

// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms
{
    public sealed partial class JoinHouseholdForm : Page, IDialogForm
    {
        public JoinHouseholdFormViewmodel ViewModel { get; }
        
        private bool isValidJoinCode;

        public JoinHouseholdForm()
        {
            ViewModel = App.GetService<JoinHouseholdFormViewmodel>();
            InitializeComponent();
        }
        
        public void ValidateForm()
        {
            ValidateJoinCode();
        }

        private void ValidateJoinCode()
        {
            if (txtJoinCode.Text == string.Empty)
            {
                NoJoinCodeError.Visibility = Visibility.Visible;
                isValidJoinCode = false;
            }
            else
            {
                NoJoinCodeError.Visibility = Visibility.Collapsed;
                isValidJoinCode = true;
            }
        }

        public bool IsValidForm() => isValidJoinCode;

        public void SetModel(object model)
        {
            ViewModel.Budget = (Budget)model;
        }

    }
}
