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
using System.Text;
using System.Threading.Tasks;
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

        public FurnituresVM(IContentDialogService contentDialogService)
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
        }
        private async void InitDatas()
        {
            ReloadFurnitures();
        }
        private async void ReloadFurnitures()
        {
            Furnitures = new ObservableCollection<FurnitureDTO>();
            Furnitures.CollectionChanged += (a, e) => SearchByText();
            Furnitures.AddRange(await new APIService().GetFurnituresAsync(0));
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
                    var newFurniture = await new APIService().AddFurnitureAsync(new FurnitureDTO()
                    {
                        Name = SelectedName,
                        Unit = SelectedUnitType,
                        UnitPrice = SelectedPrice ?? 0
                    });
                    Furnitures.Add(newFurniture);
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
                    var newFurniture = await new APIService().UpdateFurnitureAsync(new FurnitureDTO()
                    {
                        Id = furniture.Id,
                        Name = SelectedName,
                        UnitPrice = SelectedPrice ?? 0,
                    });
                    Furnitures.Remove(furniture);
                    Furnitures.Add(newFurniture);
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
                    await new APIService().RemoveFurnitureAsync(furniture.Id);
                    Furnitures.Remove(furniture);
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
