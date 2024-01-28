using System;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace UST_MA
{
    public partial class IT_DEP : Form
    {
        IFirebaseConfig firebaseConfig = new FirebaseConfig
        {
            AuthSecret = "qFNTGFqxwSdtOormFRVIDlBIVhJw3wbd5jJwOSLc",
            BasePath = "https://ustma-8fb95-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient firebaseClient;

        public IT_DEP()
        {
            InitializeComponent();
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            if (firebaseClient != null)
            {
                lblConnStatus.Text = "Firebase connected";
            }
            else
            {
                lblConnStatus.Text = "Firebase connection failed";
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void InsertDataIntoFirebase(string name, string department, string problem, string systemmake, string tag5s, string location, DateTime dateTime)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var data = new Data
                {
                    Name = name,
                    Department = department,
                    Problem = problem,
                    SystemMake = systemmake,
                    Tag5s = tag5s,
                    Location = location,
                    DateTime = dateTime.ToString()
                };

                PushResponse response = firebaseClient.Push("Complaints", data);
                MessageBox.Show("Data inserted into Firebase successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting data into Firebase: " + ex.Message);
            }
        }
        private void IT_DEP_Load(object sender, EventArgs e)
        {

        }
        private void name_TextChanged(object sender, EventArgs e)
        {
        }

        private void department_TextChanged(object sender, EventArgs e)
        {
        }

        private void problem_TextChanged(object sender, EventArgs e)
        {
        }

        private void systemmake_TextChanged(object sender, EventArgs e)
        {
        }

        private void tag5s_TextChanged(object sender, EventArgs e)
        {
        }

        private void Location_TextChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void button1_Click(object sender, EventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // Call InsertDataIntoFirebase method with the values from your controls
            InsertDataIntoFirebase(name.Text, department.Text, problem.Text, systemmake.Text, tag5s.Text, Location.Text, dateTimePicker1.Value);
            var newForm = new thanks();
            newForm.Show();
            this.Hide();
        }
    }

    public class Data
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string Problem { get; set; }
        public string SystemMake { get; set; }
        public string Tag5s { get; set; }
        public string Location { get; set; }
        public string DateTime { get; set; }
    }
}
