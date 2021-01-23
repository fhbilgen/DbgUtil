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
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeCalculationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(334, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // timeCalculationsToolStripMenuItem
            // 
            this.timeCalculationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeRelatedToolStripMenuItem});
            this.timeCalculationsToolStripMenuItem.Name = "timeCalculationsToolStripMenuItem";
            this.timeCalculationsToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.timeCalculationsToolStripMenuItem.Text = "Time Calculations";
            // 
            // timeRelatedToolStripMenuItem
            // 
            this.timeRelatedToolStripMenuItem.Name = "timeRelatedToolStripMenuItem";
            this.timeRelatedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.timeRelatedToolStripMenuItem.Text = ".time Related";
            this.timeRelatedToolStripMenuItem.Click += new System.EventHandler(this.timeRelatedToolStripMenuItem_Click);
            // 
            // frmDbgUtilMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 87);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
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
    }
}

