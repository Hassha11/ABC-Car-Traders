using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace ABC_Car_Traders
{
    public partial class Customers : Form
    {
        SqlConnection Conn = new SqlConnection("Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;");

        // Variable to hold the selected customer details
        private string selectedCustomerDetails = string.Empty;
        clsSQLQuerys sql = new clsSQLQuerys();

        int SYS_ID;

        public Customers()
        {
            InitializeComponent();
            lstCustomers.DoubleClick += lstCustomers_DoubleClick;
        }

        public class SQL
        {
            public static DataSet getDataSet(string query, SqlConnection connection, string tblCustomers)
            {
                DataSet ds = new DataSet();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, tblCustomers);
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

            int CusID { get; set; }

            public static string strCustomers()
            {
                return "SELECT * FROM Registration WHERE UserRole = 'Customer' ";
            }
        }

        private void carPartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form CarParts = new CarParts();
            CarParts.ShowDialog();
        }

        private void carsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Cars = new Cars();
            Cars.ShowDialog();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form Orders = new Orders();
            Orders.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();

                SqlTransaction transaction = Conn.BeginTransaction();

                if (txtUserName.Text == "")
                {
                    MessageBox.Show("Please fill the User Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtPass.Text == "")
                {
                    MessageBox.Show("Please fill the Password", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtName.Text == "")
                {
                    MessageBox.Show("Please fill the Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtAddress.Text == "")
                {
                    MessageBox.Show("Please fill the Address", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtContact.Text == "")
                {
                    MessageBox.Show("Please fill the Contact No", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (txtNIC.Text == "")
                {
                    MessageBox.Show("Please fill the NIC", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else if (cmbGender.Text == "")
                {
                    MessageBox.Show("Please select the Gender", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else
                {

                    string queryCustomer = @"INSERT INTO Registration (UserName, UserRole, Address, ContactNo, Password, NIC, Gender,Name)
                    VALUES (@username, 'Customer', @address, @contactno, @password, @nic, @gender, @name); SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(queryCustomer, Conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@userrole", "Customer");
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@contactno", txtContact.Text);
                        cmd.Parameters.AddWithValue("@password", txtPass.Text);
                        cmd.Parameters.AddWithValue("@nic", txtNIC.Text);
                        cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                        cmd.Parameters.AddWithValue("@name", txtName.Text);

                        var newUserID = cmd.ExecuteScalar();

                        if (newUserID != null)
                        {
                            transaction.Commit();
                            MessageBox.Show("Customer Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Customers_Load(sender, e);
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

        private void Customers_Load(object sender, EventArgs e)
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

            try
            {
                lstCustomers.FullRowSelect = true;

                Cursor.Current = Cursors.WaitCursor;
                DataSet dsCustomers = SQL.getDataSet(SQL.strCustomers(), Conn, "tblCustomers");

                lstCustomers.Clear();
                lstCustomers.View = View.Details;
                lstCustomers.Columns.Add("User Name", 100, HorizontalAlignment.Left);
                lstCustomers.Columns.Add("Name", 100, HorizontalAlignment.Left);
                lstCustomers.Columns.Add("Password", 100, HorizontalAlignment.Left);
                lstCustomers.Columns.Add("Address", 100, HorizontalAlignment.Left);
                lstCustomers.Columns.Add("Contact Number", 100, HorizontalAlignment.Left);
                lstCustomers.Columns.Add("NIC", 100, HorizontalAlignment.Left);
                lstCustomers.Columns.Add("Gender", 100, HorizontalAlignment.Left);

                if (dsCustomers != null && dsCustomers.Tables.Contains("tblCustomers") && dsCustomers.Tables["tblCustomers"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCustomers.Tables["tblCustomers"].Rows)
                    {
                        ListViewItem li = new ListViewItem(row["UserName"].ToString());
                        li.SubItems.Add(row["Name"].ToString());
                        li.SubItems.Add(row["Password"].ToString());
                        li.SubItems.Add(row["Address"].ToString());
                        li.SubItems.Add(row["ContactNo"].ToString());
                        li.SubItems.Add(row["NIC"].ToString());
                        li.SubItems.Add(row["Gender"].ToString());
                        lstCustomers.Items.Add(li);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstCustomers.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a customer to update.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Conn.Open();
                SqlTransaction transaction = Conn.BeginTransaction();

                string updateCustomerQuery = @"UPDATE Registration 
                                        SET UserName = @username, 
                                            Address = @address, 
                                            ContactNo = @contactno, 
                                            Password = @password, 
                                            NIC = @nic, 
                                            Gender = @gender, 
                                            Name = @name
                                        WHERE NIC = @nic";

                using (SqlCommand cmd = new SqlCommand(updateCustomerQuery, Conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@contactno", txtContact.Text);
                    cmd.Parameters.AddWithValue("@password", txtPass.Text);
                    cmd.Parameters.AddWithValue("@nic", txtNIC.Text);
                    cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        transaction.Commit();
                        MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Customers_Load(sender, e); // Refresh the ListView after update
                    }
                    else
                    {
                        MessageBox.Show("Update failed. No rows affected.", "Unsuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void lstCustomers_DoubleClick(object sender, EventArgs e)
        {
            if (lstCustomers.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = lstCustomers.SelectedItems[0];

                // Populate the text fields with the selected item data
                txtUserName.Text = selectedItem.SubItems.Count > 0 ? selectedItem.SubItems[0].Text : string.Empty;
                txtName.Text = selectedItem.SubItems.Count > 1 ? selectedItem.SubItems[1].Text : string.Empty;
                txtPass.Text = selectedItem.SubItems.Count > 2 ? selectedItem.SubItems[2].Text : string.Empty;
                txtAddress.Text = selectedItem.SubItems.Count > 3 ? selectedItem.SubItems[3].Text : string.Empty;
                txtContact.Text = selectedItem.SubItems.Count > 4 ? selectedItem.SubItems[4].Text : string.Empty;
                txtNIC.Text = selectedItem.SubItems.Count > 5 ? selectedItem.SubItems[5].Text : string.Empty;
                cmbGender.Text = selectedItem.SubItems.Count > 6 ? selectedItem.SubItems[6].Text.Trim() : string.Empty;

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstCustomers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (lstCustomers.SelectedItems.Count > 0)
            {
                // Confirm deletion
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Conn.Open();
                        SqlTransaction transaction = Conn.BeginTransaction();

                        // Assuming you're deleting by username or NIC
                        string deleteCustomerQuery = @"DELETE FROM Registration WHERE UserName = @username OR NIC = @nic";

                        using (SqlCommand cmd = new SqlCommand(deleteCustomerQuery, Conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@username", txtUserName.Text);
                            cmd.Parameters.AddWithValue("@nic", txtNIC.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //Customers_Load(sender, e);// Refresh the ListView after deletion
                                Customers_Load(sender, e); // Refresh the ListView after deletion
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
                MessageBox.Show("Please select a customer to delete.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPass.Clear();
            txtAddress.Clear();
            txtContact.Clear();
            txtNIC.Clear();
            cmbGender.SelectedItem = null;
            txtName.Clear();
            txtSearch.Clear();
        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (lstCustomers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a customer.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("ABC Car Traders", new Font("Times New Roman", 28, FontStyle.Regular), Brushes.Black, new Point(250, 50));
            e.Graphics.DrawString("Customer Details", new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, new Point(300, 100));

            //e.Graphics.DrawString("User Name", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 250));
            //e.Graphics.DrawString("Name", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 300));
            //e.Graphics.DrawString("User Role", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 350));
            //e.Graphics.DrawString("Address", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 400));
            //e.Graphics.DrawString("Contact No", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 450));
            //e.Graphics.DrawString("NIC", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 500));
            //e.Graphics.DrawString("Gender", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 550));

            //if (!string.IsNullOrEmpty(selectedCustomerDetails))
            //{
            //    string[] lines = selectedCustomerDetails.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            //    int startX = 50;
            //    int startY = 200;
            //    int lineHeight = 50; // Adjust this value to control the gap between rows

            //    foreach (string line in lines)
            //    {
            //        //e.Graphics.DrawString(line, new Font("Arial", 16, FontStyle.Regular), Brushes.Black, new Point(startX, startY));
            //        startY += lineHeight; // Move down by the height of one line plus the desired gap
            //    }
            //}



            // Print the selected customer details
            if (!string.IsNullOrEmpty(selectedCustomerDetails))
            {
                e.Graphics.DrawString(selectedCustomerDetails, new Font("Times New Roman", 16, FontStyle.Regular), Brushes.Black, new Point(100, 200));
            }

            else
            {
                e.Graphics.DrawString("No customer selected.", new Font("Times New Roman", 14, FontStyle.Regular), Brushes.Black, new Point(60, 250));
            }
        }

        private void lstCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCustomers.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lstCustomers.SelectedItems[0];
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("User Name  : " + selectedItem.SubItems[0].Text);
                sb.AppendLine(); // Adds an extra blank line
                sb.AppendLine("Name       : " + selectedItem.SubItems[1].Text);
                sb.AppendLine();
                sb.AppendLine("Password   : " + selectedItem.SubItems[2].Text);
                sb.AppendLine();
                sb.AppendLine("Address    : " + selectedItem.SubItems[3].Text);
                sb.AppendLine();
                sb.AppendLine("Contact No : " + selectedItem.SubItems[4].Text);
                sb.AppendLine();
                sb.AppendLine("NIC        : " + selectedItem.SubItems[5].Text);
                sb.AppendLine();
                sb.AppendLine("Gender     : " + selectedItem.SubItems[6].Text);

                selectedCustomerDetails = sb.ToString();
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
                this.Close();
                Form login = new Login();
                login.Show();
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

                string query = @"SELECT * FROM Registration 
                         WHERE UserName LIKE @search 
                         OR Address LIKE @search 
                         OR ContactNo LIKE @search 
                         OR Password LIKE @search 
                         OR NIC LIKE @search 
                         OR Gender LIKE @search
                         OR Name LIKE @search";

                using (SqlCommand cmd = new SqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dsCustomers = new DataSet();
                    adapter.Fill(dsCustomers, "tblCustomers");

                    // Clear and reload the ListView with the search results
                    lstCustomers.Items.Clear();

                    if (dsCustomers != null && dsCustomers.Tables.Contains("tblCustomers") && dsCustomers.Tables["tblCustomers"].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsCustomers.Tables["tblCustomers"].Rows)
                        {
                            ListViewItem li = new ListViewItem(row["UserName"].ToString());
                            li.SubItems.Add(row["Address"].ToString());
                            li.SubItems.Add(row["ContactNo"].ToString());
                            li.SubItems.Add(row["Password"].ToString());
                            li.SubItems.Add(row["NIC"].ToString());
                            li.SubItems.Add(row["Gender"].ToString());
                            li.SubItems.Add(row["Name"].ToString());

                            // Store CustomerID in the Tag property
                            //li.Tag = row["CustomerID"].ToString();

                            lstCustomers.Items.Add(li);
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
    }
}
