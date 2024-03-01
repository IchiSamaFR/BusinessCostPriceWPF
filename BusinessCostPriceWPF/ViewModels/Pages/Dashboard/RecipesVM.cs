using BusinessCostPriceWPF.Models;
using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using BusinessCostPriceWPF.Views.Pages.Dialogs;
using BusinessCostPriceWPF.Views.Pages.Ingredients;
using BusinessCostPriceWPF.Views.Pages.Recipes;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Pages.Dashboard
{
    public partial class RecipesVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private ObservableCollection<Ingredient> _availableIngredients = new ObservableCollection<Ingredient>();
        private ObservableCollection<Recipe> _availableRecipeIngredients = new ObservableCollection<Recipe>();

        #region -- DialogBox --
        private int _modifiedId;

        [ObservableProperty]
        private string _selectedName = string.Empty;

        [ObservableProperty]
        private double _selectedQuantity = 0;

        [ObservableProperty]
        private double _selectedCharges = 0;
        
        [ObservableProperty]
        private List<Unit> _unitsType = new List<Unit>();

        [ObservableProperty]
        private Unit _selectedUnitType = Unit.Kilogram;

        [ObservableProperty]
        private IIngredient _selectedIngredient;

        [ObservableProperty]
        private IEnumerable<IIngredient> _allIngredients;

        [ObservableProperty]
        private ObservableCollection<RecipeIngredientDTO> _selectedRecipeIngredients = new ObservableCollection<RecipeIngredientDTO>();
        private List<RecipeIngredientDTO> _baseSelectedRecipeIngredients = new List<RecipeIngredientDTO>();

        private IEnumerable<IIngredient> GetAllIngredients()
        {
            var tmp = new List<IIngredient>();
            tmp.AddRange(_availableIngredients.Where(avIng => !_selectedRecipeIngredients.Any(ing => ing.IngredientId == avIng.Id)));
            tmp.AddRange(_availableRecipeIngredients.Where(avIng => !_selectedRecipeIngredients.Any(ing => ing.IngredientRecipeId == avIng.Id) && avIng.Id != _modifiedId));
            return tmp;
        }
        #endregion

        #region -- RemoveDialogBox --
        [ObservableProperty]
        private IIngredient _removedIngredient;

        [ObservableProperty]
        private ObservableCollection<Recipe> _removedFromRecipes = new ObservableCollection<Recipe>();
        #endregion

        [ObservableProperty]
        private string _nameToFind = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>();

        public IEnumerable<Recipe> ShowedRecipes
        {
            get
            {
                return Recipes.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower())).OrderBy(i => i.Name);
            }
        }

        private readonly IContentDialogService _contentDialogService;

        public RecipesVM(IContentDialogService contentDialogService)
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

        private async void InitDatas()
        {
            ReloadRecipes();
            _availableIngredients.Clear();
            _availableIngredients.AddRange((await new APIService().GetIngredientsAsync(0)).Select(Ingredient.Build));
            _availableRecipeIngredients.Clear();
            _availableRecipeIngredients.AddRange((await new APIService().GetRecipesAsync(0)).Select(Recipe.Build));
        }
        private async void ReloadRecipes()
        {
            Recipes = new ObservableCollection<Recipe>();
            Recipes.CollectionChanged += (a, e) => SearchByText();
            Recipes.AddRange((await new APIService().GetRecipesAsync(0)).Select(Recipe.Build));
        }

        private void ClearSelection()
        {
            NameToFind = string.Empty;
            SelectedName = string.Empty;
        }

        [RelayCommand]
        public async void AddRecipe()
        {
            _modifiedId = 0;
            SelectedName = string.Empty;
            SelectedQuantity = 0;
            SelectedUnitType = Unit.Kilogram;

            SelectedIngredient = null;
            SelectedRecipeIngredients.Clear();
            AllIngredients = GetAllIngredients();

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
                    try
                    {
                        var recipe = await new APIService().AddRecipeAsync(new RecipeDTO()
                        {
                            Name = SelectedName,
                            RecipeQuantity = SelectedQuantity,
                            Charges = SelectedCharges,
                            Unit = SelectedUnitType
                        });

                        foreach (var item in SelectedRecipeIngredients)
                        {
                            item.RecipeId = recipe.Id;
                            await new APIService().AddRecipeIngredientAsync(item);
                        }

                        Recipes.Add(Recipe.Build(await new APIService().GetRecipeAsync(recipe.Id)));
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de l'ajout d'une recette", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
        }

        [RelayCommand]
        public async void UpdateRecipe(Recipe recipe)
        {
            _modifiedId = recipe.Id;
            SelectedName = recipe.Name;
            SelectedQuantity = recipe.RecipeQuantity;
            SelectedCharges = recipe.Charges;
            SelectedUnitType = recipe.Unit;

            SelectedIngredient = null;
            SelectedRecipeIngredients.Clear();
            SelectedRecipeIngredients.AddRange(await new APIService().GetRecipeIngredientsAsync(recipe.Id));
            _baseSelectedRecipeIngredients = SelectedRecipeIngredients.Select(ri => ri.ToSend()).ToList();

            AllIngredients = GetAllIngredients();

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
                    try
                    {
                        var addedRecipe = await new APIService().UpdateRecipeAsync(new RecipeDTO()
                        {
                            Id = recipe.Id,
                            Name = SelectedName,
                            Unit = SelectedUnitType,
                            Charges = SelectedCharges,
                            RecipeQuantity = SelectedQuantity
                        });

                        foreach (var item in _baseSelectedRecipeIngredients.Where(baseIng => !SelectedRecipeIngredients.Any(selIng => selIng.Id == baseIng.Id)))
                        {
                            await new APIService().RemoveRecipeIngredientAsync(item.Id);
                        }

                        foreach (var item in SelectedRecipeIngredients.Where(selIng => _baseSelectedRecipeIngredients.Any(baseIng => baseIng.Id == selIng.Id && baseIng.Quantity != selIng.Quantity)))
                        {
                            await new APIService().UpdateRecipeIngredientAsync(item);
                        }

                        foreach (var item in SelectedRecipeIngredients.Where(selIng => selIng.Id == 0))
                        {
                            item.RecipeId = recipe.Id;
                            await new APIService().AddRecipeIngredientAsync(item);
                        }
                        recipe.Fill(addedRecipe);
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de la modification d'une recette", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
        }

        [RelayCommand]
        public async void RemoveRecipe(Recipe recipe)
        {
            RemovedIngredient = recipe;

            UserControl content = new RemoveIngredientDialog();
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
                    try
                    {
                        await new APIService().RemoveRecipeAsync(recipe.Id);
                        Recipes.Remove(recipe);
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de la suppression d'une recette", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
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

                foreach (var recipeIngredient in SelectedRecipeIngredients)
                {
                    outputFile.WriteLine($"{recipeIngredient.IIngredient.Name};{recipeIngredient.Quantity};{recipeIngredient.IIngredient.UnitPrice};{recipeIngredient.Price}");
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
            var tmp = new RecipeIngredientDTO()
            {
                IngredientId = SelectedIngredient is Ingredient ? SelectedIngredient.Id : null,
                IngredientRecipeId = SelectedIngredient is Recipe ? SelectedIngredient.Id : null,
                IIngredient = SelectedIngredient is Ingredient ? Ingredient.BuildDTO(SelectedIngredient as Ingredient) : Recipe.BuildDTO(SelectedIngredient as Recipe),
            };
            SelectedRecipeIngredients.Add(tmp);
            SelectedIngredient = null;
        }

        [RelayCommand]
        public void RemoveIngredientToRecipe(RecipeIngredientDTO ingredient)
        {
            SelectedRecipeIngredients.Remove(ingredient);
        }

        [RelayCommand]
        public void SearchByText()
        {
            OnPropertyChanged(nameof(ShowedRecipes));
        }
    }
}
