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
        protected readonly string ConnectionString;

        /// <summary>
        /// Constructor for the executor.
        /// </summary>
        /// <param name="connectionString">the connection string</param>
        /// <exception cref="ArgumentException">Throws this if the connectionstring parameter is empty</exception>
        internal Executor(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("ConnectionString cannot be empty");

            ConnectionString = connectionString;
        }

        /// <summary>
        /// Executes the sqlQuery and returns the result in the form of a dataset
        /// </summary>
        /// <param name="sqlQuery">the query to be executed</param>
        /// <returns>a dataset with the result in it</returns>
        /// <exception cref="SqlException">If query is invalid.</exception>
        /// <exception cref="ArgumentException">throws this if the query is empty</exception>
        internal DataSet Execute(string sqlQuery)
        {
            if (String.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentException("sqlQuery cannot be empty");


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();

                try
                {
                    connection.Open();

                    adapter.Fill(dataSet);

                }
                finally 
                {
                    connection.Close();
                }

                return dataSet;


            }
        }

        /// <summary>
        /// Executes the SqlCommand and returns the result in the form of a dataset
        /// </summary>
        /// <param name="command">the command to be executed</param>
        /// <returns>a dataset with the result in it</returns>
        /// <exception cref="SqlException">If the command query is invalid.</exception>
        /// <exception cref="NullReferenceException">throws this if the command is null</exception>
        internal DataSet Execute(SqlCommand command)
        {
            if (command == null)
                throw new NullReferenceException("the SqlCommand cannot be null");

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {


                DataSet dataSet = new DataSet();

                try
                {
                    connection.Open();

                    command.Connection = connection;

                    adapter.Fill(dataSet);

                }
                finally
                {
                    connection.Close();
                }

                return dataSet;
            }
        }
    }
}
