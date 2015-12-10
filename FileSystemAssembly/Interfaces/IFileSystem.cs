using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSystemAssembly.Interfaces
{
    public interface IFileSystem
    {
        bool FileExists(string fileName);
        bool FolderExists(string folderName);
        List<string> ReadFile(string fileName);
        object ReadXml(string configFileName, Type type);
        void WriteFile(string fileName, string[] outputLines);
    }
}
