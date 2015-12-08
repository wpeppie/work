using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Interface;
using System.Collections;
using System.Globalization;

namespace InputFileComparer
{
    public class FileLineCollection
    {
        private Config _config;
        private IFileSystem _fileSystem;
        private List<string> _skippableKeys = new List<string>();
        private List<string> _skippedLines = new List<string>();
        private Dictionary<string, FileLine> _fileLines = new Dictionary<string,FileLine>();
        private string _headerLine;
        private bool _saveOriginalData;
        private int[] _keyColumns;
        private int[] _comparisonColumns;
private  int _dateColumn;

        public FileLineCollection(Config config, IFileSystem fileSystem, bool saveOriginalData)
        {
            _config = config;
            _fileSystem = fileSystem;
            _saveOriginalData = saveOriginalData;
        }

        public void ParseLines(List<string> lines, int headerLine)
        {
            _headerLine = lines[headerLine];
            var items = lines[headerLine].ToUpperInvariant().Split(_config.ColumnsSeparator.ToArray()
                , StringSplitOptions.None);
            _keyColumns = ColumnNameParser.GetColumnNumbersFromHeaderLine(items, _config.KeyColumns);
            _comparisonColumns = ColumnNameParser.GetColumnNumbersFromHeaderLine(items, _config.ComparisonColumns);
            _dateColumn = ColumnNameParser.GetSingleColumnNumberFromHeaderLine(items, _config.DateColumnName);
            lines.RemoveAt(headerLine);
            CreateFileLineCollection(lines);
        }

        private void CreateFileLineCollection(List<string> lines)
        {
            foreach (var line in lines)
            {
                var fileLine = FileLine.Create(_keyColumns, line, _dateColumn, _comparisonColumns
                    , _config.ColumnsSeparator, _saveOriginalData);

                if (!_skippableKeys.Contains(fileLine.Key))
                {
                    if (!KeyAlreadyExists(fileLine.Key))
                    {
                        _fileLines.Add(fileLine.Key, fileLine);
                    }
                    else
                    {
                        if (_saveOriginalData)
                        {
                            _skippedLines.Add(_fileLines[fileLine.Key].Line);
                            _skippedLines.Add(line);
                        }
                        _fileLines.Remove(fileLine.Key);
                        _skippableKeys.Add(fileLine.Key);
                    }

                }
                else
                {
                    if (_saveOriginalData)
                    {
                        _skippedLines.Add(line);
                    }
                }
            }
        }


        private bool KeyAlreadyExists(string key)
        {
            return (_fileLines.Keys.Contains(key));
        }
        
        public int NumberOfComparableLines { get
            { return _fileLines.Count;}
        }

        public void FilterComparableLines(FileLineCollection fileLineCollectionSource)
        {
            var keysWithIdenticalData = new List<string>();
            foreach (var fileLine in _fileLines.Values)
            {
                if (fileLine.ComparisonData == fileLineCollectionSource.GetComparisonData(fileLine.Key))
                {
                    keysWithIdenticalData.Add(fileLine.Key);
                }
            }
            foreach (var keyWithIdenticalData in keysWithIdenticalData)
            {
                _fileLines.Remove(keyWithIdenticalData);
            }
        }

        public string GetComparisonData(string key)
        {
            return _fileLines.ContainsKey(key)?_fileLines[key].ComparisonData:string.Empty;
        }


        public string[] GetOutputLines()
        {
            var lines = new List<string>();
            lines.Add(_headerLine);
            foreach (var fileLine in _fileLines)
            {
                lines.Add(fileLine.Value.Line);
            }
            lines.AddRange(_skippedLines);
            return lines.ToArray();
        }
    }
}
