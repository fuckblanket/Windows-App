﻿using System;
using System.Windows;
using LightVPN.Client.Auth;
using LightVPN.Client.Auth.Interfaces;
using LightVPN.Client.Auth.Models;
using LightVPN.Client.OpenVPN;
using LightVPN.Client.OpenVPN.Interfaces;
using LightVPN.Client.OpenVPN.Models;
using LightVPN.Client.Windows.Common;
using LightVPN.Client.Windows.Configuration;
using LightVPN.Client.Windows.Configuration.Interfaces;
using LightVPN.Client.Windows.Configuration.Models;
using LightVPN.Client.Windows.Models;
using LightVPN.Client.Windows.Services;
using LightVPN.Client.Windows.Services.Interfaces;
using LightVPN.Client.Windows.Utils;

namespace LightVPN.Client.Windows
{
    /// <inheritdoc />
    /// <summary>
    ///     Handles the injection of services
    /// </summary>
    internal sealed class Startup : Application
    {
        /// <summary>
        ///     Creates a new service collection and configures all the services (this does not startup the WPF UI)
        /// </summary>
        [STAThread]
        internal static void Main()
        {
            Globals.Container.RegisterSingleton<IApiClient, ApiClient>();
            Globals.Container.RegisterInstance<IVpnManager>(new VpnManager(new OpenVpnConfiguration
            {
                OpenVpnLogPath = Globals.OpenVpnLogPath,
                OpenVpnPath = Globals.OpenVpnPath,
                TapAdapterName = "LightVPN-TAP",
                TapCtlPath = Globals.OpenVpnPath
            }));
            Globals.Container.RegisterInstance<IConfigurationManager>(
                new ConfigurationManager(Globals.AppSettingsPath));
            Globals.Container.RegisterSingleton<IOpenVpnService, OpenVpnService>();

            var res = new ResourceDictionary();

            var app = new Startup
            {
                Resources = res,
                StartupUri = new Uri("Windows/LoginWindow.xaml", UriKind.RelativeOrAbsolute)
            };

            // Clears merged dictionaries
            res.MergedDictionaries.Clear();

            // Adds all the required resource dictionaries
            Uri colorsUri =
                new("Resources/Colors.xaml", UriKind
                    .RelativeOrAbsolute);
            Uri fontsUri =
                new("Resources/Fonts.xaml", UriKind
                    .RelativeOrAbsolute);
            var typographyUri = new Uri("Resources/Typography.xaml", UriKind
                .RelativeOrAbsolute);
            Uri buttonsUri =
                new("Resources/Buttons.xaml", UriKind
                    .RelativeOrAbsolute);
            Uri windowsUri =
                new("Resources/Windows.xaml", UriKind
                    .RelativeOrAbsolute);
            Uri mdUri =
                new("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml"
                    , UriKind.RelativeOrAbsolute);
            Uri mdUri1 =
                new("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.PopupBox.xaml"
                    , UriKind.RelativeOrAbsolute);

            res.MergedDictionaries.Add(new ResourceDictionary { Source = mdUri });
            res.MergedDictionaries.Add(new ResourceDictionary { Source = mdUri1 });
            res.MergedDictionaries.Add(new ResourceDictionary { Source = colorsUri });
            res.MergedDictionaries.Add(new ResourceDictionary { Source = fontsUri });
            res.MergedDictionaries.Add(new ResourceDictionary { Source = typographyUri });
            res.MergedDictionaries.Add(new ResourceDictionary { Source = buttonsUri });
            res.MergedDictionaries.Add(new ResourceDictionary { Source = windowsUri });

            ThemeManager.SwitchTheme(ThemeColor.Default, BackgroundMode.Light);

            try
            {
                Globals.Container.GetInstance<IApiClient>().GetAsync<GenericResponse>("profile").GetAwaiter()
                    .GetResult();

                app.StartupUri = new Uri("Windows/MainWindow.xaml", UriKind.RelativeOrAbsolute);
            }
            catch (Exception)
            {
                // ignored
            }

            app.Run();
        }
    }
}