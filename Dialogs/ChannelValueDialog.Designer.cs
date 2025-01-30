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
            this.bDelete = new System.Windows.Forms.Button();
            this.rbLinear = new System.Windows.Forms.RadioButton();
            this.rbStep = new System.Windows.Forms.RadioButton();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bOk.Location = new System.Drawing.Point(35, 84);
            this.bOk.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(57, 19);
            this.bOk.TabIndex = 3;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(294, 84);
            this.bCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(57, 19);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(73, 19);
            this.tbValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(82, 20);
            this.tbValue.TabIndex = 0;
            this.tbValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbValue_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 109);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(384, 22);
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
            this.lValue.Location = new System.Drawing.Point(33, 21);
            this.lValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lValue.Name = "lValue";
            this.lValue.Size = new System.Drawing.Size(37, 13);
            this.lValue.TabIndex = 4;
            this.lValue.Text = "Value:";
            // 
            // lStyle
            // 
            this.lStyle.AutoSize = true;
            this.lStyle.Location = new System.Drawing.Point(32, 47);
            this.lStyle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lStyle.Name = "lStyle";
            this.lStyle.Size = new System.Drawing.Size(33, 13);
            this.lStyle.TabIndex = 5;
            this.lStyle.Text = "Style:";
            // 
            // bDelete
            // 
            this.bDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDelete.Location = new System.Drawing.Point(97, 84);
            this.bDelete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(57, 19);
            this.bDelete.TabIndex = 4;
            this.bDelete.Text = "Delete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // rbLinear
            // 
            this.rbLinear.AutoSize = true;
            this.rbLinear.Checked = true;
            this.rbLinear.Location = new System.Drawing.Point(73, 45);
            this.rbLinear.Name = "rbLinear";
            this.rbLinear.Size = new System.Drawing.Size(54, 17);
            this.rbLinear.TabIndex = 1;
            this.rbLinear.TabStop = true;
            this.rbLinear.Text = "Linear";
            this.rbLinear.UseVisualStyleBackColor = true;
            // 
            // rbStep
            // 
            this.rbStep.AutoSize = true;
            this.rbStep.Location = new System.Drawing.Point(133, 45);
            this.rbStep.Name = "rbStep";
            this.rbStep.Size = new System.Drawing.Size(47, 17);
            this.rbStep.TabIndex = 2;
            this.rbStep.Text = "Step";
            this.rbStep.UseVisualStyleBackColor = true;
            // 
            // ChannelValueDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 131);
            this.Controls.Add(this.rbStep);
            this.Controls.Add(this.rbLinear);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.lStyle);
            this.Controls.Add(this.lValue);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
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
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.RadioButton rbLinear;
        private System.Windows.Forms.RadioButton rbStep;
    }
}