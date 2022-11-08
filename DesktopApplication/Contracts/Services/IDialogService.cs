using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Services;
public interface IDialogService
{
    //when making new service start here by making a method of the service you want
    void ShowDialog();
    void AddExpenseDialog();
    void DeleteExpenseDialog();
    void EditExpenseDialog();
    void AddCategoryGroupDialog();
    void DeleteCategoryGroupDialog();
    void AddAccountDialog();
    void DeleteAccountDialog();
    void EditCategoryGroupDialog();
}
