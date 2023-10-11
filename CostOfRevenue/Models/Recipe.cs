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
        private DateTime _date;


        [ObservableProperty]
        private List<RecipeIngredient> _recipeIngredients;

        private float _price;
        [JsonIgnore]
        public float Price
        {
            get
            {
                return RecipeIngredients.Sum(r => r.Price);
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

        public Recipe(string id, string name, DateTime date)
        {
            _id = id;
            _name = name;
            _date = date.Date;
        }
        [JsonConstructor]
        public Recipe(string id, string name, DateTime date, IEnumerable<RecipeIngredient> ingredients) : this(id, name, date)
        {
            Id = id;
            Name = name;
            Date = date.Date;
            RecipeIngredients = ingredients?.ToList();
        }

        public void SetIngredients(IEnumerable<RecipeIngredient> recipeIngredients)
        {
            RecipeIngredients = recipeIngredients.ToList();
        }
    }
}
