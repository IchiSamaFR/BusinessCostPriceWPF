﻿using BusinessCostPriceAPI.Client.Models;
using BusinessCostPriceAPI.Client.Services;
using BusinessCostPriceWPF.Services;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Pages.Login
{
    public partial class LoginVM : ObservableObject
    {
        public Action? OnLogged { get; set; }

        [ObservableProperty]
        private string _mailAdress;

        [ObservableProperty]
        private string _passwordText;

        [ObservableProperty]
        private bool _isLogging;

        private IAPIService _apiService;

        public LoginVM(IAPIService service)
        {
            _apiService = service;
            ClearValues();
#if DEBUG
            MailAdress = "mytempmail@et.et";
            PasswordText = "This-Password3";
#endif
        }

        private void ClearValues()
        {
            MailAdress = string.Empty;
            PasswordText = string.Empty;
        }

        [RelayCommand]
        public async void Log(object password)
        {
            IsLogging = true;

            var passwordBox = password as PasswordBox;
            try
            {
                var result = await _apiService.LoginAsync(new AuthenticateDTO()
                {
                    Email = MailAdress,
                    Password = passwordBox.Password,
                    Token = ""
                });

                _apiService.JwtToken = result.Token;
                ClearValues();
            }
            catch (ApiException ex)
            {
                ExceptionService.ShowError("Erreur de connexion", ex.Response);
            }

            OnLogged.Invoke();
            IsLogging = false;
        }

        [RelayCommand]
        public async void Register(object password)
        {
            IsLogging = true;

            var passwordBox = password as PasswordBox;
            try
            {
                var result = await _apiService.RegisterAsync(new AuthenticateDTO()
                {
                    Email = MailAdress,
                    Password = passwordBox.Password,
                    Token = ""
                });

                _apiService.JwtToken = result.Token;
            }
            catch (ApiException ex)
            {
                ExceptionService.ShowError("Erreur d'inscription", ex.Response);
            }

            OnLogged.Invoke();
            IsLogging = false;
            ClearValues();
        }

        public void Disconnect()
        {
            _apiService.JwtToken = string.Empty;
            OnLogged.Invoke();
        }
    }
}
