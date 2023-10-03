using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Models
{
    public struct Recipe
    {
        public string Id;
        public string Name;

        public List<RecipeIngredient> Ingredients;
    }
}
