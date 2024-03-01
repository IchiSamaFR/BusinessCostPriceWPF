using BusinessCostPriceWPF.Models;
using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Pages.Dashboard
{
    public partial class StockVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private Dictionary<IStock, double> _ingredientsBaseStock = new Dictionary<IStock, double>();

        private ObservableCollection<Ingredient> _availableIngredients = new ObservableCollection<Ingredient>();
        private ObservableCollection<Furniture> _availableFurnitures = new ObservableCollection<Furniture>();

        public IEnumerable<IStock> ShowedStocks
        {
            get
            {
                return AllStocks.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
            }
        }

        private List<IStock> AllStocks
        {
            get
            {
                var tmp = new List<IStock>();
                _availableIngredients.Foreach(item => tmp.Add(item));
                _availableFurnitures.Foreach(item => tmp.Add(item));
                return tmp;
            }
        }

        [ObservableProperty]
        private string _nameToFind = string.Empty;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();

            InitData();
        }

        public void OnNavigatedFrom()
        {
            Save();
        }

        private void InitializeViewModel()
        {
        }

        private async void InitData()
        {
            _availableIngredients.Clear();
            _availableFurnitures.Clear();
            _availableIngredients.AddRange((await new APIService().GetIngredientsAsync(0)).Select(Ingredient.Build));
            _availableFurnitures.AddRange((await new APIService().GetFurnituresAsync(0)).Select(Furniture.Build));

            _ingredientsBaseStock.Clear();
            _availableIngredients.Foreach(item => _ingredientsBaseStock.Add(item, item.StockQuantity));
            _availableFurnitures.Foreach(item => _ingredientsBaseStock.Add(item, item.StockQuantity));
            SearchByText();
        }

        [RelayCommand]
        public async void Save()
        {
            try
            {
                foreach (var stock in ShowedStocks)
                {
                    var baseStock = _ingredientsBaseStock[stock];
                    if (stock.StockQuantity != baseStock)
                    {
                        if (stock is Ingredient)
                        {
                            var res = await new APIService().AddIngredientStockAsync(new IngredientStockInfoDTO()
                            {
                                IngredientId = stock.Id,
                                StockQuantity = stock.StockQuantity
                            });
                            _ingredientsBaseStock[stock] = res.StockQuantity;
                        }
                        else if (stock is Furniture)
                        {
                            var res = await new APIService().AddFurnitureStockAsync(new FurnitureStockInfoDTO()
                            {
                                FurnitureId = stock.Id,
                                StockQuantity = stock.StockQuantity
                            });
                            _ingredientsBaseStock[stock] = res.StockQuantity;
                        }
                    }
                }
            }
            catch (ApiException ex)
            {
                ExceptionService.ShowError("Erreur lors de la modification des stocks", ex.Response);
            }
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
                double total = 0;

                foreach (var ingredient in AllStocks)
                {
                    outputFile.WriteLine($"{ingredient.Name};{ingredient.StockQuantity.ToString("0.00")};{ingredient.UnitPrice.ToString("0.00")};{(ingredient.UnitPrice * ingredient.StockQuantity).ToString("0.00")}");
                    total += ingredient.UnitPrice * ingredient.StockQuantity;
                }
                outputFile.WriteLine(string.Empty);
                outputFile.WriteLine($";;Prix Total;{total.ToString("0.00")}");
            }
        }

        [RelayCommand]
        public void SearchByText()
        {
            OnPropertyChanged(nameof(ShowedStocks));
        }
    }
}
