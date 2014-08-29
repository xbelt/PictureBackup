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
        }

        private void IntervalChanged(object sender, EventArgs e)
        {
            Cfg.Settings["settings"]["updateInterval"].intValue = (int)updateIntervalNumericUpDown.Value;
            Cfg.Commit();
        }
    }
}
