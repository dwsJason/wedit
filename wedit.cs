﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wedit
{
    public partial class wedit : Form
    {
        spData m_spriteFile = null;
        int    m_frameNo = 0;

        // Sprite Frame Render Info
        Bitmap m_bitmap = null;
        int    m_offset_x = 0;
        int    m_offset_y = 0;
        int    m_zoom = 1;  // pow2 zoom

        // Grid Canvas Render Options
        int m_canvas_offset_x = 0;
        int m_canvas_offset_y = 0;

        // BluePrint Blue
        Color m_BackColor = Color.FromArgb(255, 17, 110, 169);
        Color m_GridColor = Color.FromArgb(255,255, 255, 255);
        

        public wedit()
        {
            InitializeComponent();

            // Fill in the Background Graph Paper
            Panel p = splitContainer1.Panel2;
            DrawGrid(p.Size.Width, p.Size.Height);

            //pictureBox.Resize += new EventHandler(PictureBoxResize);
            this.splitContainer1.Panel2.Resize += new EventHandler(PictureBoxResize);
            //spData goldfish = new spData("c:\\dev\\wedit\\sprites\\GOLDFISH.SP");
            //spData taffy = new spData("c:\\dev\\wedit\\sprites\\TAFFY.SP");

            // Keyboard Controls
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(Wedit_KeyPress);
            //this.menuStrip1.KeyPress += new KeyPressEventHandler(wedit_KeyPress);
            //this.splitContainer1.KeyPress +=
            //    new KeyPressEventHandler(wedit_KeyPress);
            //this.splitContainer1.Panel1.KeyPress += new KeyPressEventHandler(wedit_KeyPress); 
            //this.splitContainer1.Panel2.KeyPress += new KeyPressEventHandler(wedit_KeyPress); 
            fastObjectListView1.KeyPress += new KeyPressEventHandler(Wedit_KeyPress);

            this.PreviewKeyDown += new PreviewKeyDownEventHandler(Wedit_KeyDownPreview);
            //fastObjectListView1.PreviewKeyDown += new PreviewKeyDownEventHandler(Wedit_KeyDownPreview);

            this.KeyDown += new KeyEventHandler(Wedit_KeyDown);
            this.KeyUp += new KeyEventHandler(Wedit_KeyUp);

            this.pictureBox.MouseMove += new MouseEventHandler(Wedit_MouseMove);
            this.pictureBox.MouseDown += new MouseEventHandler(Wedit_MouseMove);
        }
        
        // MouseMove Handler
        void Wedit_MouseMove(object obj, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Console.WriteLine("Mouse {0},{1}", e.X, e.Y);

                Panel p = splitContainer1.Panel2;

                int width = p.Size.Width;
                int height = p.Size.Height;
                int cx = width / 2;
                int cy = height / 2;

                m_canvas_offset_x = e.X - cx;
                m_canvas_offset_y = e.Y - cy;
                
                Paint(); 
            }
        }

        // Keydown Handler
        void Wedit_KeyDown(object obj, KeyEventArgs e)
        {
            Console.WriteLine("KeyDown: {0}", e.KeyCode);
        }
        // Keyup Handler
        void Wedit_KeyUp(object obj, KeyEventArgs e)
        {
            Console.WriteLine("KeyUp: {0}", e.KeyCode);
        }
        // Weirdness required to get Arrow keys to come through
        // to my key handler
        void Wedit_KeyDownPreview(object obj, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Right:
                case Keys.Up:
                case Keys.Left:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void Paint()
        {
            if (null != m_spriteFile && !m_spriteFile.IsEmpty())
            {
                m_bitmap = m_spriteFile.BitmapFromFrame(m_frameNo);
                if (null != m_bitmap)
                {
                    spPixels pix = m_spriteFile.GetFrame(m_frameNo);
                    m_offset_x = -pix.m_offset_x;
                    m_offset_y = -pix.m_offset_y;
                }
            }

            // Fill in the Background Graph Paper
            Panel p = splitContainer1.Panel2;
            DrawGrid(p.Size.Width, p.Size.Height);
        }


        private void PictureBoxResize(object sender, EventArgs e)
        {
            Panel p = (Panel) sender;
            DrawGrid(p.Size.Width, p.Size.Height);
        }

        private void DrawGrid(int width, int height)
        {
            if (width < 1)
                return;
            if (height < 1)
                return;

            //Console.WriteLine("{0} x {1}", pictureBox.Size.Width, pictureBox.Size.Height);

            Size s = new Size(width, height);
            pictureBox.Size = s;

            // Red 171,21,23
            // Green 21,171,45
            // Blue 17,110,169
            // Purple 96,64,128
            // Snow 255,250,250
            //Color bgColor = Color.FromArgb(255, 17, 110, 169);
            Color bgColor = m_BackColor;
            //Color originColor = Color.FromArgb(255, 255, 255, 255);
            //Color fgColor = Color.FromArgb(128, 255, 255, 255);
            //Color halfColor = Color.FromArgb(64, 255, 255, 255);
            Color originColor = m_GridColor;
            Color fgColor = Color.FromArgb(128, m_GridColor.R, m_GridColor.G, m_GridColor.B);
            Color halfColor = Color.FromArgb(64, m_GridColor.R, m_GridColor.G, m_GridColor.B);

            Bitmap bmp = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(bmp);

            gr.Clear(bgColor);

            {
                // Draw the Grid
                // Try 8 pixel spacing

                int cx = width / 2;
                int cy = height / 2;

                cx += m_canvas_offset_x;
                cy += m_canvas_offset_y;

                Pen penFull = new Pen(fgColor);
                Pen penHalf = new Pen(halfColor);
                Pen penOrigin = new Pen(originColor);

                int step_x = 8;
                int step_y = 8;

                // Thin Line Grid
                // Draw Vertical Grid
                for (int x = cx + step_x; x < width; x += step_x)
                {
                    //int flipx = ((x - cx) * -1) + cx;
                    gr.DrawLine(penHalf, x, 0, x, height);
                    //gr.DrawLine(penHalf, flipx, 0, flipx, height);
                }
                for (int x = cx - step_x; x >= 0; x -= step_x)
                {
                    gr.DrawLine(penHalf, x, 0, x, height);
                }


                // Draw Horizontal Grid
                for (int y = cy + step_y; y < height; y += step_y)
                {
                    //int flipy = ((y - cy) * -1) + cy;
                    gr.DrawLine(penHalf, 0, y, width, y);
                    //gr.DrawLine(penHalf, 0, flipy, width, flipy);
                }
                for (int y = cy - step_y; y >= 0; y -= step_y)
                {
                    gr.DrawLine(penHalf, 0, y, width, y);
                }


                // Thick Line Grid
                step_x = 64;
                step_y = 64;
                // Draw Vertical Grid
                for (int x = cx + step_x; x < width; x+=step_x)
                {
                    //int flipx = ((x - cx) * -1) + cx;
                    gr.DrawLine(penFull, x, 0, x, height);
                    //gr.DrawLine(penFull, flipx, 0, flipx, height);
                }
                for (int x = cx - step_x; x >= 0; x-=step_x)
                {
                    gr.DrawLine(penFull, x, 0, x, height);
                }


                // Draw Horizontal Grid
                for (int y = cy + step_y; y < height; y += step_y)
                {
                    //int flipy = ((y - cy) * -1) + cy;
                    gr.DrawLine(penFull, 0, y, width, y);
                    //gr.DrawLine(penFull, 0, flipy, width, flipy);
                }
                for (int y = cy - step_y; y >= 0; y -= step_y)
                {
                    gr.DrawLine(penFull, 0, y, width, y);
                }


                // Draw Origin
                gr.DrawLine(penOrigin, cx, 0, cx, height);
                gr.DrawLine(penOrigin, 0, cy, width, cy);

                penFull.Dispose();
                penHalf.Dispose();
                penOrigin.Dispose();

                if (null != m_bitmap)
                {
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    //gr.DrawImage(m_bitmap, cx + m_offset_x, cy + m_offset_y);
                    gr.DrawImage(m_bitmap, cx + (m_offset_x << m_zoom), cy + (m_offset_y << m_zoom),
                        m_bitmap.Width << m_zoom, m_bitmap.Height << m_zoom);
                }
            }

            if (null != pictureBox.Image)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
            pictureBox.Image = bmp;

            pictureBox.Invalidate();
        
            gr.Dispose();

        }

        private void loadSpriteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openSpriteFileDialog.ShowDialog();

            if (DialogResult.OK == result)
            {
                m_spriteFile = new spData(openSpriteFileDialog.FileName);
                m_frameNo = 0;

                Paint();
            }
        }

        private void openSpriteFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Wedit_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("key {0}", e.KeyChar);
            //e.Handled = true;

            switch (e.KeyChar)
            {
                case (char)'4':
                case (char)Keys.NumPad4:
                case (char)Keys.Left:
                    m_frameNo -= 1;
                    if (m_frameNo < 0)
                    {
                        m_frameNo = 0;
                    }
                    Paint();
                    e.Handled = true;
                    break;
                case (char)'6':
                case (char)Keys.NumPad6:
                case (char)Keys.Right:

                    if (null != m_spriteFile)
                    {
                        int numFrames = m_spriteFile.NumFrames();
                        if (numFrames > 0) {
                            m_frameNo++;
                            if (m_frameNo >= numFrames)
                            {
                                m_frameNo = numFrames-1;
                            }
                        }
                    }
                    Paint();
                    e.Handled = true;
                    break;
                case (char)'-':
                case (char)Keys.Subtract:
                    m_zoom--;
                    if (m_zoom < 0)
                    {
                        m_zoom = 0;
                    }
                    Paint();
                    e.Handled = true;
                    break;
                case (char)'+':
                case (char)Keys.Add:
                    m_zoom++;
                    if (m_zoom > 4)
                    {
                        m_zoom = 4;
                    }
                    Paint();
                    e.Handled = true;
                    break;
            }
        }

        private void RedButton_Click(object sender, EventArgs e)
        {
            m_BackColor = RedButton.BackColor;
            m_GridColor = Color.White;
            Paint();
        }

        private void GreenButton_Click(object sender, EventArgs e)
        {
            m_BackColor = GreenButton.BackColor;
            m_GridColor = Color.White;
            Paint();
        }

        private void BlueButton_Click(object sender, EventArgs e)
        {
            m_BackColor = BlueButton.BackColor;
            m_GridColor = Color.White;
            Paint();
        }

        private void PurpleButton_Click(object sender, EventArgs e)
        {
            m_BackColor = PurpleButton.BackColor;
            m_GridColor = Color.White;
            Paint();
        }

        private void WhiteButton_Click(object sender, EventArgs e)
        {
            m_BackColor = WhiteButton.BackColor;
            m_GridColor = Color.Black;
            Paint();
        }
    }
}
