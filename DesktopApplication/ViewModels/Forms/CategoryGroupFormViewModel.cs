using CommunityToolkit.Mvvm.ComponentModel;
using ModelsLibrary;

namespace DesktopApplication.ViewModels.Forms;
public class CategoryGroupFormViewModel : ObservableRecipient
{
    private BudgetCategoryGroup _budgetCategoryGroup = new();
    public BudgetCategoryGroup BudgetCategoryGroup
    {
        get => _budgetCategoryGroup!;
        set
        {
            _budgetCategoryGroup= value;
            CategoryGroupDesc = _budgetCategoryGroup.CategoryGroupDesc;
        }
    }

    private string? _categoryGroupDesc;
    public string? CategoryGroupDesc
    {
        get => _categoryGroupDesc;
        set
        {
            SetProperty(ref _categoryGroupDesc, value);
            _budgetCategoryGroup.CategoryGroupDesc = _categoryGroupDesc!;
        }
    }
}
