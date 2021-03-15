using System;
using UsageWatcher.Helpers;
using UsageWatcher.Models;
using UsageWatcher.Enums;

namespace UsageWatcher.Service
{
    internal class SaveService : ISaveService
    {
        private const string TODAY_PREFIX = "td_";
        private const string FILE_SUFFIX = "_usagestore.json";

        private readonly SavePreference preference;
        private readonly DataPrecision precision;
        private readonly string savePrefix;

        public SaveService(string savePrefix, SavePreference preference, DataPrecision precision)
        {
            this.savePrefix = savePrefix;
            this.preference = preference;
            this.precision = precision;
        }

        public void Save(IUsageKeeper keeper)
        {
            switch (preference)
            {
                case SavePreference.KeepDataForToday:
                    SaveOnlyKeepingDate(ref keeper, DateTime.Now.Date);
                    break;

                case SavePreference.KeepDataForAWeek:
                    SaveOnlyKeepingDate(ref keeper, DateTime.Now.Date);
                    break;

                case SavePreference.KeepDataForTwoWeeks:
                    SaveOnlyKeepingDate(ref keeper, DateTime.Now.Date);
                    break;

                case SavePreference.KeepDataForFourWeeks:
                    SaveOnlyKeepingDate(ref keeper, DateTime.Now.Date);
                    break;

                case SavePreference.KeepDataForever:
                    SaveKeepingEverything(ref keeper);
                    break;

                default:
                    break;
            }
        }

        public IUsageKeeper GetSavedUsages()
        {
            IUsageKeeper keeper = null;
            switch (precision)
            {
                case DataPrecision.High:
                    string path = GetSaveDirLocation() + GetSaveFileName();
                    keeper = Serializer.JsonObjectDeserialize<HighPrecisionUsageKeeper>(path);
                    break;

                case DataPrecision.Low:
                    throw new NotImplementedException();
                //break;

                default:
                    break;
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

        private void SaveOnlyKeepingDate(ref IUsageKeeper keeper, DateTime date)
        {
            keeper.EraseUsageNotOfDate(date.Date);

            Serializer.JsonObjectSerialize(GetSaveDirLocation(), GetSaveFileName(), ref keeper, DoBackup.Yes);
        }

        private void SaveKeepingEverything(ref IUsageKeeper keeper)
        {
            Serializer.JsonObjectSerialize(GetSaveDirLocation(), GetSaveFileName(), ref keeper, DoBackup.Yes);
        }

        private string GetSaveFileName()
        {
            string fileName = String.Empty;
            switch (preference)
            {
                case SavePreference.KeepDataForToday:
                    fileName = TODAY_PREFIX + savePrefix + FILE_SUFFIX;
                    break;

                case SavePreference.KeepDataForever:
                    fileName = savePrefix + FILE_SUFFIX;
                    break;

                default:
                    break;
            }

            return fileName;
        }

        private static string GetSaveDirLocation()
        {
            string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return userAppData + "\\Usagewatcher\\";
        }
    }
}
