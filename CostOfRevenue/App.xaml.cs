// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using CostOfRevenue.Services;
using CostOfRevenue.ViewModels.Pages;
using CostOfRevenue.ViewModels.Windows;
using CostOfRevenue.Views.Pages;
using CostOfRevenue.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;

namespace CostOfRevenue
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

                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowVM>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();

                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardVM>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsVM>();

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
