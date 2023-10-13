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
        private IEnumerable<Ingredient> _showedIngredients;

        [ObservableProperty]
        private IEnumerable<Ingredient> _allIngredients;

        [ObservableProperty]
        private string _nameToFind = string.Empty;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            AllIngredients = DataService.GetLastIngredients;
            _ingredientsBaseStock.Clear();
            AllIngredients.Foreach(i => _ingredientsBaseStock.Add(i.Id, i.StockQuantity));
            SearchByText();
        }

        public void OnNavigatedFrom()
        {
            Save();
        }

        private void InitializeViewModel()
        {
        }

        public void Save()
        {
            foreach (var ingredient in ShowedIngredients)
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

        [RelayCommand]
        public void SearchByText()
        {
            ShowedIngredients = AllIngredients.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
        }
    }
}
