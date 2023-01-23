using System;
using System.IO;

namespace EjesUI.Models
{
    public class AppConfig
    {
        public string ConfigurationsFolder { get; set; }
        public string AppPropertiesFileName { get; set; }
        public string ApiURL {
            get
            {
                return "https://a14a-190-110-60-138.sa.ngrok.io";
            }
        }
        public string DefaultDownloadPath {
            get
            {
                string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Downloads\\Ejes Docs\\";


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public int CONSTANTE_TORQUE_SI = 9549;
        public int CONSTANTE_TORQUE_FPS = 63000;
        public string SI = "SI";
        public string FPS = "FPS";
    }
}
