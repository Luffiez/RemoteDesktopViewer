
namespace RemoteDesktopViewer.Model
{
    public class ConnectionDetailsModel : BaseModel
    {
        private string connectionDetails;
        private string connectButton;

        public string ConnectionDetails
        {
            get { return connectionDetails; }
            set
            {
                connectionDetails = value;
                OnPropertyChanged("ConnectionDetails");
            }
        }

        public string ConnectButton
        {
            get { return connectButton; }
            set
            {
                connectButton = value;
                OnPropertyChanged("ConnectButton");
            }
        }
    }
}
