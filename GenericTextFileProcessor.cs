using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleUI
{
    public static class GenericTextFileProcessor
    {
        public static List<T> LoadFromTextFile<T>(string filePath) where T : class, new()
        {
            var rows = File.ReadAllLines(filePath).ToList();
            List<T> outputList = new List<T>();

            var properties = typeof(T).GetProperties();

            if (rows.Count < 2)
                throw new IndexOutOfRangeException("The file was either empty or missing.");

            var headerColumns = rows[0].Split(',');
            rows.RemoveAt(0);

            foreach (var row in rows)
            {
                T entry = new T();

                var rowValues = row.Split(',');

                foreach (var column in headerColumns.Select((columnName, columnIndex) => new { columnName, columnIndex }))
                {
                    var propertyRef = properties.SingleOrDefault(property => property.Name == column.columnName);
                    if (propertyRef != null)
                    {
                        object typedValue = Convert.ChangeType(rowValues[column.columnIndex], propertyRef.PropertyType);
                        propertyRef.SetValue(entry, typedValue);
                    }
                }

                outputList.Add(entry);
            }

            return outputList;
        }

        public static void SaveToTextFile<T>(List<T> lstData, string filePath) where T : class
        {
            List<string> rows = new List<string>();

            if (!(lstData?.Count > 0))
                throw new ArgumentNullException("data", "You must populate the data parameter with at least one value.");

            var properties = typeof(T).GetProperties();
            rows.Add(string.Join(",", properties.Select(c => c.Name)));

            foreach (var row in lstData)
            {
                var rowValues = properties.Select(c => c.GetValue(row)).ToList();
                rows.Add(string.Join(",", rowValues));
            }
            File.WriteAllLines(filePath, rows);
        }
    }
}
