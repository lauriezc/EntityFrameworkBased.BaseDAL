using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFrameworkBased.BaseDAL;

namespace test
{
    class program
    {
        static void Main(string[] args)
        {
            B1DAL dal = new B1DAL();
            var model = dal.GetModel("id", 1);
            List<Filter> fl=new List<Filter>();
            fl.AddFilter("id",FilterOption.LessThan,3);
            var list = dal.GetPageList(fl, 1, 15, "id");
        }
    }
}
