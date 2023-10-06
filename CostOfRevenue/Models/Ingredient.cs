using CostOfRevenue.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Models
{
    public partial class Ingredient : ObservableObject
    {
        [ObservableProperty]
        public string _id;
        [ObservableProperty]
        public string _name;
        [ObservableProperty]
        public Enums.Unit _unit;
        [ObservableProperty]
        public float _price;
        [ObservableProperty]
        public DateTime _date;
        [ObservableProperty]
        public float _quantity;

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
