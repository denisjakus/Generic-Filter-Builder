/// <summary>
///   Author:    Alen Radica
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;

namespace GenericFilterBuilder
{
    public class FilterValueItem
    {
        public string FilterKey { get; set; }
        public string FilterValue { get; set; }

        public FilterValueItem()
        {
        }

        public FilterValueItem(string filterKey, string filterValue)
        {
            FilterKey = filterKey;
            FilterValue = filterValue;
        }
    }
}
