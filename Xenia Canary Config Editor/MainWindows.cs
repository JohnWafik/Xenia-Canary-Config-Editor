using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenia_Canary_Config_Editor
{
    public partial class MainWindows : Form
    {
        [DllImportAttribute("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        //allow the user to move the form by clicking and dragging the titlebar
        const int HT_CAPTION = 0x2;
        const int WM_NCLBUTTONDOWN = 0xA1;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        Process Xenia = new Process();
        public MainWindows()
        {
            InitializeComponent();
        }
        private void MainWindows_Load(object sender, EventArgs e)
        {
            LoadingGamesList();
        }
        public void LoadingGamesList()
        {
            label1.Text = "Xenia Launcher v" + System.Windows.Forms.Application.ProductVersion;
            listView1.Items.Clear();
            string romsDir = "";
            //read the config file if exists
            if (File.Exists("XeniaLauncherConfig.conf"))
            {
                string[] lines = File.ReadAllLines("XeniaLauncherConfig.conf");
                //set the titlebar text to the first line of the config file
                romsDir = lines[0];
            }
            //else create new file
            else
            {
                //set the directory of the roms folder in the file
                romsDir = "C:\\Users\\" + Environment.UserName + "\\Documents\\";
                File.WriteAllText("XeniaLauncherConfig.conf", romsDir);
            }
            Console.WriteLine(romsDir);
            string[] filePaths = Directory.GetFiles(romsDir);
            foreach (string file in filePaths)
            {
                if (file.Contains(".iso"))
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = Path.GetFileName(file);
                    item.ToolTipText = file;
                    item.ImageIndex = 0;
                    listView1.Items.Add(item);
                }
            }

        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LaunchGame(sender, e);
        }
        private void LaunchGame(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Xenia.StartInfo.FileName = "xenia_canary.exe";
                Xenia.StartInfo.Arguments = "\"" + listView1.SelectedItems[0].ToolTipText + "\"";
                Xenia.Start();
            }
        }
        private void listView1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                toolStripStatusLabel1.Text = listView1.SelectedItems[0].ToolTipText;

            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LaunchGame(sender, e);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ConfigForm frm = new ConfigForm();
            frm.Show();
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine("Double Clicked");
            //toggle formstate from maximized to normal
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            //toggle formstate from normal to maximized
            else if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            //close the application
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //open the config form
            ConfigForm frm = new ConfigForm();
            frm.mainWindows = this;
            frm.Show();
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            //move the form by clicking and dragging the titlebar
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            LoadingGamesList();
        }
    }
}
