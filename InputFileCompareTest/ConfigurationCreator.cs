using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputFileCompareTest
{
    internal class ConfigurationCreator
    {
        internal static InputFileComparer.Config CreateCorrectConfig()
        {
            var config = new InputFileComparer.Config();
            config.KeyColumns = new[] { "#BBUNIQUE" };
            config.ComparisonColumns = new[] { "COL1" };
            config.FirstFile = @"C:\Temp\File1.txt";
            config.SecondFile = @"C:\Temp\File2.txt";
            config.DateColumnName = "DATE";
            config.OutputFileName = @"C:\Temp\File3.txt";
            config.FirstFileHeaderLine = 0;
            config.SecondFileHeaderLine = 0;
            config.ColumnsSeparator = "|";
            return config;
        }
    }
}
