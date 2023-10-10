using CostOfRevenue.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Models
{
    public partial class RecipeIngredient : ObservableObject
    {
        [ObservableProperty]
        private string _id;

        [ObservableProperty]
        private float _quantity;

        [JsonIgnore]
        private Ingredient _ingredient;
        [JsonIgnore]
        public Ingredient Ingredient
        {
            get
            {
                if (_ingredient == null)
                {
                    _ingredient = DataService.GetLastIngredients.FirstOrDefault(i => i.Id == Id);
                }
                return _ingredient;
            }
        }

        [JsonConstructor]
        public RecipeIngredient(string id, float quantity)
        {
            _id = id;
            _quantity = quantity;
        }
        public RecipeIngredient(Ingredient ingredient)
        {
            _id = ingredient.Id;
            _quantity = 0;
        }
    }
}
