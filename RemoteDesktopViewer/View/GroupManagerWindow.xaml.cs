using RemoteDesktopViewer.Model;
using RemoteDesktopViewer.ViewModel;
using System.Windows;

namespace RemoteDesktopViewer.View
{
    /// <summary>
    /// Interaction logic for GroupManagerWindow.xaml
    /// </summary>
    public partial class GroupManagerWindow : Window
    {
        GroupManagerViewModel groupManagerViewModel;

        public GroupManagerWindow(GroupManagerViewModel _groupManagerViewModel)
        {
            groupManagerViewModel = _groupManagerViewModel;
            DataContext = _groupManagerViewModel;
            InitializeComponent();
        }

        private void NewConnection_OnClick(object sender, RoutedEventArgs e)
        {
            groupManagerViewModel.CreateNewConnection();
        }


        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            groupManagerViewModel.SaveGroup();
            Close();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteConnection_OnClick(object sender, RoutedEventArgs e)
        {
            if(ConnectionList.SelectedItem is ConnectionModel connection)
            {
                groupManagerViewModel.DeleteConnection(connection);
            }
        }
    }
}
