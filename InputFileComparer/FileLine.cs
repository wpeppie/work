using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputFileComparer
{
    public class FileLine
    {
        private string _key;
        private string _date;
        private string _comparisonData;
        private string _line;


        private FileLine(string key, string date, string comparisonData, string line)
        {
            _key = key;
            _date = date;
            _comparisonData = comparisonData;
            _line = line;
        }

        internal static FileLine Create(int[] keyColumns, string line, int dateColumn
            , int[] comparisonColumns, string separator, bool saveOriginalData)
        {
            var items = line.Split(separator.ToArray(), StringSplitOptions.None);
            var key = GenerateKey(keyColumns, items, separator);
            var date = items[dateColumn];
            var comparisonData = GetComparisonData(items, comparisonColumns, separator);
            var fileLine = new FileLine(key, date, comparisonData, saveOriginalData?line:string.Empty);
            return fileLine;
        }

        public string Key
        {
            get { return _key; }
        }

        public string Date
        {
            get { return _date; }
        }

        public string ComparisonData
        {
            get { return _comparisonData; }
        }

        public string Line
        {
            get { return _line; }
        }

        private static string GetComparisonData(string[] items, int[] comparisonColumns, string separator)
        {
            var comparisonData = string.Empty;
            foreach (var comparisonColumn in comparisonColumns)
            {
                comparisonData += items[comparisonColumn] + separator;
            }
            return comparisonData;
        }

        private static string GenerateKey(int[] keyColumns, string[] items, string separator)
        {
            var key = string.Empty;
            foreach (var keyColumn in keyColumns)
            {
                key += items[keyColumn] + separator;
            }
            return key;
        }
    }
}
