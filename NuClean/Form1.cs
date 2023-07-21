using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Management;
using LibreHardwareMonitor;
using Microsoft.VisualBasic.Devices;
using LibreHardwareMonitor.Hardware;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;

namespace NuClean
{
    public partial class Form1 : MaterialForm
    {
        public string Language { get; set; }
        private LibreHardwareMonitor.Hardware.Computer computer;
        private System.Windows.Forms.CheckBox chkStartup;


        public Form1()
        {
            InitializeComponent();
            //timer1.Start();
            materialCheckbox1.Checked = Properties.Settings.Default.RunOnStartup;

            computer = new LibreHardwareMonitor.Hardware.Computer();
            computer.IsCpuEnabled = true;
            computer.Open();


            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Cyan100, Primary.BlueGrey600, Primary.BlueGrey600, Accent.DeepOrange200, TextShade.BLACK);

        }




        // Clean temporary files
        private async void MaterialFloatingActionButton2Click(object sender, EventArgs e)
        {
            await AnimateProgressBarAsync(materialProgressBar1);
            await Function.CleanTemporaryFiles();
            await Function.CleanSystemLogs();
            Function.RunGarbageCollection();
            string output = "Cleaning temp process completed." + Environment.NewLine;
            MessageBox.Show("Cleaning temp process completed.");



        }

        public static async Task AnimateProgressBarAsync(MaterialProgressBar progressBar)
        {
            int interval = 10; // Interval in milliseconds
            int steps = 100;   // Number of steps
            int stepDuration = 1000 / steps; // Duration of each step in milliseconds
            int stepValue = 100 / steps; // Value to increment the progress bar in each step

            for (int i = 0; i <= steps; i++)
            {
                progressBar.Value = i * stepValue;
                await Task.Delay(stepDuration);
            }
        }


