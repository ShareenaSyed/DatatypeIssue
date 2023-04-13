using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class DbResultDataTableHelper
    {
        public static DataTable GetDataTable<T>(string filePath) where T : new()
        {

            var dataTable = new DataTable();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
            }

            PopulateRows<T>(dataTable, filePath);
            return dataTable;
        }
        private static DataTable PopulateRows<T>(DataTable dtCsv, string csvFile) where T : new()
        {
            using (var sr = new StreamReader(csvFile))
            {
                while (!sr.EndOfStream)
                {
                    var fulltext = sr.ReadToEnd();
                    var rows = fulltext.Replace("\r","").Split('\n'); //split full file text into rows
                    var firstrow = rows[0].Split(new string[] { "," }, StringSplitOptions.None);
                    for (var i = 1; i < rows.Length; i++)
                    {

                        var rowValues = rows[i].Split(new string[] { "," }, StringSplitOptions.None); //split each row with comma to get individual values  
                        {

                            var dr = dtCsv.NewRow();
                            var obj = new T();
                            for (var k = 0; k < rowValues.Length; k++)
                            {
                               
                                //var propertyInfo = obj.GetType().GetProperty(dtCsv.Columns[firstrow[k]].ToString());
                                //if (propertyInfo != null)
                                //{
                                //    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                //    if (rowValues[k].GetType().FullName != propertyInfo.PropertyType.FullName)
                                //    {
                                //        //propertyInfo?.SetValue(rowValues[k],
                                //        //    Convert.ChangeType(obj, t), null);
                                //        dr[k] = Convert.ChangeType(rowValues[k], t);
                                //    }
                                //}
                                dr[k] = rowValues[k];
                            }
                            dtCsv.Rows.Add(dr); //add other rows  

                        }
                    }
                }
            }

            return dtCsv;
        }
        public static void ConvertColumnType(this DataTable dt, string columnName, Type newType)
        {
            using (DataColumn dc = new DataColumn(columnName + "_new", newType))
            {
                // Add the new column which has the new type, and move it to the ordinal of the old column
                int ordinal = dt.Columns[columnName].Ordinal;
                dt.Columns.Add(dc);
                dc.SetOrdinal(ordinal);

                // Get and convert the values of the old column, and insert them into the new
                foreach (DataRow dr in dt.Rows)
                    dr[dc.ColumnName] = Convert.ChangeType(dr[columnName], newType);

                // Remove the old column
                dt.Columns.Remove(columnName);

                // Give the new column the old column's name
                dc.ColumnName = columnName;
            }
        }
    }
}
