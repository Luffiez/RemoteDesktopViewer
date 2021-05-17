using RemoteDesktopViewer.Functionality;
using RemoteDesktopViewer.Model;
using RemoteDesktopViewer.View;
using System;
using System.Collections.ObjectModel;

namespace RemoteDesktopViewer.ViewModel
{
    public class GroupManagerViewModel
    {
        private ConnectionGroupModel currentModel;
        private ConnectionGroupModel originalModel;

        public event EventHandler<ConnectionGroupModel> GroupUpdated;

        GroupManagerWindow window;

        public ConnectionGroupModel CurrentModel
        {
            get { return currentModel; }
            set { currentModel = value; }
        }

        private void OnGroupUpdated()
        {
            EventHandler<ConnectionGroupModel> localCopy = GroupUpdated;

            if(localCopy != null)
            {
                localCopy(this, CurrentModel);
            }
        }

        public GroupManagerViewModel()
        {
            CurrentModel = new ConnectionGroupModel();
        }

        public void OpenWindow_Edit(ConnectionGroupModel groupToEdit)
        {
            originalModel = groupToEdit;
            CurrentModel = CreateCopy(groupToEdit);
            window = new GroupManagerWindow(this);
            window.Show();
        }

        private ConnectionGroupModel CreateCopy(ConnectionGroupModel model)
        {
            ConnectionGroupModel copy = new ConnectionGroupModel();

            copy.GroupName = new string(model.GroupName.ToCharArray());
            ObservableCollection<ConnectionModel> connections = new ObservableCollection<ConnectionModel>();

            foreach (ConnectionModel conn in model.GroupConnections)
            {
                ConnectionModel newConn = new ConnectionModel();

                newConn.ConnectionName = conn.ConnectionName;
                newConn.ConnectionDescription = conn.ConnectionDescription;
                newConn.ConnectionStatus = conn.ConnectionStatus;
                newConn.ConnectionAdress = conn.ConnectionAdress;

                connections.Add(newConn);
            }
            copy.GroupConnections = connections;
            return copy;
        }

        internal void DeleteConnection(ConnectionModel connectionToDelete)
        {
            if (CurrentModel.GroupConnections.Contains(connectionToDelete))
            {
                CurrentModel.GroupConnections.Remove(connectionToDelete);
            }
        }

        public void OpenWindow_Create()
        {
            CurrentModel = new ConnectionGroupModel();
            currentModel.GroupConnections = new ObservableCollection<ConnectionModel>();
            window = new GroupManagerWindow(this);
            window.ShowDialog();
        }

        internal void SaveGroup()
        {
            GroupManager.SaveGroup(originalModel, CurrentModel);
            OnGroupUpdated();
        }

        internal void CreateNewConnection()
        {
            ConnectionModel newConnection = new ConnectionModel();
            newConnection.ConnectionName = "New Connection";
            newConnection.ConnectionDescription = "???";
            newConnection.ConnectionStatus = "???";
            newConnection.ConnectionAdress = "???";

            ObservableCollection<ConnectionModel> updatedGroup = new ObservableCollection<ConnectionModel>();
            if (CurrentModel.GroupConnections != null)
            {
                foreach (var connection in currentModel.GroupConnections)
                {
                    updatedGroup.Add(connection);
                }
            }
            else
                CurrentModel.GroupConnections = new ObservableCollection<ConnectionModel>();

            updatedGroup.Add(newConnection);

            CurrentModel.GroupConnections = updatedGroup;
        }
    }
}
