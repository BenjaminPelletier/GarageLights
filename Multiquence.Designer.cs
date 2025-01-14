namespace GarageLights
{
    partial class Multiquence
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scNameValues = new System.Windows.Forms.SplitContainer();
            this.tvNames = new System.Windows.Forms.TreeView();
            this.pbValues = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.scNameValues)).BeginInit();
            this.scNameValues.Panel1.SuspendLayout();
            this.scNameValues.Panel2.SuspendLayout();
            this.scNameValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbValues)).BeginInit();
            this.SuspendLayout();
            // 
            // scNameValues
            // 
            this.scNameValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scNameValues.Location = new System.Drawing.Point(0, 0);
            this.scNameValues.Name = "scNameValues";
            // 
            // scNameValues.Panel1
            // 
            this.scNameValues.Panel1.Controls.Add(this.tvNames);
            // 
            // scNameValues.Panel2
            // 
            this.scNameValues.Panel2.Controls.Add(this.pbValues);
            this.scNameValues.Size = new System.Drawing.Size(565, 279);
            this.scNameValues.SplitterDistance = 188;
            this.scNameValues.TabIndex = 0;
            // 
            // tvNames
            // 
            this.tvNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvNames.Location = new System.Drawing.Point(3, 3);
            this.tvNames.Name = "tvNames";
            this.tvNames.Size = new System.Drawing.Size(182, 273);
            this.tvNames.TabIndex = 0;
            // 
            // pbValues
            // 
            this.pbValues.Location = new System.Drawing.Point(3, 3);
            this.pbValues.Name = "pbValues";
            this.pbValues.Size = new System.Drawing.Size(367, 273);
            this.pbValues.TabIndex = 0;
            this.pbValues.TabStop = false;
            // 
            // Multiquence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scNameValues);
            this.Name = "Multiquence";
            this.Size = new System.Drawing.Size(565, 279);
            this.scNameValues.Panel1.ResumeLayout(false);
            this.scNameValues.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scNameValues)).EndInit();
            this.scNameValues.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbValues)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scNameValues;
        private System.Windows.Forms.TreeView tvNames;
        private System.Windows.Forms.PictureBox pbValues;
    }
}
