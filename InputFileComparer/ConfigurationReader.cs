using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Interface;
using System.IO;

namespace InputFileComparer
{
    public class ConfigurationReader
    {
        public static Config GetConfiguration(IFileSystem fileSystem, string configFileName)
        {
            if (!fileSystem.FileExists(configFileName))
            {
                throw new FileNotFoundException("The file '" + configFileName + "' was not found");
            }
            return (Config) fileSystem.ReadXml(configFileName, typeof(Config));
        }
    }
}
