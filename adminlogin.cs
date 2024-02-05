using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_MA
{
    public partial class adminlogin : Form
    {
        public adminlogin()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void adminlogin_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = textBox1.Text;
            string pass = textBox2.Text;

            if (a == "USTMA" || a == "ustma" && pass == "UST@UPL")
            {
                var newForm = new admindash();
                newForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            homw form1 = new homw();
            form1.Show();
            this.Hide();
        }
    }
}
