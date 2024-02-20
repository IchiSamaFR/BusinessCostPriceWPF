using BusinessCostPriceWPF.Models;
using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using BusinessCostPriceWPF.Views.Pages.Dialogs;
using BusinessCostPriceWPF.Views.Pages.Ingredients;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Pages.Dashboard
{
    public partial class IngredientsVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        #region -- DialogBox --
        private int _modifiedId;

        [ObservableProperty]
        private List<Unit> _unitsType = new List<Unit>();

        [ObservableProperty]
        private Unit _selectedUnitType = Unit.Kilogram;

        [ObservableProperty]
        private string _selectedName = string.Empty;

        [ObservableProperty]
        private double? _selectedPrice = null;
        #endregion

        #region -- RemoveDialogBox --
        [ObservableProperty]
        private IngredientDTO _removedIngredient;

        [ObservableProperty]
        private ObservableCollection<RecipeDTO> _removedFromRecipes = new ObservableCollection<RecipeDTO>();
        #endregion


        [ObservableProperty]
        private string _nameToFind = string.Empty;

        [ObservableProperty]
        private ObservableCollection<IngredientDTO> _showedIngredients = new ObservableCollection<IngredientDTO>();

        [ObservableProperty]
        private ObservableCollection<IngredientPriceInfoDTO> _showedIngredientPrices = new ObservableCollection<IngredientPriceInfoDTO>();

        private readonly IContentDialogService _contentDialogService;

        public IngredientsVM(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
        }


        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
            InitDatas();
        }

        public void OnNavigatedFrom()
        {
        }


        private void InitializeViewModel()
        {
            UnitsType = new List<Unit>() { Unit.Kilogram, Unit.Liter, Unit.Piece, Unit.Dozen };

            ClearSelection();
            SearchByText();
        }

        private void ClearSelection()
        {
            NameToFind = string.Empty;
            SelectedUnitType = Unit.Kilogram;
            SelectedName = string.Empty;
            SelectedPrice = null;
        }

        private async void InitDatas()
        {
            ShowedIngredients.Clear();
            ShowedIngredients.AddRange(await new APIService().GetIngredientsAsync(0));
        }

        [RelayCommand]
        public async void AddIngredient()
        {
            _modifiedId = 0;
            SelectedUnitType = Unit.Kilogram;
            SelectedName = string.Empty;
            SelectedPrice = null;

            var content = new IngredientAddDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Ajouter un produit",
                    Content = content,
                    PrimaryButtonText = "Ajouter",
                    CloseButtonText = "Annuler",
                }
            );

            switch (result)
            {
                case ContentDialogResult.Primary:
                    var ing = await new APIService().AddIngredientAsync(new IngredientDTO()
                    {
                        Name = SelectedName,
                        Unit = SelectedUnitType,
                        UnitPrice = SelectedPrice ?? 0
                    });
                    ShowedIngredients.Add(ing);
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveIngredients();
            SearchByText();
        }

        [RelayCommand]
        public async void UpdateIngredient(IngredientDTO ingredient)
        {
            _modifiedId = ingredient.Id;
            SelectedUnitType = ingredient.Unit;
            SelectedName = ingredient.Name;
            SelectedPrice = ingredient.UnitPrice;

            ShowedIngredientPrices.Clear();
            ShowedIngredientPrices.AddRange(await new APIService().GetIngredientPriceDetailsAsync(ingredient.Id));

            var content = new IngredientAddDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Modifier un produit",
                    Content = content,
                    PrimaryButtonText = "Modifier",
                    CloseButtonText = "Annuler",
                }
            );

            switch (result)
            {
                case ContentDialogResult.Primary:
                    var ingPrice = await new APIService().AddIngredientPriceAsync(new IngredientPriceInfoDTO()
                    {
                        IngredientId = ingredient.Id,
                        UnitPrice = SelectedPrice ?? 0
                    });
                    if(ShowedIngredientPrices.LastOrDefault()?.Date.Date == DateTime.Today.Date)
                    {
                        ShowedIngredientPrices.LastOrDefault().UnitPrice = ingPrice.UnitPrice;
                    }
                    else
                    {
                        ShowedIngredientPrices.Add(ingPrice);
                    }
                    ingredient.UnitPrice = ingPrice.UnitPrice;
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveIngredients();
            SearchByText();
        }

        [RelayCommand]
        public async void RemoveIngredient(IngredientDTO ingredient)
        {
            RemovedIngredient = ingredient;
            //RemovedFromRecipes = DataService.GetLastRecipes.Where(r => r.Ingredients.Any(i => i.Id == ingredient.Id)).ToList();

            UserControl content = RemovedFromRecipes.Any() ? new RemoveIngredientWithRecipeDialog() : new RemoveIngredientDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Supprimer un ingrédient",
                    Content = content,
                    PrimaryButtonText = "Supprimer",
                    CloseButtonText = "Annuler",
                }
            );

            switch (result)
            {
                case ContentDialogResult.Primary:
                    await new APIService().RemoveIngredientAsync(ingredient.Id);
                    ShowedIngredients.Remove(RemovedIngredient);
                    //DataService.Remove(ingredient);
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveIngredients();
            SearchByText();
        }

        [RelayCommand]
        public void SearchByText()
        {
            //ShowedIngredients = DataService.GetLastIngredients.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
        }

    }
}
