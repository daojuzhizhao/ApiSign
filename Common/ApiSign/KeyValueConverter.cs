using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class KeyValueConverter
    {
        public static string ToKeyValuePairString(object data)
        {
            if (data == null) return string.Empty;

            if (data is DtoBase[] dataArray)
            {
                return string.Join("&", dataArray.Select(x => x.ToString()).ToArray());
            }

            var keyValuePairs = data.GetType().GetTypeInfo().GetProperties()
                .Select(p => new { key = p.Name, value = p.GetValue(data) })
                .OrderBy(x => x.key)
                .Select(x =>
                {
                    string valueString;
                    if (x.value == null)
                    {
                        return null;
                    }
                    var valueType = x.value.GetType();
                    var valueTypeName = valueType.Name;
                    if (valueTypeName == "DateTime")
                    {
                        valueString = ((DateTime)x.value).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else if (valueTypeName == "List`1")
                    {
                        throw new InvalidCastException($"请使用数组代替List：{valueType.FullName}");
                    }
                    else if (x.value is Array)
                    {
                        valueString = string.Join("&", (x.value as Array).Cast<object>());
                    }
                    else if (valueType.IsEnum)
                    {
                        valueString = ((int)x.value).ToString();
                    }
                    else if (valueTypeName == "Decimal")
                    {
                        valueString = ((decimal)x.value).ToString("N2");
                    }
                    else
                    {
                        valueString = x.value.ToString();
                    }

                    return x.key + "=" + valueString;
                })
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

            return string.Join("&", keyValuePairs);
        }
    }
}
