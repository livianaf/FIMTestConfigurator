using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace FIMTestConfigurator {
    //________________________________________________________________________________________________________________________________________________________________________________
    public static class StringExtensions {
        //________________________________________________________________________________________________________________________________________________________________________________
        public static bool cEquals(this string value, string otherValue) { return value.Equals(otherValue, StringComparison.Ordinal); }
        public static bool iEquals(this string value, string otherValue) { return value.Equals(otherValue, StringComparison.OrdinalIgnoreCase); }
        public static int iIndexOf(this string value, string otherValue) { return value.IndexOf(otherValue, StringComparison.OrdinalIgnoreCase); }
        public static bool iStartsWith(this string value, string otherValue) { return value.StartsWith(otherValue, StringComparison.OrdinalIgnoreCase); }
        //________________________________________________________________________________________________________________________________________________________________________________
        public static T secParse<T>(this string value) where T : struct {
            T _authenticationType = default(T);
            if (string.IsNullOrWhiteSpace(value)) return _authenticationType;
            string _value = value.Replace("|", ", ");
            if (Enum.TryParse<T>(_value, out _authenticationType))
                if (Enum.IsDefined(typeof(T), _authenticationType) | _authenticationType.ToString().Contains(","))
                    return _authenticationType;
            throw new InvalidEnumArgumentException("Not possible to parse [" + value + "] to [" + typeof(T) + "]");
            }
        //________________________________________________________________________________________________________________________________________________________________________________
        public static DataTable ConvertToDataTable<T>(this IList<T> data, List<string> allowedAttributes = null) {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            if (allowedAttributes == null)
                foreach (PropertyDescriptor prop in properties) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            else
                foreach (string k in allowedAttributes) {
                    PropertyDescriptor prop = properties.Find(k, true);
                    if(prop!=null) table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                    
            foreach (T item in data) {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    if (allowedAttributes == null || allowedAttributes.Contains(prop.Name))
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
                }
            return table;
            }
        }
    }
