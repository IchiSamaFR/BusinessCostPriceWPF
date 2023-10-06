using CostOfRevenue.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Models
{
    public partial class Recipe : ObservableObject
    {
        [ObservableProperty]
        private string _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        public List<Ingredient> _ingredients;

        public Recipe(string id, string name, DateTime date, List<Ingredient> ingredients)
        {
            _id = id;
            _name = name;
            _date = date.Date;
            _ingredients = ingredients;
        }
    }
}
