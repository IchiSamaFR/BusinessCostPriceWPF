using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using CostOfRevenue.Views.Pages.Dialogs;
using CostOfRevenue.Views.Pages.Ingredients;
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

namespace CostOfRevenue.ViewModels.Pages
{
    public partial class IngredientsVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        #region -- DialogBox --
        private string _modifiedId;

        [ObservableProperty]
        private List<Enums.Unit> _unitsType = new List<Enums.Unit>();

        [ObservableProperty]
        private Enums.Unit _selectedUnitType = Enums.Unit.kilogram;

        [ObservableProperty]
        private string _selectedName = string.Empty;

        [ObservableProperty]
        private decimal? _selectedPrice = null;
        #endregion

        #region -- RemoveDialogBox --
        [ObservableProperty]
        private IIngredient _removedIngredient;

        [ObservableProperty]
        private IEnumerable<Recipe> _removedFromRecipes;
        #endregion


        [ObservableProperty]
        private string _nameToFind = string.Empty;

        [ObservableProperty]
        private IEnumerable<Ingredient> _showedIngredients;

        private readonly IContentDialogService _contentDialogService;

        public IngredientsVM(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
        }


        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            UnitsType = new List<Enums.Unit>() { Enums.Unit.kilogram, Enums.Unit.liter, Enums.Unit.piece, Enums.Unit.dozen };

            ClearSelection();
            SearchByText();
        }

        private void ClearSelection()
        {
            NameToFind = string.Empty;
            SelectedUnitType = Enums.Unit.kilogram;
            SelectedName = string.Empty;
            SelectedPrice = null;
        }

        [RelayCommand]
        public async void AddIngredient()
        {
            _modifiedId = string.Empty;
            SelectedUnitType = Enums.Unit.kilogram;
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
                    DataService.Ingredients.Add(new Ingredient(Guid.NewGuid().ToString(), SelectedName, SelectedUnitType, SelectedPrice ?? 0, DateTime.Now));
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
        public async void UpdateIngredient(Ingredient ingredient)
        {
            _modifiedId = ingredient.Id;
            SelectedUnitType = ingredient.Unit;
            SelectedName = ingredient.Name;
            SelectedPrice = ingredient.UnitPrice;

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
                    if (ingredient.Date.Date == DateTime.Now.Date)
                    {
                        ingredient.Name = SelectedName;
                        ingredient.Unit = SelectedUnitType;
                        ingredient.UnitPrice = SelectedPrice ?? 0;
                    }
                    else
                    {
                        DataService.Ingredients.Add(new Ingredient(_modifiedId, SelectedName, SelectedUnitType, SelectedPrice ?? 0, DateTime.Now));
                    }
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
        public async void RemoveIngredient(Ingredient ingredient)
        {
            RemovedIngredient = ingredient;
            RemovedFromRecipes = DataService.GetLastRecipes.Where(r => r.Ingredients.Any(i => i.Id == ingredient.Id)).ToList();

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
                    DataService.Remove(ingredient);
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
            ShowedIngredients = DataService.GetLastIngredients.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
        }

    }
}
