using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DbgHelpers;

namespace MemOps
{

    public partial class Compare2Files : Form
    {

        public Compare2Files()
        {
            InitializeComponent();          
        }

        private void btnFile1_Click(object sender, EventArgs e)
        {            
            tbFile1.Text = DoFileDialog();
        }
        private void btnFile2_Click(object sender, EventArgs e)
        {
            tbFile2.Text = DoFileDialog();
        }
        private string DoFileDialog()
        {
            string result = "";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (.txt)|*.txt|Log Files (*.log)|*.log";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;

            DialogResult userClickedOK = ofd.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == DialogResult.OK)
            {
                result = ofd.FileName;
            }

            return result;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            ComparisionUtility cu = new ComparisionUtility();


            // Check File 1
            cu.File1 = tbFile1.Text;
            // Check File 2
            cu.File2 = tbFile2.Text;
            // check Same Type Same Count

            if (cbEqualCount.CheckState == CheckState.Checked)
                cu.RemoveSameCount = true;
            else
                cu.RemoveSameCount = false;

            // Comparision Method
            if (rbHeapCompare.Checked)
                cu.ComparisionMethod = ComparisionMethods.CompareNetHeap;
            else
                if (rbMemPage.Checked)
                    cu.ComparisionMethod = ComparisionMethods.CompareMemPages;

            // Generate Report
            DecideOnReport2Run(cu);

            MessageBox.Show("Report has been generated");

        }

        private void DecideOnReport2Run(ComparisionUtility cu)
        {
            switch (cu.ComparisionMethod)
            {
                case ComparisionMethods.CompareNetHeap:
                    //GenerateReportNetHeap(cu);
                    break;

                case ComparisionMethods.CompareMemPages:
                    GenerateReportMemPages(cu);
                    break;

                default:
                    break;
            }
        }

        //private void GenerateReportNetHeap( ComparisionUtility cu )
        //{

        //    List<HeapCompareStatEntry> MergedHeaps = null;
        //    var ReportFile = cu.CalculateReportFileName(cu.File1);

        //    MergedHeaps = cu.CompareTwoHeapsBasedOnTypeName();


        //    // Example #4: Append new text to an existing file.
        //    // The using statement automatically flushes AND CLOSES the stream and calls 
        //    // IDisposable.Dispose on the stream object.
        //    using(System.IO.StreamWriter file = new System.IO.StreamWriter(ReportFile, false) )            
        //    {
        //        file.WriteLine("MT\tClassName\tCount1\tCount2\tCountDelta\tTotal1\tTotal2\tTotalDelta");

        //        foreach (HeapCompareStatEntry hcse in MergedHeaps)
        //        {
        //            file.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}", hcse.MT, hcse.ClassName, hcse.Count, hcse.Count2, hcse.Count2 - hcse.Count, hcse.Total, hcse.Total2, hcse.Total2 - hcse.Total);
        //        }
        //    }
        //}

        private void GenerateReportMemPages( ComparisionUtility cu)
        {
            var ReportFile = cu.CalculateReportFileName(cu.File1);
            // virtual Memory region Compare
            MemoryRegions mr1 = new MemoryRegions();
            MemoryRegions mr2 = new MemoryRegions();
            MemoryRegions mrDiff = new MemoryRegions();
            mr1.InitializeMemoryRegions(cu.File1);
            mr2.InitializeMemoryRegions(cu.File2);

            mrDiff.MemRegs = MemoryRegions.Diff(mr2.MemRegs, mr1.MemRegs);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(ReportFile, false))
            {
                foreach (MemoryRegionEntry mer in mrDiff.MemRegs)
                    file.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8} ", mer.BaseAddress.ToString("X"), mer.EndAddress.ToString("X"), mer.RegionSize.ToString("X"), mer.RegionSizeDec.ToString(), mer.Type, mer.State, mer.Protection, mer.Usage, mer.MoreInfo);
            }
            
        }

    }
}
