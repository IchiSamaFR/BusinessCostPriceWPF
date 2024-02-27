using BusinessCostPriceWPF.ViewModels.Pages.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace BusinessCostPriceWPF.ViewModels.Pages.Dashboard
{
    public partial class UserVM : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private LoginVM _loginVM;

        public UserVM(LoginVM loginVM)
        {
            _loginVM = loginVM;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
            _loginVM.Disconnect();
        }

        public void OnNavigatedFrom()
        {

        }

        private void InitializeViewModel()
        {

        }
    }
}
