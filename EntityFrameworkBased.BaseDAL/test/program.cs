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
            b1 b = new b1();
            b.b = 10;
            var m=dal.Add(b);
            var model = dal.GetModel("id", 1);
            List<Filter> fl=new List<Filter>();
            fl.AddFilter("id",FilterOption.LessThan,3);
            var list = dal.GetPageList(fl, 1, 15, "id");

            BaseDAL<a1> dala = new BaseDAL<a1>();
            dala.DbConnect = new DbEnum().GetDbConnectionName(dbE.T.testEntities);
            List<Filter> afl1=new List<Filter>();
            afl1.AddFilter("name",FilterOption.Contains,"3");


            List<Filter> afl=new List<Filter>();
            afl.AddFilter("name",FilterOption.Contains,"3");
            afl.AddFilter("name",FilterOption.Contains,"9");

            var alist1 = dala.GetPageList(afl, 1, 100, "a").PageObject as List<a1>;
            var alist2 = dala.GetPageList(afl1, 1, 100, "a").PageObject as List<a1>;
        }
    }
}
