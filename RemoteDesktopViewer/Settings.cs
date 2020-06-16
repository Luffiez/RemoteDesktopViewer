
namespace RemoteDesktopViewer
{
    public class Settings
    {
        private string path = $"{System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Settings.xml";

        public ISettings settings;

        public Settings(MainWindow mainWindow)
        {
            settings = LoadSettings();
        }

        public ISettings LoadSettings()
        {
            return XmlHelper.LoadXml<ISettings>(path);
        }

        public void SaveSettings()
        {
            XmlHelper.SaveXml(path, settings);
        }

        public void SaveWindowSize(double _height, double _width)
        {
            settings.WindowWidth = _height;
            settings.WindowWidth = _width;
            SaveSettings();
        }

        public void SaveWindowTheme(string _theme)
        {
            settings.WindowTheme = _theme;
            SaveSettings();
        }
    }

    public class ISettings
    {
        public double WindowWidth { get; set; }
        public double WindowHeigth { get; set; }
        public string WindowTheme { get; set; }
    }
}
