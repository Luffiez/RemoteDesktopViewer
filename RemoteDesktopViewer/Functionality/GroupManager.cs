using Newtonsoft.Json;
using RemoteDesktopViewer.Model;
using System.Collections.Generic;
using System.IO;

namespace RemoteDesktopViewer.Functionality
{
    public static class GroupManager
    {
        public static string GROUP_PATH = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Groups\\";
        public static List<ConnectionGroupModel> LoadGroups()
        {
            List<string> jsonFiles = new List<string>();
            List<ConnectionGroupModel> groups = new List<ConnectionGroupModel>();
            jsonFiles.AddRange(Directory.GetFiles(GROUP_PATH));

            foreach (string file in jsonFiles)
            {
                string jsonGroup = File.ReadAllText(file);
                groups.Add(JsonConvert.DeserializeObject<ConnectionGroupModel>(jsonGroup));
            }

            return groups;
        }

        public static void SaveGroup(ConnectionGroupModel originalGroupSettings, ConnectionGroupModel groupToSave)
        {
            string jsonGroup = JsonConvert.SerializeObject(groupToSave);

            if (!Directory.Exists(GROUP_PATH))
                Directory.CreateDirectory(GROUP_PATH);

            if (originalGroupSettings != null && File.Exists(Path.Combine(GROUP_PATH, originalGroupSettings.GroupName)))
                File.Delete(Path.Combine(GROUP_PATH, originalGroupSettings.GroupName));

            File.WriteAllText(Path.Combine(GROUP_PATH, groupToSave.GroupName), jsonGroup);
        }

        public static void DeleteGroup(ConnectionGroupModel group)
        {
            if (File.Exists(Path.Combine(GROUP_PATH, group.GroupName)))
                File.Delete(Path.Combine(GROUP_PATH, group.GroupName));
        }
    }
}
