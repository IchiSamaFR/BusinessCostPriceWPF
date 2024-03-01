using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCostPriceWPF.Models
{
    public interface IStock
    {
        int Id { get; }
        string Name { get; }
        Unit Unit { get; }
        double UnitPrice { get; }
        double StockQuantity { get; set; }
    }
}
