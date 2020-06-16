using MahApps.Metro;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace RemoteDesktopViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public StationManager stationManager;
        public Settings SettingsManager;

        // Auto Update Loop For Station List
        TimeSpan startTimeSpan = TimeSpan.Zero;
        TimeSpan periodTimeSpan = TimeSpan.FromSeconds(10);

        IStation selectedStation;
        IStation connectedStation;

        public MainWindow()
        {
            InitializeComponent();
            SetupStations();
            SettingsManager = new Settings(this);
            Closing += App_Exit;
            LoadSettings();
            // Auto Update Loop For Station List
            Timer timer = new Timer((e) =>
            {
                UpdateStatusOfAllStations();
            }, null, startTimeSpan, periodTimeSpan);
        }


        #region Functionality
        private void ConnectToStation(IStation station, bool takeover)
        {
            if (station == null)
            {
                return;
            }
            // Reload Station information
            stationManager.LoadLocalStationList();
            Command.UpdateStationStatus(station, this);

            // Update UIs
            UpdateStationStatusListUI();
            SetSelectedStationStatus();

            if (station.Status == "Occupied" && !takeover)
            {
                MessageBox.Show("This station is currently occupied by another user.\nRight-Click allows sesison join/take over", "Session Occupied");
                return;
            }

            string process = "";
            process += " /v:" + station.Ip;

            Process RemoteConnectProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mstsc.exe",
                    Arguments = process,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Maximized,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            RemoteConnectProcess.Start();
            RemoteConnectProcess.WaitForInputIdle();
            Connected(station);
            
            var outputResultPromise = RemoteConnectProcess.StandardOutput.ReadToEndAsync();
            outputResultPromise.ContinueWith(o => Disconnected(station));
        }

        private void Connected(IStation station)
        {
            connectedStation = station;
            UpdateStationStatusListUI();
            stationManager.SaveLocalStationList();
        }

        private void Disconnected(IStation station)
        {
            if (connectedStation == null)
                return;

            //Console.WriteLine($"User: {Settings.data.user} disconnected from station: {station.Name}");
            stationManager.stations.list.Find(s => s.Name == station.Name).Status = "Available";

            connectedStation = null;
            UpdateStationStatusListUI();
            stationManager.SaveLocalStationList();
        }

        private void UpdateStatusOfAllStations()
        {
            stationManager.LoadLocalStationList();
            Console.WriteLine("Updating status of all stations");
            foreach (IStation station in stationManager.stations.list.ToList())
            {
                if (station != null)
                {
                    Task.Run(() => Command.UpdateStationStatus(station, this));
                }
            }
        }

        public void OpenStationManager()
        {
            EditStationWindow popup = new EditStationWindow(this);
            popup.ShowDialog();
        }

        public void OpenStationManager(IStation station)
        {
            EditStationWindow popup = new EditStationWindow(this, station);
            popup.ShowDialog();
        }

        public void OpenStationSelector(bool edit)
        {
            SelectStationWindow popup = new SelectStationWindow(this, edit);
            popup.ShowDialog();
        }

        private void App_Exit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SettingsManager.SaveWindowSize(Height, Width);
        }
      
        internal void LoadSettings()
        {
            SetTheme(SettingsManager.settings.WindowTheme, "BaseDark");
            SetWindowSize(SettingsManager.settings.WindowHeigth, SettingsManager.settings.WindowWidth);
        }

        void SetupStations()
        {
            stationManager = new StationManager(this);
            stationManager.LoadLocalStationList();

            // Used to initialize XML file if it is empty.
            //stationManager.stations.Add(new Station() { Name = "1001", Type = "Enc6120", Ip = "segaee0015.eipu.ericsson.se" });
        }
        #endregion

        #region UI
        public void ReloadStationListUI()
        {
            if (StationList.Items.Count == stationManager.stations.list.Count)
            {
                UpdateStationStatusListUI();
                return;
            }

            // this.Dispatcher.Invoke(() =>
            this.Dispatcher.Invoke(new Action(() =>
            {
                StationList.ItemsSource = null;
                StationList.ItemsSource = stationManager.stations.list;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationList.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Group");
                if (view.GroupDescriptions.Count == 0)
                    view.GroupDescriptions.Add(groupDescription);
            }));
        }

        public void UpdateStationStatusListUI()
        {
            // this.Dispatcher.Invoke(() =>
            this.Dispatcher.Invoke(new Action(() =>
            {
                int id = 0;

                foreach (IStation item in StationList.Items)
                {
                    item.Name = stationManager.stations.list[id].Name;
                    item.Status = stationManager.stations.list[id].Status;
                    item.Type = stationManager.stations.list[id].Type;
                    item.Ip = stationManager.stations.list[id].Ip;

                    id++;
                }

                StationList.Items.Refresh();
            }));
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (StationList.SelectedItem == null || !IsActive)
                return;

            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                IStation selected = StationList.SelectedItem as IStation;
                if (selected != null)
                {
                    selectedStation = selected;
                    SetSelectedStationStatus();
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (StationList.SelectedItem == null || !IsActive)
                return;

            if (e.Key == Key.Enter)
            {
                ConnectToStation(selectedStation, false);
            }
        }

        public void SetStationStatus(IStation station, string status)
        {
            station.Status = status;
            stationManager.SaveLocalStationList();
            UpdateStationStatusListUI();
            if (selectedStation != null)
                StationList.SelectedItem = StationList.Items.IndexOf(station);
        }
        public void SetSelectedStationStatus()
        {
            if (selectedStation == null)
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                SelectedStationText.Text = $"{selectedStation.Name} - {selectedStation.Status.ToString()}";
                switch (selectedStation.Status)
                {
                    case "Available":
                        SelectedStationText.Foreground = Brushes.DarkGreen;
                        break;
                    case "Unknown":
                        SelectedStationText.Foreground = Brushes.DarkRed;
                        break;
                    case "Occupied":
                        SelectedStationText.Foreground = Brushes.DarkBlue;
                        break;
                    default:
                        SelectedStationText.Foreground = Brushes.DimGray;
                        break;
                }
            }));
        }
        #endregion

        #region Buttons
        private void Click_Connect(object sender, RoutedEventArgs e)
        {
            ConnectToStation(selectedStation, false);
        }

        public void Click_Import(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xml";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                stationManager.LoadRemoteStationList(dlg.FileName);
                stationManager.SaveLocalStationList();
            }
        }

        private void Click_Export(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "XML Files|*.xml";
            saveFileDialog.Title = "Export the Station List";
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.CheckPathExists = true;
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                stationManager.ExportStationList(saveFileDialog.FileName);
            }
        }

        private void StationList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView view)
            {
                if (view.SelectedIndex > stationManager.stations.list.Count || view.SelectedIndex == -1)
                    return;

                IStation station = stationManager.stations.list[view.SelectedIndex];
                selectedStation = station;
                SetSelectedStationStatus();
            }
        }

        private void StationList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView view)
            {
                if (view.SelectedIndex > stationManager.stations.list.Count || view.SelectedIndex == -1)
                    return;

                IStation station = stationManager.stations.list[view.SelectedIndex];
                selectedStation = station;
                SetSelectedStationStatus();

                if(station.Status != "Available")
                {
                    JoinSession_MenuItem.IsEnabled = true;
                    TakeOver_MenuItem.IsEnabled = true;

                }
                else
                {
                    JoinSession_MenuItem.IsEnabled = false;
                    TakeOver_MenuItem.IsEnabled = false;
                }
            }
        }

        public void Click_EditStation(object sender, RoutedEventArgs e)
        {
            OpenStationSelector(true);
        }

        public void RightClick_EditStation(object sender, RoutedEventArgs e)
        {
            OpenStationManager(selectedStation);
        }

        private void Click_AddStation(object sender, RoutedEventArgs e)
        {
            OpenStationManager();
            ReloadStationListUI();
        }

        public void Click_RemoveStation(object sender, RoutedEventArgs e)
        {
            OpenStationSelector(false);
        }

        private void Click_RefreshStations(object sender, RoutedEventArgs e)
        {
            UpdateStatusOfAllStations();
        }

        public void Click_ChangeTheme(object sender, RoutedEventArgs e)
        {
            ThemeManagerWindow popup = new ThemeManagerWindow(this, SettingsManager.settings.WindowTheme);
            popup.ShowDialog();
        }

        internal void SetTheme(string themeAccent, string themeBase = "BaseDark")
        {         
            ThemeManager.ChangeThemeColorScheme(Application.Current, themeAccent);
        }

        internal void SetWindowSize(double height, double width)
        {
            Height = height;
            Width = width;
            SettingsManager.SaveWindowSize(height, width);
        }

        private void Click_JoinSession(object sender, RoutedEventArgs e)
        {
            if (selectedStation == null)
                return;

            string ID = selectedStation.SessionID;
            if (ID != null)
            {
                MessageBoxResult overtake = MessageBox.Show($"This station is currently being used by another user. \nWould you like to join the session?", "Session Occupied", MessageBoxButton.YesNo);
                if (overtake == MessageBoxResult.Yes)
                {
                    Process shadowProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "mstsc.exe",
                            Arguments = $"/shadow:{ID} /v:{selectedStation.Ip} /control",
                            CreateNoWindow = true
                        }
                    };

                    shadowProcess.Start();
                    shadowProcess.WaitForExit();
                }
            }
        }

        private void Click_TakeOver(object sender, RoutedEventArgs e)
        {
            string msg = $"Are you sure you want to take over the station {selectedStation.Name}?";
            MessageBoxResult overtake = MessageBox.Show(msg, "Session Occupied", MessageBoxButton.YesNo);
            if (overtake == MessageBoxResult.Yes)
            {
                ConnectToStation(selectedStation, true);
            }
        }

        private void Click_About(object sender, RoutedEventArgs e)
        {   
            MessageBox.Show($"Remote Desktop Viewer is an open source Remote Desktop Managing program created by Erik Rodriguez under the Creative Commons license, ©2020.", "About");
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            UpdateStatusOfAllStations();
        }
        #endregion
    }
}
