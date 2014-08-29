using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cfg = PictureBackup.Config.Config;

namespace PictureBackup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            inListView.KeyDown += DeleteIn;
            outListView.KeyDown += DeleteOut;
            Config.Config.Init();
            for (var i = 0; i < Cfg.Settings["in"]["count"].intValue; i++)
            {
                inListView.Items.Add(new ListViewItem(Cfg.Settings["in"]["Entry" + i].Value));
            }

            for (var i = 0; i < Cfg.Settings["out"]["count"].intValue; i++)
            {
                inListView.Items.Add(new ListViewItem(Cfg.Settings["out"]["Entry" + i].Value));
            }
        }

        private void DeleteOut(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem selectedItem in outListView.SelectedItems)
                {
                    var index = selectedItem.Index;
                    var count = Cfg.Settings["out"]["count"].intValue;
                    Cfg.Settings["out"]["count"].intValue = count - 1;
                    if (index < count - 1)
                    {
                        var newValue = Cfg.Settings["out"]["Entry" + (count - 1)].Value;
                        Cfg.Settings["out"]["Entry" + index].Value = newValue;
                    }
                    Cfg.Xmlconfig.Commit();
                    selectedItem.Remove();
                }
            }
        }

        private void DeleteIn(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem selectedItem in inListView.SelectedItems)
                {
                    var index = selectedItem.Index;
                    var count = Cfg.Settings["in"]["count"].intValue;
                    Cfg.Settings["in"]["count"].intValue = count - 1;
                    if (index < count - 1)
                    {
                        var newValue = Cfg.Settings["in"]["Entry" + (count - 1)].Value;
                        Cfg.Settings["in"]["Entry" + index].Value = newValue;
                    }
                    Cfg.Xmlconfig.Commit();
                    selectedItem.Remove();
                }
            }
        }

        private void InButtonAddClick(object sender, EventArgs e)
        {
            using (var odd = new FolderBrowserDialog())
            {
                if (odd.ShowDialog() == DialogResult.OK)
                {
                    var oldCount = Cfg.Settings["in"]["count"].intValue;
                    Cfg.Settings["in"]["Entry" + oldCount].Value = odd.SelectedPath;
                    Cfg.Settings["in"]["count"].intValue = oldCount + 1;
                    Cfg.Xmlconfig.Commit();
                    inListView.Items.Add(new ListViewItem(odd.SelectedPath));
                }
            }
        }

        private void OutButtonAddClick(object sender, EventArgs e)
        {
            using (var odd = new FolderBrowserDialog())
            {
                if (odd.ShowDialog() == DialogResult.OK)
                {
                    var oldCount = Cfg.Settings["out"]["count"].intValue;
                    Cfg.Settings["out"]["Entry" + oldCount].Value = odd.SelectedPath;
                    Cfg.Settings["out"]["count"].intValue = oldCount + 1;
                    Cfg.Xmlconfig.Commit();
                    outListView.Items.Add(new ListViewItem(odd.SelectedPath));
                }
            }
        }
    }
}
