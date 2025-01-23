namespace GarageLights.Dialogs
{
    partial class ChannelValueDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelValueDialog));
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lValue = new System.Windows.Forms.Label();
            this.lStyle = new System.Windows.Forms.Label();
            this.cbStyle = new System.Windows.Forms.ComboBox();
            this.bDelete = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bOk.Location = new System.Drawing.Point(53, 110);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(85, 29);
            this.bOk.TabIndex = 2;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(284, 110);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(85, 29);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(109, 29);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(121, 26);
            this.tbValue.TabIndex = 0;
            this.tbValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 161);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(419, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslMessage
            // 
            this.tsslMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tsslMessage.Name = "tsslMessage";
            this.tsslMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // lValue
            // 
            this.lValue.AutoSize = true;
            this.lValue.Location = new System.Drawing.Point(49, 32);
            this.lValue.Name = "lValue";
            this.lValue.Size = new System.Drawing.Size(54, 20);
            this.lValue.TabIndex = 4;
            this.lValue.Text = "Value:";
            // 
            // lStyle
            // 
            this.lStyle.AutoSize = true;
            this.lStyle.Location = new System.Drawing.Point(30, 73);
            this.lStyle.Name = "lStyle";
            this.lStyle.Size = new System.Drawing.Size(48, 20);
            this.lStyle.TabIndex = 5;
            this.lStyle.Text = "Style:";
            // 
            // cbStyle
            // 
            this.cbStyle.FormattingEnabled = true;
            this.cbStyle.Items.AddRange(new object[] {
            "Linear",
            "Step"});
            this.cbStyle.Location = new System.Drawing.Point(109, 61);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new System.Drawing.Size(121, 28);
            this.cbStyle.TabIndex = 1;
            this.cbStyle.Text = "Linear";
            // 
            // bDelete
            // 
            this.bDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDelete.Location = new System.Drawing.Point(145, 110);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(85, 29);
            this.bDelete.TabIndex = 3;
            this.bDelete.Text = "Delete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // ChannelValueDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 183);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.cbStyle);
            this.Controls.Add(this.lStyle);
            this.Controls.Add(this.lValue);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChannelValueDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Channel value";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslMessage;
        private System.Windows.Forms.Label lValue;
        private System.Windows.Forms.Label lStyle;
        private System.Windows.Forms.ComboBox cbStyle;
        private System.Windows.Forms.Button bDelete;
    }
}