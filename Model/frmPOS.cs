using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public int order_id = 0;
        public string OrderType;
        public string WaiterName;
        
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
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(50, 55, 89);
                    b.Size = new Size(134, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["catName"].ToString();

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

        private void AddItems(string id, String pro_id, string name, string cat, string price, Image fImage)
        {
            var w = new ucProduct()
            {
                fName = name,
                fPrice = price,
                fCategory = cat,
                fImage = fImage,
                id = Convert.ToInt32(pro_id)
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    // this will check if the item is already in the cart
                    if (Convert.ToInt32(item.Cells["dgvpro_id"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                        double.Parse(item.Cells["dgvPrice"].Value.ToString());

                        return;
                    }
                }
                //this line will add new item to the cart and 2nd 0 from id
                guna2DataGridView1.Rows.Add(new object[] { 0, 0, wdg.id, wdg.fName, 1, wdg.fPrice, wdg.fPrice });
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
                AddItems("0", item["fId"].ToString(), item["fName"].ToString(), item["catName"].ToString(),
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

        private double currentTotal = 0;
        private void GetTotal()
        {
            currentTotal = 0;
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                currentTotal += Convert.ToDouble(item.Cells["dgvAmount"].Value);
            }

            lblTotal.Text = currentTotal.ToString("N2");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            order_id = 0;
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
            OrderType = "Dine In";
            // need to create a form for table and waiter selection
            frmTableSelect frm = new frmTableSelect();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.TableName != "")
                {
                    lblTable.Text = frm.TableName;
                    lblTable.Visible = true;
                }

                else
                {
                    lblTable.Text = "";
                    lblTable.Visible = false;
                }
            }

            frmWaiterSelect frm2 = new frmWaiterSelect();
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                if (frm2.WaiterName != "")
                {
                    lblWaiter.Text = frm2.WaiterName;
                    lblWaiter.Visible = true;
                }

                else
                {
                    lblWaiter.Text = "";
                    lblWaiter.Visible = false;

                }
            }
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            //Save the data to the database
            //create tables

            string qry1 = ""; //for order table
            string qry2 = ""; // for orderDetails table

            //int order_id = 0;
            int order_item_id = 0;

            if (order_id == 0) //insert
            {
                qry1 = @"insert into orders (aDate, aTime, orderType, TableName, WaiterName, total, status) 
                         values (@aDate, @aTime, @orderType, @TableName, @WaiterName, @Total, @Status); Select SCOPE_IDENTITY()";
            }

            else // update
            {
                qry1 = @"update orders set Status = @Status, Total = @Total where order_id = @ID"; // removed the comma
            }

            
            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", order_id);
            cmd.Parameters.AddWithValue("@aDate",Convert.ToDateTime(DateTime.Now.Date));  //ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());  //ToString("hh:mm:ss tt"));
            cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
            cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@Status", "Pending");
            cmd.Parameters.AddWithValue("@orderType", OrderType);
            cmd.Parameters.AddWithValue("@Total", Convert.ToDouble(lblTotal.Text));// as we only saving data for kitchen value will update when payment recevied
            
            if (MainClass.con.State == ConnectionState.Closed)
            {
                MainClass.con.Open();
            }
            if (order_id == 0)
            {
                order_id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else
            {
                cmd.ExecuteNonQuery();
            }
            if (MainClass.con.State == ConnectionState.Open)
            {
                MainClass.con.Close();
            }
            /*
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                order_item_id = Convert.ToInt32(row.Cells["dgvId"].Value);

                if (order_item_id == 0)
                {
                    qry2 = "insert into order_item values ( @order_id, @proID, @quantity, @price, @subtotal)";
                }
                else
                {
                    qry2 = "update order_item set proID = @proID, quantity = @quantity, " +
                        "price = @price, subtotal = @subtotal where order_item_id = @order_item_id";
                }

                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", order_item_id);
                cmd2.Parameters.AddWithValue("@order_id", order_id);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvpro_id"].Value));
                cmd2.Parameters.AddWithValue("@quantity", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@subtotal", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                if (MainClass.con.State == ConnectionState.Closed)
                {
                    MainClass.con.Open();
                }
                cmd2.ExecuteNonQuery();

                if (MainClass.con.State == ConnectionState.Open)
                {
                    MainClass.con.Close();
                }
            }
            */
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.IsNewRow) continue; // skip the empty row

                order_item_id = row.Cells["dgvId"].Value == null ? 0 : Convert.ToInt32(row.Cells["dgvId"].Value);

                if (order_item_id == 0)
                {
                    qry2 = @"INSERT INTO order_item (order_id, pro_id, quantity, price, subtotal) 
                 VALUES (@order_id, @pro_id, @quantity, @price, @subtotal)";
                }
                else
                {
                    qry2 = @"UPDATE order_item 
                 SET pro_id = @pro_id, quantity = @quantity, price = @price, subtotal = @subtotal 
                 WHERE order_item_id = @order_item_id";
                }

                using (SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con))
                {
                    cmd2.Parameters.AddWithValue("@order_item_id", order_item_id);
                    cmd2.Parameters.AddWithValue("@order_id", order_id);
                    cmd2.Parameters.AddWithValue("@pro_id", Convert.ToInt32(row.Cells["dgvpro_id"].Value));
                    cmd2.Parameters.AddWithValue("@quantity", Convert.ToInt32(row.Cells["dgvQty"].Value));
                    cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                    cmd2.Parameters.AddWithValue("@subtotal", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                    if (MainClass.con.State == ConnectionState.Closed) MainClass.con.Open();
                    cmd2.ExecuteNonQuery();
                    if (MainClass.con.State == ConnectionState.Open) MainClass.con.Close();
                }
            }

            guna2MessageDialog1.Show ("Order has been placed successfully");
            //order_id = 0;
            //order_item_id = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            lblTotal.Text = "00";
        }

        public int id = 0;
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            frm.ShowDialog();

            if (frm.order_id > 0)
            {
                id = frm.order_id;
                LoadEntries();
            }
        }

        private void LoadEntries()
        {
            string qry = @"Select * from Orders o
                                inner join order_item oi on o.order_id = oi.order_id
                                inner join foodItems f on f.fID = oi.pro_id
                                    where o.order_id = " + id + "";

            SqlCommand cmd2 = new SqlCommand(qry, MainClass.con);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            //MessageBox.Show("Rows returned: " + dt2.Rows.Count.ToString()); --for debugging an issue


            guna2DataGridView1.Rows.Clear();
            foreach(DataRow item in dt2.Rows)
            {
                string order_item_id = item["order_item_id"].ToString();
                string proName = item["fName"].ToString();
                string proID = item["pro_id"].ToString();
                string quantity = item["quantity"].ToString();
                string price = item["price"].ToString();
                string subtotal = item["subtotal"].ToString();

                object[] obj = {0, order_item_id, proID, proName, quantity, price, subtotal};
                
                guna2DataGridView1.Rows.Add(obj);
            }
            GetTotal();
        }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            GetTotal();
            frmCheckout frm = new frmCheckout(id, currentTotal);
            frm.ShowDialog();
        }
    }          
}
