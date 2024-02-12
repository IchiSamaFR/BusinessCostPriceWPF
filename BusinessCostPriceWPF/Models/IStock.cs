﻿using BusinessCostPriceWPF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCostPriceWPF.Models
{
    public interface IStock
    {
        string IconString { get; }
        string Id { get; }
        string Name { get; }
        Enums.Unit Unit { get; }
        decimal UnitPrice { get; }
        DateTime Date { get; }
        float StockQuantity { get; set; }
    }
}