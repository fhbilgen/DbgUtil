using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbgUtils
{
    public partial class frmTickToTime : Form
    {
        public frmTickToTime()
        {
            InitializeComponent();
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTicks.Text) || !long.TryParse(txtTicks.Text, out long ticks))
            {
                MessageBox.Show("Please enter a valid number of ticks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TimeSpan time = new TimeSpan(ticks);
            string timeStr = string.Format("{0:D6}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms", time.Days, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
            txtTime.Text = timeStr;
        }

    }
}
