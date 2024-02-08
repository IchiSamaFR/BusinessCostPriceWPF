// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Pages
{
    public partial class SettingsVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = string.Empty;

        [ObservableProperty]
        private Wpf.Ui.Appearance.ThemeType _currentTheme = Wpf.Ui.Appearance.ThemeType.Unknown;

        [ObservableProperty]
        private List<Wpf.Ui.Appearance.ThemeType> _themes = new List<Wpf.Ui.Appearance.ThemeType>();

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
            CurrentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
            AppVersion = $"v{GetAssemblyVersion()}";
            Themes = new List<Wpf.Ui.Appearance.ThemeType>() { Wpf.Ui.Appearance.ThemeType.Light, Wpf.Ui.Appearance.ThemeType.Dark };

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? string.Empty;
        }

        partial void OnCurrentThemeChanging(Wpf.Ui.Appearance.ThemeType parameter)
        {
            if(CurrentTheme != parameter)
            {
                Wpf.Ui.Appearance.Theme.Apply(parameter);
            }
        }
    }
}
