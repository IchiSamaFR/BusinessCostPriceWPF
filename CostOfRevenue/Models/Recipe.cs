using CostOfRevenue.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Models
{
    public class Recipe
    {
        public string Id;
        public string Name;

        public List<Ingredient> Ingredients;
    }
}
