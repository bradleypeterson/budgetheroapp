using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Contracts.Views
{
    public interface IDialogForm
    {
        public void ValidateForm();
        public bool IsValidForm();
        public void SetModel(object model);
    }
}
