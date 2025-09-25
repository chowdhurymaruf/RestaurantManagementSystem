using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management_System.Model
{
    public partial class frmFoodItemAdd : SampleAdd
    {
        public frmFoodItemAdd()
        {
            InitializeComponent();
        }
        public int Id = 0;
        public int cID = 0;
        private void frmFoodItemAdd_Load(object sender, EventArgs e)
        {
            //for combobox fill
            string qry = "select catID, catName from category ";

            MainClass.CBFill(qry, cbCat);

            cbCat.DisplayMember = "catName";   // what the user sees
            cbCat.ValueMember = "catID";       // actual ID behind the scenes
            
            if (cID > 0) //for Update
            {
                cbCat.SelectedValue = cID;
            }

            if (Id > 0) //For Update
            {
                ForUpdateLoadData();
            }
           

        }
        string filePath;
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                //21:45
                txtImage.ImageLocation = filePath;
            }
        }
        public override void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (Id == 0) //For Save
            {
                qry = "Insert into foodItems values (@Name, @price, @cat, @img)";
            }
            else //For Update
            {
                qry = "Update foodItems Set fName = @Name, fPrice = @price, CategoryID = @cat, fImage = @img where fID = @Id ";
            }

            //For image
            Image temp = new Bitmap(txtImage.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imageByteArray = ms.ToArray();
            //For hashtable
            Hashtable ht = new Hashtable();
            ht.Add("@Id", Id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@price", txtPrice.Text);
            ht.Add("@cat", Convert.ToInt32(cbCat.SelectedValue));
            ht.Add("@img", imageByteArray);

            if (MainClass.SQl(qry, ht) > 0)
            {
                MessageBox.Show("Data Saved Successfully");
                Id = 0;
                cID = 0;
                txtName.Text = "";
                txtPrice.Text = "";
                cbCat.SelectedIndex = -1;
                cbCat.SelectedIndex = 0;
                txtImage.Image = Properties.Resources.browse_Icon;
                //txtName.Clear();
                txtName.Focus();

                this.Close();
            }
        }

        public void ForUpdateLoadData()
        {
            string qry= @"Select * from foodItems where fID = "+Id +"";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["fName"].ToString();
                txtPrice.Text = dt.Rows[0]["fPrice"].ToString();
                cbCat.SelectedValue = dt.Rows[0]["CategoryID"];
                byte[] img = (byte[])dt.Rows[0]["fImage"];
                using (MemoryStream ms = new MemoryStream(img))
                {
                    txtImage.Image = Image.FromStream(ms);

                }
            }
        }       
    }
}
