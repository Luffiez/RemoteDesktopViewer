
using System;

namespace RemoteDesktopViewer.Model
{
    public class ConnectionModel : BaseModel
    {
        private string connectionName;
        private string connectionDescription;
        private string connectionStatus;
        private string connectionAdress;
        private string connectionID;
        private string connectionUser;
        private DateTime lastUpdated;


        public string ConnectionName
        {
            get
            {
                return connectionName;
            }
            set
            {
                connectionName = value;
                OnPropertyChanged("ConnectionName");
            }
        }

        public string ConnectionDescription
        {
            get
            {
                return connectionDescription;
            }
            set
            {
                connectionDescription = value;
                OnPropertyChanged("ConnectionDescription");
            }
        }

        public string ConnectionStatus
        {
            get
            {
                return connectionStatus;
            }
            set
            {
                connectionStatus = value;
                OnPropertyChanged("ConnectionStatus");
            }
        }

        public string ConnectionAdress
        {
            get
            {
                return connectionAdress;
            }
            set
            {
                connectionAdress = value;
                OnPropertyChanged("ConnectionAdress");
            }
        }

        public string ConnectionID
        {
            get
            {
                return connectionID;
            }
            set
            {
                connectionID = value;
                OnPropertyChanged("ConnectionID");
            }
        }

        public string ConnectionUser
        {
            get
            {
                return connectionUser;
            }
            set
            {
                connectionUser = value;
                OnPropertyChanged("ConnectionUser");
            }
        }

        public DateTime LastUpdated
        {
            get
            {
                return lastUpdated;
            }
            set
            {
                lastUpdated = value;
                OnPropertyChanged("LastUpdated");
            }
        }
    }
}
