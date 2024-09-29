using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Xenia_Canary_Config_Editor
{
    public partial class NewConfigEditor : Form
    {
        string text = null;
        string[] lines;
        string[] values;
        public NewConfigEditor()
        {
            InitializeComponent();
        }

        private void NewConfigEditor_Load(object sender, EventArgs e)
        {
            try
            {
                var fileStream = new FileStream("xenia-canary.config.toml", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                lines = text.Split('\n');
                values = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("#"))
                    {
                        lines[i] = lines[i].Remove(lines[i].IndexOf("#"));
                    }
                    if (lines[i].Contains("["))
                    {
                        string NodeName = lines[i].Replace("[", "").Replace("]", "");
                        treeView1.Nodes.Add(NodeName);
                        for (int j = i + 1; j < lines.Length; j++)
                        {
                            if (lines[j].Contains("#"))
                            {
                                lines[j] = lines[j].Remove(lines[j].IndexOf("#"));
                            }
                            if (!String.IsNullOrEmpty(lines[j]) && !String.IsNullOrWhiteSpace(lines[j]))
                            {
                                if (lines[j].Contains("["))
                                {
                                    break;
                                }
                                else
                                {
                                    string[] keyvalue = lines[j].Split('=');
                                    treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add(j.ToString(), keyvalue[0].Trim());
                                    values[j] = keyvalue[1].Trim();
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    label2.Text = treeView1.SelectedNode.Text;
                    textBox1.Text = values[int.Parse(treeView1.SelectedNode.Name)];
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                //if file exists
                if (File.Exists("xenia-canary.config.toml"))
                {
                    using (StreamWriter writer = new StreamWriter("xenia-canary.config.toml", false))
                    {
                        string output = text;
                        {
                            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
                            {
                                output = output.Replace(dataGridViewRow.Cells[0].Value.ToString() + " = " + dataGridViewRow.Cells[1].Value.ToString(),
                                        dataGridViewRow.Cells[0].Value.ToString() + " = " + dataGridViewRow.Cells[2].Value.ToString());
                            }
                            writer.Write(output);
                        }
                        writer.Close();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("xenia-canary.config.toml not found!","Error Message!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No changes made!");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (textBox1.Text != values[int.Parse(treeView1.SelectedNode.Name)])
                    {
                        foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
                        {
                            if (dataGridViewRow.Cells[0].Value.ToString() == treeView1.SelectedNode.Text)
                            {
                                dataGridView1.Rows.Remove(dataGridViewRow);
                            }
                        }
                        dataGridView1.Rows.Add(treeView1.SelectedNode.Text, values[int.Parse(treeView1.SelectedNode.Name)], textBox1.Text, "Delete");
                    }
                }
            }
            catch { }
        }
        private void searchintreeviewchildnodes()
        {
            try
            {
                if (textBox2.Text != "")
                {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        foreach (TreeNode childnode in node.Nodes)
                        {
                            if (childnode.Text.Contains(textBox2.Text))
                            {
                                childnode.BackColor = Color.Yellow;
                                childnode.Parent.Expand();
                                break;
                            }
                            else
                            {
                                childnode.BackColor = Color.White;
                                childnode.Parent.Collapse();
                            }
                        }
                    }
                }
                else
                {
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        node.BackColor = Color.White;
                        foreach (TreeNode childnode in node.Nodes)
                        {
                            childnode.BackColor = Color.White;
                            childnode.Parent.Collapse();
                        }
                    }
                }
            }
            catch
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    node.BackColor = Color.White;
                    foreach (TreeNode childnode in node.Nodes)
                    {
                        childnode.BackColor = Color.White;
                        childnode.Parent.Collapse();
                    }
                }
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            searchintreeviewchildnodes();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if clicked on a button in the datagridview cell show message box
            if (e.ColumnIndex == 3)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this row?", "Delete Row", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
        }
    }
}
