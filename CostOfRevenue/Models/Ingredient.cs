using CostOfRevenue.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace CostOfRevenue.Models
{
    public partial class Ingredient : ObservableObject, IIngredient
    {
        [JsonIgnore]
        public string IconString { get; } = "DocumentOnePage24";

        [ObservableProperty]
        private string _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private Enums.Unit _unit;
        [ObservableProperty]
        private float _unitPrice;
        [ObservableProperty]
        private DateTime _date;
        [ObservableProperty]
        private float _stockQuantity;

        [JsonConstructor]
        public Ingredient(string id, string name, Enums.Unit unit, float unitPrice, DateTime date, float stockQuantity)
        {
            _id = id;
            _name = name;
            _unit = unit;
            _unitPrice = unitPrice;
            _date = date.Date;
            _stockQuantity = stockQuantity;
        }
        public Ingredient(string id, string name, Enums.Unit unit, float unitPrice, DateTime date)
        {
            _id = id;
            _name = name;
            _unit = unit;
            _unitPrice = unitPrice;
            _date = date.Date;
        }
        public Ingredient(string id, string name, Enums.Unit unit, float unitPrice)
        {
            _id = id;
            _name = name;
            _unit = unit;
            _unitPrice = unitPrice;
        }
    }
}
