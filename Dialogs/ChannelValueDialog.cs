﻿using GarageLights.Keyframes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Dialogs
{
    internal partial class ChannelValueDialog : Form
    {
        private ChannelKeyframe channelKeyframe;

        public ChannelValueDialog()
        {
            InitializeComponent();
        }

        public string ChannelName
        {
            set
            {
                this.Text = value;
            }
        }

        public ChannelKeyframe ChannelKeyframe
        {
            get
            {
                return channelKeyframe;
            }
            set
            {
                tbValue.Text = value.Value.ToString();
                tbValue.SelectAll();
                if (value.Style == KeyframeStyle.Linear)
                {
                    rbLinear.Checked = true;
                }
                else if (value.Style == KeyframeStyle.Step)
                {
                    rbStep.Checked = true;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            int value;
            if (!int.TryParse(tbValue.Text, out value))
            {
                tsslMessage.Text = "Could not parse '" + tbValue.Text + "'";
                return;
            }
            if (value < 0 || value > 255)
            {
                tsslMessage.Text = value + " is outside the range 0-255";
                return;
            }

            KeyframeStyle style;
            if (rbLinear.Checked)
            {
                style = KeyframeStyle.Linear;
            }
            else if (rbStep.Checked)
            {
                style = KeyframeStyle.Step;
            }
            else
            {
                throw new NotImplementedException();
            }

            channelKeyframe = new ChannelKeyframe() { Value = value, Style = style };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            channelKeyframe = null;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                bOk.PerformClick();
                e.Handled = true;
            }
        }
    }
}
