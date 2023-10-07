using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
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
        private float? _selectedPrice = null;
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
            SelectedUnitType = UnitsType.FirstOrDefault();
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
            SearchByText();
        }

        [RelayCommand]
        public async void UpdateIngredient(Ingredient ingredient)
        {
            _modifiedId = ingredient.Id;
            SelectedUnitType = ingredient.Unit;
            SelectedName = ingredient.Name;
            SelectedPrice = ingredient.Price;

            var content = new IngredientAddDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Modifier un produit",
                    Content = content,
                    PrimaryButtonText = "Modifier",
                    SecondaryButtonText = "Supprimer",
                    CloseButtonText = "Annuler",
                }
            );

            switch (result)
            {
                case ContentDialogResult.Primary:
                    if (ingredient.Date == DateTime.Now.Date)
                    {
                        ingredient.Name = SelectedName;
                        ingredient.Unit = SelectedUnitType;
                        ingredient.Price = SelectedPrice ?? 0;
                    }
                    else
                    {
                        DataService.Ingredients.Add(new Ingredient(_modifiedId, SelectedName, SelectedUnitType, SelectedPrice ?? 0, DateTime.Now));
                    }
                    break;
                case ContentDialogResult.Secondary:
                    DataService.Ingredients.Remove(ingredient);
                    break;
                case ContentDialogResult.None:
                default:
                    break;
            }
            SearchByText();
        }

        [RelayCommand]
        public void SearchByText()
        {
            ShowedIngredients = DataService.GetLastIngredients.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
        }

    }
}
