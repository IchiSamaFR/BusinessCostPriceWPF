﻿using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using CostOfRevenue.Views.Pages.Dialogs;
using CostOfRevenue.Views.Pages.Ingredients;
using CostOfRevenue.Views.Pages.Recipes;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace CostOfRevenue.ViewModels.Pages
{
    public partial class RecipesVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        #region -- DialogBox --
        private string _modifiedId;

        [ObservableProperty]
        private string _selectedName = string.Empty;

        [ObservableProperty]
        private IIngredient _selectedIngredient;

        [ObservableProperty]
        private IEnumerable<IIngredient> _allIngredients;

        [ObservableProperty]
        private ObservableCollection<RecipeIngredient> _selectedRecipeIngredients = new ObservableCollection<RecipeIngredient>();

        private IEnumerable<IIngredient> GetAllIngredients(Recipe recipe = null)
        {
            return DataService.GetLastIIngredients.Where(i =>
            {
                if (i is Recipe)
                {
                    var rec = (Recipe)i;
                    return !rec.Ingredients.Contains(recipe) && rec != recipe;
                }
                return true;
            });
        }
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
        private IEnumerable<Recipe> _showedRecipes;


        private readonly IContentDialogService _contentDialogService;

        public RecipesVM(IContentDialogService contentDialogService)
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
            ClearSelection();
            SearchByText();
        }

        private void ClearSelection()
        {
            NameToFind = string.Empty;
            SelectedName = string.Empty;
        }

        [RelayCommand]
        public async void AddRecipe()
        {
            AllIngredients = GetAllIngredients();
            AllIngredients = DataService.GetLastIIngredients;
            _modifiedId = string.Empty;
            SelectedName = string.Empty;

            SelectedIngredient = null;
            SelectedRecipeIngredients.Clear();

            var content = new RecipeAddDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Ajouter une recette",
                    Content = content,
                    PrimaryButtonText = "Ajouter",
                    CloseButtonText = "Annuler",
                }
            );

            switch (result)
            {
                case ContentDialogResult.Primary:
                    DataService.Recipes.Add(new Recipe(Guid.NewGuid().ToString(), SelectedName, DateTime.Now, SelectedRecipeIngredients));
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveRecipes();
            SearchByText();
        }

        [RelayCommand]
        public async void UpdateRecipe(Recipe recipe)
        {
            AllIngredients = GetAllIngredients(recipe);
            _modifiedId = recipe.Id;
            SelectedName = recipe.Name;
            SelectedRecipeIngredients.Clear();
            SelectedRecipeIngredients.AddRange(recipe.RecipeIngredients);

            var content = new RecipeAddDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Modifier une recette",
                    Content = content,
                    PrimaryButtonText = "Modifier",
                    CloseButtonText = "Annuler",
                }
            );

            switch (result)
            {
                case ContentDialogResult.Primary:
                    if (recipe.Date == DateTime.Now.Date)
                    {
                        recipe.Name = SelectedName;
                        recipe.SetIngredients(SelectedRecipeIngredients);
                    }
                    else
                    {
                        DataService.Recipes.Add(new Recipe(_modifiedId, SelectedName, DateTime.Now, SelectedRecipeIngredients));
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveRecipes();
            SearchByText();
        }

        [RelayCommand]
        public async void RemoveRecipe(Recipe recipe)
        {
            RemovedIngredient = recipe;
            RemovedFromRecipes = DataService.Recipes.Where(r => r.Ingredients.Any(i => i.Id == recipe.Id)).ToList();

            UserControl content = RemovedFromRecipes.Any() ? new RemoveIngredientWithRecipeDialog() : new RemoveIngredientDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Supprimer une recette",
                    Content = content,
                    PrimaryButtonText = "Supprimer",
                    CloseButtonText = "Annuler",
                }
            );

            switch (result)
            {
                case ContentDialogResult.Primary:
                    DataService.Remove(recipe);
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveRecipes();
            SearchByText();
        }

        [RelayCommand]
        public async void AddIngredientToRecipe()
        {
            if (SelectedIngredient == null || SelectedRecipeIngredients.Any(s => s.Id == SelectedIngredient.Id))
            {
                return;
            }
            SelectedRecipeIngredients.Add(new RecipeIngredient(SelectedIngredient));
            SelectedIngredient = null;
        }

        [RelayCommand]
        public void RemoveIngredientToRecipe(RecipeIngredient ingredient)
        {
            SelectedRecipeIngredients.Remove(ingredient);
        }

        [RelayCommand]
        public void SearchByText()
        {
            ShowedRecipes = DataService.GetLastRecipes.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
        }
    }
}
