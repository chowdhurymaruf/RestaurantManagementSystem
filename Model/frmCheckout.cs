using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management_System.Model
{
    public partial class frmCheckout : SampleAdd
    {
        private int orderId;
        private double totalAmount;

        public frmCheckout(int orderId, double totalAmount)
        {
            InitializeComponent();
            this.orderId = orderId;
            this.totalAmount = totalAmount;
            txtTotal.Text = totalAmount.ToString("N2");
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.SelectedIndex == -1)
            {
                messageInvalid.Show("Please select a payment method.");
                return;
            }

            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            DateTime paymentDate = dtpPaymentDate.Value;

            try
            {
                if (MainClass.con.State == System.Data.ConnectionState.Closed)
                    MainClass.con.Open();

                //MessageBox.Show("Order ID: " + orderId.ToString());// debugging line

                string qry1 = @"INSERT INTO Payments (order_id, payment_date, payment_method, amount, status)
                                VALUES (@order_id, @payment_date, @payment_method, @amount, 'Paid')";

                using (SqlCommand cmd1 = new SqlCommand(qry1, MainClass.con))
                {
                    cmd1.Parameters.AddWithValue("@order_id", orderId);
                    cmd1.Parameters.AddWithValue("@payment_date", paymentDate);
                    cmd1.Parameters.AddWithValue("@payment_method", paymentMethod);
                    cmd1.Parameters.AddWithValue("@amount", totalAmount);
                    cmd1.ExecuteNonQuery();
                }

                string qry2 = @"UPDATE Orders SET Status = 'Paid' WHERE order_id = @order_id";

                using (SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con))
                {
                    cmd2.Parameters.AddWithValue("@order_id", orderId);
                    cmd2.ExecuteNonQuery();
                }
                messageSaved.Show("Payment saved and order marked as Paid.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (MainClass.con.State == System.Data.ConnectionState.Open)
                    MainClass.con.Close();
            }
        }

        public override void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
