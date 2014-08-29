using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Cfg = PictureBackup.Config.Config;

namespace PictureBackup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Config.Config.Init();
            inListView.KeyDown += DeleteIn;
            outListView.KeyDown += DeleteOut;

            for (var i = 0; i < Cfg.Settings["in"]["count"].intValue; i++)
            {
                inListView.Items.Add(new ListViewItem(Cfg.Settings["in"]["Entry" + i].Value));
            }

            for (var i = 0; i < Cfg.Settings["out"]["count"].intValue; i++)
            {
                outListView.Items.Add(new ListViewItem(Cfg.Settings["out"]["Entry" + i].Value));
            }

            (new Thread(AutomaticBackup) {IsBackground = true}).Start();
        }

        private void AutomaticBackup()
        {
            while (true)
            {
                pictureBox1.Image = Image.FromFile("Resources/Green.png");
                foreach (ListViewItem item in GetListViewItems(inListView))
                {
                    if (Cfg.Settings["settings"]["recursive"].boolValue)
                    {
                        var folders = Directory.GetDirectories(item.Text, "*", SearchOption.AllDirectories);
                        SyncFolders(folders, item.Text);
                        var directFiles = Directory.GetFiles(item.Text, "*", SearchOption.AllDirectories);
                        SyncFiles(directFiles, item.Text);
                    }
                    else
                    {
                        var directFiles = Directory.GetFiles(item.Text, "*", SearchOption.TopDirectoryOnly);
                        SyncFiles(directFiles, item.Text);
                    }
                }

                foreach (ListViewItem item in GetListViewItems(outListView))
                {
                    //TODO: track deletions
                }

                pictureBox1.Image = Image.FromFile("Resources/Red.png");
                Thread.Sleep(Cfg.Settings["settings"]["updateInterval"].intValue * 60 * 1000);
            }
        }

        private void SyncFolders(string[] folders, string baseFolder)
        {
            var relative = new Uri(baseFolder, UriKind.Absolute);
            foreach (var folder in folders)
            {
                var relativeFolderPath = Uri.UnescapeDataString(relative.MakeRelativeUri(new Uri(folder, UriKind.Absolute)).ToString());

                foreach (ListViewItem item in GetListViewItems(outListView))
                {
                    Directory.CreateDirectory(Path.Combine(item.Text, relativeFolderPath));
                }
            }
        }

        private void SyncFiles(IEnumerable<string> directFiles, string relativePath)
        {
            var relative = new Uri(relativePath, UriKind.Absolute);
            foreach (var file in directFiles)
            {
                var relativeFilePath = Uri.UnescapeDataString(relative.MakeRelativeUri(new Uri(file, UriKind.Absolute)).ToString());

                foreach (ListViewItem item in GetListViewItems(outListView))
                {
                    if (!File.Exists(Path.Combine(item.Text, relativeFilePath)))
                    {
                        File.Copy(file, Path.Combine(item.Text, relativeFilePath));
                    }
                }
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
                    Cfg.Commit();
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
                    Cfg.Commit();
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
                    Cfg.Commit();
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
                    Cfg.Commit();
                    outListView.Items.Add(new ListViewItem(odd.SelectedPath));
                }
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            var window = new Settings();
            window.Show();
        }

        private delegate ListView.ListViewItemCollection GetItems(ListView lstview);

        private ListView.ListViewItemCollection GetListViewItems(ListView lstview)
        {
            var temp = new ListView.ListViewItemCollection(new ListView());
            if (!lstview.InvokeRequired)
            {
                foreach (ListViewItem item in lstview.Items)
                    temp.Add((ListViewItem)item.Clone());
                return temp;
            }
            else
                return (ListView.ListViewItemCollection)Invoke(new GetItems(GetListViewItems), new object[] { lstview });
        }
    }
}
