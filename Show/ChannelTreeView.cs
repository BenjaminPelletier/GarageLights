using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GarageLights.Show
{
    internal class ChannelTreeView : TreeView
    {
        public ChannelTreeView()
        {
            DrawNode += ChannelTreeView_DrawNode;
            
        }

        private void ChannelTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // Draw the background and node text for a selected node.
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                // Draw the background of the selected node. The NodeBounds
                // method makes the highlight rectangle large enough to
                // include the text of a node tag, if one is present.
                e.Graphics.FillRectangle(Brushes.Green, NodeBounds(e.Node));

                // Retrieve the node font. If the node font has not been set,
                // use the TreeView font.
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                // Draw the node text.
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                    Rectangle.Inflate(e.Bounds, 2, 0));
            }

            // Use the default background and node text.
            else
            {
                e.DrawDefault = true;
            }

            // If a node tag is present, draw its string representation 
            // to the right of the label text.
            //if (e.Node.Tag != null)
            //{
            //    e.Graphics.DrawString(e.Node.Tag.ToString(), tagFont,
            //        Brushes.Yellow, e.Bounds.Right + 2, e.Bounds.Top);
            //}

            // If the node has focus, draw the focus rectangle large, making
            // it large enough to include the text of the node tag, if present.
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusBounds = NodeBounds(e.Node);
                    focusBounds.Size = new Size(focusBounds.Width - 1,
                    focusBounds.Height - 1);
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }

            //e.Graphics.DrawString(e.Node.Text, Font, Brushes.Black, e.Bounds.Location);
            Debug.Print("ChannelTreeView_DrawNode(" + e.Node + ")");
        }

        private void DrawNode2(object sender, DrawTreeNodeEventArgs e)
        {
            Debug.Print("Draw " + e.Node.Text);

            Graphics g = e.Graphics;

            // Determine background color
            Color bgColor = e.Node.IsSelected
                ? SystemColors.Highlight
                : e.Node.BackColor != Color.Empty
                    ? e.Node.BackColor
                    : e.Node.TreeView.BackColor;

            using (Brush backgroundBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(backgroundBrush, e.Bounds);
            }

            // Determine text color
            Color textColor = e.Node.IsSelected
                ? SystemColors.HighlightText
                : e.Node.ForeColor != Color.Empty
                    ? e.Node.ForeColor
                    : e.Node.TreeView.ForeColor;

            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left;

            // Adjust text bounds to leave space for expand/collapse glyph
            Rectangle textBounds = new Rectangle(
                e.Bounds.X + 18, // Assuming 18px for glyph and indentation
                e.Bounds.Y,
                e.Bounds.Width - 18,
                e.Bounds.Height
            );

            TextRenderer.DrawText(g, e.Node.Text, e.Node.NodeFont ?? e.Node.TreeView.Font, textBounds, textColor, flags);

            // Draw expand/collapse glyphs
            if (e.Node.Nodes.Count > 0)
            {
                bool isExpanded = e.Node.IsExpanded;
                Point glyphCenter = new Point(
                    e.Bounds.X + 9, // Center of the 18px glyph area
                    e.Bounds.Y + e.Bounds.Height / 2
                );

                Rectangle glyphRect = new Rectangle(glyphCenter.X - 4, glyphCenter.Y - 4, 8, 8);

                using (Pen pen = new Pen(Color.Black))
                {
                    g.DrawRectangle(pen, glyphRect);

                    // Horizontal line (always present)
                    g.DrawLine(pen, glyphCenter.X - 3, glyphCenter.Y, glyphCenter.X + 3, glyphCenter.Y);

                    // Vertical line (only when not expanded)
                    if (!isExpanded)
                    {
                        g.DrawLine(pen, glyphCenter.X, glyphCenter.Y - 3, glyphCenter.X, glyphCenter.Y + 3);
                    }
                }
            }

            // Draw focus rectangle
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(SystemColors.HighlightText))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusRect = new Rectangle(
                        e.Bounds.X + 1,
                        e.Bounds.Y + 1,
                        e.Bounds.Width - 2,
                        e.Bounds.Height - 2
                    );
                    g.DrawRectangle(focusPen, focusRect);
                }
            }
        }

        // Returns the bounds of the specified node, including the region 
        // occupied by the node label and any node tag displayed.
        private Rectangle NodeBounds(TreeNode node)
        {
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            //if (node.Tag != null)
            //{
            //    // Retrieve a Graphics object from the TreeView handle
            //    // and use it to calculate the display width of the tag.
            //    Graphics g = CreateGraphics();
            //    int tagWidth = (int)g.MeasureString
            //        (node.Tag.ToString(), tagFont).Width + 6;

            //    // Adjust the node bounds using the calculated value.
            //    bounds.Offset(tagWidth / 2, 0);
            //    bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
            //    g.Dispose();
            //}

            return bounds;
        }
    }
}
