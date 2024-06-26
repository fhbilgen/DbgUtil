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
    public partial class frmDbgUtilMain : Form
    {
        public frmDbgUtilMain()
        {
            InitializeComponent();
        }

        private void timeRelatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTimeCalculations timeCalculations = new frmTimeCalculations();
            timeCalculations.ShowDialog();
        }

        private void ticksTTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTickToTime tickToTime = new frmTickToTime();
            tickToTime.ShowDialog();
        }
    }
}
