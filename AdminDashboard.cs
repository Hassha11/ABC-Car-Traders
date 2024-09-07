using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Car_Traders
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            //this.Close();
            Form login = new Login();
            login.Show();
        }

        private void btnCars_Click(object sender, EventArgs e)
        {
            //this.Close();
            Form Cars = new Cars();
            Cars.Show();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            //this.Close();
            Form Customers = new Customers();
            Customers.Show();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            //this.Close();
            Form Orders = new Orders();
            Orders.Show();
        }

        private void btnCarParts_Click(object sender, EventArgs e)
        {
            //this.Close();
            Form CarParts = new CarParts();
            CarParts.Show();
        }
    }
}
