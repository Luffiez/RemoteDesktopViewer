using Microsoft.Win32;
using RemoteDesktopViewer.Functionality;
using RemoteDesktopViewer.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace RemoteDesktopViewer.ViewModel
{
    public class MainWindowViewModel : BaseModel
    {
        private ConnectionGroupModel connectionGroupModel;
        private ConnectionDetailsModel connectionDetails;
        private GroupManagerViewModel groupManagerViewModel;
        private MainWindowModel mainWindowModel;

        private UpdateConnectionStatus statusUpdater;
        private ConnectionManager connectionManager;

        public ConnectionGroupModel GroupBeingEdited;
     
        public ConnectionGroupModel ConnectionGroupModel
        {
            get { return connectionGroupModel; } set {connectionGroupModel = value; }
        }

        public ConnectionDetailsModel ConnectionDetails
        {
            get { return connectionDetails; }
            set { connectionDetails = value; }
        }

        internal void ConnectToStation(ConnectionModel item)
        {
            connectionManager.ConnectToStation(item, false);
        }

        internal void TakeOverStation(ConnectionModel item)
        {
            connectionManager.ConnectToStation(item, true);
        }

        public MainWindowModel MainWindowModel
        {
            get { return mainWindowModel; }
            set { mainWindowModel = value; }
        }

        public MainWindowViewModel(GroupManagerViewModel _groupManagerViewModel)
        {
            ConnectionGroupModel = new ConnectionGroupModel();
            ConnectionDetails = new ConnectionDetailsModel();
            groupManagerViewModel = _groupManagerViewModel;
            MainWindowModel = new MainWindowModel();
            statusUpdater = new UpdateConnectionStatus();
            connectionManager = new ConnectionManager();
            groupManagerViewModel.GroupUpdated += GroupManagerViewModel_GroupUpdated;

            RefreshGroupList();
        }

        private void GroupManagerViewModel_GroupUpdated(object sender, ConnectionGroupModel group)
        {
            int id = MainWindowModel.Groups.IndexOf(GroupBeingEdited);

            if (id == -1) // We just created a new group (e), so we should add it to the list.
            {
                MainWindowModel.Groups.Add(group);
            }
            else
            {
                MainWindowModel.Groups[id].GroupConnections = group.GroupConnections;
                MainWindowModel.Groups[id].GroupName = group.GroupName;
                GroupBeingEdited = null;
            }

            ChangeSelectedGroup(group);
        }

        public IList<ConnectionModel> GetConnections()
        {
            if (MainWindowModel.Groups.Count > 0)
                return MainWindowModel.Groups[0].GroupConnections;
            else
                return null;
        }

        internal void DeleteGroup(ConnectionGroupModel group)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{group.GroupName}'?", "Delete group?", MessageBoxButton.YesNo);

            if (MainWindowModel.Groups.Contains(group) && result == MessageBoxResult.Yes)
            {
                MainWindowModel.Groups.Remove(group);
                GroupManager.DeleteGroup(group);
                ChangeSelectedGroup();
            }
        }

        internal void JoinStation(ConnectionModel connection)
        {
            connectionManager.JoinStation(connection);
        }

        internal void ChangeSelectedGroup(ConnectionGroupModel group = null)
        {
            if (group != null)
                MainWindowModel.Connections = group.GroupConnections;
            else
                MainWindowModel.Connections = null;

            UpdateGroupStatus();
        }

        public void UpdateGroupStatus()
        {
            if (MainWindowModel != null && MainWindowModel.Connections != null)
            {
                foreach (ConnectionModel conn in MainWindowModel.Connections)
                {
                    conn.ConnectionUser = string.Empty;
                    conn.ConnectionStatus = "Updating...";
                    Task.Factory.StartNew(() => statusUpdater.GetConnectionStatus(conn));
                }
            }
        }

        public async void UpdateConnectionStatus(int index)
        {

            if (index == null ||MainWindowModel.Connections.Count <= index || index < 0)
                return;

            ConnectionModel conn = MainWindowModel.Connections[index];
            SetConnectionDetails(conn);
            conn.ConnectionUser = string.Empty;
            conn.ConnectionStatus = "Updating...";
            Task statusTask = new Task(() => statusUpdater.GetConnectionStatus(conn));

            statusTask.Start();
            await statusTask;
            SetConnectionDetails(conn);
        }

        internal void ImportGroup()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                string fileName = dlg.FileName;
                ConnectionGroupModel importedGroup = GroupManager.ImportGroup(fileName);
                MainWindowModel.Groups.Add(importedGroup);

            }
        }

        internal void SetConnectionDetails(ConnectionModel conn)
        {
            string details = "";
            details += "Name: " + conn.ConnectionName + "\n";
            details += "Description: " + conn.ConnectionDescription + "\n";
            details += "Status: " + conn.ConnectionStatus + "\n";
            if(!string.IsNullOrEmpty(conn.ConnectionUser))
                details += "User: " + conn.ConnectionUser + "\n";
            details += "Adress: " + conn.ConnectionAdress;

            ConnectionDetails.ConnectionDetails = details;

            string button = "Unknown";
            switch (conn.ConnectionStatus)
            {
                case "Available": button = "Connect"; break;
                case "Occupied": button = "Join"; break;
                default: button = "Unknown"; break;
            }

            ConnectionDetails.ConnectButton = button;
        }

        internal void RefreshGroupList()
        {
            MainWindowModel.Groups = GroupManager.LoadGroups();
            MainWindowModel.Connections = GetConnections();

            if (MainWindowModel.Connections != null && MainWindowModel.Connections.Count > 0)
            {
                SetConnectionDetails(MainWindowModel.Connections[0]);
                UpdateGroupStatus();
            }
        }
    }
}
