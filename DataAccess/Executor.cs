using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Data;
using System.Data.SqlClient;

[assembly: InternalsVisibleTo("DataAccessTest")]
namespace DataAccess
{
    /// <summary>
    /// Internal class for handling the connection to the sql server
    /// </summary>
    internal class Executor
    {
        protected string ConnectionString { get => connection?.ConnectionString; }

        private SqlConnection connection;

        /// <summary>
        /// Constructor for the executor.
        /// </summary>
        /// <param name="connectionString">the connection string</param>
        /// <exception cref="ArgumentException">Throws this if the connectionstring parameter is empty</exception>
        internal Executor(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("ConnectionString cannot be empty");

            connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Executes the sqlQuery and returns the result in the form of a dataset
        /// </summary>
        /// <param name="sqlQuery">the query to be executed</param>
        /// <param name="parameters">the parameters. these are for parameterized querys.</param>
        /// <returns>a dataset with the result in it</returns>
        /// <exception cref="SqlException">If query is invalid.</exception>
        /// <exception cref="ArgumentException">throws this if the query is empty</exception>
        internal DataSet Execute(string sqlQuery, params (string parameterName,SqlDbType parameterType,object parameterValue)[] parameters)
        {
            if (String.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentException("sqlQuery cannot be null or whitespace");

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


        /// <summary>
        /// Runs the input procedure with the parameters specified
        /// </summary>
        /// <param name="storedProcedureName">the name of the procedure</param>
        /// <param name="parameters">Parameters tuple for the procedure</param>
        /// <returns>A dataset that represents the result of the storedprocedure</returns>
        /// <exception cref="SqlException">If query is invalid.</exception>
        /// <exception cref="ArgumentException">if storedProcedureName is empty</exception>
        internal DataSet ExecuteProcedure(string storedProcedureName, params (string parameterName, SqlDbType parameterType, int parameterSize,ParameterDirection direction,object parameterValue)[] parameters)
        {
            if (String.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentException("storedProcedureName cannot be null or whitespace");


            connection.Open();

            SqlCommand command = new SqlCommand(storedProcedureName, connection);

            command.CommandType = CommandType.StoredProcedure;


            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    command.Parameters.Add(item.parameterName, item.parameterType,item.parameterSize);
                    command.Parameters[item.parameterName].Value = item.parameterValue;
                    command.Parameters[item.parameterName].Direction = item.direction;
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
