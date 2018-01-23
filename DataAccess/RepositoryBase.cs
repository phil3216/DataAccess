using System.Linq;
using System.IO;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataAccess
{
    public abstract class RepositoryBase
    {

        private Executor executor;

        public RepositoryBase(string databaseName, string configFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);
            string connectionString = doc.SelectNodes($"{databaseName}/ConnectionString")[0].Value;
            executor = new Executor(connectionString);
        }

        protected DataSet Execute(string sqlQuery, params (string parameterName, SqlDbType parameterType, object parameterValue)[] parameters)
        {
            return executor.Execute(sqlQuery,parameters);
        }

        protected DataSet ExecuteProcedure(string storedProcedureName, params (string parameterName, SqlDbType parameterType, int parameterSize, ParameterDirection direction, object parameterValue)[] parameters)
        {
            return executor.ExecuteProcedure(storedProcedureName, parameters);
        }

    }
}
