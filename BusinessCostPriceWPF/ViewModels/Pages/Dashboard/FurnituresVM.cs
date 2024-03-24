using BusinessCostPriceAPI.Client.Models;
using BusinessCostPriceAPI.Client.Services;
using BusinessCostPriceWPF.Resources;
using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Views.Pages.Dialogs;
using BusinessCostPriceWPF.Views.Pages.Ingredients;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Pages.Dashboard
{
    public partial class FurnituresVM : ObservableObject, INavigationAware
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
        private FurnitureDTO _removedIngredient;
        #endregion


        [ObservableProperty]
        private string _nameToFind = string.Empty;

        [ObservableProperty]
        private ObservableCollection<FurnitureDTO> _furnitures = new ObservableCollection<FurnitureDTO>();
        public IEnumerable<FurnitureDTO> ShowedFurnitures
        {
            get
            {
                return Furnitures.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower())).OrderBy(i => i.Name);
            }
        }

        private readonly IContentDialogService _contentDialogService;

        private IAPIService _apiService;
        public FurnituresVM(IContentDialogService contentDialogService, IAPIService service)
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
        }
        private async void InitDatas()
        {
            ReloadFurnitures();
        }
        private async void ReloadFurnitures()
        {
            Furnitures = new ObservableCollection<FurnitureDTO>();
            Furnitures.CollectionChanged += (a, e) => SearchByText();
            Furnitures.AddRange(await _apiService.GetFurnituresAsync(0));
        }

        private void ClearSelection()
        {
            NameToFind = string.Empty;
            SelectedUnitType = Unit.Kilogram;
            SelectedName = string.Empty;
            SelectedPrice = null;
        }

        [RelayCommand]
        public async void AddFurniture()
        {
            _modifiedId = 0;
            SelectedUnitType = Unit.Piece;
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
                    try
                    {
                        var newFurniture = await _apiService.AddFurnitureAsync(new FurnitureDTO()
                        {
                            Name = SelectedName,
                            Unit = SelectedUnitType,
                            UnitPrice = SelectedPrice ?? 0
                        });
                        Furnitures.Add(newFurniture);
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de l'ajout d'une fourniture", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
        }

        [RelayCommand]
        public async void UpdateFurniture(FurnitureDTO furniture)
        {
            _modifiedId = furniture.Id;
            SelectedUnitType = furniture.Unit;
            SelectedName = furniture.Name;
            SelectedPrice = furniture.UnitPrice;

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
                        var newFurniture = await _apiService.UpdateFurnitureAsync(new FurnitureDTO()
                        {
                            Id = furniture.Id,
                            Name = SelectedName,
                            UnitPrice = SelectedPrice ?? 0,
                        });
                        Furnitures.Remove(furniture);
                        Furnitures.Add(newFurniture);
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de la modification d'une fourniture", ex.Response);
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
        }

        [RelayCommand]
        public async void RemoveFurniture(FurnitureDTO furniture)
        {
            RemovedIngredient = furniture;

            UserControl content = new RemoveIngredientDialog();
            content.DataContext = this;

            var result = await _contentDialogService.ShowSimpleDialogAsync(
                new SimpleContentDialogCreateOptions()
                {
                    Title = "Supprimer une fourniture",
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
                        await _apiService.RemoveFurnitureAsync(furniture.Id);
                        Furnitures.Remove(furniture);
                    }
                    catch (ApiException ex)
                    {
                        ExceptionService.ShowError("Erreur lors de la suppression d'une fourniture", ex.Response);
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
            OnPropertyChanged(nameof(ShowedFurnitures));
        }
    }
}
