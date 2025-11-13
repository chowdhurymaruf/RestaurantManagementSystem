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

namespace Restaurant_Management_System.Model
{
    public partial class frmStaffAdd : SampleAdd
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        public int Id = 0;

    public override void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (Id == 0) //For Save
            {
                qry = "Insert into Staff values (@Name, @phone, @role)";
            }
            else //For Update
            {
                qry = "Update staff Set sName = @Name, sPhone = @phone, sRole = @role where staffID = @Id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@Id", Id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@phone", txtPhone.Text);
            ht.Add("@role", cbRole.Text);

            if (MainClass.SQl(qry, ht) > 0)
            {
                MessageBox.Show("Data Saved Successfully");
                Id = 0;
                txtName.Text = "";
                txtPhone.Text = "";
                cbRole.SelectedIndex = -1;
                txtName.Clear();
                txtName.Focus();

                this.Close();
            }
        }

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
