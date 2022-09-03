using AppleLogin.Interfaces;
using System;
using Xamarin.Forms;
using AppleLogin.Model;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace AppleLogin
{
    public partial class App : Application
    {
        public const string LoggedInKey = "WNAYSK88JX.com.companyname.BeepIt.Singin";
        public const string AppleUserIdKey = "WNAYSK88JX.com.companyname.BeepIt";
        string userId;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            var appleSignInService = DependencyService.Get<IAppleSignInService>();
            if (appleSignInService != null)
            {
                userId = await SecureStorage.GetAsync(AppleUserIdKey);
                if (appleSignInService.IsAvailable && !string.IsNullOrEmpty(userId))
                {

                    var credentialState = await appleSignInService.GetCredentialStateAsync(userId);

                    switch (credentialState)
                    {
                        case AppleSignInCredentialState.Authorized:
                            //Normal app workflow...
                            break;
                        case AppleSignInCredentialState.NotFound:
                        case AppleSignInCredentialState.Revoked:
                            //Logout;
                            SecureStorage.Remove(AppleUserIdKey);
                            Preferences.Set(LoggedInKey, false);
                            MainPage = new Logado();
                            break;
                    }
                }
            }

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
