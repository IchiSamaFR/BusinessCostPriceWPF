using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using CostOfRevenue.Views.Pages.Ingredients;
using CostOfRevenue.Views.Pages.Recipes;
using System.Collections.ObjectModel;
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
        private Ingredient _selectedIngredient;

        [ObservableProperty]
        private IEnumerable<Ingredient> _allIngredients;

        [ObservableProperty]
        private ObservableCollection<Ingredient> _selectedRecipeIngredients = new ObservableCollection<Ingredient>();
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
            AllIngredients = DataService.Ingredients;

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
            SearchByText();
        }

        [RelayCommand]
        public async void UpdateRecipe(Recipe recipe)
        {
            _modifiedId = recipe.Id;
            SelectedName = recipe.Name;
            SelectedRecipeIngredients.Clear();
            SelectedRecipeIngredients.AddRange(recipe.Ingredients);

            var content = new RecipeAddDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Modifier une recette",
                    Content = content,
                    PrimaryButtonText = "Modifier",
                    SecondaryButtonText = "Supprimer",
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
                    DataService.Recipes.Remove(recipe);
                    break;
                case ContentDialogResult.None:
                default:
                    break;
            }
            SearchByText();
        }

        [RelayCommand]
        public async void AddIngredientToRecipe()
        {
            if (SelectedRecipeIngredients.Contains(SelectedIngredient))
            {
                return;
            }
            SelectedRecipeIngredients.Add(SelectedIngredient);
            SelectedIngredient = null;
        }

        [RelayCommand]
        public void RemoveIngredientToRecipe(Ingredient ingredient)
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
