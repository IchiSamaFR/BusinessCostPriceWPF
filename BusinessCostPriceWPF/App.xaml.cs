// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using BusinessCostPriceWPF.ViewModels.Pages.Dashboard;
using BusinessCostPriceWPF.ViewModels.Pages.Login;
using BusinessCostPriceWPF.ViewModels.Windows;
using BusinessCostPriceWPF.Views.Pages;
using BusinessCostPriceWPF.Views.Pages.Dashboard;
using BusinessCostPriceWPF.Views.Pages.Login;
using BusinessCostPriceWPF.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;

namespace BusinessCostPriceWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                services.AddSingleton<ExceptionService>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowVM>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();

                services.AddSingleton<LoginView>();
                services.AddSingleton<LoginVM>();

                services.AddSingleton<HomePage>();
                services.AddSingleton<HomeVM>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsVM>();
                services.AddSingleton<UserVM>();
                services.AddSingleton<UserPage>();

                services.AddSingleton<FurnituresPage>();
                services.AddSingleton<FurnituresVM>();

                services.AddSingleton<IngredientsPage>();
                services.AddSingleton<IngredientsVM>();

                services.AddSingleton<RecipesPage>();
                services.AddSingleton<RecipesVM>();

                services.AddSingleton<StockVM>();
                services.AddSingleton<StockPage>();
            }).Build();

        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            DataService.Initialize();
            _host.Start();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
