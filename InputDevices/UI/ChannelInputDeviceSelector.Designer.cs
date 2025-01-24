namespace GarageLights.InputDevices.UI
{
    partial class ChannelInputDeviceSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelInputDeviceSelector));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpNone = new System.Windows.Forms.TabPage();
            this.tpSerialDmxReader = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.lPort = new System.Windows.Forms.Label();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudFirstChannel = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslError = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.tpNone.SuspendLayout();
            this.tpSerialDmxReader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFirstChannel)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpNone);
            this.tabControl1.Controls.Add(this.tpSerialDmxReader);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(503, 200);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tpNone
            // 
            this.tpNone.Controls.Add(this.label1);
            this.tpNone.Location = new System.Drawing.Point(4, 29);
            this.tpNone.Name = "tpNone";
            this.tpNone.Padding = new System.Windows.Forms.Padding(3);
            this.tpNone.Size = new System.Drawing.Size(495, 170);
            this.tpNone.TabIndex = 0;
            this.tpNone.Text = "None";
            this.tpNone.UseVisualStyleBackColor = true;
            // 
            // tpSerialDmxReader
            // 
            this.tpSerialDmxReader.Controls.Add(this.nudFirstChannel);
            this.tpSerialDmxReader.Controls.Add(this.label2);
            this.tpSerialDmxReader.Controls.Add(this.cbPort);
            this.tpSerialDmxReader.Controls.Add(this.lPort);
            this.tpSerialDmxReader.Controls.Add(this.textBox1);
            this.tpSerialDmxReader.Location = new System.Drawing.Point(4, 29);
            this.tpSerialDmxReader.Name = "tpSerialDmxReader";
            this.tpSerialDmxReader.Padding = new System.Windows.Forms.Padding(3);
            this.tpSerialDmxReader.Size = new System.Drawing.Size(495, 167);
            this.tpSerialDmxReader.TabIndex = 1;
            this.tpSerialDmxReader.Text = "Serial DMX reader";
            this.tpSerialDmxReader.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "No channel input device will be used";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(6, 6);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(483, 46);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "A DMX input device attached to a serial reader from DMXSerialSketch will be used";
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(388, 218);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(85, 29);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bOk.Location = new System.Drawing.Point(53, 218);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(85, 29);
            this.bOk.TabIndex = 5;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // lPort
            // 
            this.lPort.AutoSize = true;
            this.lPort.Location = new System.Drawing.Point(6, 61);
            this.lPort.Name = "lPort";
            this.lPort.Size = new System.Drawing.Size(42, 20);
            this.lPort.TabIndex = 1;
            this.lPort.Text = "Port:";
            // 
            // cbPort
            // 
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(116, 58);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(203, 28);
            this.cbPort.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "First channel:";
            // 
            // nudFirstChannel
            // 
            this.nudFirstChannel.Location = new System.Drawing.Point(116, 92);
            this.nudFirstChannel.Maximum = new decimal(new int[] {
            511,
            0,
            0,
            0});
            this.nudFirstChannel.Name = "nudFirstChannel";
            this.nudFirstChannel.Size = new System.Drawing.Size(77, 26);
            this.nudFirstChannel.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslError});
            this.statusStrip1.Location = new System.Drawing.Point(0, 264);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(527, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslError
            // 
            this.tsslError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tsslError.Name = "tsslError";
            this.tsslError.Size = new System.Drawing.Size(0, 17);
            // 
            // ChannelInputDeviceSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 286);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChannelInputDeviceSelector";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Channel input device";
            this.tabControl1.ResumeLayout(false);
            this.tpNone.ResumeLayout(false);
            this.tpNone.PerformLayout();
            this.tpSerialDmxReader.ResumeLayout(false);
            this.tpSerialDmxReader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFirstChannel)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpNone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpSerialDmxReader;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.NumericUpDown nudFirstChannel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Label lPort;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslError;
    }
}