        public static Task<(float cpuTemperature, float gpuTemperature)> GetCPUTemperatureAsync()
        {
            return Task.Run(() =>
            {
                LibreHardwareMonitor.Hardware.Computer computer = new LibreHardwareMonitor.Hardware.Computer();
                computer.Open();

                float cpuTemperature = 0;
                float gpuTemperature = 0;

                foreach (var hardwareItem in computer.Hardware)
                {
                    hardwareItem.Update();

                    if (hardwareItem.HardwareType == HardwareType.Cpu)
                    {
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Package"))
                            {
                                cpuTemperature = sensor.Value ?? 0;
                            }
                        }
                    }
                    else if (hardwareItem.HardwareType == HardwareType.GpuNvidia || hardwareItem.HardwareType == HardwareType.GpuAmd)
                    {
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Temperature"))
                            {
                                gpuTemperature = sensor.Value ?? 0;
                            }
                        }
                    }
                }

                return (cpuTemperature, gpuTemperature);
            });
        }


        public static bool AskForPermission(string message)
        {
            var result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
        // Clean browser cache
        private async void materialFloatingActionButton3_Click(object sender, EventArgs e)
        {
            await AnimateProgressBarAsync(materialProgressBar7);

            if (AskForPermission("Are you sure ?") == true)
            {
                Function.CleanBrowserCache();
                Function.CleanChromeCache();
                Function.CleanFirefoxCache();
                Function.CleanInternetExplorerCache();
                string output = "Cleaning browser process completed." + Environment.NewLine;
                MessageBox.Show("Cleaning complete.", "Cleanup Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Function Aborted");

            }
        }

        private void AppendToRichTextBox(string text)
        {
            // Append the text to the RichTextBox
            rtbOutput.AppendText(text + Environment.NewLine);
        }

        private void EnableDarkTheme()
        {
            Function.DarkThemeOn();
        }

        private void DisableDarkTheme()
        {
            Function.DarkThemeOff();
        }



        private void materialFloatingActionButton1_Click(object sender, EventArgs e)
        {

        }

        private void materialMaskedTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void misc_Click(object sender, EventArgs e)
        {

        }

        private void materialProgressBarOther1_Click(object sender, EventArgs e)
        {

        }

        //UAC button
        private void materialFloatingActionButton4_Click(object sender, EventArgs e)
        {
            Function.UAC();
        }

        //Disk Cleanup Button
        private void materialFloatingActionButton7_Click(object sender, EventArgs e)
        {
            Function.DiskCleanup();
        }

        //GodMode Button
        private void materialFloatingActionButton5_Click(object sender, EventArgs e)
        {
            Function.GodMode();
        }

        // Uninstaller Button
        private void materialFloatingActionButton6_Click(object sender, EventArgs e)
        {
            Function.Uninstaller();
        }

        //Scheduled Tasks
        private void materialFloatingActionButton8_Click(object sender, EventArgs e)
        {
            Function.ScheduledTasks();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Shown += Form1_Shown;


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the PictureBox with the next frame
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // Programmatically trigger the click event of the button you want to press automatically
            richTextBox1.Show();

            materialButton2.PerformClick();
            materialButton2.Visible = false;

        }
        private async void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string systemInfo = await Function.LoadSystemInfo2(computer);
            rtbOutput.Text = systemInfo;
        }

        private void materialTextBox21_Click(object sender, EventArgs e)
        {

        }
        private void materialCard2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
        //Network Settings
        private void materialFloatingActionButton1_Click_1(object sender, EventArgs e)
        {
            Function.OpenCmdWithIpConfigAll();

        }
        //Disable Superfetch
        private void materialFloatingActionButton4_Click_1(object sender, EventArgs e)
        {
            AnimateProgressBarAsync(materialProgressBar2);
            Function.DisableSuperfetch();
        }

        private void materialLabel7_Click(object sender, EventArgs e)
        {

        }

        private void materialMultiLineTextBox21_Click(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            // Replace "https://www.example.com" with your desired link
            string url = "https://www.paypal.com/donate/?hosted_button_id=SP6LQVVRJ6VC8";

            try
            {
                // Create a ProcessStartInfo object
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Set UseShellExecute to true to use the default shell
                };

                // Open the link using the default web browser
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("Error opening link: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void materialButton2_Click(object sender, EventArgs e)
        {
            string systemInfo = await Function.LoadSystemInfo2(computer);
            richTextBox1.Text = systemInfo;
        }

        private async void materialButton2_Click_1(object sender, EventArgs e)
        {
            string systemInfo = await Function.LoadSystemInfo2(computer);
            richTextBox1.Text = systemInfo;
        }

        private void materialTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialMultiLineTextBox21_Click_1(object sender, EventArgs e)
        {

        }

        private async void materialButton3_Click(object sender, EventArgs e)
        {
            await Form1.AnimateProgressBarAsync(Form1.materialProgressBar6);
            {
                // Handle the form submission here (send email, store data, etc.)
                // Example: Sending an email using SmtpClient
                string name = materialTextBox21.Text;
                string email = materialTextBox22.Text;
                string subject = materialTextBox23.Text;
                string message = materialMultiLineTextBox21.Text;

                // Create a MailMessage object with sender and recipient details
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("efstratiospara@gmail.com", "NuClean");
                mail.To.Add("efstratiospara@gmail.com");
                mail.Subject = subject;
                mail.Body = $"FROM NUCLEAN APP!!! Name: {name}\nEmail: {email}\n\n{message}";

                // Create a SmtpClient object with your SMTP server details
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("efstratiospara", "xivuldmombqnnrij");
                smtpClient.EnableSsl = true;

                try
                {
                    // Send the email
                    smtpClient.Send(mail);

                    // Optionally, show a confirmation message to the user
                    MessageBox.Show("Your message has been sent!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Display an error message if the email fails to send
                    MessageBox.Show($"An error occurred while sending the email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void materialTextBox21_Click_1(object sender, EventArgs e)
        {

        }

        private void materialTextBox22_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void materialProgressBar1_Click(object sender, EventArgs e)
        {
        }

        private void materialProgressBar2_Click(object sender, EventArgs e)
        {

        }
        //Boost Ram
        private void materialFloatingActionButton9_Click(object sender, EventArgs e)
        {
            Function.BoostRam();
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            Function.Startup();
        }
        private void SetApplicationLanguage()
        {
            // Get the selected language from the settings.
            string selectedLanguage = Properties.Settings.Default.Language;

            // Set the thread's current UI culture to the selected language.
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);

            // Refresh the Form to apply the new language.
            this.Controls.Clear();
            InitializeComponent();

        }

        private string GetCultureIdentifier(string language)
        {
            switch (language)
            {
                case "English (United States)":
                    return "en-US";
                case "Spanish (Spain)":
                    return "es-ES";
                case "French (France)":
                    return "fr-FR";
                case "Greek (Greece)":
                    return "gr-GR";
                // Add more cases for other languages as needed.
                default:
                    return string.Empty;
            }
        }
        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedLanguage = comboBoxLanguage.SelectedItem.ToString();
            Properties.Settings.Default.Language = GetCultureIdentifier(selectedLanguage);
            Properties.Settings.Default.Save();
        }
    }
}