using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityFrameworkBased.BaseDAL
{
    public class DbEnum
    {
        public string GetDbConnectionName<T>(int value)
        {
            return Enum.Parse(typeof(T), value.ToString()).ToString();
        }
        public string GetDbConnectionName(object value)
        {
            return Enum.Parse(value.GetType(), value.ToString()).ToString();
        }
    }
}
