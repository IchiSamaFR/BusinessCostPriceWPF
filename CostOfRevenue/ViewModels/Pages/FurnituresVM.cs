using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using CostOfRevenue.Services;
using CostOfRevenue.Views.Pages.Dialogs;
using CostOfRevenue.Views.Pages.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace CostOfRevenue.ViewModels.Pages
{
    public partial class FurnituresVM : ObservableObject, INavigationAware
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
        private Furniture _removedIngredient;
        #endregion


        [ObservableProperty]
        private string _nameToFind = string.Empty;

        [ObservableProperty]
        private IEnumerable<Furniture> _showedFurnitures;

        private readonly IContentDialogService _contentDialogService;

        public FurnituresVM(IContentDialogService contentDialogService)
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
        public async void AddFurniture()
        {
            _modifiedId = string.Empty;
            SelectedUnitType = Enums.Unit.piece;
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
                    DataService.Furnitures.Add(new Furniture(Guid.NewGuid().ToString(), SelectedName, SelectedUnitType, SelectedPrice ?? 0, DateTime.Now));
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveFurnitures();
            SearchByText();
        }

        [RelayCommand]
        public async void UpdateFurniture(Furniture furniture)
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
                    if (furniture.Date.Date == DateTime.Now.Date)
                    {
                        furniture.Name = SelectedName;
                        furniture.Unit = SelectedUnitType;
                        furniture.UnitPrice = SelectedPrice ?? 0;
                    }
                    else
                    {
                        DataService.Furnitures.Add(new Furniture(_modifiedId, SelectedName, SelectedUnitType, SelectedPrice ?? 0, DateTime.Now));
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveFurnitures();
            SearchByText();
        }

        [RelayCommand]
        public async void RemoveFurniture(Furniture furniture)
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
                    DataService.Remove(furniture);
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                default:
                    break;
            }
            DataService.SaveFurnitures();
            SearchByText();
        }

        [RelayCommand]
        public void SearchByText()
        {
            ShowedFurnitures = DataService.GetLastFurnitures.Where(i => i.Name.ToLower().Contains(NameToFind.ToLower()));
        }
    }
}
