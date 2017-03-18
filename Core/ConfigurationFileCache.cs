using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Cfg;

namespace Stock.Core
{
    public class ConfigurationFileCache
    {
        private readonly string _cacheFile;
        private readonly Assembly _definitionsAssmbly;

        public ConfigurationFileCache(Assembly definitionsAssembly)
        {
            _definitionsAssmbly = definitionsAssembly;
            
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _cacheFile = currentDirectory + "nh.cfg";
        }

        public void DeleteCacheFile()
        {
            if (File.Exists(_cacheFile))
                File.Delete(_cacheFile);
        }

        public bool IsConfigurationFileValid
        {
            get
            {
                if (!File.Exists(_cacheFile))
                    return false;
                var configInfo = new FileInfo(_cacheFile);
                var asmInfo = new FileInfo(_definitionsAssmbly.Location);

                if (configInfo.Length < 5*1024)
                    return false;

                return configInfo.LastWriteTime >= asmInfo.LastWriteTime;
            }
        }

        public void SaveConfigurationToFile(Configuration configuration)
        {
            using (var file = File.Open(_cacheFile, FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, configuration);
            }
        }

        public Configuration LoadConfigurationFromFile()
        {
            if (!IsConfigurationFileValid)
                return null;

            using (var file = File.Open(_cacheFile, FileMode.Open, FileAccess.Read))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(file) as Configuration;
            }
        }
    
    }
}
