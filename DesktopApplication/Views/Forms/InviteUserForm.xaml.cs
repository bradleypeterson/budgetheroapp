using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using DesktopApplication.Contracts.Views;
using ModelsLibrary;

namespace DesktopApplication.Views.Forms
{
    public sealed partial class InviteUserForm : Page, IDialogForm
    {
        public InviteUserFormViewModel ViewModel { get; }

        private bool isValidEmailAddress;
        
        public InviteUserForm()
        {
            ViewModel = App.GetService<InviteUserFormViewModel>();
            InitializeComponent();
        }
        
        public void ValidateForm()
        {
            ValidateEmail();
        }

        private void ValidateEmail()
        {
            if (txtInviteEmail.Text == string.Empty)
            {
                NoEmailerror.Visibility = Visibility.Visible;
                isValidEmailAddress = false;
            }
            else
            {
                NoEmailerror.Visibility = Visibility.Collapsed;
                isValidEmailAddress = true;
            }
        }

        public bool IsValidForm() => isValidEmailAddress;

        public void SetModel(object model)
        {
            throw new NotImplementedException();
        }
    }
}
