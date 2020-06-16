using System.Windows;


namespace RemoteDesktopViewer
{
    /// <summary>
    /// Interaction logic for SelectStationWindow.xaml
    /// </summary>
    /// 
    public partial class SelectStationWindow
    {
        bool edit;
        MainWindow mainWindow;

        public SelectStationWindow(MainWindow _mainWindow, bool _edit)
        {
            InitializeComponent();

            edit = _edit;
            mainWindow = _mainWindow;

            // Set buttont text
            if(edit)
                Action.Content = "Edit";
            else
                Action.Content = "Delete";

            // Populate list
            string[] stationNames = new string[mainWindow.stationManager.stations.list.Count];
            for(int i = 0; i < mainWindow.stationManager.stations.list.Count; i++)
            {
                stationNames[i] = mainWindow.stationManager.stations.list[i].Name;
            }
            StationList.ItemsSource = stationNames;
            StationList.SelectedIndex = 0;
        }

        private void Click_Action(object sender, RoutedEventArgs e)
        {
            if(edit)
            {
                EditStation();
            }
            else
            {
                DeleteStation();
            }

            CloseWindow();
        }

        private void CloseWindow()
        {
            mainWindow.Show();
            this.Close();
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void EditStation()
        {
            mainWindow.OpenStationManager(mainWindow.stationManager.stations.list[StationList.SelectedIndex]);
            mainWindow.ReloadStationListUI();
        }

        private void DeleteStation()
        {
            MessageBoxResult delete = MessageBox.Show($"Are you sure you want to remove the following station: {mainWindow.stationManager.stations.list[StationList.SelectedIndex].Name}?", "Remove Station", MessageBoxButton.YesNo);
            if (delete == MessageBoxResult.Yes)
            {
                mainWindow.stationManager.RemoveStationFromList(StationList.SelectedIndex);
                mainWindow.ReloadStationListUI();
            }
        }
    }
}
