using EasyState.Core.Utility;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace EasyState.Data
{
    public class DatabaseFile<TDataModel> where TDataModel : new()
    {
        private readonly static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        protected string dbFilePath;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };
        public DatabaseFile(string dbFileName)
        {
            dbFilePath = Path.Combine(FilePaths.EditorDataFolder, dbFileName);
        }

        public TDataModel Load()
        {
            TDataModel model;
            if (!FileExists())
            {
                model = new TDataModel();

            }
            else
            {
                string json = string.Empty;
                _readWriteLock.EnterReadLock();
                try
                {
                    json = ReadFile();
                }
                finally
                {
                    _readWriteLock.ExitReadLock();
                }
                model = JsonConvert.DeserializeObject<TDataModel>(json);

            }
            InitializeDatabaseFile(model);
            return model;
        }

        public void Save(TDataModel data)
        {
            data = SanitizeData(data);

            string json = JsonConvert.SerializeObject(data, Formatting.None, _serializerSettings);
            bool isUpdate = FileExists();
            if (isUpdate)
            {

                string currentData = string.Empty;
                _readWriteLock.EnterReadLock();
                try
                {
                    currentData = ReadFile();
                }
                finally
                {
                    _readWriteLock.ExitReadLock();
                }
                if(json == currentData)
                {
                    //no file changes 
                    return;
                }
            }
            _readWriteLock.EnterWriteLock();
            try
            {
                File.WriteAllText(dbFilePath, json);
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }

            OnFileChanged(data, isUpdate);
            EasyStateFileEvents.OnFileChanged(typeof(TDataModel).Name);
        }
        public void DeleteFile()
        {
            if (FileExists())
            {
                File.Delete(dbFilePath);
                OnFileDeleted();
            }
        }
        protected virtual void InitializeDatabaseFile(TDataModel dataModel) { }
        protected virtual TDataModel SanitizeData(TDataModel data) => data;
        protected virtual void OnFileDeleted() { }
        protected virtual void OnFileChanged(TDataModel data, bool isUpdate) { }
        protected void LogFileChange(string message)
        {
            if (EasyState.Settings.EasyStateSettings.Instance.LogFileChanges)
            {
                UnityEngine.Debug.Log(message + " To stop these message set LogFileChanges to false in easy state settings.");
            }
        }
        private bool FileExists()
        {
            if (dbFilePath.Contains("://"))
            {
                return true;
            }
            return File.Exists(dbFilePath);
        }
        private string ReadFile()
        {
            if (dbFilePath.Contains("://"))
            {
#pragma warning disable CS0618
                WWW request = new WWW(dbFilePath);
                // max timeout 5 seconds
                int maxTries = 1000 * 5;
                int safetyCount = 0;
                while (!request.isDone)
                {
                    Thread.Sleep(1);
                    safetyCount++;
                    if (safetyCount > maxTries)
                    {
                        throw new System.Exception("Something went wrong trying to load file, aborting after waiting for 5000ms.");
                    }
                }
                return request.text;
#pragma warning restore CS0618 
            }
            else
            {
                return File.ReadAllText(dbFilePath);
            }

        }
    }
}