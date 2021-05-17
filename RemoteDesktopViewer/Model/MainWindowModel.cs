using RemoteDesktopViewer.Functionality;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RemoteDesktopViewer.Model
{
    public class MainWindowModel : BaseModel
    {
        private ObservableCollection<ConnectionGroupModel> groups;
        private IList<ConnectionModel> connections;

        public ObservableCollection<ConnectionGroupModel> Groups
        {
            get { return groups; }
            set
            {
                groups = null;
                groups = value;
                groups.Sort();
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
