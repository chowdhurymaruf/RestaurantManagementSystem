using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management_System.Model;

namespace Restaurant_Management_System.View
{
    public partial class frmFoodItemView : SampleView
    {
        public frmFoodItemView()
        {
            InitializeComponent();
        }

        private void frmFoodItemView_Load(object sender, EventArgs e)
        {
            GetData();
        }
        public void GetData()
        {
            string qry = "select fID, fName, fPrice, CategoryID, c.catName from foodItems f inner join category c on c.catID = f.CategoryID where fName like '%" + txtSearch.Text + "%'";

            ListBox lb = new ListBox();
            lb.Items.Add(dgvId);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvCatID);
            lb.Items.Add(dgvCategory);

            MainClass.LoadData(qry, guna2DataGridView1, lb);
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmFoodItemAdd frm = new frmFoodItemAdd();
            //frm.Owner = this;
            frm.ShowDialog();
            GetData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Let's create table first
            GetData();

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmFoodItemAdd frm = new frmFoodItemAdd();
                frm.Id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvId"].Value);
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                frm.txtPrice.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvPhone"].Value);
                frm.cbCat.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvRole"].Value);
                frm.ShowDialog();
                GetData();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvId"].Value);
                string qry = "Delete from foodItems where fID = " + id + "";
                Hashtable ht = new Hashtable();
                MainClass.SQl(qry, null);
                MainClass.SQl(qry, ht);

                MessageBox.Show("Data Deleted Successfully");
                GetData();

            }
        }
    }
}
