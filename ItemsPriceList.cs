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
using System.Xml.Linq;
using static ABC_Car_Traders.clsConnection;

namespace ABC_Car_Traders
{
    public partial class CarParts : Form
    {
        SqlConnection Conn = new SqlConnection("Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;");

        // Variable to hold the selected Item details
        private string selectedItemDetails = string.Empty;

        public CarParts()
        {
            InitializeComponent();
        }

        public class SQL
        {
            public static DataSet getDataSet(string query, SqlConnection connection, string tblPrice)
            {
                DataSet ds = new DataSet();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, tblPrice);
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

            public static string strPriceList()
            {
                return "SELECT * FROM PriceList";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Customers = new Customers();
            Customers.Show();
        }

        private void carsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Cars = new Cars();
            Cars.Show();
        }

        private void carPartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form CarParts = new CarParts();
            CarParts.Show();
        }

        private void itemsPriceListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Orders = new Orders();
            Orders.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtItemName.Clear();
            txtModel.Clear();
            txtPrice.Clear();
            txtYear.Clear();
            cmbCondition.SelectedItem = null;
            txtWarrenty.Clear();
            txtSearch.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();

                SqlTransaction transaction = Conn.BeginTransaction();

                if (txtItemName.Text == "")
                {
                    MessageBox.Show("Please enter the Item Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtModel.Text == "")
                {
                    MessageBox.Show("Please enter the Model", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtPrice.Text == "")
                {
                    MessageBox.Show("Please fill the Price", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtYear.Text == "")
                {
                    MessageBox.Show("Please enter the Year", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (cmbCondition.Text == "")
                {
                    MessageBox.Show("Please select the Codition", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtWarrenty.Text == "")
                {
                    MessageBox.Show("Please fill the Warrenty", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else
                {
                    string queryPrice = @"INSERT INTO PriceList (ItemName, Model, Price, Year, Condition, Warrenty)
                    VALUES (@itemname, @model, @price, @year, @condition, @warrenty); SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(queryPrice, Conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@itemname", txtItemName.Text);
                        cmd.Parameters.AddWithValue("@model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                        cmd.Parameters.AddWithValue("@year", txtYear.Text);
                        cmd.Parameters.AddWithValue("@condition", cmbCondition.Text);
                        cmd.Parameters.AddWithValue("@warrenty", txtWarrenty.Text);

                        var newUserID = cmd.ExecuteScalar();

                        if (newUserID != null)
                        {
                            transaction.Commit();
                            MessageBox.Show("Item Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ItemsPriceList_Load(sender, e);
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

        private void ItemsPriceList_Load(object sender, EventArgs e)
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
                    btnPrint.Enabled = false;
                }

                if (SYS.SYS_ID == 1)
                {
                    btnDelete.Enabled = false;
                }

                lstPrice.FullRowSelect = true;

                Cursor.Current = Cursors.WaitCursor;
                DataSet dsPrice = SQL.getDataSet(SQL.strPriceList(), Conn, "tblPrice");

                lstPrice.Clear();
                lstPrice.View = View.Details;
                lstPrice.Columns.Add("ItemName", 100, HorizontalAlignment.Left);
                lstPrice.Columns.Add("Model", 100, HorizontalAlignment.Left);
                lstPrice.Columns.Add("Price", 100, HorizontalAlignment.Left);
                lstPrice.Columns.Add("Year", 100, HorizontalAlignment.Left);
                lstPrice.Columns.Add("Condition", 100, HorizontalAlignment.Left);
                lstPrice.Columns.Add("Warrenty", 100, HorizontalAlignment.Left);

                if (dsPrice != null && dsPrice.Tables.Contains("tblPrice") && dsPrice.Tables["tblPrice"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsPrice.Tables["tblPrice"].Rows)
                    {
                        ListViewItem li = new ListViewItem(row["ItemName"].ToString());
                        li.SubItems.Add(row["Model"].ToString());
                        li.SubItems.Add(row["Price"].ToString());
                        li.SubItems.Add(row["Year"].ToString());
                        li.SubItems.Add(row["Condition"].ToString());
                        li.SubItems.Add(row["Warrenty"].ToString());
                        //lstCars.Items.Add(li);

                        // Store CarID in the Tag property
                        li.Tag = row["ItemID"].ToString();

                        lstPrice.Items.Add(li);
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

                string query = @"SELECT * FROM PriceList 
                         WHERE ItemName LIKE @search 
                         OR Model LIKE @search 
                         OR Price LIKE @search 
                         OR Year LIKE @search 
                         OR Condition LIKE @search
                         OR Warrenty LIKE @search";

                using (SqlCommand cmd = new SqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dsPrice = new DataSet();
                    adapter.Fill(dsPrice, "tblPrice");

                    // Clear and reload the ListView with the search results
                    lstPrice.Items.Clear();

                    if (dsPrice != null && dsPrice.Tables.Contains("tblPrice") && dsPrice.Tables["tblPrice"].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsPrice.Tables["tblPrice"].Rows)
                        {
                            ListViewItem li = new ListViewItem(row["ItemName"].ToString());
                            li.SubItems.Add(row["Model"].ToString());
                            li.SubItems.Add(row["Price"].ToString());
                            li.SubItems.Add(row["Year"].ToString());
                            li.SubItems.Add(row["Condition"].ToString());
                            li.SubItems.Add(row["Warrenty"].ToString());

                            // Store ItemID in the Tag property
                            li.Tag = row["ItemID"].ToString();

                            lstPrice.Items.Add(li);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstPrice.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lstPrice.SelectedItems[0];

                string ItemID = selectedItem.Tag.ToString();

                // Confirm deletion
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this Item?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Conn.Open();
                        SqlTransaction transaction = Conn.BeginTransaction();

                        string deleteItemQuery = @"DELETE FROM PriceList WHERE ItemID = @itemID";

                        using (SqlCommand cmd = new SqlCommand(deleteItemQuery, Conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@itemID", ItemID);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Item deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ItemsPriceList_Load(sender, e); // Refresh the ListView after deletion
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
                MessageBox.Show("Please select a Item to delete.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lstPrice_DoubleClick(object sender, EventArgs e)
        {
            if (lstPrice.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = lstPrice.SelectedItems[0];

                // Populate the text fields with the selected item data
                txtItemName.Text = selectedItem.SubItems.Count > 0 ? selectedItem.SubItems[0].Text : string.Empty;
                txtModel.Text = selectedItem.SubItems.Count > 1 ? selectedItem.SubItems[1].Text : string.Empty;
                txtPrice.Text = selectedItem.SubItems.Count > 2 ? selectedItem.SubItems[2].Text : string.Empty;
                txtYear.Text = selectedItem.SubItems.Count > 2 ? selectedItem.SubItems[3].Text : string.Empty;
                cmbCondition.Text = selectedItem.SubItems.Count > 4 ? selectedItem.SubItems[4].Text : string.Empty;
                txtWarrenty.Text = selectedItem.SubItems.Count > 5 ? selectedItem.SubItems[5].Text : string.Empty;
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (lstPrice.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a Item to update.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Conn.Open();
                SqlTransaction transaction = Conn.BeginTransaction();

                if (lstPrice.SelectedItems.Count > 0)
                {
                    // Retrieve the selected ListViewItem
                    ListViewItem selectedItem = lstPrice.SelectedItems[0];

                    // Get the CarID from the Tag property
                    string ItemID = selectedItem.Tag.ToString();

                    string updateCustomerQuery = @"UPDATE PriceList 
                                        SET ItemName = @itemname, 
                                            Model = @model, 
                                            Price = @price, 
                                            Year = @year, 
                                            Condition = @condition, 
                                            Warrenty = @warrenty
                                        WHERE ItemID = @itemID";

                    using (SqlCommand cmd = new SqlCommand(updateCustomerQuery, Conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@itemID", ItemID);
                        cmd.Parameters.AddWithValue("@itemname", txtItemName.Text);
                        cmd.Parameters.AddWithValue("@model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                        cmd.Parameters.AddWithValue("@year", txtYear.Text);
                        cmd.Parameters.AddWithValue("@condition", cmbCondition.Text);
                        cmd.Parameters.AddWithValue("@warrenty", txtWarrenty.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            MessageBox.Show("Item updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ItemsPriceList_Load(sender, e); // Refresh the ListView after update
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (lstPrice.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a Item.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("ABC Car Traders", new Font("Times New Roman", 28, FontStyle.Regular), Brushes.Black, new Point(250, 50));
            e.Graphics.DrawString("Item Details", new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, new Point(300, 100));

            // Print the selected item details
            if (!string.IsNullOrEmpty(selectedItemDetails))
            {
                e.Graphics.DrawString(selectedItemDetails, new Font("Times New Roman", 16, FontStyle.Regular), Brushes.Black, new Point(50, 200));
            }
            else
            {
                e.Graphics.DrawString("No Item selected.", new Font("Times New Roman", 14, FontStyle.Regular), Brushes.Black, new Point(60, 250));
            }
        }

        private void lstPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPrice.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lstPrice.SelectedItems[0];
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Item Name : " + selectedItem.SubItems[0].Text);
                sb.AppendLine(); // Adds an extra blank line
                sb.AppendLine("Model     : " + selectedItem.SubItems[1].Text);
                sb.AppendLine();
                sb.AppendLine("Price     : " + selectedItem.SubItems[2].Text);
                sb.AppendLine();
                sb.AppendLine("Year      : " + selectedItem.SubItems[3].Text);
                sb.AppendLine();
                sb.AppendLine("Condition : " + selectedItem.SubItems[4].Text);
                sb.AppendLine();
                sb.AppendLine("Warrenty  : " + selectedItem.SubItems[5].Text);

                selectedItemDetails = sb.ToString();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
                this.Close();
                Form login = new Login();
                login.Show();
        }
    }
}
