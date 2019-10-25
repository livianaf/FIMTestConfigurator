using System.Configuration;
using System.Linq;

namespace FIMTestConfigurator {
    public class ConfigurationHelper {
        public static string GetSetting( string key ) {
            return ConfigurationManager.AppSettings[key];
            }

        public static void SetSetting( string key, string value ) {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key)) configuration.AppSettings.Settings.Add(key, value);
            else configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified, false);
            ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
