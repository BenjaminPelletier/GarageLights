namespace GarageLights.Show
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bStop = new System.Windows.Forms.Button();
            this.bPlay = new System.Windows.Forms.Button();
            this.tvChannels = new GarageLights.Show.ChannelTreeView();
            this.keyframeControl1 = new GarageLights.Show.KeyframeControl();
            this.showScroller1 = new GarageLights.Show.ShowScroller();
            this.audioControl1 = new GarageLights.AudioControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bStop);
            this.splitContainer1.Panel1.Controls.Add(this.bPlay);
            this.splitContainer1.Panel1.Controls.Add(this.tvChannels);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.keyframeControl1);
            this.splitContainer1.Panel2.Controls.Add(this.showScroller1);
            this.splitContainer1.Panel2.Controls.Add(this.audioControl1);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(848, 429);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // bStop
            // 
            this.bStop.Location = new System.Drawing.Point(4, 162);
            this.bStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(64, 40);
            this.bStop.TabIndex = 2;
            this.bStop.Text = "Stop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bPlay
            // 
            this.bPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bPlay.Location = new System.Drawing.Point(151, 163);
            this.bPlay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(64, 40);
            this.bPlay.TabIndex = 1;
            this.bPlay.Text = "Play";
            this.bPlay.UseVisualStyleBackColor = true;
            // 
            // tvChannels
            // 
            this.tvChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvChannels.CheckBoxes = true;
            this.tvChannels.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.tvChannels.FullRowSelect = true;
            this.tvChannels.Location = new System.Drawing.Point(4, 212);
            this.tvChannels.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tvChannels.Name = "tvChannels";
            this.tvChannels.Size = new System.Drawing.Size(211, 210);
            this.tvChannels.TabIndex = 0;
            // 
            // keyframeControl1
            // 
            this.keyframeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyframeControl1.Location = new System.Drawing.Point(6, 212);
            this.keyframeControl1.Name = "keyframeControl1";
            this.keyframeControl1.Size = new System.Drawing.Size(604, 210);
            this.keyframeControl1.TabIndex = 2;
            // 
            // showScroller1
            // 
            this.showScroller1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showScroller1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.showScroller1.Location = new System.Drawing.Point(6, 5);
            this.showScroller1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.showScroller1.Name = "showScroller1";
            this.showScroller1.Size = new System.Drawing.Size(604, 39);
            this.showScroller1.TabIndex = 1;
            // 
            // audioControl1
            // 
            this.audioControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioControl1.AudioPosition = 0F;
            this.audioControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.audioControl1.LeftTime = 0F;
            this.audioControl1.Location = new System.Drawing.Point(6, 54);
            this.audioControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.audioControl1.Name = "audioControl1";
            this.audioControl1.RightTime = 0F;
            this.audioControl1.Size = new System.Drawing.Size(604, 149);
            this.audioControl1.TabIndex = 0;
            // 
            // Multiquence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Multiquence";
            this.Size = new System.Drawing.Size(848, 429);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChannelTreeView tvChannels;
        private ShowScroller showScroller1;
        private AudioControl audioControl1;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bPlay;
        private KeyframeControl keyframeControl1;
    }
}
