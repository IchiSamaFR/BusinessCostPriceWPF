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
        public IIngredient Ingredient
        {
            get
            {
                if(Id == "0")
                {
                    return DataService.DeletedIngredient;
                }
                return DataService.GetLastIIngredients.FirstOrDefault(i => i.Id == Id);
            }
        }

        public float Price
        {
            get
            {
                return Ingredient.UnitPrice * Quantity;
            }
        }

        [JsonConstructor]
        public RecipeIngredient(string id, float quantity)
        {
            _id = id;
            _quantity = quantity;
        }
        public RecipeIngredient(IIngredient ingredient)
        {
            _id = ingredient.Id;
            _quantity = 0;
        }
    }
}
