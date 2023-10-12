using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Resources
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> list, Action<T> func)
        {
            foreach (var item in list)
            {
                func?.Invoke(item);
            }
            return list;
        }
    }
}
