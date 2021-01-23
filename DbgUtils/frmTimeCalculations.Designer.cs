namespace DbgUtils
{
    partial class frmTimeCalculations
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
            this.btnCalculate = new System.Windows.Forms.Button();
            this.txtDSeconds = new System.Windows.Forms.TextBox();
            this.txtTSeconds = new System.Windows.Forms.TextBox();
            this.txtDMinutes = new System.Windows.Forms.TextBox();
            this.txtTMinutes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTSeconds2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTSeconds1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTMinutes2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTMinutes1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCalc1 = new System.Windows.Forms.Button();
            this.txtCPUs2 = new System.Windows.Forms.TextBox();
            this.btnCalc2 = new System.Windows.Forms.Button();
            this.txtCPUs1 = new System.Windows.Forms.TextBox();
            this.txtDays1 = new System.Windows.Forms.TextBox();
            this.txtDays2 = new System.Windows.Forms.TextBox();
            this.txtHours1 = new System.Windows.Forms.TextBox();
            this.txtSeconds2 = new System.Windows.Forms.TextBox();
            this.txtHours2 = new System.Windows.Forms.TextBox();
            this.txtSeconds1 = new System.Windows.Forms.TextBox();
            this.txtMinutes1 = new System.Windows.Forms.TextBox();
            this.txtMinutes2 = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(236, 19);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 53);
            this.btnCalculate.TabIndex = 0;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // txtDSeconds
            // 
            this.txtDSeconds.Location = new System.Drawing.Point(526, 52);
            this.txtDSeconds.Name = "txtDSeconds";
            this.txtDSeconds.Size = new System.Drawing.Size(89, 20);
            this.txtDSeconds.TabIndex = 33;
            this.txtDSeconds.TabStop = false;
            // 
            // txtTSeconds
            // 
            this.txtTSeconds.Location = new System.Drawing.Point(526, 16);
            this.txtTSeconds.Name = "txtTSeconds";
            this.txtTSeconds.Size = new System.Drawing.Size(89, 20);
            this.txtTSeconds.TabIndex = 32;
            this.txtTSeconds.TabStop = false;
            // 
            // txtDMinutes
            // 
            this.txtDMinutes.Location = new System.Drawing.Point(420, 52);
            this.txtDMinutes.Name = "txtDMinutes";
            this.txtDMinutes.Size = new System.Drawing.Size(89, 20);
            this.txtDMinutes.TabIndex = 31;
            this.txtDMinutes.TabStop = false;
            // 
            // txtTMinutes
            // 
            this.txtTMinutes.Location = new System.Drawing.Point(420, 16);
            this.txtTMinutes.Name = "txtTMinutes";
            this.txtTMinutes.Size = new System.Drawing.Size(89, 20);
            this.txtTMinutes.TabIndex = 30;
            this.txtTMinutes.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(356, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Difference";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(356, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Total";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnCalculate);
            this.groupBox1.Controls.Add(this.txtDSeconds);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTSeconds);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDMinutes);
            this.groupBox1.Controls.Add(this.txtTMinutes);
            this.groupBox1.Location = new System.Drawing.Point(13, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 88);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Results";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtTSeconds2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtTSeconds1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtTMinutes2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtTMinutes1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnCalc1);
            this.groupBox2.Controls.Add(this.txtCPUs2);
            this.groupBox2.Controls.Add(this.btnCalc2);
            this.groupBox2.Controls.Add(this.txtCPUs1);
            this.groupBox2.Controls.Add(this.txtDays1);
            this.groupBox2.Controls.Add(this.txtDays2);
            this.groupBox2.Controls.Add(this.txtHours1);
            this.groupBox2.Controls.Add(this.txtSeconds2);
            this.groupBox2.Controls.Add(this.txtHours2);
            this.groupBox2.Controls.Add(this.txtSeconds1);
            this.groupBox2.Controls.Add(this.txtMinutes1);
            this.groupBox2.Controls.Add(this.txtMinutes2);
            this.groupBox2.Location = new System.Drawing.Point(12, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(625, 118);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Hours";
            // 
            // txtTSeconds2
            // 
            this.txtTSeconds2.Location = new System.Drawing.Point(526, 82);
            this.txtTSeconds2.Name = "txtTSeconds2";
            this.txtTSeconds2.Size = new System.Drawing.Size(89, 20);
            this.txtTSeconds2.TabIndex = 25;
            this.txtTSeconds2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Days";
            // 
            // txtTSeconds1
            // 
            this.txtTSeconds1.Location = new System.Drawing.Point(526, 46);
            this.txtTSeconds1.Name = "txtTSeconds1";
            this.txtTSeconds1.Size = new System.Drawing.Size(89, 20);
            this.txtTSeconds1.TabIndex = 24;
            this.txtTSeconds1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(139, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Minutes";
            // 
            // txtTMinutes2
            // 
            this.txtTMinutes2.Location = new System.Drawing.Point(420, 82);
            this.txtTMinutes2.Name = "txtTMinutes2";
            this.txtTMinutes2.Size = new System.Drawing.Size(89, 20);
            this.txtTMinutes2.TabIndex = 23;
            this.txtTMinutes2.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(203, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Seconds";
            // 
            // txtTMinutes1
            // 
            this.txtTMinutes1.Location = new System.Drawing.Point(420, 46);
            this.txtTMinutes1.Name = "txtTMinutes1";
            this.txtTMinutes1.Size = new System.Drawing.Size(89, 20);
            this.txtTMinutes1.TabIndex = 22;
            this.txtTMinutes1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(526, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Total Seconds";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(264, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "CPUs";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(420, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Total Minutes";
            // 
            // btnCalc1
            // 
            this.btnCalc1.Location = new System.Drawing.Point(328, 43);
            this.btnCalc1.Name = "btnCalc1";
            this.btnCalc1.Size = new System.Drawing.Size(75, 23);
            this.btnCalc1.TabIndex = 5;
            this.btnCalc1.Text = "=>";
            this.btnCalc1.UseVisualStyleBackColor = true;
            this.btnCalc1.Click += new System.EventHandler(this.btnCalc1_Click);
            // 
            // txtCPUs2
            // 
            this.txtCPUs2.Location = new System.Drawing.Point(264, 82);
            this.txtCPUs2.Name = "txtCPUs2";
            this.txtCPUs2.Size = new System.Drawing.Size(46, 20);
            this.txtCPUs2.TabIndex = 10;
            // 
            // btnCalc2
            // 
            this.btnCalc2.Location = new System.Drawing.Point(328, 79);
            this.btnCalc2.Name = "btnCalc2";
            this.btnCalc2.Size = new System.Drawing.Size(75, 23);
            this.btnCalc2.TabIndex = 11;
            this.btnCalc2.Text = "=>";
            this.btnCalc2.UseVisualStyleBackColor = true;
            this.btnCalc2.Click += new System.EventHandler(this.btnCalc2_Click);
            // 
            // txtCPUs1
            // 
            this.txtCPUs1.Location = new System.Drawing.Point(264, 46);
            this.txtCPUs1.Name = "txtCPUs1";
            this.txtCPUs1.Size = new System.Drawing.Size(46, 20);
            this.txtCPUs1.TabIndex = 4;
            // 
            // txtDays1
            // 
            this.txtDays1.Location = new System.Drawing.Point(12, 46);
            this.txtDays1.Name = "txtDays1";
            this.txtDays1.Size = new System.Drawing.Size(46, 20);
            this.txtDays1.TabIndex = 0;
            // 
            // txtDays2
            // 
            this.txtDays2.Location = new System.Drawing.Point(12, 82);
            this.txtDays2.Name = "txtDays2";
            this.txtDays2.Size = new System.Drawing.Size(46, 20);
            this.txtDays2.TabIndex = 6;
            // 
            // txtHours1
            // 
            this.txtHours1.Location = new System.Drawing.Point(75, 46);
            this.txtHours1.Name = "txtHours1";
            this.txtHours1.Size = new System.Drawing.Size(46, 20);
            this.txtHours1.TabIndex = 1;
            // 
            // txtSeconds2
            // 
            this.txtSeconds2.Location = new System.Drawing.Point(203, 82);
            this.txtSeconds2.Name = "txtSeconds2";
            this.txtSeconds2.Size = new System.Drawing.Size(46, 20);
            this.txtSeconds2.TabIndex = 9;
            // 
            // txtHours2
            // 
            this.txtHours2.Location = new System.Drawing.Point(75, 82);
            this.txtHours2.Name = "txtHours2";
            this.txtHours2.Size = new System.Drawing.Size(46, 20);
            this.txtHours2.TabIndex = 7;
            // 
            // txtSeconds1
            // 
            this.txtSeconds1.Location = new System.Drawing.Point(203, 46);
            this.txtSeconds1.Name = "txtSeconds1";
            this.txtSeconds1.Size = new System.Drawing.Size(46, 20);
            this.txtSeconds1.TabIndex = 3;
            // 
            // txtMinutes1
            // 
            this.txtMinutes1.Location = new System.Drawing.Point(139, 46);
            this.txtMinutes1.Name = "txtMinutes1";
            this.txtMinutes1.Size = new System.Drawing.Size(46, 20);
            this.txtMinutes1.TabIndex = 2;
            // 
            // txtMinutes2
            // 
            this.txtMinutes2.Location = new System.Drawing.Point(139, 82);
            this.txtMinutes2.Name = "txtMinutes2";
            this.txtMinutes2.Size = new System.Drawing.Size(46, 20);
            this.txtMinutes2.TabIndex = 8;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(141, 19);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 53);
            this.btnClear.TabIndex = 34;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmTimeCalculations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 237);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "frmTimeCalculations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Time Calculations";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.TextBox txtDSeconds;
        private System.Windows.Forms.TextBox txtTSeconds;
        private System.Windows.Forms.TextBox txtDMinutes;
        private System.Windows.Forms.TextBox txtTMinutes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTSeconds2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTSeconds1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTMinutes2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTMinutes1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCalc1;
        private System.Windows.Forms.TextBox txtCPUs2;
        private System.Windows.Forms.Button btnCalc2;
        private System.Windows.Forms.TextBox txtCPUs1;
        private System.Windows.Forms.TextBox txtDays1;
        private System.Windows.Forms.TextBox txtDays2;
        private System.Windows.Forms.TextBox txtHours1;
        private System.Windows.Forms.TextBox txtSeconds2;
        private System.Windows.Forms.TextBox txtHours2;
        private System.Windows.Forms.TextBox txtSeconds1;
        private System.Windows.Forms.TextBox txtMinutes1;
        private System.Windows.Forms.TextBox txtMinutes2;
        private System.Windows.Forms.Button btnClear;
    }
}