using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UST_MA
{  
    public partial class Form3 : Form
    { 
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var newForm = new IT_DEP();
            newForm.Show();
            this.Hide();        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        
    }
}
