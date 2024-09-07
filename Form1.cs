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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set up the progress bar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            // Start the timer
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Increment the progress bar value
            progressBar1.PerformStep();

            // Check if the progress bar is fully loaded
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                // Stop the timer
                timer1.Stop();

                // Load the login form
                Login loginForm = new Login();
                loginForm.Show();

                // Optionally hide or close the main form
                this.Hide();
            }
        }
    }
}
