using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Application.Classes
{
    public class CategoryGroup
    {
        private string categoryGroupName;
        public string CategoryGroupName
        {
            get { return categoryGroupName; }
            set { categoryGroupName = value; }
        }

        private int numberOfCategories;
        public int NumberOfCategories
        {
            get { return numberOfCategories; }
            set { numberOfCategories = value; }
        }

        public int GetNumberOfCategories()
        {
            return 0;
        }

        public static ObservableCollection<CategoryGroup> GetCategories()
        {
            var categoryGroups = new ObservableCollection<CategoryGroup>();

            return categoryGroups;
        }
        



    }
}
