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
    public partial class frmTableAdd : SampleAdd
    {
        public frmTableAdd()
        {
            InitializeComponent();
        }

        public int Id = 0;

        public override void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (Id == 0) //For Save
            {
                qry = "Insert into tables values (@Name)";
            }
            else //For Update
            {
                qry = "Update tables Set tName = @Name where tID = @Id ";
            }

            Hashtable ht = new Hashtable();
            ht.Add("@Id", Id);
            ht.Add("@Name", txtName.Text);

            if (MainClass.SQl(qry, ht) > 0)
            {
                MessageBox.Show("Data Saved Successfully");
                Id = 0;
                txtName.Text = "";
                txtName.Clear();
                txtName.Focus();

                this.Close();
            }
        }

        private void frmTableAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
