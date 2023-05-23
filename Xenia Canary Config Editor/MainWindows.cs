using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenia_Canary_Config_Editor
{
    public partial class MainWindows : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        public MainWindows()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Process[] processes = Process.GetProcessesByName("xenia-canary");
            //foreach (Process proc in processes)
            //{
            //    proc.WaitForInputIdle();
            //    SetParent(proc.MainWindowHandle, this.Handle);
            //}

        }

        private void MainWindows_Load(object sender, EventArgs e)
        {
            Process Xenia = Process.Start("xenia_canary.exe");
            Xenia.WaitForInputIdle();
            while (Xenia.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(1000);
                Xenia.Refresh();
            }
            SetParent(Xenia.MainWindowHandle, this.panel1.Handle);
        }
    }
}
