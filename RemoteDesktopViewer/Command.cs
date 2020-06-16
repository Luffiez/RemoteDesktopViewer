using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace RemoteDesktopViewer
{
    public static class Command
    {
        #region DLLImports - quser is a 32-bit command
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64EnableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int Wow64RevertWow64FsRedirection(ref IntPtr ptr);
        #endregion

        /// <summary>
        /// Updates the station information based on its return status.
        /// </summary>
        /// <param name="station">Which station to check</param>
        /// <param name="mainWindow">Referens to the MainWindow</param>
        public static void UpdateStationStatus(IStation station, MainWindow mainWindow)
        {
            // Get the return status
            string status = StationOccupied(station).ToString();

            // Set the station status for station in list
            mainWindow.SetStationStatus(station, status);

            // Set the station status for selected station box
            mainWindow.SetSelectedStationStatus();
        }

        enum ReturnStatus { Available, Occupied, Unknown };

        /// <summary>
        /// Checks status of station based on output from the command quser
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private static ReturnStatus StationOccupied(IStation station)
        {
            IntPtr val = IntPtr.Zero;
            Wow64DisableWow64FsRedirection(ref val);

            // Setup quser-process info
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c quser /server:" + station.Ip,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };

            List<string> lines = new List<string>();

            //Start the process
            proc.Start();
            ReturnStatus status = ReturnStatus.Unknown;
            station.SessionID = string.Empty;

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
                        station.SessionID = connectionDetails[2];
                        
                        //connectione exists and is occupied
                        status = ReturnStatus.Occupied;
                        break;
                    }

                    //connection exists and is available, no ID is joinable.
                    status = ReturnStatus.Available;
                    break;
                }
            }
          
            proc.Dispose();
            Wow64EnableWow64FsRedirection(ref val);

            return status;  
        }
    }
}
