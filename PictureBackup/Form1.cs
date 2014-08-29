using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureBackup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Config.Config.Init();
            var current = 0;
            while (true)
            {
                string value = Config.Config.Settings["in"][current + ""].Value;
                if (value != "")
                {
                    listView1.Items.Add(new ListViewItem(value));
                }
                else
                {
                    break;
                }
            }

            current = 0;
            while (true)
            {
                string value = Config.Config.Settings["out"][current + ""].Value;
                if (value != "")
                {
                    listView2.Items.Add(new ListViewItem(value));
                }
                else
                {
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var odd = new FolderBrowserDialog())
            {
                if (odd.ShowDialog() == DialogResult.OK)
                {
                    Config.Config.Settings["in"][listView1.Items.Count + ""].Value = odd.SelectedPath;
                    listView1.Items.Add(new ListViewItem(odd.SelectedPath));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var odd = new FolderBrowserDialog())
            {
                if (odd.ShowDialog() == DialogResult.OK)
                {
                    Config.Config.Settings["out"][listView1.Items.Count + ""].Value = odd.SelectedPath;
                    listView2.Items.Add(new ListViewItem(odd.SelectedPath));
                }
            }
        }
    }
}
