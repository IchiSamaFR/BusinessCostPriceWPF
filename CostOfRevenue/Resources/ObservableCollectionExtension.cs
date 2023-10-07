using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Resources
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> lst, IEnumerable<T> added)
        {
            if(added == null)
            {
                return lst;
            }
            foreach (var item in added)
            {
                lst.Add(item);
            }
            return lst;
        }
    }
}
