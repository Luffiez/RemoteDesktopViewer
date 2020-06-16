using System;
using System.Linq;
using System.Windows;

namespace RemoteDesktopViewer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditStationWindow
    {
        private MainWindow mainWindow;
        private IStation stationToEdit;
        
        /// <summary>
        /// Used for adding a new staion
        /// </summary>
        /// <param name="main"></param>
        public EditStationWindow(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            GroupBox.ItemsSource = Enum.GetValues(typeof(StationGroup)).Cast<StationGroup>();
        }

        /// <summary>
        /// Used when editing an existing station
        /// </summary>
        /// <param name="main"></param>
        /// <param name="station">The station to edit</param>
        public EditStationWindow(MainWindow main, IStation station)
        {
            InitializeComponent();
            mainWindow = main;
            stationToEdit = station;
            PopulateElements();
        }

        /// <summary>
        /// Populates the UI elements with station information
        /// </summary>
        private void PopulateElements()
        {
            StationName.Text = stationToEdit.Name;
            StationType.Text = stationToEdit.Type;
            StationIp.Text = stationToEdit.Ip;
            GroupBox.Text = stationToEdit.Group.ToString();
            GroupBox.ItemsSource = Enum.GetValues(typeof(StationGroup)).Cast<StationGroup>();
        }

        /// <summary>
        /// Saves the information provided to the station list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Click_SaveStation(object sender, RoutedEventArgs e)
        {
            // Used to store multiple 'errors'
            string errorMsg = string.Empty;

            // If no station name has been assigned to the StationNameTextbox
            if(string.IsNullOrEmpty(StationName.Text))
            {
                errorMsg += "Station needs a name!.";
            }

            // If no station Ip has been assigned to the StationIpTextbox
            if (string.IsNullOrEmpty(StationIp.Text))
            {
                errorMsg += "\nStation needs an IP-Adress.";
            }

            // If any errors exist, display them with a MessageBox
            if(!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg, "Invalid Station Settings!", MessageBoxButton.OK);
                return;
            }

            // Type does not have to be specified, default to 'Unspecified' if none has been provided.
            if (string.IsNullOrEmpty(StationType.Text))
            {
                StationType.Text = "Unspecified";
            }

            // StationToEdit is null if we want to CREATE a new station
            if(stationToEdit == null)
            {
                // Create the new station with relevant information
                IStation newStation = new IStation()
                {
                    Name = StationName.Text,
                    Type = StationType.Text,
                    Ip = StationIp.Text,
                    Group = (StationGroup)GroupBox.SelectedIndex
                 };

                // Double check that no station with said name exists already.
                if (!mainWindow.stationManager.AddStationToList(newStation))
                {
                    MessageBox.Show("Station with that name already exists!\n Edit existing station or use another name.", "Station already exists!", MessageBoxButton.OK);
                    return;
                }
            }
            // StationToEdit is NOT null if we want to EDIT an exisiting station
            else
            {
                //Update existing station with relevant information.
                stationToEdit.Name = StationName.Text;
                stationToEdit.Type = StationType.Text;
                stationToEdit.Ip = StationIp.Text;
                stationToEdit.Group = (StationGroup)GroupBox.SelectedIndex;
                stationToEdit = null;
            }

            // Save the station list after adding/editing and close this window.
            mainWindow.stationManager.SaveLocalStationList();
            CloseWindow();
        }

        /// <summary>
        /// Closes the window and display the MainWindow
        /// </summary>
        private void CloseWindow()
        {
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Closes the window and display the MainWindow
        /// </summary>
        private void Click_CancelCreation(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
    }
}
