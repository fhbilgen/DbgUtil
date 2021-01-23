using System;
using System.Windows.Forms;
using DbgHelpers;

namespace DbgUtils
{
    public partial class frmTimeCalculations : Form
    {
        TimeCalculation T1;
        TimeCalculation T2;

        public frmTimeCalculations()
        {
            InitializeComponent();
        }

        private void btnCalc1_Click(object sender, EventArgs e)
        {            
            T1 = new TimeCalculation(int.Parse(txtHours1.Text), int.Parse(txtMinutes1.Text), int.Parse(txtSeconds1.Text), int.Parse(txtCPUs1.Text), int.Parse(txtDays1.Text) );
            
            txtTMinutes1.Text = T1.TotalMinutes.ToString();
            txtTSeconds1.Text = T1.TotalSeconds.ToString();            
        }

        private void btnCalc2_Click(object sender, EventArgs e)
        {
            T2 = new TimeCalculation(int.Parse(txtHours2.Text), int.Parse(txtMinutes2.Text), int.Parse(txtSeconds2.Text), int.Parse(txtCPUs2.Text), int.Parse(txtDays2.Text));

            txtTMinutes2.Text = T2.TotalMinutes.ToString();
            txtTSeconds2.Text = T2.TotalSeconds.ToString();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            txtTMinutes.Text = TimeCalculation.SumMinutes(T1, T2).ToString();
            txtTSeconds.Text = TimeCalculation.SumSeconds(T1, T2).ToString();

            txtDMinutes.Text = TimeCalculation.DifferenceMinutes(T1, T2).ToString();
            txtDSeconds.Text = TimeCalculation.DifferenceSeconds(T1, T2).ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {        
            txtDSeconds.Clear();
            txtTSeconds.Clear();
            txtDMinutes.Clear();
            txtTMinutes.Clear();
            txtTSeconds2.Clear();
            txtTSeconds1.Clear();
            txtTMinutes2.Clear();
            txtTMinutes1.Clear();
            txtCPUs2.Clear();
            txtCPUs1.Clear();
            txtDays1.Clear();
            txtDays2.Clear();
            txtHours1.Clear();
            txtSeconds2.Clear();
            txtHours2.Clear();
            txtSeconds1.Clear();
            txtMinutes1.Clear();
            txtMinutes2.Clear();
        }

    }
}
