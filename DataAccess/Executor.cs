using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Data;
using System.Data.SqlClient;

[assembly: InternalsVisibleTo("DataAccessTest")]
namespace DataAccess
{
    internal class Executor
    {
        protected string ConnectionString { get => connection?.ConnectionString; }

        private SqlConnection connection;

        internal Executor(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Executes the sqlQuery and returns the result in the form of a dataset
        /// </summary>
        /// <param name="sqlQuery">the query to be executed</param>
        /// <param name="parameters">the parameters. these are for parameterized querys.</param>
        /// <returns>a dataset with the result in it</returns>
        /// <exception cref="SqlException">If the connection is invalid</exception>
        internal DataSet Execute(string sqlQuery, params (string parameterName,SqlDbType parameterType,object parameterValue)[] parameters)
        {
            connection.Open();

            SqlCommand command = new SqlCommand(sqlQuery,connection);

            if(parameters != null)
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item.parameterName, item.parameterType);
                    command.Parameters[item.parameterName].Value = item.parameterValue;
                }
            }


            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            connection.Close();

            return dataSet;
        }
    }
}
