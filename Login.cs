using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Car_Traders
{
    public partial class Login : Form
    {
        SqlConnection Conn = new SqlConnection ("Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;");

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtUserName.Text == "")
                {
                    MessageBox.Show("Please enter the Username", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (txtPassword.Text == "")
                {
                    MessageBox.Show("Please enter the Password", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (cmbUserRole.Text == "")
                {
                    MessageBox.Show("Please select the User Role", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {

                    Conn.Open();

                    // Check if the user exists in the Users table
                    string query = "SELECT UserName, Password, UserRole FROM Registration WHERE UserName = @Username AND Password = @Password AND UserRole = @Userrole";
                    using (SqlCommand cmd = new SqlCommand(query, Conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                        cmd.Parameters.AddWithValue("@Userrole", cmbUserRole.Text);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            //MessageBox.Show("Login Successful");
                            this.Close();

                            //string strUser = "SELECT USERROLE FROM REGISTRATION WHERE USERNAME = '" + txtUserName.Text + "' AND PASSWORD = '" + txtPassword.Text + "' ";

                            if (cmbUserRole.SelectedItem.ToString() == "Admin")
                            {
                                SYS.SYS_ID = 0;
                                Form AdminDashboard = new AdminDashboard();
                                AdminDashboard.Show();
                            }
                            else if (cmbUserRole.SelectedItem.ToString() == "Customer")
                            {
                                //Form CustomerDashboard = new CustomerDashboard();
                                //CustomerDashboard.Show();
                                SYS.SYS_ID = 1;
                                Form Customer = new Customers();
                                Customer.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter the correct Username and Password", "Unsuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPassword.Clear();
            cmbUserRole.SelectedItem = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit the system?", "Confirm Exit", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }

            else
            {
                return;
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            this.Close();
            Registration form = new Registration();
            form.Show();
        }

        private void linkLblSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            Registration form = new Registration();
            form.Show();
        }
    }
}
