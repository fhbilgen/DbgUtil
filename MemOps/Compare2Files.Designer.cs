namespace MemOps
{
    partial class Compare2Files
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFile1 = new System.Windows.Forms.Label();
            this.lblFile2 = new System.Windows.Forms.Label();
            this.tbFile1 = new System.Windows.Forms.TextBox();
            this.tbFile2 = new System.Windows.Forms.TextBox();
            this.btnFile1 = new System.Windows.Forms.Button();
            this.btnFile2 = new System.Windows.Forms.Button();
            this.cbEqualCount = new System.Windows.Forms.CheckBox();
            this.gbComparisionMethod = new System.Windows.Forms.GroupBox();
            this.rbMemPage = new System.Windows.Forms.RadioButton();
            this.rbHeapCompare = new System.Windows.Forms.RadioButton();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.gbComparisionMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFile1
            // 
            this.lblFile1.AutoSize = true;
            this.lblFile1.Location = new System.Drawing.Point(39, 42);
            this.lblFile1.Name = "lblFile1";
            this.lblFile1.Size = new System.Drawing.Size(59, 20);
            this.lblFile1.TabIndex = 0;
            this.lblFile1.Text = "File 1 : ";
            // 
            // lblFile2
            // 
            this.lblFile2.AutoSize = true;
            this.lblFile2.Location = new System.Drawing.Point(39, 102);
            this.lblFile2.Name = "lblFile2";
            this.lblFile2.Size = new System.Drawing.Size(59, 20);
            this.lblFile2.TabIndex = 1;
            this.lblFile2.Text = "File 2 : ";
            // 
            // tbFile1
            // 
            this.tbFile1.Location = new System.Drawing.Point(124, 42);
            this.tbFile1.Name = "tbFile1";
            this.tbFile1.Size = new System.Drawing.Size(596, 26);
            this.tbFile1.TabIndex = 2;
            // 
            // tbFile2
            // 
            this.tbFile2.Location = new System.Drawing.Point(124, 102);
            this.tbFile2.Name = "tbFile2";
            this.tbFile2.Size = new System.Drawing.Size(596, 26);
            this.tbFile2.TabIndex = 3;
            // 
            // btnFile1
            // 
            this.btnFile1.Location = new System.Drawing.Point(740, 34);
            this.btnFile1.Name = "btnFile1";
            this.btnFile1.Size = new System.Drawing.Size(56, 37);
            this.btnFile1.TabIndex = 4;
            this.btnFile1.Text = "...";
            this.btnFile1.UseVisualStyleBackColor = true;
            this.btnFile1.Click += new System.EventHandler(this.btnFile1_Click);
            // 
            // btnFile2
            // 
            this.btnFile2.Location = new System.Drawing.Point(740, 97);
            this.btnFile2.Name = "btnFile2";
            this.btnFile2.Size = new System.Drawing.Size(56, 37);
            this.btnFile2.TabIndex = 5;
            this.btnFile2.Text = "...";
            this.btnFile2.UseVisualStyleBackColor = true;
            this.btnFile2.Click += new System.EventHandler(this.btnFile2_Click);
            // 
            // cbEqualCount
            // 
            this.cbEqualCount.AutoSize = true;
            this.cbEqualCount.Location = new System.Drawing.Point(124, 166);
            this.cbEqualCount.Name = "cbEqualCount";
            this.cbEqualCount.Size = new System.Drawing.Size(354, 24);
            this.cbEqualCount.TabIndex = 6;
            this.cbEqualCount.Text = "Remove same typed objects with equal count";
            this.cbEqualCount.UseVisualStyleBackColor = true;
            // 
            // gbComparisionMethod
            // 
            this.gbComparisionMethod.Controls.Add(this.rbMemPage);
            this.gbComparisionMethod.Controls.Add(this.rbHeapCompare);
            this.gbComparisionMethod.Location = new System.Drawing.Point(124, 233);
            this.gbComparisionMethod.Name = "gbComparisionMethod";
            this.gbComparisionMethod.Size = new System.Drawing.Size(596, 96);
            this.gbComparisionMethod.TabIndex = 7;
            this.gbComparisionMethod.TabStop = false;
            this.gbComparisionMethod.Text = "Comparision Method";
            // 
            // rbMemPage
            // 
            this.rbMemPage.AutoSize = true;
            this.rbMemPage.Location = new System.Drawing.Point(252, 39);
            this.rbMemPage.Name = "rbMemPage";
            this.rbMemPage.Size = new System.Drawing.Size(223, 24);
            this.rbMemPage.TabIndex = 1;
            this.rbMemPage.Text = "Memory Page Comparision";
            this.rbMemPage.UseVisualStyleBackColor = true;
            // 
            // rbHeapCompare
            // 
            this.rbHeapCompare.AutoSize = true;
            this.rbHeapCompare.Checked = true;
            this.rbHeapCompare.Location = new System.Drawing.Point(23, 39);
            this.rbHeapCompare.Name = "rbHeapCompare";
            this.rbHeapCompare.Size = new System.Drawing.Size(204, 24);
            this.rbHeapCompare.TabIndex = 0;
            this.rbHeapCompare.TabStop = true;
            this.rbHeapCompare.Text = ".NET Heap Comparision";
            this.rbHeapCompare.UseVisualStyleBackColor = true;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Location = new System.Drawing.Point(241, 364);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(290, 47);
            this.btnGenerateReport.TabIndex = 8;
            this.btnGenerateReport.Text = "Generate Difference Report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // Compare2Files
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 492);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.gbComparisionMethod);
            this.Controls.Add(this.cbEqualCount);
            this.Controls.Add(this.btnFile2);
            this.Controls.Add(this.btnFile1);
            this.Controls.Add(this.tbFile2);
            this.Controls.Add(this.tbFile1);
            this.Controls.Add(this.lblFile2);
            this.Controls.Add(this.lblFile1);
            this.Name = "Compare2Files";
            this.Text = "Form1";
            this.gbComparisionMethod.ResumeLayout(false);
            this.gbComparisionMethod.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFile1;
        private System.Windows.Forms.Label lblFile2;
        private System.Windows.Forms.TextBox tbFile1;
        private System.Windows.Forms.TextBox tbFile2;
        private System.Windows.Forms.Button btnFile1;
        private System.Windows.Forms.Button btnFile2;
        private System.Windows.Forms.CheckBox cbEqualCount;
        private System.Windows.Forms.GroupBox gbComparisionMethod;
        private System.Windows.Forms.RadioButton rbMemPage;
        private System.Windows.Forms.RadioButton rbHeapCompare;
        private System.Windows.Forms.Button btnGenerateReport;
    }
}

