using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public LoginVM()
        {
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
                var result = await new APIService().LoginAsync(new AuthenticateDTO()
                {
                    Email = MailAdress,
                    Password = passwordBox.Password,
                    Token = ""
                });

                APIService.JwtToken = result.Token;
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
                var result = await new APIService().RegisterAsync(new AuthenticateDTO()
                {
                    Email = MailAdress,
                    Password = passwordBox.Password,
                    Token = ""
                });

                APIService.JwtToken = result.Token;
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
            APIService.JwtToken = string.Empty;
            OnLogged.Invoke();
        }
    }
}
