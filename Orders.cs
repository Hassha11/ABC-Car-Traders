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
using System.Diagnostics;

namespace ABC_Car_Traders
{
    public partial class Orders : Form
    {
        SqlConnection Conn = new SqlConnection("Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;");

        // Variable to hold the selected order details
        private string selectedOrderDetails = string.Empty;

        public Orders()
        {
            InitializeComponent();
            txtNIC.TextChanged += txtNIC_TextChanged;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class SQL
        {
            public static DataSet getDataSet(string query, SqlConnection connection, string tblOrders)
            {
                DataSet ds = new DataSet();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, tblOrders);
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

            public static string strOrders()
            {
                return "SELECT * FROM Orders ";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (lstOrders.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a order.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void lstOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstOrders.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lstOrders.SelectedItems[0];
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Customer Name : " + selectedItem.SubItems[0].Text);
                sb.AppendLine(); // Adds an extra blank line
                sb.AppendLine("NIC : " + selectedItem.SubItems[1].Text);
                sb.AppendLine();
                sb.AppendLine("Address : " + selectedItem.SubItems[2].Text);
                sb.AppendLine();
                sb.AppendLine("Contact No : " + selectedItem.SubItems[3].Text);
                sb.AppendLine();
                sb.AppendLine("Items : " + selectedItem.SubItems[4].Text);
                sb.AppendLine();
                sb.AppendLine("Price : " + selectedItem.SubItems[5].Text);
                sb.AppendLine();
                sb.AppendLine("Quantity : " + selectedItem.SubItems[6].Text);
                sb.AppendLine();
                sb.AppendLine("Charges : " + selectedItem.SubItems[7].Text);

                selectedOrderDetails = sb.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstOrders.SelectedItems.Count > 0)
            {
                // Confirm deletion
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this order?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Conn.Open();
                        SqlTransaction transaction = Conn.BeginTransaction();

                        // Assuming you're deleting by NIC
                        string deleteCustomerQuery = @"DELETE FROM Orders WHERE NIC = @nic";

                        using (SqlCommand cmd = new SqlCommand(deleteCustomerQuery, Conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@nic", txtNIC.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Orders_Load(sender, e); // Refresh the ListView after deletion
                            }
                            else
                            {
                                MessageBox.Show("Delete failed. No rows affected.");
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
                MessageBox.Show("Please select a order to delete.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstOrders.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a Order to update.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Conn.Open();
                SqlTransaction transaction = Conn.BeginTransaction();

                string updateCustomerQuery = @"UPDATE Orders 
                                        SET CustomerName = @customername, 
                                            NIC = @nic, 
                                            Address = @address, 
                                            ContactNo = @contactno, 
                                            Items = @items, 
                                            Price = @price, 
                                            Quantity = @quantity,
                                            Charges = @chages,
                                            Status = @status
                                        WHERE NIC = @nic";

                using (SqlCommand cmd = new SqlCommand(updateCustomerQuery, Conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@customername", txtCusName.Text);
                    cmd.Parameters.AddWithValue("@nic", txtNIC.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@contactno", txtContactNo.Text);
                    cmd.Parameters.AddWithValue("@items", cmbItems.Text);
                    cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@quantity", cmbQuantity.Text);
                    cmd.Parameters.AddWithValue("@chages", txtCharge.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        transaction.Commit();
                        MessageBox.Show("Order updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Orders_Load(sender, e); // Refresh the ListView after update
                    }
                    else
                    {
                        MessageBox.Show("Update failed. No rows affected.");
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();

                SqlTransaction transaction = Conn.BeginTransaction();

                if (txtNIC.Text == "")
                {
                    MessageBox.Show("Please enter the NIC", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtCusName.Text == "")
                {
                    MessageBox.Show("Please enter the Customer Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtNIC.Text == "")
                {
                    MessageBox.Show("Please fill the NIC", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtAddress.Text == "")
                {
                    MessageBox.Show("Please fill the Address", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtAddress.Text == "")
                {
                    MessageBox.Show("Please fill the Address", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtContactNo.Text == "")
                {
                    MessageBox.Show("Please fill the Contact No", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (cmbItems.Text == "")
                {
                    MessageBox.Show("Please select the Item", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (cmbQuantity.Text == "")
                {
                    MessageBox.Show("Please select the Quantity", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (cmbStatus.Text == "")
                {
                    MessageBox.Show("Please select the Order Status", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else
                {
                    string queryCustomer = @"INSERT INTO Orders (CustomerName, NIC, Address, ContactNo, Items, Price, Quantity, Charges, Status)
                    VALUES (@customername, @nic, @address, @contactno, @items, @price, @quantity, @charges, @status); SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(queryCustomer, Conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customername", txtCusName.Text);
                        cmd.Parameters.AddWithValue("@nic", txtNIC.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@contactno", txtContactNo.Text);
                        cmd.Parameters.AddWithValue("@items", cmbItems.Text);
                        cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                        cmd.Parameters.AddWithValue("@quantity", cmbQuantity.Text);
                        cmd.Parameters.AddWithValue("@charges", txtCharge.Text);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                        var newUserID = cmd.ExecuteScalar();

                        if (newUserID != null)
                        {
                            transaction.Commit();
                            MessageBox.Show("Order Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Orders_Load(sender, e);
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

        private void Orders_Load(object sender, EventArgs e)
        {
            try
            {
                if (SYS.SYS_ID == 0)
                {
                    btnAdd.Enabled = false;
                }

                if (SYS.SYS_ID == 0)
                {
                    btnUpdate.Enabled = false;
                }

                if (SYS.SYS_ID == 0)
                {
                    btnFill.Enabled = false;
                }

                if (SYS.SYS_ID == 1)
                {
                    btnPrint.Enabled = false;
                }

                lstOrders.FullRowSelect = true;

                Cursor.Current = Cursors.WaitCursor;
                DataSet dsCustomers = SQL.getDataSet(SQL.strOrders(), Conn, "tblOrders");

                lstOrders.Clear();
                lstOrders.View = View.Details;
                lstOrders.Columns.Add("Customer Name", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("NIC", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("Address", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("Contact Number", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("Items", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("Price", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("Quantity", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("Charges", 100, HorizontalAlignment.Left);
                lstOrders.Columns.Add("Status", 100, HorizontalAlignment.Left);

                if (dsCustomers != null && dsCustomers.Tables.Contains("tblOrders") && dsCustomers.Tables["tblOrders"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCustomers.Tables["tblOrders"].Rows)
                    {
                        ListViewItem li = new ListViewItem(row["CustomerName"].ToString());
                        li.SubItems.Add(row["NIC"].ToString());
                        li.SubItems.Add(row["Address"].ToString());
                        li.SubItems.Add(row["ContactNo"].ToString());
                        li.SubItems.Add(row["Items"].ToString());
                        li.SubItems.Add(row["Price"].ToString());
                        li.SubItems.Add(row["Quantity"].ToString());
                        li.SubItems.Add(row["Charges"].ToString());
                        li.SubItems.Add(row["Status"].ToString());
                        lstOrders.Items.Add(li);
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

        private void btnCharge_Click(object sender, EventArgs e)
        {
            if (txtCusName.Text == "")
            {
                MessageBox.Show("Please enter the Customer Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else if (txtNIC.Text == "")
            {
                MessageBox.Show("Please fill the NIC", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please fill the Address", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please fill the Address", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else if (txtContactNo.Text == "")
            {
                MessageBox.Show("Please fill the Contact No", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else if (cmbItems.Text == "")
            {
                MessageBox.Show("Please select the Item", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else if (cmbQuantity.Text == "")
            {
                MessageBox.Show("Please select the Quantity", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else if (cmbStatus.Text == "")
            {
                MessageBox.Show("Please select the Status", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtCusName.Clear();
            txtNIC.Clear();
            txtAddress.Clear();
            txtContactNo.Clear();
            //cmbItems.SelectedValue = null;
            cmbItems.Text = "";
            txtPrice.Clear();
            //cmbQuantity.SelectedValue = null;
            cmbQuantity.Text = "";
            txtCharge.Clear();
            //cmbStatus.SelectedValue = null;
            cmbStatus.Text = "";
            txtSearch.Clear();
        }

        private void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void carsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Cars = new Cars();
            Cars.Show();
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Customers = new Customers();
            Customers.Show();
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
            Form ItemsPriceList = new CarParts();
            ItemsPriceList.Show();
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

                string query = @"SELECT * FROM Orders 
                         WHERE CustomerName LIKE @search 
                         OR NIC LIKE @search 
                         OR Address LIKE @search 
                         OR ContactNo LIKE @search 
                         OR Items LIKE @search 
                         OR Price LIKE @search 
                         OR Quantity LIKE @search
                         OR Charges LIKE @search
                         OR Status Like @search";

                using (SqlCommand cmd = new SqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dsOrders = new DataSet();
                    adapter.Fill(dsOrders, "tblOrders");

                    // Clear and reload the ListView with the search results
                    lstOrders.Items.Clear();

                    if (dsOrders != null && dsOrders.Tables.Contains("tblOrders") && dsOrders.Tables["tblOrders"].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsOrders.Tables["tblOrders"].Rows)
                        {
                            ListViewItem li = new ListViewItem(row["CustomerName"].ToString());
                            li.SubItems.Add(row["NIC"].ToString());
                            li.SubItems.Add(row["Address"].ToString());
                            li.SubItems.Add(row["ContactNo"].ToString());
                            li.SubItems.Add(row["Items"].ToString());
                            li.SubItems.Add(row["Price"].ToString());
                            li.SubItems.Add(row["Quantity"].ToString());
                            li.SubItems.Add(row["Charges"].ToString());
                            li.SubItems.Add(row["Status"].ToString());

                            // Store OrderID in the Tag property
                            li.Tag = row["OrderID"].ToString();

                            lstOrders.Items.Add(li);
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

        private void txtNIC_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get Price for selected Item
            string strPrice = "SELECT Price FROM PriceList WHERE ItemName = '" + cmbItems.SelectedItem.ToString() + "' ";

            if (cmbItems.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the Item.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbItems.SelectedIndex > 0)
            {
                SqlCommand cmd = new SqlCommand(strPrice, Conn);

                // Add the parameter to avoid SQL injection
                cmd.Parameters.AddWithValue("@ItemName", cmbItems.SelectedItem.ToString());

                try
                {
                    Conn.Open();

                    // Execute the command and retrieve the price
                    object result = cmd.ExecuteScalar();

                    // If the result is not null, assign it to txtPrice.Text
                    if (result != null)
                    {
                        txtPrice.Text = result.ToString();
                    }
                    else
                    {
                        txtPrice.Text = "Price not found";
                    }

                    Conn.Close();
                }
                catch (Exception ex)
                {
                    // Handle any errors that may have occurred
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void cmbQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal price;
            int quantity;
            decimal total;

            //Convert txtPrice.Text to a decimal
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Please enter a valid Price", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //Convert cmbQuantity.Text to an integer
            if (!int.TryParse(cmbQuantity.Text, out quantity))
            {
                MessageBox.Show("Please select a valid Quantity", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Calculate the Total
            total = price * quantity;

            // Display the total
            txtCharge.Text = total.ToString();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("ABC Car Traders", new Font("Times New Roman", 28, FontStyle.Regular), Brushes.Black, new Point(250, 50));
            e.Graphics.DrawString("Order Details", new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, new Point(300, 100));

            // Print the selected order details
            if (!string.IsNullOrEmpty(selectedOrderDetails))
            {
                e.Graphics.DrawString(selectedOrderDetails, new Font("Times New Roman", 16, FontStyle.Regular), Brushes.Black, new Point(50, 200));
            }
            else
            {
                e.Graphics.DrawString("No order selected.", new Font("Times New Roman", 14, FontStyle.Regular), Brushes.Black, new Point(60, 250));
            }
        }

        private void lstOrders_DoubleClick(object sender, EventArgs e)
        {
            if (lstOrders.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = lstOrders.SelectedItems[0];

                // Populate the text fields with the selected item data
                txtCusName.Text = selectedItem.SubItems.Count > 0 ? selectedItem.SubItems[0].Text : string.Empty;
                txtNIC.Text = selectedItem.SubItems.Count > 1 ? selectedItem.SubItems[1].Text : string.Empty;
                txtAddress.Text = selectedItem.SubItems.Count > 2 ? selectedItem.SubItems[2].Text : string.Empty;
                txtContactNo.Text = selectedItem.SubItems.Count > 3 ? selectedItem.SubItems[3].Text : string.Empty;
                cmbItems.Text = selectedItem.SubItems.Count > 4 ? selectedItem.SubItems[4].Text.Trim() : string.Empty;
                txtPrice.Text = selectedItem.SubItems.Count > 5 ? selectedItem.SubItems[5].Text : string.Empty;
                cmbQuantity.Text = selectedItem.SubItems.Count > 6 ? selectedItem.SubItems[6].Text.Trim() : string.Empty;
                txtCharge.Text = selectedItem.SubItems.Count > 7 ? selectedItem.SubItems[7].Text : string.Empty;
                cmbStatus.Text = selectedItem.SubItems.Count > 8 ? selectedItem.SubItems[8].Text : string.Empty;
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
                this.Close();
                Form login = new Login();
                login.Show();
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            if (txtNIC.Text == "")
            {
                MessageBox.Show("Please enter the NIC", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {

                try
                {
                    string query = "SELECT Name, Address, ContactNo FROM Registration WHERE NIC = @nic AND UserRole = 'Customer' ";
                    SqlCommand cmd = new SqlCommand(query, Conn);
                    cmd.Parameters.AddWithValue("@nic", txtNIC.Text);

                    Conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtCusName.Text = reader["Name"].ToString();
                        txtAddress.Text = reader["Address"].ToString();
                        txtContactNo.Text = reader["ContactNo"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No order found with this NIC.");
                        txtCusName.Clear();
                        txtAddress.Clear();
                        txtContactNo.Clear();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while retrieving order details: " + ex.Message);
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }
            }
        }
    }
}
