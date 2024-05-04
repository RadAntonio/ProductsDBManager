using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace client
{
    public partial class Form1 : Form
    {
        private DataTable selectedTable;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
         
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void days_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedTableName = days.SelectedItem.ToString();
                ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                selectedTable = service.returnTable(selectedTableName);
                dataGridView1.DataSource = selectedTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                string[] tables = service.GetAllTables();
                foreach (string tableName in tables)
                {
                    days.Items.Add(tableName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedTableName = days.SelectedItem.ToString();
                string name = txtName.Text;
                decimal price = decimal.Parse(txtPrice.Text);
                DateTime date = DateTime.Parse(txtDate.Text); 
                ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                service.AddItemToTable(selectedTableName, name, price, date);
                MessageBox.Show("Item added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedTableName = days.SelectedItem.ToString();
                int selectedId = int.Parse(del.Text);
                ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                service.DeleteItemFromTable(selectedTableName, selectedId);
                MessageBox.Show("Item deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];


                string name = selectedRow.Cells["name"].Value.ToString();
                decimal price = Convert.ToDecimal(selectedRow.Cells["price"].Value);
                DateTime date = Convert.ToDateTime(selectedRow.Cells["date"].Value);
                txtName.Text = name;
                txtPrice.Text = price.ToString();
                txtDate.Text = date.ToString();
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string selectedTableName = days.SelectedItem.ToString();

                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                    string newName = txtName.Text;
                    decimal newPrice = Convert.ToDecimal(txtPrice.Text);
                    DateTime newDate = Convert.ToDateTime(txtDate.Text);
                    ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                    service.UpdateItemInTable(selectedTableName, id, newName, newPrice, newDate);
                    MessageBox.Show("Item updated successfully.");
                }
                else
                {
                    MessageBox.Show("Please select a row to update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnLess_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedTableName = days.SelectedItem.ToString();
                decimal maxPrice = decimal.Parse(filter.Text);
                ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                DataTable filteredItems = service.FilterItemsByPrice(selectedTableName, maxPrice, "<");
                dataGridView1.DataSource = filteredItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnGreater_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedTableName = days.SelectedItem.ToString();
                decimal maxPrice = decimal.Parse(filter.Text);
                ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                DataTable filteredItems = service.FilterItemsByPrice(selectedTableName, maxPrice, ">");
                dataGridView1.DataSource = filteredItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSum_Click(object sender, EventArgs e)
        {
            try
            {
               
                string selectedTableName = days.SelectedItem.ToString();
                DateTime selectedDate = dateTimePicker1.Value.Date;
                ServiceReference1.WebServiceSoapClient service = new ServiceReference1.WebServiceSoapClient();
                decimal totalPrice = service.CalculateTotalPriceByDate(selectedTableName, selectedDate.ToString("yyyy-MM-dd"));
                MessageBox.Show("Total price for " + selectedDate.ToString("yyyy-MM-dd") + " is: " + totalPrice.ToString("C"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
