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
    }
}