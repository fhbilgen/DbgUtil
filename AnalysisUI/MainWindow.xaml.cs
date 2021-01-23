//using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Reflection;
using DbgHelpers;
using System.IO;

namespace AnalysisUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string FirstFile { get;  set; }
        public string SecondFile { get; set; }
        public string ReportFilePath { get; set; }
        public string ReportFileName { get; set; }
        public string ReportFileFullPath { get; set; }
        public bool RemoveSameTypeAndCountObjects { get; set; }
        private void btnFirstFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            var userClickedOK = openFileDialog.ShowDialog();

            if (userClickedOK == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                FirstFile = openFileDialog.FileName;
            }
            else FirstFile = "";

            tbFirstFile.Text = FirstFile;
            openFileDialog = null;
        }

        private void btnSecondFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            var userClickedOK = openFileDialog.ShowDialog();

            if (userClickedOK == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                SecondFile = openFileDialog.FileName;
            }
            else SecondFile = "";

            tbSecondFile.Text = SecondFile;
            openFileDialog = null;
        }

        private void btnReportFilePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.Description = "Select the folder for the report file";
            folderBrowserDialog.ShowNewFolderButton = true;
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.

            var userClickedOK = folderBrowserDialog.ShowDialog();

            if (userClickedOK == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                ReportFilePath = folderBrowserDialog.SelectedPath;
            }
            else ReportFilePath = "";

            tbReportFilePath.Text = ReportFilePath;
            
            folderBrowserDialog = null;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ReportFileName = tbReportFileName.Text;
            ReportFileFullPath = ReportFilePath + "\\" + ReportFileName;
            RemoveSameTypeAndCountObjects = cbRemoveSameCountTypeObjects.IsChecked.Value;

            List<HeapCompareStatEntry> MergedHeaps = null;

            try
            {
                MergedHeaps = ManagedHeapOperations.CompareTwoHeapsBasedOnTypeName(FirstFile, SecondFile, RemoveSameTypeAndCountObjects);


                using (StreamWriter sw = new StreamWriter(ReportFileFullPath))
                {
                    sw.WriteLine("ClassName\tCount1\tCount2\tCountDelta\tTotal1\tTotal2\tTotalDelta");
                    foreach (HeapCompareStatEntry hcse in MergedHeaps)
                    {
                        sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", hcse.ClassName, hcse.Count, hcse.Count2, hcse.Count2 - hcse.Count, hcse.Total, hcse.Total2, hcse.Total2 - hcse.Total);
                    }

                    System.Windows.MessageBox.Show("Report File is generated");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

        }
    }
}
