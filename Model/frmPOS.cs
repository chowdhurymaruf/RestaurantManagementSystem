using System;
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
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }
        public int MainID = 0;
        public string OrderType;
        public string WaiterName;
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            LoadfoodItems();
        }

        private void AddCategory()
        {
            string qry = "select * from Category";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row  in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(50, 55, 89);
                    b.Size = new Size(134, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row ["catName"].ToString();

                    //event for click
                    b.Click += new EventHandler(b_Click);
                    CategoryPanel.Controls.Add(b);


                }

            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            if (b.Text == "All Categories")
            {
                txtSearch.Text = "1";
                txtSearch.Text = "";
                return;
            }
            
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.fCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        private void AddItems(string id, string name, string cat, string price,Image fImage)
        {
            var w = new ucProduct()
            {
                fName = name,
                fPrice = price,
                fCategory = cat,
                fImage = fImage,
                id = Convert.ToInt32(id)
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    // this will check if the item is already in the cart
                    if (Convert.ToInt32(item.Cells["dgvId"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1 ;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) * 
                        double.Parse(item.Cells["dgvPrice"].Value.ToString());

                        return;
                    }
                }
                //this line will add new item to the cart
                guna2DataGridView1.Rows.Add(new object[] { 0, wdg.id, wdg.fName, 1, wdg.fPrice, wdg.fPrice });
                GetTotal();
            };
        }

        private void LoadfoodItems()
        {
            string qry = "select * from foodItems inner join category on catID = CategoryID";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["fImage"];
                byte[] imagebytearray = imagearray;
                AddItems(item["fId"].ToString(), item["fName"].ToString(), item["catName"].ToString(),
                    item["fPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));
            }
            
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in CategoryPanel.Controls)
            {
                if (item is Guna.UI2.WinForms.Guna2Button)
                {
                    Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)item;
                    b.Checked = false;
                }
            }
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.fName.ToLower().Contains(txtSearch.Text.ToLower());
            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }
        private void GetTotal()
        {
            double tot = 0;
            lblTotal.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }

            lblTotal.Text = tot.ToString("N2");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "00";
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Delivery";
        }

        private void btnTakeAway_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Take Away";
        }

        private void btnDineIn_Click(object sender, EventArgs e)
        {
            // need to create a form for table and waiter selection
            frmTableSelect frm = new frmTableSelect();
            if (frm.TableName != "")
            {
                lblTable.Text = frm.TableName;
                
            }

            else
            {
                lblTable.Text = "";
               
            }

            frmWaiterSelect frm2 = new frmWaiterSelect();
            if (frm2.WaiterName != "")
            {
                lblWaiter.Text = frm2.WaiterName;

            }

            else
            {
                lblWaiter.Text = "";

            }
        }
    }
}
