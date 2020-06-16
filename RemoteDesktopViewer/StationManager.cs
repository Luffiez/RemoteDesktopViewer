using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace RemoteDesktopViewer
{
    public class StationManager
    {
        public Stations stations { get; set; }     
        public string path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\StationList.xml";
        MainWindow mainWindow;
          
        public StationManager(MainWindow window)
        {
            mainWindow = window;
        }

        /// <summary>
        /// Loads the local station list and refreshes the UI.
        /// </summary>
        public void LoadLocalStationList()
        {
            if(!File.Exists(path))
            {
                MessageBox.Show($"File does not exist in path : {path}!\nPlease update Xml path or make sure the connection to 'shares' is active.");
                mainWindow.Click_Import(null, null);
                return;
            }

            var xml = XmlHelper.LoadXml<Stations>(path);
            if (xml != null)
            {
                if (stations == null)
                    stations = new Stations();

                stations.list = OrderByGroup(xml);
                mainWindow.ReloadStationListUI();
            }
            else
            {
                MessageBox.Show($"Could not load station list from path: {path}");
            }
        }

        /// <summary>
        /// Imports a station list and saves it as the local station list and refreshes the UI.
        /// </summary>
        /// <param name="remotePath"></param>
        public void LoadRemoteStationList(string remotePath)
        {
            if (!File.Exists(remotePath))
            {
                MessageBox.Show($"File does not exist in path : {remotePath}!\nPlease update Xml path or make sure the connection to 'shares' is active.");
                mainWindow.Click_Import(null, null);
                return;
            }

            var xml = XmlHelper.LoadXml<Stations>(remotePath);
            if (xml != null)
            {
                if (stations == null)
                    stations = new Stations();

                stations.list = OrderByGroup(xml);
                mainWindow.ReloadStationListUI();
            }
            else
            {
                MessageBox.Show($"Could not load station list from path: {remotePath}");
            }
        }

        /// <summary>
        /// Saves the list of stations to the stationlist.xml
        /// </summary>
        public void SaveLocalStationList()
        {
            mainWindow.ReloadStationListUI();
            XmlHelper.SaveXml(path, stations);
        }

        /// <summary>
        /// Exports the local station list to the specified path as an XML-file.
        /// </summary>
        /// <param name="_path"></param>
        public void ExportStationList(string _path)
        {
            XmlHelper.SaveXml(_path, stations);
        }

        /// <summary>
        /// Sorts stations by group. A first, B second and C last.
        /// </summary>
        /// <param name="stations">The list of stations to sort</param>
        /// <returns></returns>
        List<IStation> OrderByGroup(Stations stations)
        {
            List<IStation> desktops = new List<IStation>();
            List<IStation> servers = new List<IStation>();
            List<IStation> others = new List<IStation>();

            foreach (IStation s in stations.list)
            {
                switch (s.Group)
                {
                    case StationGroup.Desktop:
                        desktops.Add(s);
                        break;
                    case StationGroup.Server:
                        servers.Add(s);
                        break;
                    case StationGroup.Other:
                        others.Add(s);
                        break;
                    default:
                        desktops.Add(s);
                        break;
                }
            }
            
            return desktops.Concat(servers).Concat(others).ToList();
        }

        internal void UpdateStationData(Stations stationList)
        {
            stations.list = stationList.list;
            mainWindow.ReloadStationListUI();
        }

        /// <summary>
        /// Attempts to add a new station to the list. Returns FALSE if station with given name alread exists.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public bool AddStationToList(IStation station)
        {
            LoadLocalStationList();
            foreach (IStation s in stations.list)
            {
                if(s.Name == station.Name)
                {
                    return false;
                }
            }
            stations.list.Add(station);
            SaveLocalStationList();
            return true;
        }

        /// <summary>
        /// Removes the specified station ID from the list of stations.
        /// </summary>
        /// <param name="station"></param>
        public void RemoveStationFromList(int station)
        {
            LoadLocalStationList();
            stations.list.RemoveAt(station);
            SaveLocalStationList();
        }
    }

    public class Stations
    {
        public List<IStation> list { get; set; }
    }

    public enum StationGroup { Desktop, Server, Other };

    public class IStation
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Ip { get; set; }

        public StationGroup Group { get; set; }
        public string SessionID { get; set; }
    }
}
