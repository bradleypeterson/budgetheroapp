using CommunityToolkit.Mvvm.ComponentModel;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Forms
{
    public class JoinHouseholdFormViewmodel : ObservableRecipient
    {
        private Budget _budget = new();
        public Budget Budget 
        {
            get => _budget!;
            set=> _budget = value;      
        }

        private string? _joinCode;
        public string JoinCode
        {
            get => _joinCode;
            set
            {
                SetProperty(ref _joinCode, value);
            }
        }
    }
}
