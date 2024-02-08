using BusinessCostPriceWPF.Resources;
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
        string IconString { get; }
        string Id { get; }
        string Name { get; }
        Enums.Unit Unit { get; }
        decimal UnitPrice { get; }
    }
}
