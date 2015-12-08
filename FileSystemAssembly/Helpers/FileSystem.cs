using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSystemAssembly.Helpers
{
    public class FileSystem: Interface.IFileSystem
    {
        bool Interface.IFileSystem.FileExists(string fileName)
        {
            throw new NotImplementedException();
        }

        bool Interface.IFileSystem.FolderExists(string folderName)
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
