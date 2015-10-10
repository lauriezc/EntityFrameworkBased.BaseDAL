using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFrameworkBased.BaseDAL;

namespace test
{
    public class B1DAL : BaseDAL<b1>
    {
        public B1DAL()
        {
            base.DbConnect = new DbEnum().GetDbConnectionName(dbE.T.testEntities);
        }
    }
}
