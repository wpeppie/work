using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Interfaces;

namespace InputFileComparer
{
    public abstract class LineCollection
    {
        protected Config _config;
        protected IFileSystem _fileSystem;
        protected List<string> _skippableKeys = new List<string>();
        protected Dictionary<string, Line> _lines = new Dictionary<string, Line>();

        protected string _headerLine;
        protected int[] _keyColumns;
        protected int[] _comparisonColumns;
        protected int _dateColumn;
        
        protected void ParseHeaderLine(List<string> lines, int headerLine)
        {
            var items = lines[headerLine].ToUpperInvariant().Split(_config.ColumnsSeparator.ToArray()
                , StringSplitOptions.None);
            _headerLine = lines[headerLine];
            _keyColumns = ColumnNameParser.GetColumnNumbersFromHeaderLine(items, _config.KeyColumns);
            _comparisonColumns = ColumnNameParser.GetColumnNumbersFromHeaderLine(items, _config.ComparisonColumns);
            _dateColumn = ColumnNameParser.GetSingleColumnNumberFromHeaderLine(items, _config.DateColumnName);
            lines.RemoveAt(headerLine);
        }

        protected bool KeyAlreadyExists(string key)
        {
            return (_lines.Keys.Contains(key));
        }

        public int NumberOfComparableLines
        {
            get
            { return _lines.Count; }
        }
    }
}
