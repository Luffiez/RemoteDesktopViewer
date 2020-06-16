using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RemoteDesktopViewer
{
    public static class XmlHelper
    {
        /// <summary>
        /// Used to load an xml file from provided path and deserialize it with the provided T-type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadXml<T>(string path)
        {
            using (TextReader stringReader = new StreamReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        } 

        /// <summary>
        /// Used to serialize and save provided T-type into provided path as XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        public static void SaveXml<T>(string path, T obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                try 
                { 
                    xmlSerializer.Serialize(writer, obj);
                    writer.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error Serializing XML-file. Error: " + e.Message);
                }
            }
        }
    }
}
