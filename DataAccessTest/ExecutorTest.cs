using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;

namespace DataAccessTest
{
    [TestClass]
    public class ExecutorTest
    {
        [TestMethod]
        public void TestExecuteNoParams()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Employees", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            connection.Close();

            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");

            DataSet set = executor.Execute("SELECT * FROM Employees");

            string test =  set.Tables[0].Rows[0].ItemArray.Aggregate((x,y) => x.ToString() + y.ToString()).ToString();

            Assert.AreEqual(test, dataSet.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString());
        }

        [TestMethod]
        public void TestExecuteParams()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Employees where EmployeeID=1", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            connection.Close();

            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");

            DataSet set = executor.Execute("SELECT * FROM Employees where EmployeeID=@testid", ("testid",SqlDbType.Int,1));

            string test = set.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString();

            Assert.AreEqual(test, dataSet.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString());

        }

        [TestMethod]
        public void TestExecuteArgumentException()
        {
            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            Assert.ThrowsException<ArgumentException>(() => executor.Execute(""));
        }



        [TestMethod]
        public void TestExecuteSqlException()
        {
            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            Assert.ThrowsException<SqlException>(() => executor.Execute("select * from tearaestsdased"));
        }

        [TestMethod]
        public void TestExecuteProcedureNoParam()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            connection.Open();

            SqlCommand command = new SqlCommand("EXEC What_DB_is_this", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            connection.Close();

            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");

            DataSet set = executor.ExecuteProcedure("What_DB_is_this");

            string test = set.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString();

            Assert.AreEqual(test, dataSet.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString());
        }

        [TestMethod]
        public void TestExecuteProcedure()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            connection.Open();

            SqlCommand command = new SqlCommand("EXEC What_DB_is_that 1", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            connection.Close();

            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");

            DataSet set = executor.ExecuteProcedure("What_DB_is_that", ("id",SqlDbType.Int,1, ParameterDirection.Input,1));

            string test = set.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString();

            Assert.AreEqual(test, dataSet.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString());
        }

        [TestMethod]
        public void TestExecuteProcedureArgumentException()
        {
            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            Assert.ThrowsException<ArgumentException>(() => executor.ExecuteProcedure(""));
        }


        [TestMethod]
        public void TestExecuteProcedureSqlException()
        {
            Executor executor = new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            Assert.ThrowsException<SqlException>(() => executor.ExecuteProcedure("sedaefdsfsdfdsfd"));
        }


        [TestMethod]
        public void TestConstructorException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Executor(@""));
        }

        [TestMethod]
        public void TestConstructor()
        {
            new Executor(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
        }
    }
}