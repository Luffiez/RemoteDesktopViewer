using System.Collections.Generic;

namespace RemoteDesktopViewer.Model
{
    public class ConnectionGroupModel : BaseModel
    {
        private string groupName;
        private IList<ConnectionModel> groupConnections;

        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        public IList<ConnectionModel> GroupConnections
        {
            get { return groupConnections; }
            set
            {
                groupConnections = value;
                OnPropertyChanged("GroupConnections");
            }
        }
    }
}
