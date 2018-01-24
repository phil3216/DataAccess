using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationManagerDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            RepositoryBaseInheritorTest repo = new RepositoryBaseInheritorTest("TestDatabase");
        }
    }

    public class RepositoryBaseInheritorTest : RepositoryBase
    {
        public RepositoryBaseInheritorTest(string databaseName, string configFilePath) : base(databaseName, configFilePath)
        {

        }
        public RepositoryBaseInheritorTest(string databaseName) : base(databaseName)
        {

        }

        public DataSet executeTest(string sqlQuery)
        {
            return Execute(sqlQuery);
        }

        public DataSet executeTest(SqlCommand command)
        {
            return Execute(command);
        }
    }
}
