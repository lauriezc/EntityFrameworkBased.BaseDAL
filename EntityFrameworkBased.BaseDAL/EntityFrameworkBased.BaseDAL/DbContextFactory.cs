using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Configuration;

namespace EntityFrameworkBased.BaseDAL
{
    public class DbContextFactory
    {
        public DbContext GetDbContext(string conName = "")
        {
            var connectionString = ConfigurationManager.ConnectionStrings[conName];
            if (connectionString == null)
                throw new ConfigurationErrorsException(String.Format("ConnectString name={0} doesn't exist.", conName));
            return new DbContext(connectionString.ToString());
        }

        public DbContext GetDbContext<T>(int conValue = 0)
        {
            string conName = string.Empty;
            conName = new DbEnum().GetDbConnectionName<T>(conValue);
            var connectionString = ConfigurationManager.ConnectionStrings[conName];
            if (connectionString == null)
                throw new ConfigurationErrorsException(String.Format("ConnectString name={0} doesn't exist.", conName));
            return new DbContext(connectionString.ToString());
        }
    }
}
