using RemoteDesktopViewer.Functionality;
using RemoteDesktopViewer.Model;
using RemoteDesktopViewer.View;
using System;
using System.Collections.Generic;

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
            EventHandler< ConnectionGroupModel> localCopy = GroupUpdated;

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
            List<ConnectionModel> connections = new List<ConnectionModel>();

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
            List<ConnectionModel> updatedGroup = new List<ConnectionModel>();
            updatedGroup.AddRange(currentModel.GroupConnections);
            updatedGroup.Remove(connectionToDelete);
            CurrentModel.GroupConnections = updatedGroup;
        }

        public void OpenWindow_Create()
        {
            CurrentModel = new ConnectionGroupModel();
            window = new GroupManagerWindow(this);
            window.ShowDialog();
        }

        internal void SaveGroup()
        {
            // Save Model
            GroupManager.SaveGroup(originalModel, CurrentModel);
            OnGroupUpdated();
            //originalModel = CurrentModel;
        }

        internal void CreateNewConnection()
        {
            ConnectionModel newConnection = new ConnectionModel();
            newConnection.ConnectionName = "New Connection";
            newConnection.ConnectionDescription = "???";
            newConnection.ConnectionStatus = "???";
            newConnection.ConnectionAdress = "???";

            List<ConnectionModel> updatedGroup = new List<ConnectionModel>();
            if (CurrentModel.GroupConnections != null)
                updatedGroup.AddRange(currentModel.GroupConnections);
            else
                CurrentModel.GroupConnections = new List<ConnectionModel>();

            updatedGroup.Add(newConnection);

            CurrentModel.GroupConnections = updatedGroup;
        }
    }
}
