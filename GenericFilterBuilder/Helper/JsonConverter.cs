/// <summary>
///   Author:    Alen Radica
/// </summary>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GenericFilterBuilder
{
    internal class JsonConverter
    {
        public static T TryParse<T>(string jsonData) where T : new()
        {
            if (jsonData == null || !IsValidJson(jsonData))
                return default;

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static List<FilterValueItem> ConvertUserFiltersHelperMethod(string filterValues)
        {
            var filters = TryParse<List<FilterValueItem>>(filterValues);
            return filters ?? null;
        }
    }
}
