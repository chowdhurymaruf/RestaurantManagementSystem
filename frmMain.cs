using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.Model;
using Restaurant_Management_System.View;

namespace Restaurant_Management_System
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(Object sender, EventArgs e)
        {
            IblUser.Text = MainClass.USER;
        }

        //Method to add controls

        public void Add_Controls(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            Add_Controls(new frmCategoryView());
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnTables_Click(object sender, EventArgs e)
        {
            Add_Controls(new frmTableView());
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Add_Controls(new frmHome());
        }

        private void frmMain_Load_1(object sender, EventArgs e)
        {

        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            Add_Controls(new frmStaffView());
        }

        private void btnFoodItems_Click(object sender, EventArgs e)
        {
            Add_Controls(new frmFoodItemView());
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            frmPOS frm = new frmPOS();
            frm.Show();
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            Add_Controls(new frmKitchenView());
        }
    }
}
