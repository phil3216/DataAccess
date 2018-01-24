using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Common;

namespace DataAccess
{

    /// <summary>
    /// Base class to control a database connection
    /// </summary>
    public abstract class RepositoryBase
    {
        private Executor executor;

        /// <summary>
        /// Constructs the class
        /// </summary>
        /// <param name="databaseName">The name of the database to find</param>
        /// <param name="configFilePath">The path to the config file</param>
        public RepositoryBase(string databaseName, string configFilePath)
        {
            if (String.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentException("databaseName is empty");

            if (!File.Exists(configFilePath))
                throw new FileNotFoundException("the config file does not exist");
           
            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);
            string connection = doc.SelectNodes("configuration/connectionStrings/add")
                                   .Cast<XmlNode>()
                                   .Where(x => x.Attributes["name"].Value == databaseName)
                                   .First()
                                   .Attributes["connectionString"]
                                   .Value;



            if (String.IsNullOrWhiteSpace(connection))
                throw new ArgumentException("the connection string is empty");

            DbConnectionStringBuilder csb = new DbConnectionStringBuilder();
            csb.ConnectionString = connection;

            executor = new Executor(connection);
        }

        /// <summary>
        /// Reads the connection string from the app.config file in the current appdomain
        /// </summary>
        /// <param name="databaseName">the name of the database</param>
        public RepositoryBase(string databaseName)
        {
            string connection = ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
            executor = new Executor(connection);
        }

        /// <summary>
        /// Executes the sqlQuery and returns the result in the form of a dataset
        /// </summary>
        /// <param name="sqlQuery">the query to be executed</param>
        /// <returns>a dataset with the result in it</returns>
        /// <exception cref="SqlException">If query is invalid.</exception>
        /// <exception cref="ArgumentException">throws this if the query is empty</exception>
        protected DataSet Execute(string sqlQuery)
        {
            return executor.Execute(sqlQuery);
        }

        /// <summary>
        /// Executes the SqlCommand and returns the result in the form of a dataset
        /// </summary>
        /// <param name="command">the command to be executed</param>
        /// <returns>a dataset with the result in it</returns>
        /// <exception cref="SqlException">If the command query is invalid.</exception>
        /// <exception cref="NullReferenceException">throws this if the command is null</exception>
        protected DataSet Execute(SqlCommand command)
        {
            return executor.Execute(command);
        }

    }
}
