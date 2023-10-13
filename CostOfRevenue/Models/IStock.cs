using CostOfRevenue.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Models
{
    public interface IStock
    {
        string IconString { get; }
        string Id { get; }
        string Name { get; }
        Enums.Unit Unit { get; }
        float UnitPrice { get; }
        float StockQuantity { get; }
    }
}
