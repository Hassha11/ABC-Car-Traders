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

namespace ABC_Car_Traders
{
    public partial class Registration : Form
    {
        SqlConnection Conn = new SqlConnection("Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;");

        public Registration()
        {
            InitializeComponent();
        }

        //private async void btnRegister_Click(object sender, EventArgs e)
        private void btnRegister_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Conn.Open();

            //    SqlTransaction transaction = Conn.BeginTransaction();

            //    if (txtUserName.Text == "")
            //    {
            //        MessageBox.Show("Please enter the User Name");
            //    }

            //    else if (txtPass.Text == "")
            //    {
            //        MessageBox.Show("Please enter the Password");
            //    }

            //    else if (cmbUserRole.Text == "")
            //    {
            //        MessageBox.Show("Please select the User Role");
            //    }

            //    else if (txtAddress.Text == "")
            //    {
            //        MessageBox.Show("Please enter the Address");
            //    }

            //    else if (txtContact.Text == "")
            //    {
            //        MessageBox.Show("Please enter the Contact No");
            //    }

            //    else if (txtNIC.Text == "")
            //    {
            //        MessageBox.Show("Please enter the NIC");
            //    }

            //    else if (cmbGender.Text == "")
            //    {
            //        MessageBox.Show("Please enter the Gender");
            //    }

            //    else
            //    {
            //        // INSERT Customer data to the Registration table
            //        string queryRegistration = @"INSERT INTO Registration (UserName, UserRole, Address, ContactNo, Password, NIC, Gender, Name)
            //        VALUES (@Username, @UserRole, @Address, @ContactNo, @Password, @Nic, @Gender, @Name); SELECT SCOPE_IDENTITY()";

            //        // INSERT User login details to the User table
            //        string queryUser = @"INSERT INTO [Users] (UserID, UserName, Password)
            //        VALUES (@UserID, @Username, @Password)";

            //        // Execute Registration query and get the new User ID
            //        using (SqlCommand cmd = new SqlCommand(queryRegistration, Conn, transaction))
            //        {
            //            cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
            //            cmd.Parameters.AddWithValue("@UserRole", cmbUserRole.Text);
            //            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            //            cmd.Parameters.AddWithValue("@ContactNo", txtContact.Text);
            //            cmd.Parameters.AddWithValue("@Password", txtPass.Text);
            //            cmd.Parameters.AddWithValue("@Nic", txtNIC.Text);
            //            cmd.Parameters.AddWithValue("@Gender", cmbGender.Text);
            //            cmd.Parameters.AddWithValue("@Name", txtName.Text);


            //            // Execute the registration command and get the new User ID
            //            var newUserID = cmd.ExecuteScalar();

            //            if (newUserID != null)
            //            {
            //                // Execute User query with the new User ID
            //                using (SqlCommand cmdUser = new SqlCommand(queryUser, Conn, transaction))
            //                {
            //                    cmdUser.Parameters.AddWithValue("@UserID", newUserID);
            //                    cmdUser.Parameters.AddWithValue("@Username", txtUserName.Text);
            //                    cmdUser.Parameters.AddWithValue("@Password", txtPass.Text);

            //                    int userRowsAffected = cmdUser.ExecuteNonQuery();

            //                    // Check if both inserts were successful
            //                    if (userRowsAffected > 0)
            //                    {
            //                        transaction.Commit();
            //                        MessageBox.Show("Registration Successful");
            //                        this.Close();
            //                        Form login = new Login();
            //                        login.Show();
            //                    }
            //                    else
            //                    {
            //                        transaction.Rollback();
            //                        MessageBox.Show("Registration Unsuccessful");
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                transaction.Rollback();
            //                MessageBox.Show("Registration Unsuccessful");
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An error occurred: " + ex.Message);
            //}
            //finally
            //{
            //    if (Conn.State == System.Data.ConnectionState.Open)
            //    {
            //        Conn.Close();
            //    }
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //this.Close();
            //Form login = new Login();
            //login.Show();
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Form login = new Login();
            login.Show();
        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();

                SqlTransaction transaction = Conn.BeginTransaction();

                if (txtUserName.Text == "")
                {
                    MessageBox.Show("Please enter the User Name", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (txtPass.Text == "")
                {
                    MessageBox.Show("Please enter the Password", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (cmbUserRole.Text == "")
                {
                    MessageBox.Show("Please select the User Role", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (txtAddress.Text == "")
                {
                    MessageBox.Show("Please enter the Address", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (txtContact.Text == "")
                {
                    MessageBox.Show("Please enter the Contact No", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (txtNIC.Text == "")
                {
                    MessageBox.Show("Please enter the NIC", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else if (cmbGender.Text == "")
                {
                    MessageBox.Show("Please enter the Gender", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    // INSERT Customer data to the Registration table
                    string queryRegistration = @"INSERT INTO Registration (UserName, UserRole, Address, ContactNo, Password, NIC, Gender, Name)
                    VALUES (@Username, @UserRole, @Address, @ContactNo, @Password, @Nic, @Gender, @Name); SELECT SCOPE_IDENTITY()";

                    // INSERT User login details to the User table
                    string queryUser = @"INSERT INTO [Users] (UserID, UserName, Password)
                    VALUES (@UserID, @Username, @Password)";

                    // Execute Registration query and get the new User ID
                    using (SqlCommand cmd = new SqlCommand(queryRegistration, Conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@UserRole", cmbUserRole.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@ContactNo", txtContact.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                        cmd.Parameters.AddWithValue("@Nic", txtNIC.Text);
                        cmd.Parameters.AddWithValue("@Gender", cmbGender.Text);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);


                        // Execute the registration command and get the new User ID
                        var newUserID = cmd.ExecuteScalar();

                        if (newUserID != null)
                        {
                            // Execute User query with the new User ID
                            using (SqlCommand cmdUser = new SqlCommand(queryUser, Conn, transaction))
                            {
                                cmdUser.Parameters.AddWithValue("@UserID", newUserID);
                                cmdUser.Parameters.AddWithValue("@Username", txtUserName.Text);
                                cmdUser.Parameters.AddWithValue("@Password", txtPass.Text);

                                int userRowsAffected = cmdUser.ExecuteNonQuery();

                                // Check if both inserts were successful
                                if (userRowsAffected > 0)
                                {
                                    transaction.Commit();
                                    MessageBox.Show("Registration Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                    Form login = new Login();
                                    login.Show();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("Registration Unsuccessful", "Unsuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Registration Unsuccessful", "Unsuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
