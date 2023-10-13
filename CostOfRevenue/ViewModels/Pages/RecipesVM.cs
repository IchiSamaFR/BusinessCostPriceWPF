using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using CostOfRevenue.Views.Pages.Dialogs;
using CostOfRevenue.Views.Pages.Ingredients;
using CostOfRevenue.Views.Pages.Recipes;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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
        private float _selectedQuantity = 0;

        [ObservableProperty]
        private float _selectedCharges = 0;
        
        [ObservableProperty]
        private List<Enums.Unit> _unitsType = new List<Enums.Unit>();

        [ObservableProperty]
        private Enums.Unit _selectedUnitType = Enums.Unit.kilogram;

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
            UnitsType = new List<Enums.Unit>() { Enums.Unit.kilogram, Enums.Unit.liter, Enums.Unit.piece, Enums.Unit.dozen };

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
            _modifiedId = string.Empty;
            SelectedName = string.Empty;
            SelectedQuantity = 0;
            SelectedUnitType = Enums.Unit.kilogram;
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
                    DataService.Recipes.Add(new Recipe(Guid.NewGuid().ToString(), SelectedName, DateTime.Now, SelectedQuantity, SelectedCharges, SelectedUnitType,  SelectedRecipeIngredients));
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
            SelectedQuantity = recipe.RecipeQuantity;
            SelectedCharges = recipe.Charges;
            SelectedUnitType = recipe.Unit;
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
                        recipe.RecipeQuantity = SelectedQuantity;
                        recipe.Charges = SelectedCharges;
                        recipe.Unit = SelectedUnitType;
                        recipe.SetIngredients(SelectedRecipeIngredients);
                    }
                    else
                    {
                        DataService.Recipes.Add(new Recipe(_modifiedId, SelectedName, DateTime.Now, SelectedQuantity, SelectedCharges, SelectedUnitType, SelectedRecipeIngredients));
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
            RemovedFromRecipes = DataService.GetLastRecipes.Where(r => r.Ingredients.Any(i => i.Id == recipe.Id)).ToList();

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
        public async void ExportRecipe(Recipe recipe)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Texte CSV|*.csv";
            saveFileDialog.Title = "Export de la recette";
            saveFileDialog.FileName = $"recette_{recipe.Name}_{DateTime.Now.ToString("dd-MM-yyyy")}.csv";
            saveFileDialog.ShowDialog();

            if(saveFileDialog.FileName == string.Empty)
            {
                return;
            }

            using (StreamWriter outputFile = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
            {
                outputFile.WriteLine($"Nom;Quantité;Prix Unitaire;Prix Totale");

                foreach (var recipeIngredient in recipe.RecipeIngredients)
                {
                    outputFile.WriteLine($"{recipeIngredient.Ingredient.Name};{recipeIngredient.Quantity};{recipeIngredient.Ingredient.UnitPrice};{recipeIngredient.Price}");
                }
                outputFile.WriteLine(string.Empty);
                outputFile.WriteLine($";;Prix;{recipe.RecipePriceNoFee}");
                outputFile.WriteLine($";;Charges;{recipe.Charges}%");
                outputFile.WriteLine($"{recipe.Name};{recipe.RecipeQuantity};{recipe.UnitPrice};{recipe.RecipePrice}");
            }
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
