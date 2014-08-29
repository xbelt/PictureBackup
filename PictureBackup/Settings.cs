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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            updateIntervalNumericUpDown.ValueChanged += IntervalChanged;
            updateIntervalNumericUpDown.Value = Config.Config.Settings["settings"]["updateInterval"].intValue;

            recursiveCheckBox.Checked = Cfg.Settings["settings"]["recursive"].boolValue;

            historyCheckBox.Checked = Cfg.Settings["settings"]["history"].boolValue;

            textBox1.Text = Cfg.Settings["settings"]["historyName"].Value;
        }

        private void IntervalChanged(object sender, EventArgs e)
        {
            Cfg.Settings["settings"]["updateInterval"].intValue = (int)updateIntervalNumericUpDown.Value;
            Cfg.Commit();
        }

        private void recursiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Cfg.Settings["settings"]["recursive"].boolValue = recursiveCheckBox.Checked;
            Cfg.Commit();
        }

        private void historyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Cfg.Settings["settings"]["history"].boolValue = historyCheckBox.Checked;
            Cfg.Commit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Cfg.Settings["settings"]["historyName"].Value = textBox1.Text;
            Cfg.Commit();
        }
    }
}
