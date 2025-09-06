using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management_System
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Database create and user table
            if(MainClass.isValidUser(txtUser.Text, txtPass.Text) == false)
            {
                guna2MessageDialog1.Show("Invalid credentials, please try again");
            }
            else
            {
                this.Hide();
                frmMain frm = new frmMain();
                frm.Show();
            }

        }
    }
}
