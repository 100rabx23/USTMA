using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Specialized;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace UST_MA
{
    public partial class Form2 : Form
    {
        String randomCode;
#pragma warning disable CS0649 // Field 'Form2.name' is never assigned to, and will always have its default value null
        String name;
#pragma warning restore CS0649 // Field 'Form2.name' is never assigned to, and will always have its default value null
        public static String to;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
#pragma warning disable CS0169 // The field 'Form2.storedOtp' is never used
        private int storedOtp;
#pragma warning restore CS0169 // The field 'Form2.storedOtp' is never used
        private void button1_Click(object sender, EventArgs e)
        {
            //its for send opt button after entering number 
            String from, pass, messageBody;
            Random rand = new Random();
            randomCode = (rand.Next(999999)).ToString();
            MailMessage message = new MailMessage();
            to = (textBox1.Text).ToString();
            from = "ustma.upluniversity@gmail.com";
            pass = "ibpfzepfxadglnze";
            messageBody = $"Dear {name},\n\n" +
                                 "Welcome to the UPL University of Sustainable Technology Maintenance Application (USTMA)... We are pleased to have you on board.\n\n" +
                                 $"Your One-Time Password (OTP) for accessing the USTMA platform is: {randomCode}.\n\n" +
                                 "As a part of our commitment to ensuring a seamless experience for all users, we provide a platform where faculty and staff can report any issues or problems encountered within the system or other related areas.\n\n" +
                                 "Please use this OTP to log in to your account and start utilizing the features of USTMA. If you have any questions or require assistance, feel free to reach out to our support team.\n\n" +
                                 "Thank you for choosing USTMA. We look forward to your contributions towards maintaining a sustainable technology ecosystem at UPL University.\n\n" +
                                 "Best regards,\nUSTMA Team";
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            string Signature = "--\n saurabh";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
            //messageBody = " Hello and welcome to USTMA! Your OTP is  " + randomCode + "  from:- USTMA!";
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "OTP for Loging! --USTMA";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);

            try
            {
                smtp.Send(message);
                MessageBox.Show("Code Send Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this is used to verifing opt!!
            if (randomCode == (textBox2.Text).ToString())
            {
                MessageBox.Show("OPT VERIFY SUCCESFULLY!!");
                var newform = new Form3();
                newform.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong Code");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Name = textBox3.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
