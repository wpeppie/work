using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputFileComparer
{
    public class Config
    {
        private string[] _keyColumns;
        private string _firstFile;
        private string _secondFile;
        private string _dateColumnName;
        private int? _firstFileHeaderLine;
        private int? _secondFileHeaderLine;

        public int? SecondFileHeaderLine
        {
            get { return _secondFileHeaderLine; }
            set { _secondFileHeaderLine = value; }
        }

        public int? FirstFileHeaderLine
        {
            get { return _firstFileHeaderLine; }
            set { _firstFileHeaderLine = value; }
        }

        public string DateColumnName
        {
            get { return _dateColumnName; }
            set { _dateColumnName = value; }
        }

        public string[] KeyColumns
        {
            get { return _keyColumns; }
            set { _keyColumns = value; }
        }
        public string FirstFile
        {
            get { return _firstFile; }
            set { _firstFile = value; }
        }

        public string SecondFile
        {
            get { return _secondFile; }
            set { _secondFile = value; }
        }
    }
}
