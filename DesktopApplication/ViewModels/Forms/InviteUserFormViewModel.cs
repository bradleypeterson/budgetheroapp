using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopApplication.ViewModels.Forms
{
    public class InviteUserFormViewModel : ObservableRecipient
    {
        private string? _emailAddress;
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                SetProperty(ref _emailAddress, value);
            }
        }
    }
}
