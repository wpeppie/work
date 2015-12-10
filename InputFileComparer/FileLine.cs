using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputFileComparer
{
    public class FileLine: Line
    {
        private string _line;

        private FileLine(string key, string date, string comparisonData, string line)
        {
            _key = key;
            _date = date;
            _comparisonData = comparisonData;
            _line = line;
        }

        internal static FileLine Create(int[] keyColumns, string line, int dateColumn
            , int[] comparisonColumns, string separator)
        {
            var items = line.Split(separator.ToArray(), StringSplitOptions.None);
            var key = GenerateKey(keyColumns, items, separator);
            var date = items[dateColumn];
            var comparisonData = GetComparisonData(items, comparisonColumns, separator);
            var fileLine = new FileLine(key, date, comparisonData, line);
            return fileLine;
        }

        public string Line
        {
            get { return _line; }
        }


    }
}
