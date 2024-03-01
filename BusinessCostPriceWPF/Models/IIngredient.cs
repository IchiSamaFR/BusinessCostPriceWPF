using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.Models
{
    public interface IIngredient
    {
        int Id { get; }
        string Name { get; }
        Unit Unit { get; }
        double UnitPrice { get; }
    }
}
