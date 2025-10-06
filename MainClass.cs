using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management_System
{
     class MainClass
    {
        public static readonly string con_string = @"Data Source=MARUF\SQLEXPRESS; Initial Catalog=Restaurant Management System; Integrated Security=True;";
        public static SqlConnection con = new SqlConnection(con_string);

        public static bool isValidUser(string user, string pass)
        {
            bool isValid = false;

            string qry = @"Select * from users where username  = '" + user + "' and upass= '" + pass +"' ";
            SqlCommand cmd = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0) {
                isValid = true;
                USER = dt.Rows[0]["uName"].ToString();
            }

            return isValid;
        }

        public static string user;
        public static string USER
        {
            get { return user; }
            set { user = value; }
        }

        //Method for CURD Operation
        public static int SQl(string qry, Hashtable ht)
        {
            int res = 0;

            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                //Tweaked the code because it was throwing exception.
                if (ht != null)
                {
                    foreach (DictionaryEntry item in ht)
                    {
                        cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                    }
                }
                    
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
            return res;
        }

        //For loading data from database

        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();
                }

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
        }

        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;

            foreach(DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        //For ComboBox Fill
        public static void CBFill(string qry, ComboBox cb)
        {
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cb.DisplayMember = "name";
            cb.ValueMember = "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;
        }
     }
}
