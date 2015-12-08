using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace InputFileComparer
{
    public class ColumnNameParser
    {
        public static int[] GetColumnNumbersFromHeaderLine(string[] items, string[] columnNames)
        {
            var columnNumbers = new int[columnNames.Length];
            var itemList = items.ToList();
            for (int i = 0; i < columnNames.Length; i++)
            {
                if (items.Contains(columnNames[i].ToUpperInvariant()))
                {
                    columnNumbers[i] = itemList.IndexOf(columnNames[i].ToUpperInvariant());
                }
                else
                {
                    throw new InvalidOperationException(string.Format(
                        CultureInfo.InvariantCulture, "The column {0} was not found in the columnnames {1}"
                        , columnNames[i], string.Join(" - ", items)));
                }
            }
            return columnNumbers;
        }

        public static int GetSingleColumnNumberFromHeaderLine(string[] items, string columnName)
        {
            if (items.Contains(columnName.ToUpperInvariant()))
            {
                var itemList = items.ToList();
                return itemList.IndexOf(columnName.ToUpperInvariant());
            }
            else
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture, "The column {0} was not found in the columnnames {1}"
                    , columnName, string.Join(" - ", items)));
            }

        }

    }
}
