using RemoteDesktopViewer.Functionality;
using RemoteDesktopViewer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            connectionDetails = new ConnectionDetailsModel();
            groupManagerViewModel = _groupManagerViewModel;
            MainWindowModel = new MainWindowModel();
            statusUpdater = new UpdateConnectionStatus();
            connectionManager = new ConnectionManager();

            groupManagerViewModel.GroupUpdated += GroupManagerViewModel_GroupUpdated;

            MainWindowModel.Groups = GroupManager.LoadGroups();
            MainWindowModel.Connections = GetConnections();

            if (MainWindowModel.Connections.Count > 0)
            {
                SetConnectionDetails(0);
                UpdateGroupStatus();
            }
        }

        private void GroupManagerViewModel_GroupUpdated(object sender, ConnectionGroupModel group)
        {
            int id = MainWindowModel.Groups.IndexOf(GroupBeingEdited);
            
            if(id == -1) // We just created a new group (e), so we should add it to the list.
            {
                MainWindowModel.AddGroup(group);
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
            return MainWindowModel.Groups[0].GroupConnections;
        }

        public IList<ConnectionGroupModel> GetGroups()
        {
            List<ConnectionModel> connections1 = new List<ConnectionModel>
            {
                new ConnectionModel{ ConnectionName = "1001", ConnectionDescription = "Enc 6120", ConnectionStatus= "Available", ConnectionAdress = "segaee0015.eipu.ericsson.se"},
                new ConnectionModel{ ConnectionName = "1002", ConnectionDescription = "Rbs 6101", ConnectionStatus= "Available", ConnectionAdress = "segaee0031.eipu.ericsson.se"},
                new ConnectionModel{ ConnectionName = "1003", ConnectionDescription = "Enc 6150", ConnectionStatus= "Occupied", ConnectionAdress = "segaee0029.eipu.ericsson.se"}
            };

            List<ConnectionModel> connections2 = new List<ConnectionModel>
            {
                new ConnectionModel{ ConnectionName = "9901", ConnectionDescription = "PRTT", ConnectionStatus= "Occupied", ConnectionAdress = "segaee0038.eipu.ericsson.se"},
                new ConnectionModel{ ConnectionName = "9902", ConnectionDescription = "PRTT", ConnectionStatus= "Available", ConnectionAdress = "segaee0030.eipu.ericsson.se"},
                new ConnectionModel{ ConnectionName = "9904", ConnectionDescription = "PRTT", ConnectionStatus= "Unknown", ConnectionAdress = "segaee0009.eipu.ericsson.se"}
            };

            List<ConnectionGroupModel> groups = new List<ConnectionGroupModel>();

            groups.Add(new ConnectionGroupModel { GroupName = "Node", GroupConnections = connections1 });
            groups.Add(new ConnectionGroupModel { GroupName = "AM", GroupConnections = connections2 });

            return groups;
        }

        internal void DeleteGroup(ConnectionGroupModel item)
        {
            if(MainWindowModel.Groups.Contains(item))
                MainWindowModel.Groups.Remove(item);
        }

        internal void JoinStation(ConnectionModel connection)
        {
            connectionManager.JoinStation(connection);
        }

        internal void ChangeSelectedGroup(ConnectionGroupModel group)
        {
            MainWindowModel.Connections = group.GroupConnections;
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

        internal void SetConnectionDetails(int index)
        {
            if (MainWindowModel.Connections.Count <= index || index < 0)
                return;

            ConnectionModel conn = MainWindowModel.Connections[index];

            string details = "";
            details += "Name: " + conn.ConnectionName + "\n";
            details += "Description: " + conn.ConnectionDescription + "\n";
            details += "Status: " + conn.ConnectionStatus + "\n";
            if(!string.IsNullOrEmpty(conn.ConnectionUser))
                details += "User: " + conn.ConnectionUser + "\n";
            details += "Adress: " + conn.ConnectionAdress;

            connectionDetails.ConnectionDetails = details;

            string button = "Unknown";
            switch (conn.ConnectionStatus)
            {
                case "Available":
                    button = "Connect";
                    break;
                case "Occupied":
                    button = "Join";
                    break;
                default:
                    button = "Unknown";
                    break;
            }

            connectionDetails.ConnectButton = button;
        }
    }
}
