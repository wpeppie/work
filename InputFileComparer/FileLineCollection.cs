using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Interfaces;
using System.Collections;
using System.Globalization;

namespace InputFileComparer
{
    public class FileLineCollection: LineCollection
    {
        private List<string> _skippedLines = new List<string>();

        public FileLineCollection(Config config, IFileSystem fileSystem)
        {
            _config = config;
            _fileSystem = fileSystem;
        }
        
        public void ParseLines(List<string> lines, int headerLine)
        {

            ParseHeaderLine(lines, headerLine);
            CreateLineCollection(lines);
        }

        private void CreateLineCollection(List<string> lines)
        {
            foreach (var line in lines)
            {
                var fileLine = FileLine.Create(_keyColumns, line, _dateColumn, _comparisonColumns
                    , _config.ColumnsSeparator);

                ProcessLine(line, fileLine);
            }
        }

        private void ProcessLine(string line, FileLine fileLine)
        {
            // If the key already exists this means multiple lines with the same key are in the file
            // As it's uncertain which version of the data needs to be compared, all lines with that
            // key are skipped from further processing
            if (!_skippableKeys.Contains(fileLine.Key))
            {
                ProcessKey(line, fileLine);
            }
            else
            {
                _skippedLines.Add(line);
            }
        }

        private void ProcessKey(string line, FileLine fileLine)
        {
            if (!KeyAlreadyExists(fileLine.Key))
            {
                _lines.Add(fileLine.Key, fileLine);
            }
            else
            {
                var _currentLine = (FileLine)_lines[fileLine.Key];
                _skippedLines.Add(_currentLine.Line);
                _skippedLines.Add(line);
                _lines.Remove(fileLine.Key);
                _skippableKeys.Add(fileLine.Key);
            }
        }


        public void FilterComparableLines(ComparableLineCollection fileLineCollectionSource)
        {
            var keysWithIdenticalData = new List<string>();
            foreach (var fileLine in _lines.Values)
            {
                if (fileLine.ComparisonData == fileLineCollectionSource.GetComparisonData(fileLine.Key))
                {
                    keysWithIdenticalData.Add(fileLine.Key);
                }
            }
            foreach (var keyWithIdenticalData in keysWithIdenticalData)
            {
                _lines.Remove(keyWithIdenticalData);
            }
        }

        public string[] GetOutputLines()
        {
            var lines = new List<string>();
            lines.Add(_headerLine);
            foreach (var fileLine in _lines)
            {
                var _currentLine = (FileLine)fileLine.Value;
                lines.Add(_currentLine.Line);
            }
            lines.AddRange(_skippedLines);
            return lines.ToArray();
        }
    }
}
