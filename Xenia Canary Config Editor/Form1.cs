using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Xenia_Canary_Config_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
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
                string text;
                var fileStream = new FileStream("xenia-canary.config.toml", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                string[] ConfigData = text.Split('\n');

                string[] APU = ConfigData[1].Split(' ');
                comboBox1.SelectedIndex = comboBox1.FindString(APU[2].Remove(APU[2].Length - 1).Remove(0, 1));

                string[] ffmpeg_verbose = ConfigData[2].Split(' ');
                checkBox1.Checked = bool.Parse(ffmpeg_verbose[2]);

                string[] max_queued_frames = ConfigData[3].Split(' ');
                textBox1.Text = max_queued_frames[2];

                string[] mute = ConfigData[4].Split(' ');
                checkBox2.Checked = bool.Parse(mute[2]);

                string[] license_mask = ConfigData[59].Split(' ');
                switch (license_mask[2])
                {
                    case "0":
                        comboBox3.SelectedIndex = comboBox3.FindString("No licenses enabled");
                        break;
                    case "1":
                        comboBox3.SelectedIndex = comboBox3.FindString("First license enabled");
                        break;
                    case "-1":
                        comboBox3.SelectedIndex = comboBox3.FindString("All possible licenses enabled");
                        break;
                }

                string[] fullscreen = ConfigData[88].Split(' ');
                checkBox3.Checked = bool.Parse(fullscreen[2]);

                string[] internal_display_resolution = ConfigData[90].Split(' ');
                switch (internal_display_resolution[2])
                {
                    case "0":
                        comboBox4.SelectedIndex = comboBox4.FindString("640x480");
                        break;
                    case "1":
                        comboBox4.SelectedIndex = comboBox4.FindString("640x576");
                        break;
                    case "2":
                        comboBox4.SelectedIndex = comboBox4.FindString("720x480");
                        break;
                    case "3":
                        comboBox4.SelectedIndex = comboBox4.FindString("720x576");
                        break;
                    case "4":
                        comboBox4.SelectedIndex = comboBox4.FindString("800x600");
                        break;
                    case "5":
                        comboBox4.SelectedIndex = comboBox4.FindString("848x480");
                        break;
                    case "6":
                        comboBox4.SelectedIndex = comboBox4.FindString("1024x768");
                        break;
                    case "7":
                        comboBox4.SelectedIndex = comboBox4.FindString("1152x864");
                        break;
                    case "8":
                        comboBox4.SelectedIndex = comboBox4.FindString("1280x720 (Default)");
                        break;
                    case "9":
                        comboBox4.SelectedIndex = comboBox4.FindString("1280x768");
                        break;
                    case "10":
                        comboBox4.SelectedIndex = comboBox4.FindString("1280x960");
                        break;
                    case "11":
                        comboBox4.SelectedIndex = comboBox4.FindString("1280x1024");
                        break;
                    case "12":
                        comboBox4.SelectedIndex = comboBox4.FindString("1360x768");
                        break;
                    case "13":
                        comboBox4.SelectedIndex = comboBox4.FindString("1440x900");
                        break;
                    case "14":
                        comboBox4.SelectedIndex = comboBox4.FindString("1680x1050");
                        break;
                    case "15":
                        comboBox4.SelectedIndex = comboBox4.FindString("1920x540");
                        break;
                    case "16":
                        comboBox4.SelectedIndex = comboBox4.FindString("1920x1080");
                        break;
                }
                string[] GPU = ConfigData[208].Split(' ');
                comboBox2.SelectedIndex = comboBox2.FindString(GPU[2].Remove(GPU[2].Length - 1).Remove(0, 1));

                string[] vsync = ConfigData[267].Split(' ');
                checkBox4.Checked = bool.Parse(vsync[2]);

                string[] vsync_interval = ConfigData[268].Split(' ');
                textBox2.Text = vsync_interval[2];

                string[] mount_cache = ConfigData[356].Split(' ');
                checkBox5.Checked = bool.Parse(mount_cache[2]);

                string[] mount_scratch = ConfigData[357].Split(' ');
                checkBox6.Checked = bool.Parse(mount_scratch[2]);

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
                string text;
                var fileStream = new FileStream("xenia-canary.config.toml", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                string[] ConfigData = text.Split('\n');
                //APU
                using (StreamWriter writer = new StreamWriter("xenia-canary.config.toml", false))
                {
                    string output = text;
                    {
                        string[] APU = ConfigData[1].Split('#');
                        output = output.Replace(APU[0].TrimEnd(), "apu = \"" + comboBox1.Text + "\"");

                        string[] ffmpeg_verbose = ConfigData[2].Split('#');
                        output = output.Replace(ffmpeg_verbose[0].TrimEnd(), "ffmpeg_verbose = " + (checkBox1.Checked ? "true" : "false"));

                        string[] max_queued_frames = ConfigData[3].Split('#');
                        output = output.Replace(max_queued_frames[0].TrimEnd(), "max_queued_frames = " + textBox1.Text);

                        string[] mute = ConfigData[4].Split('#');
                        output = output.Replace(mute[0].TrimEnd(), "mute = " + (checkBox2.Checked ? "true" : "false"));

                        string[] license_mask = ConfigData[59].Split('#');
                        string license_mask_value = "0";
                        switch (comboBox3.Text)
                        {
                            case "No licenses enabled":
                                license_mask_value = "0";
                                break;
                            case "First license enabled":
                                license_mask_value = "1";
                                break;
                            case "All possible licenses enabled":
                                license_mask_value = "-1";
                                break;
                        }
                        output = output.Replace(license_mask[0].TrimEnd(), "license_mask = " + license_mask_value);


                        string[] fullscreen = ConfigData[88].Split('#');
                        output = output.Replace(fullscreen[0].TrimEnd(), "fullscreen = " + (checkBox3.Checked ? "true" : "false"));

                        string[] internal_display_resolution = ConfigData[90].Split('#');
                        string internal_display_resolution_value = "0";
                        switch (comboBox4.Text)
                        {
                            case "640x480":
                                internal_display_resolution_value = "0";
                                break;
                            case "640x576":
                                internal_display_resolution_value = "1";
                                break;
                            case "720x480":
                                internal_display_resolution_value = "2";
                                break;
                            case "720x576":
                                internal_display_resolution_value = "3";
                                break;
                            case "800x600":
                                internal_display_resolution_value = "4";
                                break;
                            case "848x480":
                                internal_display_resolution_value = "5";
                                break;
                            case "1024x768":
                                internal_display_resolution_value = "6";
                                break;
                            case "1152x864":
                                internal_display_resolution_value = "7";
                                break;
                            case "1280x720 (Default)":
                                internal_display_resolution_value = "8";
                                break;
                            case "1280x768":
                                internal_display_resolution_value = "9";
                                break;
                            case "1280x960":
                                internal_display_resolution_value = "10";
                                break;
                            case "1280x1024":
                                internal_display_resolution_value = "11";
                                break;
                            case "1360x768":
                                internal_display_resolution_value = "12";
                                break;
                            case "1440x900":
                                internal_display_resolution_value = "13";
                                break;
                            case "1680x1050":
                                internal_display_resolution_value = "14";
                                break;
                            case "1920x540":
                                internal_display_resolution_value = "15";
                                break;
                            case "1920x1080":
                                internal_display_resolution_value = "16";
                                break;
                        }
                        output = output.Replace(internal_display_resolution[0].TrimEnd(), "internal_display_resolution = " + internal_display_resolution_value);

                        string[] GPU = ConfigData[208].Split('#');
                        output = output.Replace(GPU[0].TrimEnd(), "gpu = \"" + comboBox2.Text + "\"");

                        string[] vsync = ConfigData[267].Split('#');
                        output = output.Replace(vsync[0].TrimEnd(), "vsync = " + (checkBox4.Checked ? "true" : "false"));

                        string[] vsync_interval = ConfigData[268].Split('#');
                        output = output.Replace(vsync_interval[0].TrimEnd(), "vsync_interval = " + textBox2.Text);

                        string[] mount_cache = ConfigData[356].Split('#');
                        output = output.Replace(mount_cache[0].TrimEnd(), "mount_cache = " + (checkBox5.Checked ? "true" : "false"));

                        string[] mount_scratch = ConfigData[357].Split('#');
                        output = output.Replace(mount_scratch[0].TrimEnd(), "mount_scratch = " + (checkBox6.Checked ? "true" : "false"));


                        writer.Write(output);
                    }
                    writer.Close();
                }
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
            comboBox1.SelectedIndex = comboBox1.FindString("any");
            comboBox2.SelectedIndex = comboBox2.FindString("any");
            comboBox3.SelectedIndex = comboBox3.FindString("No licenses enabled");
            comboBox4.SelectedIndex = comboBox4.FindString("1280x720 (Default)");
            checkBox1.Checked = false; checkBox2.Checked = false;
            checkBox3.Checked = false; checkBox4.Checked = true;
            checkBox5.Checked = false; checkBox6.Checked = false;
            textBox1.Text = "64";
            textBox2.Text = "16";


        }
    }
}
