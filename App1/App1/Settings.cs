using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private static readonly string SettingsDefault = string.Empty;
        private const string KeyStands = "stands";
        

        private const string KeyPermission = "permission";

        #endregion


        public static string Stands
        {
            get
            {
                return AppSettings.GetValueOrDefault(KeyStands, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(KeyStands, value);
            }
        }

        public static bool Permission
        {
            get
            {
                return AppSettings.GetValueOrDefault(KeyPermission, true);
            }
            set
            {
                AppSettings.AddOrUpdateValue(KeyPermission, value);
            }
        }

    }
}
