using System;
using UsageWatcher.Helpers;
using UsageWatcher.Models;
using UsageWatcher.Enums;
using UsageWatcher.Models.HighPrecision;
using UsageWatcher.Native;

namespace UsageWatcher.Service
{
    internal class SaveService : ISaveService
    {
        private const string TODAY_PREFIX = "td_";
        private const string ARCHIVE_PREFIX = "arc_";
        private const string FILE_SUFFIX = "_usage.json";

        private readonly SavePreference preference;
        private readonly DataPrecision precision;
        private readonly string appName;

        public SaveService(string appName, SavePreference preference, DataPrecision precision)
        {
            this.appName = appName;
            this.preference = preference;
            this.precision = precision;
        }

        public void Save(IUsageKeeper keeper, SaveType type)
        {
            if (preference == SavePreference.NoSave)
            {
                return;
            }

            Serializer.JsonObjectSerialize(GetSaveDirLocation(), GetSaveFileName(type), ref keeper, DoBackup.Yes);
        }
        
        public IUsageKeeper GetSavedUsages(SaveType type)
        {
            IUsageKeeper keeper;
            if (precision == DataPrecision.High)
            {
                string path = GetSaveDirLocation() + GetSaveFileName(type);

                keeper = type == SaveType.Today
                                    ? Serializer.JsonObjectDeserialize<HighPrecisionUsageToday>(path)
                                    : Serializer.JsonObjectDeserialize<HighPrecisionUsageKeeper>(path);
            } else
            {
                throw new NotImplementedException();
            }

            return keeper;
        }

        public SavePreference GetSavePreference()
        {
            return preference;
        }

        public DataPrecision GetDataPrecision()
        {
            return precision;
        }

        private string GetSaveFileName(SaveType type)
        {
            string prefix = type == SaveType.Today ? TODAY_PREFIX : ARCHIVE_PREFIX;

            return  prefix + appName + FILE_SUFFIX;
        }

        private static string GetSaveDirLocation()
        {
            string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return userAppData + "\\Usagewatcher\\";
        }
    }
}
