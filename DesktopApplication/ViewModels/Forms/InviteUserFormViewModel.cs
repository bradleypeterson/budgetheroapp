using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
