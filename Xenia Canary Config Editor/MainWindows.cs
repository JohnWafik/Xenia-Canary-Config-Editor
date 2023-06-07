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

        [DllImport("libcdio.dll")]
        public static extern bool iso9660_ifs_read_pvd(IntPtr iso, IntPtr pvd);

        [DllImport("libcdio.dll")]
        public static extern IntPtr iso9660_open(string path, int flags);

        [DllImport("libcdio.dll")]
        public static extern void iso9660_close(IntPtr iso);

        public const int ISO9660_LIBCDIO_OPEN_READ = 0x0001;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct iso9660_pvd_t
        {
            public byte type;
            public byte id;
            public byte version;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] unused1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string system_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string volume_id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] unused2;
            public uint volume_space_size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] unused3;
            public ushort volume_set_size;
            public ushort volume_sequence_number;
            public ushort logical_block_size;
            public uint path_table_size;
            public uint type_l_path_table_loc;
            public uint optional_type_l_path_table_loc;
            public uint type_m_path_table_loc;
            public uint optional_type_m_path_table_loc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
            public byte[] root_directory_record;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string volume_set_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string publisher_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string data_preparer_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string application_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 37)]
            public string copyright_file_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 37)]
            public string abstract_file_id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 37)]
            public string bibliographic_file_id;
            public SystemTime creation_time;
        }

        // ...

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Milliseconds;
        }





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
                    //byte[] test = new byte[10];
                    //using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open)))
                    //{
                    //    reader.BaseStream.Seek(50, SeekOrigin.Begin);
                    //    reader.Read(test, 0, 10);
                    //}
                    //get image from file and add it to image list
                    try
                    {
                        Image img = Image.FromFile(file.Replace(".iso", ".jpg"));
                        imageList1.Images.Add(img);
                        item.ImageIndex = imageList1.Images.Count - 1;
                    }catch(Exception ex)
                    {
                        item.ImageIndex = 0;
                    }

                    Console.WriteLine("--------------File------------------------");
                    Console.WriteLine(file);
                    Console.WriteLine("--------------Image------------------------");
                    //foreach (byte b in test)
                    //{
                    //    Console.Write(b);
                    //}
                    //Console.WriteLine();


                    // ...

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
            if (listView1.SelectedItems.Count > 0)
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
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
    }
}
