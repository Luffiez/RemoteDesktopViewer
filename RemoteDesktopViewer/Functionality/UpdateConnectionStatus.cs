using RemoteDesktopViewer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktopViewer.Functionality
{
    public class UpdateConnectionStatus
    {
        #region DLLImports - quser is a 32-bit command
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64EnableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64RevertWow64FsRedirection(ref IntPtr ptr);
        #endregion

        public async Task GetConnectionStatus(ConnectionModel connection)
        {
            IntPtr val = IntPtr.Zero;
            Wow64DisableWow64FsRedirection(ref val);

            // Setup quser-process info
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c quser /server:" + connection.ConnectionAdress,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };

            List<string> lines = new List<string>();

            //Start the process
            proc.Start();
            string status = "Unknown";
            string sessionID = string.Empty;
            string user = string.Empty;

            // Listen for all output by process
            while (!proc.StandardOutput.EndOfStream)
            {
                // Read every line that is output, this waits until new line is available.
                string line = proc.StandardOutput.ReadLine();
                if (line != null)
                {
                    lines.Add(line.ToLower());
                }

                // If lines contains more than 1, users listed on station.
                if (lines.Count > 1)
                {
                    // Split all lines between spaces
                    List<string> connectionDetails = new List<string>();
                    connectionDetails.AddRange(lines[1].Split(' '));
                    connectionDetails = connectionDetails.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    // Found what we wanted from the process, it can be disposed
                    proc.Dispose();
                    Wow64EnableWow64FsRedirection(ref val);

                    // If one of the split strings contains 'active', then there is an active connection.
                    if (connectionDetails.Count > 3 && connectionDetails[3] == "active")
                    {
                        // ConnectionDetails[2] contains the RDP-ID for the active session,
                        // needed if we want to join this session.
                        sessionID = connectionDetails[2];

                        //connectione exists and is occupied
                        status = "Occupied";

                        //Set connected user
                        user = connectionDetails[0];
                        break;
                    }

                    //connection exists and is available, no ID is joinable.
                    status = "Available";
                    break;
                }
            }

            proc.Dispose();
            Wow64EnableWow64FsRedirection(ref val);

            connection.ConnectionStatus = status;
            connection.ConnectionID = sessionID;
            connection.ConnectionUser = user;

        }
    }
}
