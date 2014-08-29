using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAkos;

namespace PictureBackup.Config
{
    static class Config
    {
        public static ConfigSetting Settings;
        private static Xmlconfig Xmlconfig;

        public static void Init()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var userFilePath = Path.Combine(localAppData, "LukasHaefliger");

            if (!Directory.Exists(userFilePath))
                Directory.CreateDirectory(userFilePath);
            var configFilePath = Path.Combine(userFilePath, "config_PictureBackup.xml");

            var alreadyExists = File.Exists(configFilePath);
            var config = new Xmlconfig(configFilePath, true);
            Xmlconfig = config;
            Settings = config.Settings;
            config.CommitOnUnload = true;
            if (alreadyExists)
                return;
            Settings["in"]["count"].intValue = 0;
            Settings["out"]["count"].intValue = 0;
            Settings["settings"]["updateInterval"].intValue = 5;
            config.Commit();
        }

        public static void Commit()
        {
            Xmlconfig.Commit();
        }
    }
}
