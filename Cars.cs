using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ABC_Car_Traders.clsConnection;
using System.Xml.Linq;
using System.Drawing.Printing;

namespace ABC_Car_Traders
{
    public partial class Cars : Form
    {
        SqlConnection Conn = new SqlConnection("Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;");

        // Variable to hold the selected car details
        private string selectedCarDetails = string.Empty;

        public Cars()
        {
            InitializeComponent();
            btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void carPartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form CarParts = new CarParts();
            CarParts.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();

                SqlTransaction transaction = Conn.BeginTransaction();

                if (txtBrand.Text == "")
                {
                    MessageBox.Show("Please fill the Brand Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtModel.Text == "")
                {
                    MessageBox.Show("Please fill the Model", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtYear.Text == "")
                {
                    MessageBox.Show("Please fill the Year", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtMileage.Text == "")
                {
                    MessageBox.Show("Please fill the Mileage", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtPrice.Text == "")
                {
                    MessageBox.Show("Please fill the Price", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (cmbCondition.Text == "")
                {
                    MessageBox.Show("Please select the Codition", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtDescription.Text == "")
                {
                    MessageBox.Show("Please fill the Description", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else
                {

                    string queryCar = @"INSERT INTO Cars (Brand, Model, Year, Mileage, Price, Condition, Description)
                    VALUES (@brand, @model, @year, @mileage, @price, @condition, @description); SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(queryCar, Conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@brand", txtBrand.Text);
                        cmd.Parameters.AddWithValue("@model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@year", txtYear.Text);
                        cmd.Parameters.AddWithValue("@mileage", txtMileage.Text);
                        cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                        cmd.Parameters.AddWithValue("@condition", cmbCondition.Text);
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text);

                        var newUserID = cmd.ExecuteScalar();

                        if (newUserID != null)
                        {
                            transaction.Commit();
                            MessageBox.Show("Car Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Cars_Load(sender, e);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            finally
            {
                if (Conn.State == System.Data.ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        public static string getSingleData(string query, SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                return result?.ToString(); // Returns the result as a string or null if no result
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstCars.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a Car to update.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Conn.Open();
                SqlTransaction transaction = Conn.BeginTransaction();

                if (lstCars.SelectedItems.Count > 0)
                {
                    //string query = @"SELECT CarID FROM Cars WHERE Brand = @brand AND Model = @model AND Year = @year";

                    //string CarID = getSingleData(query, Conn);

                    // Retrieve the selected ListViewItem
                    ListViewItem selectedItem = lstCars.SelectedItems[0];

                    // Get the CarID from the Tag property
                    string carID = selectedItem.Tag.ToString();


                    string updateCarQuery = @"UPDATE Cars 
                                        SET Brand = @brand, 
                                            Model = @model, 
                                            Year = @year, 
                                            Mileage = @mileage, 
                                            Price = @price, 
                                            Condition = @condition, 
                                            Description = @description
                                        WHERE CarID = @carID";

                    using (SqlCommand cmd = new SqlCommand(updateCarQuery, Conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@carID", carID);
                        cmd.Parameters.AddWithValue("@brand", txtBrand.Text);
                        cmd.Parameters.AddWithValue("@model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@year", txtYear.Text);
                        cmd.Parameters.AddWithValue("@mileage", txtMileage.Text);
                        cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                        cmd.Parameters.AddWithValue("@condition", cmbCondition.Text);
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            MessageBox.Show("Car Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Cars_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Update failed. No rows affected.", "Unsuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (Conn.State == System.Data.ConnectionState.Open)
                {
                    Conn.Close();
                }
            }

        }

        public class SQL
        {
            public static DataSet getDataSet(string query, SqlConnection connection, string tblCars)
            {
                DataSet ds = new DataSet();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, tblCars);
                        }
                    }
                    return ds;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return null;
                }
            }

            int CarID { get; set; }

            public static string strCars()
            {
                return "SELECT * FROM Cars";
            }
        }

        private void Cars_Load(object sender, EventArgs e)
        {
            try
            {
                if (SYS.SYS_ID == 1)
                {
                    btnAdd.Enabled = false;
                }

                if (SYS.SYS_ID == 1)
                {
                    btnUpdate.Enabled = false;
                }

                if (SYS.SYS_ID == 1)
                {
                    btnDelete.Enabled = false;
                }

                if (SYS.SYS_ID == 1)
                {
                    btnPrint.Enabled = false;
                }

                lstCars.FullRowSelect = true;

                Cursor.Current = Cursors.WaitCursor;
                DataSet dsCars = SQL.getDataSet(SQL.strCars(), Conn, "tblCars");

                lstCars.Clear();
                lstCars.View = View.Details;
                lstCars.Columns.Add("Brand", 100, HorizontalAlignment.Left);
                lstCars.Columns.Add("Model", 100, HorizontalAlignment.Left);
                lstCars.Columns.Add("Year", 100, HorizontalAlignment.Left);
                lstCars.Columns.Add("Mileage", 100, HorizontalAlignment.Left);
                lstCars.Columns.Add("Price", 100, HorizontalAlignment.Left);
                lstCars.Columns.Add("Condition", 100, HorizontalAlignment.Left);
                lstCars.Columns.Add("Description", 100, HorizontalAlignment.Left);

                if (dsCars != null && dsCars.Tables.Contains("tblCars") && dsCars.Tables["tblCars"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCars.Tables["tblCars"].Rows)
                    {
                        ListViewItem li = new ListViewItem(row["Brand"].ToString());
                        li.SubItems.Add(row["Model"].ToString());
                        li.SubItems.Add(row["Year"].ToString());
                        li.SubItems.Add(row["Mileage"].ToString());
                        li.SubItems.Add(row["Price"].ToString());
                        li.SubItems.Add(row["Condition"].ToString());
                        li.SubItems.Add(row["Description"].ToString());
                        //lstCars.Items.Add(li);

                        // Store CarID in the Tag property
                        li.Tag = row["CarID"].ToString();

                        lstCars.Items.Add(li);
                    }
                }
                else
                {
                    MessageBox.Show("No data found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtBrand.Clear();
            txtModel.Clear();
            txtYear.Clear();
            txtMileage.Clear();
            txtPrice.Clear();
            cmbCondition.SelectedItem = null;
            txtDescription.Clear();
            txtSearch.Clear();
        }

        private void lstCars_DoubleClick(object sender, EventArgs e)
        {
            if (lstCars.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = lstCars.SelectedItems[0];

                // Populate the text fields with the selected item data
                txtBrand.Text = selectedItem.SubItems.Count > 0 ? selectedItem.SubItems[0].Text : string.Empty;
                txtModel.Text = selectedItem.SubItems.Count > 1 ? selectedItem.SubItems[1].Text : string.Empty;
                txtYear.Text = selectedItem.SubItems.Count > 2 ? selectedItem.SubItems[2].Text : string.Empty;
                txtMileage.Text = selectedItem.SubItems.Count > 3 ? selectedItem.SubItems[3].Text : string.Empty;
                txtPrice.Text = selectedItem.SubItems.Count > 4 ? selectedItem.SubItems[4].Text : string.Empty;
                cmbCondition.Text = selectedItem.SubItems.Count > 5 ? selectedItem.SubItems[5].Text : string.Empty;
                txtDescription.Text = selectedItem.SubItems.Count > 6 ? selectedItem.SubItems[6].Text : string.Empty;

            }
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Customers = new Customers();
            Customers.Show();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Orders = new Orders();
            Orders.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstCars.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lstCars.SelectedItems[0];

                string carID = selectedItem.Tag.ToString();

                // Confirm deletion
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this car?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Conn.Open();
                        SqlTransaction transaction = Conn.BeginTransaction();

                        string deleteCarQuery = @"DELETE FROM Cars WHERE CarID = @carID";

                        using (SqlCommand cmd = new SqlCommand(deleteCarQuery, Conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@carID", carID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Car deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Cars_Load(sender, e); // Refresh the ListView after deletion
                            }
                            else
                            {
                                MessageBox.Show("Delete failed. No rows affected.", "Unsuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                    finally
                    {
                        if (Conn.State == System.Data.ConnectionState.Open)
                        {
                            Conn.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a Car to delete.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    MessageBox.Show("Please enter a search term.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Conn.Open();

                string query = @"SELECT * FROM Cars 
                         WHERE Brand LIKE @search 
                         OR Model LIKE @search 
                         OR Year LIKE @search 
                         OR Mileage LIKE @search 
                         OR Price LIKE @search 
                         OR Condition LIKE @search 
                         OR Description LIKE @search";

                using (SqlCommand cmd = new SqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dsCars = new DataSet();
                    adapter.Fill(dsCars, "tblCars");

                    // Clear and reload the ListView with the search results
                    lstCars.Items.Clear();

                    if (dsCars != null && dsCars.Tables.Contains("tblCars") && dsCars.Tables["tblCars"].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsCars.Tables["tblCars"].Rows)
                        {
                            ListViewItem li = new ListViewItem(row["Brand"].ToString());
                            li.SubItems.Add(row["Model"].ToString());
                            li.SubItems.Add(row["Year"].ToString());
                            li.SubItems.Add(row["Mileage"].ToString());
                            li.SubItems.Add(row["Price"].ToString());
                            li.SubItems.Add(row["Condition"].ToString());
                            li.SubItems.Add(row["Description"].ToString());

                            // Store CarID in the Tag property
                            li.Tag = row["CarID"].ToString();

                            lstCars.Items.Add(li);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No results found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
                this.Close();
                Form login = new Login();
                login.Show();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (lstCars.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a car.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("ABC Car Traders", new Font("Times New Roman", 28, FontStyle.Regular), Brushes.Black, new Point(250, 50));
            e.Graphics.DrawString("Car Details", new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, new Point(300, 100));

            // Print the selected car details
            if (!string.IsNullOrEmpty(selectedCarDetails))
            {
                e.Graphics.DrawString(selectedCarDetails, new Font("Times New Roman", 16, FontStyle.Regular), Brushes.Black, new Point(100, 200));
            }

            else
            {
                e.Graphics.DrawString("No car selected.", new Font("Times New Roman", 14, FontStyle.Regular), Brushes.Black, new Point(60, 250));
            }
        }

        private void lstCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCars.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lstCars.SelectedItems[0];
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Brand       : " + selectedItem.SubItems[0].Text);
                sb.AppendLine(); // Adds an extra blank line
                sb.AppendLine("Model       : " + selectedItem.SubItems[1].Text);
                sb.AppendLine();
                sb.AppendLine("Year        : " + selectedItem.SubItems[2].Text);
                sb.AppendLine(); 
                sb.AppendLine("Mileage     : " + selectedItem.SubItems[3].Text);
                sb.AppendLine();
                sb.AppendLine("Price       : " + selectedItem.SubItems[4].Text);
                sb.AppendLine();
                sb.AppendLine("Condition   :" + selectedItem.SubItems[5].Text);
                sb.AppendLine();
                sb.AppendLine("Description : " + selectedItem.SubItems[6].Text);

                selectedCarDetails = sb.ToString();
            }
        }
    }
}
