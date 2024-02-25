using BusinessCostPriceWPF.Services;
using BusinessCostPriceWPF.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCostPriceWPF.ViewModels.Pages.Login
{
    public partial class LoginVM : ObservableObject
    {
        public Action OnLogged { get; set; }

        [ObservableProperty]
        private string _mailAdress;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _isLogging;

        public LoginVM()
        {
#if DEBUG
            MailAdress = "mytempmail@et.et";
            Password = "This-Password3";
#endif
        }

        [RelayCommand]
        public async void Log()
        {
            IsLogging = true;
            var api = new APIService();
            try
            {
                var result = await api.LoginAsync(new AuthenticateDTO()
                {
                    Email = MailAdress,
                    Password = Password,
                    Token = ""
                });

                APIService.JwtToken = result.Token;
            }
            catch (ApiException ex)
            {
                ExceptionService.ShowError("Erreur de connexion", ex.Response);
            }

            OnLogged.Invoke();
            IsLogging = false;
        }

        [RelayCommand]
        public async void Register()
        {
            IsLogging = true;
            var api = new APIService();
            try
            {
                var result = await api.RegisterAsync(new AuthenticateDTO()
                {
                    Email = MailAdress,
                    Password = Password,
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
        }
    }
}
