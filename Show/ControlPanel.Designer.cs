namespace GarageLights.Show
{
    partial class ControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbRemoveKeyframe = new System.Windows.Forms.ToolStripButton();
            this.tsbAddKeyframe = new System.Windows.Forms.ToolStripButton();
            this.tsbNextKeyframe = new System.Windows.Forms.ToolStripButton();
            this.tsbPreviousKeyframe = new System.Windows.Forms.ToolStripButton();
            this.tsbGoToBeginning = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveKeyframe = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tsbWrite = new System.Windows.Forms.ToolStripButton();
            this.tsbRecordStart = new System.Windows.Forms.ToolStripButton();
            this.tsbRecordStop = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRemoveKeyframe,
            this.tsbAddKeyframe,
            this.tsbNextKeyframe,
            this.tsbPreviousKeyframe,
            this.tsbGoToBeginning,
            this.tsbMoveKeyframe});
            this.toolStrip1.Location = new System.Drawing.Point(0, 197);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(372, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbRemoveKeyframe
            // 
            this.tsbRemoveKeyframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveKeyframe.Enabled = false;
            this.tsbRemoveKeyframe.Image = ((System.Drawing.Image)(resources.GetObject("tsbRemoveKeyframe.Image")));
            this.tsbRemoveKeyframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveKeyframe.Name = "tsbRemoveKeyframe";
            this.tsbRemoveKeyframe.Size = new System.Drawing.Size(28, 28);
            this.tsbRemoveKeyframe.ToolTipText = "Remove this keyframe";
            this.tsbRemoveKeyframe.Click += new System.EventHandler(this.tsbRemoveKeyframe_Click);
            // 
            // tsbAddKeyframe
            // 
            this.tsbAddKeyframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddKeyframe.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddKeyframe.Image")));
            this.tsbAddKeyframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddKeyframe.Name = "tsbAddKeyframe";
            this.tsbAddKeyframe.Size = new System.Drawing.Size(28, 28);
            this.tsbAddKeyframe.ToolTipText = "Add keyframe here";
            this.tsbAddKeyframe.Click += new System.EventHandler(this.tsbAddKeyframe_Click);
            // 
            // tsbNextKeyframe
            // 
            this.tsbNextKeyframe.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbNextKeyframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNextKeyframe.Image = ((System.Drawing.Image)(resources.GetObject("tsbNextKeyframe.Image")));
            this.tsbNextKeyframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNextKeyframe.Name = "tsbNextKeyframe";
            this.tsbNextKeyframe.Size = new System.Drawing.Size(28, 28);
            this.tsbNextKeyframe.ToolTipText = "Go to the next keyframe";
            this.tsbNextKeyframe.Click += new System.EventHandler(this.tsbNextKeyframe_Click);
            // 
            // tsbPreviousKeyframe
            // 
            this.tsbPreviousKeyframe.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbPreviousKeyframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPreviousKeyframe.Image = ((System.Drawing.Image)(resources.GetObject("tsbPreviousKeyframe.Image")));
            this.tsbPreviousKeyframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPreviousKeyframe.Name = "tsbPreviousKeyframe";
            this.tsbPreviousKeyframe.Size = new System.Drawing.Size(28, 28);
            this.tsbPreviousKeyframe.ToolTipText = "Go to the previous keyframe";
            this.tsbPreviousKeyframe.Click += new System.EventHandler(this.tsbPreviousKeyframe_Click);
            // 
            // tsbGoToBeginning
            // 
            this.tsbGoToBeginning.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbGoToBeginning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbGoToBeginning.Image = ((System.Drawing.Image)(resources.GetObject("tsbGoToBeginning.Image")));
            this.tsbGoToBeginning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGoToBeginning.Name = "tsbGoToBeginning";
            this.tsbGoToBeginning.Size = new System.Drawing.Size(28, 28);
            this.tsbGoToBeginning.ToolTipText = "Go to beginning";
            this.tsbGoToBeginning.Click += new System.EventHandler(this.tsbGoToBeginning_Click);
            // 
            // tsbMoveKeyframe
            // 
            this.tsbMoveKeyframe.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveKeyframe.Image = ((System.Drawing.Image)(resources.GetObject("tsbMoveKeyframe.Image")));
            this.tsbMoveKeyframe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveKeyframe.Name = "tsbMoveKeyframe";
            this.tsbMoveKeyframe.Size = new System.Drawing.Size(28, 28);
            this.tsbMoveKeyframe.ToolTipText = "Move keyframe to here";
            this.tsbMoveKeyframe.Click += new System.EventHandler(this.tsbMoveKeyframe_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbWrite,
            this.tsbRecordStart,
            this.tsbRecordStop});
            this.toolStrip2.Location = new System.Drawing.Point(0, 166);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(372, 31);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tsbWrite
            // 
            this.tsbWrite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWrite.Enabled = false;
            this.tsbWrite.Image = ((System.Drawing.Image)(resources.GetObject("tsbWrite.Image")));
            this.tsbWrite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWrite.Name = "tsbWrite";
            this.tsbWrite.Size = new System.Drawing.Size(28, 28);
            this.tsbWrite.Text = "toolStripButton1";
            this.tsbWrite.Click += new System.EventHandler(this.tsbWrite_Click);
            // 
            // tsbRecordStart
            // 
            this.tsbRecordStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRecordStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbRecordStart.Image")));
            this.tsbRecordStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRecordStart.Name = "tsbRecordStart";
            this.tsbRecordStart.Size = new System.Drawing.Size(28, 28);
            this.tsbRecordStart.Text = "toolStripButton2";
            this.tsbRecordStart.Click += new System.EventHandler(this.tsbRecordStart_Click);
            // 
            // tsbRecordStop
            // 
            this.tsbRecordStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRecordStop.Image = ((System.Drawing.Image)(resources.GetObject("tsbRecordStop.Image")));
            this.tsbRecordStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRecordStop.Name = "tsbRecordStop";
            this.tsbRecordStop.Size = new System.Drawing.Size(28, 28);
            this.tsbRecordStop.Text = "toolStripButton3";
            this.tsbRecordStop.Visible = false;
            this.tsbRecordStop.Click += new System.EventHandler(this.tsbRecordStop_Click);
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ControlPanel";
            this.Size = new System.Drawing.Size(372, 228);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddKeyframe;
        private System.Windows.Forms.ToolStripButton tsbRemoveKeyframe;
        private System.Windows.Forms.ToolStripButton tsbPreviousKeyframe;
        private System.Windows.Forms.ToolStripButton tsbNextKeyframe;
        private System.Windows.Forms.ToolStripButton tsbGoToBeginning;
        private System.Windows.Forms.ToolStripButton tsbMoveKeyframe;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tsbWrite;
        private System.Windows.Forms.ToolStripButton tsbRecordStart;
        private System.Windows.Forms.ToolStripButton tsbRecordStop;
    }
}
