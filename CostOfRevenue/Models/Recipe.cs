using CostOfRevenue.Services;
using CostOfRevenue.ViewModels.Pages;
using Newtonsoft.Json;
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
        private List<RecipeIngredient> _recipeIngredients;


        [JsonIgnore]
        public List<Ingredient> Ingredients
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
            Date = date;
            RecipeIngredients = ingredients?.ToList();
        }

        public void SetIngredients(IEnumerable<RecipeIngredient> recipeIngredients)
        {
            RecipeIngredients = recipeIngredients.ToList();
        }
    }
}
