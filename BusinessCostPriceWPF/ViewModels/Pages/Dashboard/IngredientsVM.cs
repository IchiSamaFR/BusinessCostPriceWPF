﻿using BusinessCostPriceAPI.Client.Models;
using BusinessCostPriceAPI.Client.Services;
using BusinessCostPriceWPF.Models;
using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Views.Pages.Dialogs;
using BusinessCostPriceWPF.Views.Pages.Ingredients;
using System.Collections.ObjectModel;
using System.Windows.Controls;
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
        private double _selectedPrice = 0;
        #endregion

        #region -- RemoveDialogBox --
        [ObservableProperty]
        private Ingredient _removedIngredient;

        [ObservableProperty]
        private ObservableCollection<Recipe> _removedFromRecipes = new ObservableCollection<Recipe>();
        #endregion


        [ObservableProperty]
        private string _nameToFind = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Ingredient> _ingredients = new ObservableCollection<Ingredient>();
        [ObservableProperty]
        private ObservableCollection<IngredientPriceInfoDTO> _ingredientPrices = new ObservableCollection<IngredientPriceInfoDTO>();

        public IEnumerable<Ingredient> ShowedIngredients
        {
            get
            {
                return Ingredients.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower())).OrderBy(i => i.Name);
            }
        }
        public IEnumerable<IngredientPriceInfoDTO> ShowedIngredientsPrices
        {
            get
            {
                return IngredientPrices.OrderBy(i => i.Date);
            }
        }

        private readonly IContentDialogService _contentDialogService;

        private IAPIService _apiService;
        public IngredientsVM(IContentDialogService contentDialogService, IAPIService service)
        {
            _contentDialogService = contentDialogService;
            _apiService = service;
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
        private async void InitDatas()
        {
            ReloadIngredients();
        }
        private async void ReloadIngredients()
        {
            Ingredients = new ObservableCollection<Ingredient>();
            Ingredients.CollectionChanged += (a, e) => SearchByText();
            Ingredients.AddRange((await _apiService.GetIngredientsAsync(0)).Select(Ingredient.Build));
        }

        private void ClearSelection()
        {
            NameToFind = string.Empty;
            SelectedUnitType = Unit.Kilogram;
            SelectedName = string.Empty;
            SelectedPrice = 0;
        }


        [RelayCommand]
        public async void AddIngredient()
        {
            _modifiedId = 0;
            SelectedUnitType = Unit.Kilogram;
            SelectedName = string.Empty;
            SelectedPrice = 0;

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
                    try
                    {
                        var addedIngredient = await _apiService.AddIngredientAsync(new IngredientDTO()
                        {
                            Name = SelectedName,
                            Unit = SelectedUnitType,
                            UnitPrice = SelectedPrice
                        });
                        Ingredients.Add(Ingredient.Build(addedIngredient));
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de l'ajout d'un ingrédient", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
        }

        [RelayCommand]
        public async void UpdateIngredient(Ingredient ingredient)
        {
            _modifiedId = ingredient.Id;
            SelectedUnitType = ingredient.Unit;
            SelectedName = ingredient.Name;
            SelectedPrice = ingredient.UnitPrice;

            IngredientPrices.Clear();
            IngredientPrices.AddRange(await _apiService.GetIngredientPriceInfosAsync(ingredient.Id));

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
                    try
                    {
                        var addedIngredient = await _apiService.UpdateIngredientAsync(new IngredientDTO()
                        {
                            Id = ingredient.Id,
                            Name = SelectedName,
                            UnitPrice = SelectedPrice,
                            Unit = SelectedUnitType
                        });
                        if (IngredientPrices.LastOrDefault()?.Date.Date == DateTime.Today.Date)
                        {
                            IngredientPrices.LastOrDefault().UnitPrice = addedIngredient.UnitPrice;
                        }
                        else
                        {
                            IngredientPrices.Add(new IngredientPriceInfoDTO()
                            {
                                Date = DateTime.Now.Date,
                                UnitPrice = addedIngredient.UnitPrice
                            });
                        }
                        ingredient.Fill(addedIngredient);
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de la modification d'un ingrédient", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
        }

        [RelayCommand]
        public async void RemoveIngredient(Ingredient ingredient)
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
                    try
                    {
                        await _apiService.RemoveIngredientAsync(ingredient.Id);
                        Ingredients.Remove(ingredient);
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de la suppression d'un ingrédient", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
        }

        [RelayCommand]
        public void SearchByText()
        {
            OnPropertyChanged(nameof(ShowedIngredients));
        }

    }
}
