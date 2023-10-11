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
        private float _price;
        [ObservableProperty]
        private DateTime _date;

        public Ingredient(string id, string name, Enums.Unit unit, float price, DateTime date)
        {
            _id = id;
            _name = name;
            _unit = unit;
            _price = price;
            _date = date.Date;
        }
    }
}
