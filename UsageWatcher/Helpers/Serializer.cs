﻿using Newtonsoft.Json;
using System.IO;

namespace UsageWatcher.Helpers
{
    /// <summary>
    /// Keeps all the serializer methods
    /// </summary>
    internal static class Serializer
    {
        internal static void JsonObjectSerialize<T>(string saveDir, string fileName, ref T serializable, DoBackup doBackup)
        {
            string path = CreateSavePath(saveDir, fileName);
            CreateDirIfDoesntExist(saveDir);
            CreateBackupIfNeeded(path, doBackup);

            TextWriter writer = null;
            try
            {
                string output = JsonConvert.SerializeObject(serializable);
                writer = new StreamWriter(path, false);
                writer.Write(output);
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        internal static T JsonObjectDeserialize<T>(string path)
        {
            if (new FileInfo(path).Exists)
            {
                TextReader reader = null;
                try
                {
                    reader = new StreamReader(path);
                    string fileContents = reader.ReadToEnd();

                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    };

                    return JsonConvert.DeserializeObject<T>(fileContents, jsonSerializerSettings);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            else
            {
                return default(T);
            }
        }

        private static string CreateSavePath(string dirName, string fileName)
        {
            if (!dirName.EndsWith("\\", System.StringComparison.Ordinal))
            {
                dirName += "\\";
            }

            return dirName + fileName;
        }

        private static void CreateDirIfDoesntExist(string dirName)
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }

        private static void CreateBackupIfNeeded(string path, DoBackup doBackup)
        {
            if (DoBackup.Yes == doBackup && File.Exists(path))
            {
                string backupPath = path + ".bak";
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }

                File.Copy(path, backupPath);
            }
        }
    }

    internal enum DoBackup
    {
        Yes,
        No
    }
}
