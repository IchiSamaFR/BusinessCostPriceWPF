using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace CostOfRevenue.ViewModels.Pages
{
    public partial class StockVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private Dictionary<string, float> _ingredientsBaseStock = new Dictionary<string, float>();

        [ObservableProperty]
        private IEnumerable<Ingredient> _ingredients;

        [ObservableProperty]
        private bool _isEditable;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            Ingredients = DataService.GetLastIngredients.ToList();
            _ingredientsBaseStock.Clear();
            Ingredients.Foreach(i => _ingredientsBaseStock.Add(i.Id, i.StockQuantity));
        }

        public void OnNavigatedFrom()
        {
            IsEditable = false;
        }

        private void InitializeViewModel()
        {
        }

        public void Save()
        {
            foreach (var ingredient in Ingredients)
            {
                var baseStock = _ingredientsBaseStock[ingredient.Id];
                if(ingredient.StockQuantity != baseStock && ingredient.Date != DateTime.Now.Date)
                {
                    DataService.Ingredients.Add(new Ingredient(ingredient.Id, ingredient.Name, ingredient.Unit, ingredient.UnitPrice, DateTime.Now, ingredient.StockQuantity));
                    ingredient.StockQuantity = baseStock;
                }
            }

            DataService.SaveIngredients();
        }
    }
}
