using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace server
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {


        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public DataTable returnTable(string tableName)
        {
            DataTable table = new DataTable(tableName);
            try
            {
                string connectionString = "Data Source=DESKTOP-H066BCP\\SQLEXPRESS;Initial Catalog=Week;Integrated Security=True;";
                string query = $"SELECT * FROM {tableName}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving data: " + ex.Message);
            }
            return table;
        }

        [WebMethod]
        public List<string> GetAllTables()
        {
            List<string> tableNames = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-H066BCP\\SQLEXPRESS;Initial Catalog=Week;Integrated Security=True;"))
                {
                    connection.Open();
                    DataTable schema = connection.GetSchema("Tables");
                    foreach (DataRow row in schema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        tableNames.Add(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving tables: " + ex.Message);
            }

            return tableNames;
        }
        [WebMethod]
        public void AddItemToTable(string tableName, string name, decimal price, DateTime date)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-H066BCP\\SQLEXPRESS;Initial Catalog=Week;Integrated Security=True;";
                string query = $"INSERT INTO {tableName} (name, price, date) VALUES (@name, @price, @date)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@date", date);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding item to table: " + ex.Message);
            }
        }
        [WebMethod]
        public void DeleteItemFromTable(string selectedTableName, int id)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-H066BCP\\SQLEXPRESS;Initial Catalog=Week;Integrated Security=True;";
                string query = $"DELETE FROM {selectedTableName} WHERE id = @id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting item from table: " + ex.Message);
            }
        }
        [WebMethod]
        public void UpdateItemInTable(string selectedTableName, int id, string newName, decimal newPrice, DateTime newDate)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-H066BCP\\SQLEXPRESS;Initial Catalog=Week;Integrated Security=True;";
                string query = $"UPDATE {selectedTableName} SET name = @newName, price = @newPrice, date = @newDate WHERE id = @id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@newName", newName);
                        command.Parameters.AddWithValue("@newPrice", newPrice);
                        command.Parameters.AddWithValue("@newDate", newDate);
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating item in table: " + ex.Message);
            }
        }
        [WebMethod]
        public DataTable FilterItemsByPrice(string selectedTableName, decimal maxPrice, string sign)
        {
            DataTable filteredTable = new DataTable(selectedTableName);
            try
            {
                string connectionString = "Data Source=DESKTOP-H066BCP\\SQLEXPRESS;Initial Catalog=Week;Integrated Security=True;";
                string query = $"SELECT * FROM {selectedTableName} WHERE price {sign} @maxPrice";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@maxPrice", maxPrice);
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(filteredTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error filtering items by price: " + ex.Message);
            }
            return filteredTable;
        }
        [WebMethod]
        public decimal CalculateTotalPriceByDate(string selectedTableName, string selectedDate)
        {
            decimal totalPrice = 0;
            try
            {
                string connectionString = "Data Source=DESKTOP-H066BCP\\SQLEXPRESS;Initial Catalog=Week;Integrated Security=True;";
                string query = $"SELECT SUM(price) FROM {selectedTableName} WHERE CAST(date AS DATE) = @selectedDate";
                Debug.WriteLine(query);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@selectedDate", selectedDate);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            totalPrice = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error calculating total price by date: " + ex.Message);
            }
            return totalPrice;


        }
    }
}
