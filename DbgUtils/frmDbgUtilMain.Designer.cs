namespace DbgUtils
{
    partial class frmDbgUtilMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.timeCalculationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeRelatedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ticksTTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeCalculationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(445, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // timeCalculationsToolStripMenuItem
            // 
            this.timeCalculationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeRelatedToolStripMenuItem,
            this.ticksTTimeToolStripMenuItem});
            this.timeCalculationsToolStripMenuItem.Name = "timeCalculationsToolStripMenuItem";
            this.timeCalculationsToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.timeCalculationsToolStripMenuItem.Text = "Time Calculations";
            // 
            // timeRelatedToolStripMenuItem
            // 
            this.timeRelatedToolStripMenuItem.Name = "timeRelatedToolStripMenuItem";
            this.timeRelatedToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.timeRelatedToolStripMenuItem.Text = ".time Related";
            this.timeRelatedToolStripMenuItem.Click += new System.EventHandler(this.timeRelatedToolStripMenuItem_Click);
            // 
            // ticksTTimeToolStripMenuItem
            // 
            this.ticksTTimeToolStripMenuItem.Name = "ticksTTimeToolStripMenuItem";
            this.ticksTTimeToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ticksTTimeToolStripMenuItem.Text = "Ticks to Time";
            this.ticksTTimeToolStripMenuItem.Click += new System.EventHandler(this.ticksTTimeToolStripMenuItem_Click);
            // 
            // frmDbgUtilMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 107);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmDbgUtilMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debugging Utilities";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem timeCalculationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeRelatedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ticksTTimeToolStripMenuItem;
    }
}

