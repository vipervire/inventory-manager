using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

/*
 * Name: Matthew Vance
 * Date: 5/4/2017
 * Program Name: Inventory Manager
 * Program Description: A program that can Add, Delete, and Update entries into a database and display them.
 */

namespace FinalProjectVance
{
    public partial class Form1 : Form
    {
        public SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\1104\Desktop\C#\FinalProjectVance_5-4-17\FinalProjectVance\FinalProjectVance\Database2.mdf;Integrated Security=True");

        SqlCommand cmd;
        SqlDataAdapter adapt;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        //Event Method to add entered info into a record in the database.
        {
            if (txtEnterUnit.Text != "" && txtEnterName.Text != "" && txtEnterPrice.Text != "" && txtEnterQty.Text != "")
            {
                cmd = new SqlCommand("INSERT INTO [Table] (Unit,Name,Price,Quantity,Brand) values (@Unit,@Name,@Price,@Quantity,@Brand)", cn);
                cn.Open();
                cmd.Parameters.AddWithValue("@Unit", txtEnterUnit.Text);
                cmd.Parameters.AddWithValue("@Name", txtEnterName.Text);
                cmd.Parameters.AddWithValue("@Price", txtEnterPrice.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtEnterQty.Text);
                cmd.Parameters.AddWithValue("@Brand", txtEnterBrand.Text);
                cmd.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Success!\nRecord Added.");
                UpdateDataGridView();
                ClearEntryData();
            }
            else
            {
                MessageBox.Show("Please Enter Details for a Record.");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        //Event Method to update entered info into a record in the database.
        {
            if (txtEnterUnit.Text != "" && txtEnterName.Text != "" && txtEnterPrice.Text != "" && txtEnterQty.Text != "")
            {
                cmd = new SqlCommand("UPDATE [Table] SET Unit=@Unit,Name=@Name,Price=@Price,Quantity=@Quantity,Brand=@Brand WHERE ID=@ID", cn);
                cn.Open();
                cmd.Parameters.AddWithValue("@ID", lblEnterID.Text);
                cmd.Parameters.AddWithValue("@Unit", txtEnterUnit.Text);
                cmd.Parameters.AddWithValue("@Name", txtEnterName.Text);
                cmd.Parameters.AddWithValue("@Price", txtEnterPrice.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtEnterQty.Text);
                cmd.Parameters.AddWithValue("@Brand", txtEnterBrand.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Success!\nRecord Updated.");
                cn.Close();
                UpdateDataGridView();
                ClearEntryData();
            }
            else
            {
                MessageBox.Show("Please Select a Record to Update.");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        //Event Method to delete selected info from a record in the database.
        {
            if (lblEnterID.Text != "")
            {
                cmd = new SqlCommand("DELETE [Table] WHERE ID=@ID", cn);
                cn.Open();
                cmd.Parameters.AddWithValue("@ID", lblEnterID.Text);
                cmd.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Success!\nRecord Deleted.");
                UpdateDataGridView();
                ClearEntryData();
            }
            else
            {
                MessageBox.Show("Please Select a Record to Delete.");
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //Event Method to assign values to textboxes and labels from datagridview upon clicking row header.
        {
            lblEnterID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtEnterName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtEnterPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtEnterQty.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtEnterUnit.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtEnterBrand.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        //Event Method to exit program.
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        //Event Method upon load to display info in datagridview.
        {
            UpdateDataGridView();
        }
        private void UpdateDataGridView()
        //Method to display database info into datagridview and create the total value of inventory.
        {
            cn.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("SELECT * FROM [Table] ORDER BY ID", cn);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();
            //foreach record in dataset, Price * Quantity and accumulate.
            double totalValue = 0.0;
            foreach (DataRow dr in dt.Rows)
            {
                double priceInventory, quantityInventory;

                if (!double.TryParse(dr["Price"].ToString(), out priceInventory))
                {
                    break;
                }
                if (!double.TryParse(dr["Quantity"].ToString(), out quantityInventory))
                {
                    break;
                }
                totalValue += priceInventory * quantityInventory;
            }
            string stringTotalValue = String.Format("{0:C}", totalValue);
            lblTotalValue.Text = "Total Value: " + stringTotalValue;
        }
        private void ClearEntryData()
        //Method to clear info from textboxes and label.
        {
            txtEnterUnit.Text = "";
            txtEnterName.Text = "";
            txtEnterPrice.Text = "";
            txtEnterQty.Text = "";
            lblEnterID.Text = "";
            txtEnterBrand.Text = "";
        }
    }
}