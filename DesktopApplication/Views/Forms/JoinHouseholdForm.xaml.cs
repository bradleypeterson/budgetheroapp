using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using DesktopApplication.Contracts.Views;
using ModelsLibrary;
using DesktopApplication.Contracts.Data;

// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms
{
    public sealed partial class JoinHouseholdForm : Page, IDialogForm
    {
        private readonly IDataStore _dataStore;
        private bool isValidJoinCode;

        public JoinHouseholdFormViewmodel ViewModel { get; }

        public JoinHouseholdForm()
        {
            ViewModel = App.GetService<JoinHouseholdFormViewmodel>();
            InitializeComponent();
            _dataStore = App.GetService<IDataStore>();
        }
        
        public void ValidateForm()
        {
            ValidateJoinCode();
        }

        public bool? CheckJoinCode(Guid joinCode)
        {
            Budget budget = _dataStore.Budget.Get(b => b.BudgetId == joinCode);

            if (budget == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ValidateJoinCode()
        {
            
            if (txtJoinCode.Text == string.Empty)
            {
                NoJoinCodeError.Text = "Please Enter a Join Code";
                NoJoinCodeError.Visibility = Visibility.Visible;
                isValidJoinCode = false;
            }
            else
            {
                if(CheckJoinCode(ViewModel.JoinCode) == false)
                {
                    NoJoinCodeError.Text = "Invalid Join Code";
                    NoJoinCodeError.Visibility = Visibility.Visible;
                    isValidJoinCode= false;
                }
                else
                {
                    //NoJoinCodeError.Visibility = Visibility.Collapsed;
                    //isValidJoinCode = true;
                    NoJoinCodeError.Text = "VALID Join Code";
                    NoJoinCodeError.Visibility = Visibility.Visible;
                    isValidJoinCode = false;
                }
            }
        }

        public bool IsValidForm() => isValidJoinCode;

        public void SetModel(object model)
        {
            ViewModel.Budget = (Budget)model;
        }

    }
}
