using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Interfaces;

namespace FileSystemAssembly.Helpers
{
    public class FileSystem: IFileSystem
    {
        public bool FileExists(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool FolderExists(string folderName)
        {
            throw new NotImplementedException();
        }

        public List<string> ReadFile(string fileName)
        {
            throw new NotImplementedException();
        }


        public object ReadXml(string configFileName, Type type)
        {
            throw new NotImplementedException();
        }

        public void WriteFile(string fileName, string[] outputLines)
        {
            throw new NotImplementedException();
        }
    }
}
