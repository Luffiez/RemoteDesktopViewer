using RemoteDesktopViewer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RemoteDesktopViewer.Functionality
{
    public class ConnectionManager
    {
        public void ConnectToStation(ConnectionModel station, bool takeover)
        {
            if (station == null)
            {
                return;
            }

            if (station.ConnectionStatus == "Occupied" && !takeover)
            {
                MessageBox.Show("This station is currently occupied by another user.\nRight-Click allows sesison join/take over", "Session Occupied");
                return;
            }

            string process = "";
            process += " /v:" + station.ConnectionAdress;

            Process RemoteConnectProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mstsc.exe",
                    Arguments = process,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Maximized,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            RemoteConnectProcess.Start();
            RemoteConnectProcess.WaitForInputIdle();
            Connected(station);

            var outputResultPromise = RemoteConnectProcess.StandardOutput.ReadToEndAsync();
            outputResultPromise.ContinueWith(o => Disconnected(station));
        }

        public void JoinStation(ConnectionModel connectionModel)
        {
            if (connectionModel.ConnectionID != null)
            {
                MessageBoxResult overtake = MessageBox.Show($"This station is currently being used by another user. \nWould you like to join the session?", "Session Occupied", MessageBoxButton.YesNo);
                if (overtake == MessageBoxResult.Yes)
                {
                    Process shadowProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "mstsc.exe",
                            Arguments = $"/shadow:{connectionModel.ConnectionID} /v:{connectionModel.ConnectionAdress} /control",
                            CreateNoWindow = true
                        }
                    };

                    shadowProcess.Start();
                    shadowProcess.WaitForExit();
                }
            }
        }

        private void Connected(ConnectionModel station)
        {
            station.ConnectionStatus = "Occupied";
        }

        private void Disconnected(ConnectionModel station)
        {
            station.ConnectionStatus = "Available";
        }
    }
}
