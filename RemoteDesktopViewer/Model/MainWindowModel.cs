using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktopViewer.Model
{
    public class MainWindowModel : BaseModel
    {
        private IList<ConnectionGroupModel> groups;
        private IList<ConnectionModel> connections;

        public void AddGroup(ConnectionGroupModel group)
        {
            Groups.Add(group);
            OnPropertyChanged("Groups");
        }

        public IList<ConnectionGroupModel> Groups
        {
            get { return groups; }
            set
            {
                groups = value;
                OnPropertyChanged("Groups");
            }
        }

        public IList<ConnectionModel> Connections
        {
            get { return connections; }
            set
            {
                connections = value;
                OnPropertyChanged("Connections");
            }
        }
    }
}
