using Newtonsoft.Json;
using RemoteDesktopViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace RemoteDesktopViewer.Functionality
{
    public static class GroupManager
    {
        public static string GROUP_PATH = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\RemoteDesktopViewer\\Groups\\";
        public static ObservableCollection<ConnectionGroupModel> LoadGroups()
        {

            if (!Directory.Exists(GROUP_PATH))
                Directory.CreateDirectory(GROUP_PATH);

            List<string> jsonFiles = new List<string>();
            ObservableCollection<ConnectionGroupModel> groups = new ObservableCollection<ConnectionGroupModel>();

            var files = Directory.GetFiles(GROUP_PATH);
            if (files.Length == 0)
                return groups;

            jsonFiles.AddRange(files);

            foreach (string file in jsonFiles)
            {
                string jsonGroup = File.ReadAllText(file);
                groups.Add(JsonConvert.DeserializeObject<ConnectionGroupModel>(jsonGroup));
            }

            return groups;
        }

        public static void SaveGroup(ConnectionGroupModel originalGroupSettings, ConnectionGroupModel groupToSave)
        {
            if (groupToSave == null)
                return;

            string jsonGroup = JsonConvert.SerializeObject(groupToSave);

            if (!Directory.Exists(GROUP_PATH))
                Directory.CreateDirectory(GROUP_PATH);

            if (originalGroupSettings != null && File.Exists(Path.Combine(GROUP_PATH, originalGroupSettings.GroupName)))
                File.Delete(Path.Combine(GROUP_PATH, originalGroupSettings.GroupName));

            File.WriteAllText(Path.Combine(GROUP_PATH, groupToSave.GroupName), jsonGroup);
        }

        public static void DeleteGroup(ConnectionGroupModel group)
        {
            string filePath = Path.Combine(GROUP_PATH, group.GroupName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static ConnectionGroupModel ImportGroup(string path)
        {
            string groupJson = File.ReadAllText(path);
            ConnectionGroupModel groupModel = JsonConvert.DeserializeObject<ConnectionGroupModel>(groupJson);

            string destinationPath = Path.Combine(GROUP_PATH + groupModel.GroupName);

            File.WriteAllText(destinationPath, groupJson);

            return groupModel;
        }

        public static void Sort<T>(this ObservableCollection<T> collection)
        where T : IComparable<T>, IEquatable<T>
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();

            int ptr = 0;
            while (ptr < sorted.Count - 1)
            {
                if (!collection[ptr].Equals(sorted[ptr]))
                {
                    int idx = search(collection, ptr + 1, sorted[ptr]);
                    collection.Move(idx, ptr);
                }

                ptr++;
            }
        }

        public static int search<T>(ObservableCollection<T> collection, int startIndex, T other)
        {
            for (int i = startIndex; i < collection.Count; i++)
            {
                if (other.Equals(collection[i]))
                    return i;
            }

            return -1; // decide how to handle error case
        }
    }
}
