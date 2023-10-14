using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IEnumerable<IStock> _showedStocks;

        [ObservableProperty]
        private IEnumerable<IStock> _allStocks;

        [ObservableProperty]
        private string _nameToFind = string.Empty;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            AllStocks = DataService.GetLastIStock;
            _ingredientsBaseStock.Clear();
            AllStocks.Foreach(i => _ingredientsBaseStock.Add(i.Id, i.StockQuantity));
            SearchByText();
        }

        public void OnNavigatedFrom()
        {
            Save();
        }

        private void InitializeViewModel()
        {
        }

        [RelayCommand]
        public void Save()
        {
            foreach (var stock in ShowedStocks)
            {
                var baseStock = _ingredientsBaseStock[stock.Id];
                if(stock.StockQuantity != baseStock && stock.Date != DateTime.Now.Date)
                {
                    if(stock is Ingredient)
                    {
                        DataService.Ingredients.Add(new Ingredient(stock.Id, stock.Name, stock.Unit, stock.UnitPrice, DateTime.Now, stock.StockQuantity));
                    }
                    else if (stock is Furniture)
                    {
                        DataService.Furnitures.Add(new Furniture(stock.Id, stock.Name, stock.Unit, stock.UnitPrice, DateTime.Now, stock.StockQuantity));
                    }
                    stock.StockQuantity = baseStock;
                }
            }

            DataService.SaveIngredients();
        }

        [RelayCommand]
        public void Export()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Texte CSV|*.csv";
            saveFileDialog.Title = "Export de l'inventaire";
            saveFileDialog.FileName = $"inventaire_{DateTime.Now.ToString("dd-MM-yyyy")}.csv";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName == string.Empty)
            {
                return;
            }

            using (StreamWriter outputFile = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
            {
                outputFile.WriteLine($"Nom;Stock;Prix Unitaire;Prix Total");
                decimal total = 0;

                foreach (var ingredient in AllStocks)
                {
                    outputFile.WriteLine($"{ingredient.Name};{ingredient.StockQuantity};{ingredient.UnitPrice};{ingredient.UnitPrice * (decimal)ingredient.StockQuantity}");
                    total += ingredient.UnitPrice * (decimal)ingredient.StockQuantity;
                }
                outputFile.WriteLine(string.Empty);
                outputFile.WriteLine($";;Prix Total;{total}");
            }
        }

        [RelayCommand]
        public void SearchByText()
        {
            ShowedStocks = AllStocks.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
        }
    }
}
