using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RemoteDesktopViewer
{
    /// <summary>
    /// Interaction logic for ThemeManagerWindow.xaml
    /// </summary>
    public partial class ThemeManagerWindow 
    {
        MainWindow main;
        string activeTheme;
        string originalTheme;

        public ThemeManagerWindow(MainWindow mainWindow, string _theme)
        {
            InitializeComponent();
            main = mainWindow;
            activeTheme = _theme;
            originalTheme = _theme;
            ThemeList.ItemsSource = Themes;
            ThemeList.SelectedIndex = Themes.IndexOf(activeTheme);
        }

        private void Click_Theme(object sender, RoutedEventArgs e)
        {
            activeTheme = Themes[ThemeList.SelectedIndex];
            main.SetTheme(activeTheme);
        }

        private void Click_Confirm(object sender, RoutedEventArgs e)
        {
            main.SettingsManager.SaveWindowTheme(activeTheme);
            CloseWindow();
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            main.SetTheme(originalTheme);
            CloseWindow();
        }

        private void CloseWindow()
        {
            main.Show();
            this.Close();
        }

        public List<string> Themes = new List<string> {
            "Red",
            "Green",
            "Blue",
            "Purple",
            "Orange",
            "Lime",
            "Emerald",
            "Teal",
            "Cyan",
            "Cobalt",
            "Indigo",
            "Violet",
            "Pink",
            "Magenta",
            "Crimson",
            "Amber",
            "Yellow",
            "Brown",
            "Olive",
            "Steel",
            "Mauve",
            "Taupe",
            "Sienna" 
        };
    }
}
