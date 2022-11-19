using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Helpers
{
    public class CollectionUtilities
    {
        public static void LoadObservableCollection<T>(IEnumerable<T> newCollection, ObservableCollection<T> existingCollection)
        {
            if (newCollection is not null)
            {
                foreach (T item in newCollection)
                {
                    existingCollection.Add(item);
                }
            }
        }
    }
}
