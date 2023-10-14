using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using CostOfRevenue.ViewModels.Pages;
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
    public partial class Recipe : ObservableObject, IIngredient
    {
        [JsonIgnore]
        public string IconString { get; } = "Notebook24";

        [ObservableProperty]
        private string _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private Enums.Unit _unit;
        [ObservableProperty]
        private float _recipeQuantity;
        [ObservableProperty]
        private decimal _charges;
        [ObservableProperty]
        private DateTime _date;


        [ObservableProperty]
        private List<RecipeIngredient> _recipeIngredients;

        [JsonIgnore]
        public decimal RecipePriceNoFee
        {
            get
            {
                return RecipeIngredients.Sum(r => r.Price);
            }
        }
        [JsonIgnore]
        public decimal RecipePrice
        {
            get
            {
                return RecipePriceNoFee * (1 + (Charges / 100));
            }
        }
        [JsonIgnore]
        public decimal UnitPrice
        {
            get
            {
                return RecipePrice / (decimal)RecipeQuantity;
            }
        }
        [JsonIgnore]
        public List<IIngredient> Ingredients
        {
            get
            {
                return RecipeIngredients.Select(r => r.Ingredient).ToList();
            }
        }

        public Recipe(string id, string name, DateTime date, float recipeQuantity, decimal charges, Enums.Unit unit, IEnumerable<RecipeIngredient> ingredients)
        {
            _id = id;
            _name = name;
            _unit = unit;
            _recipeQuantity = recipeQuantity;
            _charges = charges;
            _date = date.Date;
            _recipeIngredients = ingredients?.ToList();
        }

        public void SetIngredients(IEnumerable<RecipeIngredient> recipeIngredients)
        {
            RecipeIngredients = recipeIngredients.ToList();
        }
    }
}
