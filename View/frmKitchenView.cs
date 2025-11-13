using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management_System.View
{
    public partial class frmKitchenView : Form
    {
        public frmKitchenView()
        {
            InitializeComponent();
        }

        private void frmKitchenView_Load(object sender, EventArgs e)
        {
            GetOrders();
        }

        private void GetOrders()
        {
            flowLayoutPanel1.Controls.Clear();
            string qry1 = @"Select * from Orders where status = 'Pending' ";
            SqlCommand cmd1 = new SqlCommand(qry1, MainClass.con);
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt1);

            FlowLayoutPanel p1;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.Width = 230;
                p1.Height = 350;
                p1.FlowDirection = FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10, 10, 10, 10);

                FlowLayoutPanel p2 = new FlowLayoutPanel();
                p2 = new FlowLayoutPanel();
                p2.BackColor = Color.FromArgb(95, 61, 204);
                p2.AutoSize = true;
                p2.Width = 230;
                p2.Height = 125;
                p2.FlowDirection = FlowDirection.TopDown;
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Margin = new Padding(0, 0, 0, 0);

                Label lbl1 = new Label();
                lbl1.ForeColor = Color.White;
                lbl1.Margin = new Padding(10, 10, 3, 0);
                lbl1.AutoSize = true;

                Label lbl2 = new Label();
                lbl2.ForeColor = Color.White;
                lbl2.Margin = new Padding(10, 5, 3, 0);
                lbl2.AutoSize = true;

                Label lbl3 = new Label();
                lbl3.ForeColor = Color.White;
                lbl3.Margin = new Padding(10, 5, 3, 0);
                lbl3.AutoSize = true;

                Label lbl4 = new Label();
                lbl4.ForeColor = Color.White;
                lbl4.Margin = new Padding(10, 5, 3, 10);
                lbl4.AutoSize = true;

                lbl1.Text = "Table :" + dt1.Rows[i]["TableName"].ToString();
                lbl2.Text = "Waiter Name :" + dt1.Rows[i]["WaiterName"].ToString();
                lbl3.Text = "Order Time :" + dt1.Rows[i]["aTime"].ToString();
                lbl4.Text = "Order Type :" + dt1.Rows[i]["orderType"].ToString();

                p2.Controls.Add(lbl1);
                p2.Controls.Add(lbl2);
                p2.Controls.Add(lbl3);
                p2.Controls.Add(lbl4);

                p1.Controls.Add(p2);

                // now add products

                int mid = 0;
                mid = Convert.ToInt32(dt1.Rows[i]["order_id"].ToString());
                
                string qry2 = @"Select * from Orders o
                                inner join order_item oi on o.order_id = oi.order_id
                                inner join foodItems f on f.fID = oi.pro_id
                                    where o.order_id = " + mid + "";

                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);

                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    Label lbl5 = new Label();
                    lbl5.ForeColor = Color.Black;
                    lbl5.Margin = new Padding(10, 5, 3, 0);
                    lbl5.AutoSize = true;

                    int no = j + 1;

                    lbl5.Text = "" + no + " " + dt2.Rows[j]["fName"].ToString() + " " + dt2.Rows[j]["quantity"].ToString();

                    p1.Controls.Add(lbl5);
                }

                //Add button to complete the order

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size = new Size(100, 35);
                b.FillColor = Color.FromArgb(0,255,0);
                b.Margin = new Padding(30, 5, 3, 10);
                b.Text = "Complete";
                b.Tag = dt1.Rows[i]["order_id"].ToString(); //store the id

                b.Click += new EventHandler(b_click);
                p1.Controls.Add(b);


                flowLayoutPanel1.Controls.Add(p1);
            }
        }

        private void b_click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            if (guna2MessageDialog1.Show("Are you sure you want to delete?") == DialogResult.Yes)
            {

                string qry = @"Update Orders set status = 'Complete' where order_id = @ID";
                Hashtable ht = new Hashtable();
                ht.Add("@ID", id);

                if (MainClass.SQl(qry, ht) > 0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Saved Successfully");
                }
                GetOrders();
            }
        }
    }       
}
