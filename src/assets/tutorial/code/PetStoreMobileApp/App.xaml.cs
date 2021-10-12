using System;
using Microsoft.Extensions.Configuration;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PetStoreClientSDK;
using LazyStackAuth;
using PetStoreMobileApp.Views;

namespace PetStoreMobileApp
{
    public partial class App : Application
    {
        public static IConfiguration AppConfig;
        public static IAuthProcess AuthProcess;
        public static LzHttpClient LzHttpClient;
        public static PetStore PetStore;

        public App()
        {
            InitializeComponent();

            using var authMessagesStream = typeof(IAuthProcess).Assembly.GetManifestResourceStream("LazyStackAuth.AuthMessages.json");
            using var awsSettingsStream = typeof(App).Assembly.GetManifestResourceStream("PetStoreMobileApp.AwsSettings.json");
            using var methodMapStream = typeof(PetStore).Assembly.GetManifestResourceStream("PetStoreClientSDK.MethodMap.json");
            using var localApisStream = typeof(App).Assembly.GetManifestResourceStream("PetStoreMobileApp.LocalApis.json");
            AppConfig = new ConfigurationBuilder()
                .AddJsonStream(authMessagesStream)
                .AddJsonStream(awsSettingsStream)
                .AddJsonStream(methodMapStream)
                .AddJsonStream(localApisStream)
                .Build();

            var authProvider = new AuthProviderCognito(
                AppConfig, 
                loginFormat: new LoginFormat(AppConfig), 
                passwordFormat: new PasswordFormat(AppConfig),
                emailFormat: new EmailFormat(AppConfig),
                codeFormat: new CodeFormat(AppConfig),
                phoneFormat: new PhoneFormat(AppConfig));

            AuthProcess = new AuthProcess(AppConfig, authProvider);
            LzHttpClient = new LzHttpClient(AppConfig, authProvider, GetLocalApiName());
            LzHttpClient.UseLocalApi = false;
            PetStore = new PetStore(LzHttpClient);
            MainPage = new NavigationPage (new MainPage());
        }

        protected string GetLocalApiName()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.UWP:
                case Device.WPF:
                    return "Local";
                case Device.Android:
                    return "LocalAndroid";
                default:
                    throw new Exception("Unknown Platform");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
