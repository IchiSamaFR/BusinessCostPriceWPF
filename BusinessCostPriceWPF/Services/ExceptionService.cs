using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Services;

namespace BusinessCostPriceWPF.Services
{
    public partial class ExceptionService : ObservableObject
    {
        private static ISnackbarService _snackbarService;
        public ExceptionService(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
        }

        private void OnOpenSnackbar(object sender)
        {

        }

        public static void ShowError(string title, string message)
        {
            _snackbarService.Show(
                title,
                message,
                ControlAppearance.Danger,
                new SymbolIcon(SymbolRegular.ErrorCircle24),
                TimeSpan.FromSeconds(4)
            );
        }

        private void UpdateSnackbarAppearance(int appearanceIndex)
        {
            var tmp = appearanceIndex switch
            {
                1 => ControlAppearance.Secondary,
                2 => ControlAppearance.Info,
                3 => ControlAppearance.Success,
                4 => ControlAppearance.Caution,
                5 => ControlAppearance.Danger,
                6 => ControlAppearance.Light,
                7 => ControlAppearance.Dark,
                8 => ControlAppearance.Transparent,
                _ => ControlAppearance.Primary
            };
        }
    }
}
