using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkBased.BaseDAL
{
    /// <summary>
    /// 此类源码来源
    /// http://www.codeproject.com/Tips/582450/Build-Where-Clause-Dynamically-in-Linq
    /// </summary>
    public class Filter
    {
        public string PropertyName { get; set; }
        public FilterOption Operation { get; set; }
        public object Value { get; set; }
    }

    public static class FilterHelper
    {
        public static void AddFilter(this List<Filter> list, string propertyName, FilterOption option, object value)
        {
            list.Add(new Filter() { PropertyName = propertyName, Operation = option, Value = value });
        }
    }

    public enum FilterOption
    {
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}
