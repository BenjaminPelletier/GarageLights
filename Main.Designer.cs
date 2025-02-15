﻿using GarageLights.Show;

namespace GarageLights
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            GarageLights.Project project1 = new GarageLights.Project();
            this.tUpdate = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goTobeginningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousKeyframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextKeyframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToCurrentPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdProject = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbPlay = new System.Windows.Forms.ToolStripButton();
            this.tsbStop = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslAudioPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.sfdProject = new System.Windows.Forms.SaveFileDialog();
            this.multiquence1 = new GarageLights.Show.Multiquence();
            this.keyframeManager = new GarageLights.Keyframes.KeyframeManager(this.components);
            this.audioPlayer = new GarageLights.Audio.AudioPlayer(this.components);
            this.channelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eraseSelectedChannelsFromKeyframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tUpdate
            // 
            this.tUpdate.Interval = 50;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.navigateToolStripMenuItem,
            this.editToolStripMenuItem,
            this.inputDeviceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1420, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveasToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveasToolStripMenuItem
            // 
            this.saveasToolStripMenuItem.Name = "saveasToolStripMenuItem";
            this.saveasToolStripMenuItem.Size = new System.Drawing.Size(167, 30);
            this.saveasToolStripMenuItem.Text = "Save &as...";
            this.saveasToolStripMenuItem.Click += new System.EventHandler(this.saveasToolStripMenuItem_Click);
            // 
            // navigateToolStripMenuItem
            // 
            this.navigateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goTobeginningToolStripMenuItem,
            this.previousKeyframeToolStripMenuItem,
            this.nextKeyframeToolStripMenuItem});
            this.navigateToolStripMenuItem.Name = "navigateToolStripMenuItem";
            this.navigateToolStripMenuItem.Size = new System.Drawing.Size(94, 29);
            this.navigateToolStripMenuItem.Text = "&Navigate";
            // 
            // goTobeginningToolStripMenuItem
            // 
            this.goTobeginningToolStripMenuItem.Name = "goTobeginningToolStripMenuItem";
            this.goTobeginningToolStripMenuItem.Size = new System.Drawing.Size(321, 30);
            this.goTobeginningToolStripMenuItem.Text = "Go to &beginning";
            this.goTobeginningToolStripMenuItem.Click += new System.EventHandler(this.goTobeginningToolStripMenuItem_Click);
            // 
            // previousKeyframeToolStripMenuItem
            // 
            this.previousKeyframeToolStripMenuItem.Name = "previousKeyframeToolStripMenuItem";
            this.previousKeyframeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
            this.previousKeyframeToolStripMenuItem.Size = new System.Drawing.Size(321, 30);
            this.previousKeyframeToolStripMenuItem.Text = "&Previous keyframe";
            this.previousKeyframeToolStripMenuItem.Click += new System.EventHandler(this.previousKeyframeToolStripMenuItem_Click);
            // 
            // nextKeyframeToolStripMenuItem
            // 
            this.nextKeyframeToolStripMenuItem.Name = "nextKeyframeToolStripMenuItem";
            this.nextKeyframeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.nextKeyframeToolStripMenuItem.Size = new System.Drawing.Size(321, 30);
            this.nextKeyframeToolStripMenuItem.Text = "&Next keyframe";
            this.nextKeyframeToolStripMenuItem.Click += new System.EventHandler(this.nextKeyframeToolStripMenuItem_Click);
            // 
            // inputDeviceToolStripMenuItem
            // 
            this.inputDeviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem});
            this.inputDeviceToolStripMenuItem.Name = "inputDeviceToolStripMenuItem";
            this.inputDeviceToolStripMenuItem.Size = new System.Drawing.Size(121, 29);
            this.inputDeviceToolStripMenuItem.Text = "&Input device";
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
            this.selectToolStripMenuItem.Text = "&Select...";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyframeToolStripMenuItem,
            this.channelsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // keyframeToolStripMenuItem
            // 
            this.keyframeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.moveToCurrentPositionToolStripMenuItem});
            this.keyframeToolStripMenuItem.Name = "keyframeToolStripMenuItem";
            this.keyframeToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.keyframeToolStripMenuItem.Text = "&Keyframe";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.addToolStripMenuItem.Size = new System.Drawing.Size(360, 30);
            this.addToolStripMenuItem.Text = "&Add at current position";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(360, 30);
            this.removeToolStripMenuItem.Text = "&Remove selected";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // moveToCurrentPositionToolStripMenuItem
            // 
            this.moveToCurrentPositionToolStripMenuItem.Name = "moveToCurrentPositionToolStripMenuItem";
            this.moveToCurrentPositionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.moveToCurrentPositionToolStripMenuItem.Size = new System.Drawing.Size(360, 30);
            this.moveToCurrentPositionToolStripMenuItem.Text = "&Move to current position";
            this.moveToCurrentPositionToolStripMenuItem.Click += new System.EventHandler(this.moveToCurrentPositionToolStripMenuItem_Click);
            // 
            // ofdProject
            // 
            this.ofdProject.FileName = "*.json";
            this.ofdProject.Filter = "Project files|*.json";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPlay,
            this.tsbStop});
            this.toolStrip1.Location = new System.Drawing.Point(0, 33);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1420, 31);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbPlay
            // 
            this.tsbPlay.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPlay.Image = ((System.Drawing.Image)(resources.GetObject("tsbPlay.Image")));
            this.tsbPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPlay.Name = "tsbPlay";
            this.tsbPlay.Size = new System.Drawing.Size(28, 28);
            this.tsbPlay.Text = "Begin playback from here";
            this.tsbPlay.Click += new System.EventHandler(this.tsbPlay_Click);
            // 
            // tsbStop
            // 
            this.tsbStop.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStop.Image = ((System.Drawing.Image)(resources.GetObject("tsbStop.Image")));
            this.tsbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new System.Drawing.Size(28, 28);
            this.tsbStop.Text = "Stop playback";
            this.tsbStop.Click += new System.EventHandler(this.tsbStop_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslAudioPosition});
            this.statusStrip1.Location = new System.Drawing.Point(0, 642);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1420, 30);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslAudioPosition
            // 
            this.tsslAudioPosition.Name = "tsslAudioPosition";
            this.tsslAudioPosition.Size = new System.Drawing.Size(22, 25);
            this.tsslAudioPosition.Text = "0";
            // 
            // sfdProject
            // 
            this.sfdProject.Filter = "Project files|*.json";
            // 
            // multiquence1
            // 
            this.multiquence1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiquence1.Location = new System.Drawing.Point(0, 64);
            this.multiquence1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.multiquence1.Name = "multiquence1";
            this.multiquence1.Project = project1;
            this.multiquence1.Size = new System.Drawing.Size(1420, 578);
            this.multiquence1.TabIndex = 4;
            // 
            // keyframeManager
            // 
            this.keyframeManager.Keyframes = null;
            // 
            // audioPlayer
            // 
            this.audioPlayer.AudioPosition = 0F;
            // 
            // channelsToolStripMenuItem
            // 
            this.channelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eraseSelectedChannelsFromKeyframeToolStripMenuItem});
            this.channelsToolStripMenuItem.Name = "channelsToolStripMenuItem";
            this.channelsToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.channelsToolStripMenuItem.Text = "&Channels";
            // 
            // eraseSelectedChannelsFromKeyframeToolStripMenuItem
            // 
            this.eraseSelectedChannelsFromKeyframeToolStripMenuItem.Name = "eraseSelectedChannelsFromKeyframeToolStripMenuItem";
            this.eraseSelectedChannelsFromKeyframeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.eraseSelectedChannelsFromKeyframeToolStripMenuItem.Size = new System.Drawing.Size(461, 30);
            this.eraseSelectedChannelsFromKeyframeToolStripMenuItem.Text = "&Erase selected channels from keyframe";
            this.eraseSelectedChannelsFromKeyframeToolStripMenuItem.Click += new System.EventHandler(this.eraseSelectedChannelsFromKeyframeToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1420, 672);
            this.Controls.Add(this.multiquence1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Garage Lights";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tUpdate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveasToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofdProject;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbPlay;
        private System.Windows.Forms.ToolStripButton tsbStop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslAudioPosition;
        private Multiquence multiquence1;
        private System.Windows.Forms.ToolStripMenuItem inputDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdProject;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private Keyframes.KeyframeManager keyframeManager;
        private Audio.AudioPlayer audioPlayer;
        private System.Windows.Forms.ToolStripMenuItem navigateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goTobeginningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousKeyframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextKeyframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyframeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToCurrentPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eraseSelectedChannelsFromKeyframeToolStripMenuItem;
    }
}

