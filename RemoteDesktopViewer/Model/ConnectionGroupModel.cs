using System;
using System.Collections.ObjectModel;

namespace RemoteDesktopViewer.Model
{
    public class ConnectionGroupModel : BaseModel, IComparable<ConnectionGroupModel>, IEquatable<ConnectionGroupModel>
    {
        private string groupName;
        private ObservableCollection<ConnectionModel> groupConnections;

        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        public ObservableCollection<ConnectionModel> GroupConnections
        {
            get { return groupConnections; }
            set
            {
                groupConnections = value;
                OnPropertyChanged("GroupConnections");
            }
        }

        public int CompareTo(ConnectionGroupModel other)
        {
            if (this.GroupName == other.groupName) 
                return 0;

            return this.GroupName.CompareTo(other.GroupName);
        }

        public bool Equals(ConnectionGroupModel other)
        {
            if (this.GroupName.Equals(other.groupName) && this.GroupConnections.Equals(other.groupConnections))
                return true;

            return false;
        }
    }
}
