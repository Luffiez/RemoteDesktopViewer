using RemoteDesktopViewer.View;
using RemoteDesktopViewer.ViewModel;
using System.Windows;

namespace RemoteDesktopViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            GroupManagerViewModel groupManagerViewModel = new GroupManagerViewModel();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(groupManagerViewModel);

            MainWindow window = new MainWindow(mainWindowViewModel, groupManagerViewModel);
            window.DataContext = mainWindowViewModel;

            GroupManagerWindow groupManagerWindow = new GroupManagerWindow(groupManagerViewModel);
            groupManagerWindow.DataContext = groupManagerViewModel;
            window.Show();
        }
    }
}
