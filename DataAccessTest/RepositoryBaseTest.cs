using System.IO;
using System.Data.SqlClient;
using System.Data;
using DataAccess;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DataAccessTest
{
    [TestClass]
    public class RepositoryBaseTest
    {
        [TestMethod]
        public void ExecuteTest()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Employees", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            connection.Close();

            RepositoryBaseInheritorTest repo = new RepositoryBaseInheritorTest("TestDatabase", @"C:\Users\phil3216\source\repos\DataAccess\DataAccessTest\repoBaseTest.xml");

            DataSet set = repo.executeTest("SELECT * FROM Employees");

            string test = set.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString();

            Assert.AreEqual(test, dataSet.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString());

        }

        [TestMethod]
        public void ExecuteCommandTest()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EmployeesDB;Integrated Security=True");
            connection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM Employees", connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            connection.Close();

            RepositoryBaseInheritorTest repo = new RepositoryBaseInheritorTest("TestDatabase", @"C:\Users\phil3216\source\repos\DataAccess\DataAccessTest\repoBaseTest.xml");

            DataSet set = repo.executeTest(new SqlCommand("SELECT * FROM Employees"));

            string test = set.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString();

            Assert.AreEqual(test, dataSet.Tables[0].Rows[0].ItemArray.Aggregate((x, y) => x.ToString() + y.ToString()).ToString());
        }


        [TestMethod]
        public void ConstructorRepository()
        {
            RepositoryBaseInheritorTest repo = new RepositoryBaseInheritorTest("TestDatabase", @"C:\Users\phil3216\source\repos\DataAccess\DataAccessTest\repoBaseTest.xml");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConstructorRepositoryFileNotFoundException()
        {
            Assert.ThrowsException<FileNotFoundException>(() => new RepositoryBaseInheritorTest("TestDatabase", @"C\repoBaseTest.xml"));
        }

        [TestMethod]
        public void ConstructorRepositoryArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new RepositoryBaseInheritorTest("", @"C:\Users\phil3216\source\repos\DataAccess\DataAccessTest\repoBaseTest.xml"));
        }
    }

    public class RepositoryBaseInheritorTest : RepositoryBase
    {
        public RepositoryBaseInheritorTest(string databaseName, string configFilePath) : base(databaseName, configFilePath)
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
