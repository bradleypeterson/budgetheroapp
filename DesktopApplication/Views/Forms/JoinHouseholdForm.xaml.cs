using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using DesktopApplication.Contracts.Views;
using ModelsLibrary;
using DesktopApplication.Contracts.Data;


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

        public bool? CheckJoinCode(string joinCode)
        {
            Guid code = new Guid();
            try
            {
                code = Guid.Parse(joinCode);
                Budget budget = _dataStore.Budget.Get(b => b.BudgetId == code);

                if (budget == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (FormatException e)
            {
                return false;
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
                    NoJoinCodeError.Visibility = Visibility.Collapsed;
                    isValidJoinCode = true;
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
