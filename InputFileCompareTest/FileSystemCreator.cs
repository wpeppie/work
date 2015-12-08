using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Interface;
using NSubstitute;

namespace InputFileCompareTest
{
    internal class FileSystemCreator
    {
        internal static IFileSystem GetFileSystem()
        {
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.FileExists(Arg.Any<string>()).Returns(true);
            fileSystem.FileExists("FileDoesNotExist").Returns(false);
            fileSystem.FolderExists(Arg.Any<string>()).Returns(true);
            fileSystem.FolderExists(@"C:\Temp\DoesNotExist").Returns(false);
            fileSystem.ReadFile(@"C:\Temp\File1.txt").Returns(GetFile1Content());
            fileSystem.ReadFile(@"C:\Temp\File2.txt").Returns(GetFile2Content());
            fileSystem.ReadXml(Arg.Any<string>(), Arg.Any<Type>()).Returns(ConfigurationCreator.CreateCorrectConfig());
            return fileSystem;
        }

        private static List<string> GetFile1Content()
        {
            var lines = new List<string>();
            lines.Add("#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5");
            lines.Add("ID1|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID2|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID3|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID4|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID5|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID6|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID7|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID7|DATE2|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID8|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            return lines;
        }

        private static List<string> GetFile2Content()
        {
            var lines = new List<string>();
            lines.Add("#BBUNIQUE|DATE|COL1|COL2|COL3|COL4|COL5");
            lines.Add("ID1|DATE1|ValueOther|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID2|DATE1|ValueSame|ValueOther|ValueSame|ValueSame|ValueSame");
            lines.Add("ID3|DATE1|ValueSame|ValueSame|ValueOther|ValueSame|ValueSame");
            lines.Add("ID4|DATE1|ValueSame|ValueSame|ValueSame|ValueOther|ValueSame");
            lines.Add("ID5|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueOther");
            lines.Add("ID6|DATE1|ValueSame|ValueSame|ValueSame|ValueSame|ValueSame");
            lines.Add("ID7|DATE1|ValueSame|ValueOther|ValueSame|ValueSame|ValueOther");
            lines.Add("ID8|DATE1|ValueOther|ValueOther|ValueOther|ValueOther|ValueOther");
            lines.Add("ID8|DATE2|ValueOther|ValueOther|ValueOther|ValueOther|ValueOther");
            lines.Add("ID8|DATE3|ValueOther|ValueOther|ValueOther|ValueOther|ValueOther");
            return lines;
        }
    }
}
