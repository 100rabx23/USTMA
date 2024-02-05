using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace UST_MA
{
    public partial class admindash : Form
    {
        private IFirebaseClient firebaseClient;

        public admindash()
        {
            InitializeComponent();
            InitializeFirebase();
        }

        private void admindash_Load(object sender, EventArgs e)
        {
            RetrieveDataFromFirebase();
        }

        private void InitializeFirebase()
        {
            IFirebaseConfig firebaseConfig = new FirebaseConfig
            {
                AuthSecret = "qFNTGFqxwSdtOormFRVIDlBIVhJw3wbd5jJwOSLc",
                BasePath = "https://ustma-8fb95-default-rtdb.firebaseio.com/"
            };

            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);
            if (firebaseClient != null)
            {
                MessageBox.Show("Firebase connected");
            }
            else
            {
                MessageBox.Show("Firebase connection failed");
            }
        }

        private async void RetrieveDataFromFirebase()
        {
            FirebaseResponse response = await firebaseClient.GetTaskAsync("Complaints");
            if (response.Body != "null")
            {
                Dictionary<string, Data> dataDictionary = response.ResultAs<Dictionary<string, Data>>();

                foreach (var entry in dataDictionary)
                {
                    Data complaintData = entry.Value;
                    AddComplaintPanel(complaintData);
                }
            }
            else
            {
                MessageBox.Show("No data found in Firebase");
            }
        }

        private void AddComplaintPanel(Data complaintData)
        {
            Panel panel = new Panel();
            panel.BackColor = Color.LightGray;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Size = new Size(600, 250); // Increased panel height to accommodate the button

            Label complaintLabel = new Label();
            complaintLabel.Text = $"Name: {complaintData.Name}\nDepartment: {complaintData.Department}\nProblem: {complaintData.Problem}\nSystem Make: {complaintData.SystemMake}\nTag 5s: {complaintData.Tag5s}\nLocation: {complaintData.Location}\nDateTime: {complaintData.DateTime}\nEmail: {complaintData.Email}\nStatus: {complaintData.Status}";
            complaintLabel.Location = new Point(10, 10);
            complaintLabel.AutoSize = true; // Adjust label size automatically
            if (complaintData.Status == "Resolved")
            {
                complaintLabel.ForeColor = Color.Green;
            }
            else
            {
                complaintLabel.ForeColor = Color.Red;
            }
            panel.Controls.Add(complaintLabel);

            Button generatePDFButton = new Button();
            generatePDFButton.Text = "Generate PDF";
            generatePDFButton.Location = new Point(10, 130);
            generatePDFButton.Click += (sender, e) =>
            {
                GenerateAndSendPDF(complaintData);
                generatePDFButton.ForeColor = Color.Green; // Change button text color to green
            };
            panel.Controls.Add(generatePDFButton);

            flowLayoutPanel1.Controls.Add(panel);

            // Add scroll functionality
            flowLayoutPanel1.AutoScroll = true;
        }


        private void GenerateAndSendPDF(Data complaintData)
        {
            string recipientEmail = complaintData.Email;
            string name = complaintData.Name;
            string department = complaintData.Department;
            string problem = complaintData.Problem;
            string systemmake = complaintData.SystemMake;
            string tag5s = complaintData.Tag5s;
            string location = complaintData.Location;
            DateTime dateTime = DateTime.Parse(complaintData.DateTime);

            // Generate PDF
            GeneratePDF(name, department, problem, systemmake, tag5s, location, dateTime, recipientEmail);

            // Send Email with PDF attachment
            SendEmailWithAttachment(recipientEmail, name, department, problem, systemmake, tag5s, location, dateTime);
        }

        private void GeneratePDF(string name, string department, string problem, string systemmake, string tag5s, string location, DateTime dateTime, string email)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            string pdfFilePath = "ComplaintDetails.pdf";

            try
            {
                // Create a PdfWriter object to write to the file
                PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));

                // Open the document
                doc.Open();
                iTextSharp.text.Image headerImg = iTextSharp.text.Image.GetInstance("img/upl.png"); // Replace "path_to_your_header_image.jpg" with the actual path to your header image file
                headerImg.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                headerImg.ScaleToFit(600f, 200f); // Adjust width and height as needed
                doc.Add(headerImg);
                // Add image to the left side
                

                // Add title
                iTextSharp.text.Font titleFont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLACK);
                Paragraph title = new Paragraph("Complaint Details", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20f; // Space after the title
                doc.Add(title);

                // Add user input details
                iTextSharp.text.Font boldFont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
                iTextSharp.text.Font regularFont = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

                AddParagraph(doc, "Name:", name, boldFont, regularFont);
                AddParagraph(doc, "Department:", department, boldFont, regularFont);
                AddParagraph(doc, "Problem:", problem, boldFont, regularFont);
                AddParagraph(doc, "System Make:", systemmake, boldFont, regularFont);
                AddParagraph(doc, "Tag 5s:", tag5s, boldFont, regularFont);
                AddParagraph(doc, "Location:", location, boldFont, regularFont);
                AddParagraph(doc, "Date Time:", dateTime.ToString(), boldFont, regularFont);
                AddParagraph(doc, "Email:", email, boldFont, regularFont);

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("img/resolved.png"); // Assuming the image "resolved.jpg" is located in the "img" folder in your project
                img.ScaleToFit(200f, 200f); // Adjust width and height as needed
                img.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                doc.Add(img);

            }
            finally
            {
                // Close the document
                doc.Close();
            }
        }


        private void AddParagraph(iTextSharp.text.Document doc, string label, string value, iTextSharp.text.Font labelFont, iTextSharp.text.Font valueFont)
        {
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph();
            paragraph.Add(new iTextSharp.text.Chunk(label, labelFont));
            paragraph.Add(new iTextSharp.text.Chunk(value, valueFont));
            paragraph.SpacingAfter = 10f; // Space after each paragraph
            doc.Add(paragraph);
        }
        private void SendEmailWithAttachment(string recipientEmail, string name, string department, string problem, string systemmake, string tag5s, string location, DateTime dateTime)
        {
            string from = "ustma.upluniversity@gmail.com";
            string pass = "ibpfzepfxadglnze"; // Update with your email password

            MailMessage message = new MailMessage(from, recipientEmail);
            message.Subject = "Your Complaint successfully resolved!";
            message.Body = "Please find the attached PDF containing complaint details.";

            // Create PDF file
            GeneratePDF(name, department, problem, systemmake, tag5s, location, dateTime, recipientEmail);
            string pdfFilePath = "ComplaintDetails.pdf";

            // Attach the PDF file
            message.Attachments.Add(new Attachment(pdfFilePath));

            // Send email
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(from, pass);

            try
            {
                smtp.Send(message);
                MessageBox.Show("Email with PDF attachment sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }
            finally
            {
                // Dispose the message and delete the temporary PDF file
                message.Dispose();
                File.Delete(pdfFilePath);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
