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
            this.tvChannels = new GarageLights.Show.ChannelTreeView();
            this.bStop = new System.Windows.Forms.Button();
            this.bPlay = new System.Windows.Forms.Button();
            this.keyframeControl1 = new GarageLights.Show.KeyframeControl();
            this.showScroller1 = new GarageLights.Show.ShowScroller();
            this.audioControl1 = new GarageLights.AudioControl();
            this.ofdAudioFile = new System.Windows.Forms.OpenFileDialog();
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
            this.splitContainer1.Panel1.Controls.Add(this.tvChannels);
            this.splitContainer1.Panel1.Controls.Add(this.bStop);
            this.splitContainer1.Panel1.Controls.Add(this.bPlay);
            this.splitContainer1.Panel1.Resize += new System.EventHandler(this.splitContainer1_Panel1_Resize);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.keyframeControl1);
            this.splitContainer1.Panel2.Controls.Add(this.showScroller1);
            this.splitContainer1.Panel2.Controls.Add(this.audioControl1);
            this.splitContainer1.Size = new System.Drawing.Size(848, 429);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvChannels
            // 
            this.tvChannels.CheckBoxes = true;
            this.tvChannels.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.tvChannels.FullRowSelect = true;
            this.tvChannels.Location = new System.Drawing.Point(4, 212);
            this.tvChannels.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tvChannels.Name = "tvChannels";
            this.tvChannels.Size = new System.Drawing.Size(214, 212);
            this.tvChannels.TabIndex = 3;
            // 
            // bStop
            // 
            this.bStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bStop.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bStop.Location = new System.Drawing.Point(126, 163);
            this.bStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(41, 40);
            this.bStop.TabIndex = 2;
            this.bStop.Text = "■";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bPlay
            // 
            this.bPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bPlay.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bPlay.Location = new System.Drawing.Point(175, 163);
            this.bPlay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bPlay.Name = "bPlay";
            this.bPlay.Size = new System.Drawing.Size(40, 40);
            this.bPlay.TabIndex = 1;
            this.bPlay.Text = "►";
            this.bPlay.UseVisualStyleBackColor = true;
            this.bPlay.Click += new System.EventHandler(this.bPlay_Click);
            // 
            // keyframeControl1
            // 
            this.keyframeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyframeControl1.Location = new System.Drawing.Point(6, 212);
            this.keyframeControl1.Name = "keyframeControl1";
            this.keyframeControl1.Size = new System.Drawing.Size(588, 210);
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
            this.showScroller1.Size = new System.Drawing.Size(588, 39);
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
            this.audioControl1.Size = new System.Drawing.Size(588, 149);
            this.audioControl1.TabIndex = 0;
            this.audioControl1.AudioLoaded += new System.EventHandler(this.audioControl1_AudioLoaded);
            this.audioControl1.AudioViewChanged += new System.EventHandler<GarageLights.AudioControl.AudioViewChangedEventArgs>(this.audioControl1_AudioViewChanged);
            this.audioControl1.AudioPositionChanged += new System.EventHandler<GarageLights.AudioControl.AudioPositionChangedEventArgs>(this.audioControl1_AudioPositionChanged);
            this.audioControl1.FileLoadRequested += new System.EventHandler(this.audioControl1_FileLoadRequested);
            // 
            // ofdAudioFile
            // 
            this.ofdAudioFile.FileName = "song.mp3";
            this.ofdAudioFile.Filter = "MP3 file|*.mp3|WAV file|*.wav";
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
        private ShowScroller showScroller1;
        private AudioControl audioControl1;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bPlay;
        private KeyframeControl keyframeControl1;
        private ChannelTreeView tvChannels;
        private System.Windows.Forms.OpenFileDialog ofdAudioFile;
    }
}
