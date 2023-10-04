using CostOfRevenue.Models;
using CostOfRevenue.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace CostOfRevenue.ViewModels.Pages
{
    public partial class IngredientsVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private List<Enums.Unit> _unitsType = new List<Enums.Unit>();

        [ObservableProperty]
        private Enums.Unit _selectedUnitType = Enums.Unit.kilogram;

        [ObservableProperty]
        private string _selectedName = string.Empty;

        [ObservableProperty]
        private float? _selectedPrice = null;

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
        }

        private void ClearSelection()
        {
            SelectedUnitType = Enums.Unit.kilogram;
            SelectedName = string.Empty;
            SelectedPrice = null;
        }
    }
}
