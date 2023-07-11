using System;
using System.IO;
using System.Windows.Forms;

namespace Xenia_Canary_Config_Editor
{
    public partial class ConfigForm : Form
    {
        public MainWindows mainWindows  { get; set; }
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("XeniaLauncherConfig.conf"))
                {
                    string[] lines = File.ReadAllLines("XeniaLauncherConfig.conf");
                    //set the titlebar text to the first line of the config file
                    textBox4.Text = lines[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //save the address in textbox4 in the Xenia config file
                File.WriteAllText("XeniaLauncherConfig.conf", textBox4.Text);
                mainWindows.LoadingGamesList();
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //open directory dialog
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder where the game is installed.";
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = fbd.SelectedPath;
            }

        }
    }
}
