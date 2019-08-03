using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
using System.Windows.Forms;
//using System.Windows.Media;
//using System.Windows.Shapes;

namespace wedit
{
    public partial class wedit : Form
    {
        public enum AppMode
        {
            VIEW,
            ANIM,
            IMPORT_FRAMES,
            ANCHOR,
            IMPORT_PALETTES,
        };

        public class ImageEntry
        {
            public int m_frameNo;
            public string m_name;
        }

        public class AnimEntry
        {
            public int m_animNo;
            public string m_name;
        }

        public class AnimEditorEntry
        {
            public enum cmd
            {
                Sprite,
                End,
                Loop,
                Goto,
                GotoSeq,
                Pause,
                SetRate,
                SetSpeed,
                MultiOp,
                Delete,
                SetFlag,
                Sound,
                HFlip,
                VFlip,
                Nop,
                Process,
                ClearFlag,
                GotoLast,
                Blank,
                RndPause,
                SetHFlip,
                ClrHFlip,
                SetVFlip,
                ClrVFlip,
                HVFlip,
                SetHVFlip,
                ClrHVFlip,
                ExtSprite,
                Brk,
                OnBrk,
                DynSound,
            };

            public static bool[] delay =
            {
                true,  //Sprite,
                false, //End,
                false, //Loop,
                false, //Goto,
                false, //GotoSeq,
                true,  //Pause,
                false, //SetRate,
                false, //SetSpeed,
                false, //MultiOp,
                false, //Delete,
                false, //SetFlag,
                false, //Sound,
                false, //HFlip,
                false, //VFlip,
                true,  //Nop,
                false, //Process,
                false, //ClearFlag,
                false, //GotoLast,
                true,  //Blank,
                true,  //RndPause,
                false, //SetHFlip,
                false, //ClrHFlip,
                false, //SetVFlip,
                false, //ClrVFlip,
                false, //HVFlip,
                false, //SetHVFlip,
                false, //ClrHVFlip,
                true,  //ExtSprite,
                false, //Brk,
                false, //OnBrk,
                false, //DynSound,
            };

            public int m_lineNo;
            public AnimEditorEntry.cmd m_cmd;
            public string m_arg;
            public int m_image;
        }

        // Application Global Map
        
        wedit.AppMode m_mode = AppMode.VIEW;

        // For animation editor
        List<AnimEditorEntry> m_anim = new List<AnimEditorEntry>();

        spData  m_spriteFile = null;
        int     m_frameNo = 0;
        int     m_animNo = 0;   // Current Animation # loaded into the editor
        int     m_animCmdIndex = 0;
        int     m_animPrevAnim = 0;
        int     m_animRate = 0x100;
        int     m_animSpeed = 0x100;
        int     m_animTime = 0x100;
        bool    m_animPlaying = false;
        System.Windows.Forms.Timer m_animTimer = new System.Windows.Forms.Timer();
        Stopwatch m_animStopWatch;
        double   m_tickTimer = 0;
        double C_TICKTIME = 16.666666666666666666667;

        // Sprite Frame Render Info
        Bitmap m_bitmap = null;
        int    m_offset_x = 0;
        int    m_offset_y = 0;
        int    m_zoom = 1;  // pow2 zoom

        // Grid Canvas Render Options
        int m_canvas_offset_x = 0;
        int m_canvas_offset_y = 0;

        // Mouse Position
        int m_canvas_mouse_x = 0;
        int m_canvas_mouse_y = 0;
        int m_canvas_mouse_anchor_x = 0;
        int m_canvas_mouse_anchor_y = 0;
        bool m_canvas_mouse_button_left = false;
        bool m_canvas_mousedown_left = false;
        bool m_canvas_mouseup_left = false;

        // BluePrint Blue
        Color m_BackColor = Color.FromArgb(255, 17, 110, 169);
        Color m_GridColor = Color.FromArgb(255,255, 255, 255);
        
        // Sprite Frame Import Editor
        Bitmap m_importImage = null;
        Rectangle m_importRect = new Rectangle(0, 0, 0, 0);   // Currently selected import rectangle
        Color m_transparent = new Color();

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
            //fastObjectListView1.KeyPress += new KeyPressEventHandler(Wedit_KeyPress);

            this.PreviewKeyDown += new PreviewKeyDownEventHandler(Wedit_KeyDownPreview);
            //fastObjectListView1.PreviewKeyDown += new PreviewKeyDownEventHandler(Wedit_KeyDownPreview);

            this.KeyDown += new KeyEventHandler(Wedit_KeyDown);
            this.KeyUp += new KeyEventHandler(Wedit_KeyUp);

            this.pictureBox.MouseMove += new MouseEventHandler(Wedit_MouseMove);
            this.pictureBox.MouseDown += new MouseEventHandler(Wedit_MouseMove);

            BrightIdeasSoftware.OLVColumn col = this.objectFramesView.ColumnsInDisplayOrder[0];
            col.AspectGetter = delegate (object x) {
                return ((ImageEntry)x).m_name;
            };
            col.AspectToStringConverter = delegate (object x) {
                return String.Empty;
            };
            col.ImageGetter = delegate (object x) {
                return ((ImageEntry)x).m_frameNo;
            };
            //col.IsTileViewColumn = true;
            this.objectFramesView.ColumnsInDisplayOrder[1].IsTileViewColumn = true;

            this.objectFramesView.SelectionChanged += new System.EventHandler(OnFrameSelectionChanged);

            // Animation Select
            this.animListView.SelectionChanged += new System.EventHandler(OnAnimSelectionChanged);
            // Animation DoubleClick
            animListView.DoubleClick += new System.EventHandler(OnAnimDoubleClicked);

            // Animation Editor Events
            cmdListView.SelectionChanged += new System.EventHandler(OnCommandSelectionChanged);

            m_animTimer.Tick += new EventHandler(animTimerEventProcessor);
            // Sets the timer interval to 5 seconds.
            m_animTimer.Interval = 8;

            ShowAnimEditor(false);

            // Setup callbacks for the radio button / toolbar
            handCheckBox.Click += new System.EventHandler(handCheckBox_Pressed);
            selectCheckBox.Click += new System.EventHandler(selectCheckBox_Pressed);
            anchorCheckBox.Click += new System.EventHandler(anchorCheckBox_Pressed);
            pinCheckBox.Click += new System.EventHandler(pinCheckBox_Pressed);
        }

        //
        //  Hide Show Animation Editor
        //
        void ShowAnimEditor(bool bShow)
        {
            if (bShow)
            {
                // Show Animation Editor
                this.animListView.Enabled = false;
                this.animEditorBox.BringToFront();
                this.animEditorBox.Enabled = true;
                SetMode( AppMode.ANIM );
            }
            else
            {
                // Hide Animation Editor
                this.animEditorBox.Enabled = false;
                this.animListView.BringToFront();
                this.animListView.Enabled = true;
                SetMode( AppMode.VIEW );
            }
        }

        void SetMode( wedit.AppMode newMode )
        {
            if (newMode != m_mode)
            {
                m_mode = newMode;
                string modeString = "default";

                switch (m_mode)
                {
                case AppMode.VIEW:
                    modeString = "VIEW";
                    break;
                case AppMode.ANIM:
                    modeString = "ANIM";
                    break;
                case AppMode.IMPORT_FRAMES:
                    modeString = "IMPORT";
                    selectCheckBox_Pressed(null, null);  // Default into Select instead of Pan
                    break;
                case AppMode.ANCHOR:
                    modeString = "ANCHOR";
                    break;
                case AppMode.IMPORT_PALETTES:
                    modeString = "CLUT IMPORT";
                    break;
                default:
                    break;
                }

                mode.Text = modeString;
            }
        }

        void PaintAnimEditor()
        {
            if (null != m_spriteFile)
            {
                labelAnimNo.Text = String.Format("{0}", m_animNo);

                // Load up n_anim, and refresh the view
                m_anim = new List<AnimEditorEntry>();
                spAnim anim = m_spriteFile.GetAnim(m_animNo);
                m_animCmdIndex = 0;

                if (null != anim)
                {
                    int lineNo = 1;
                    foreach(spAnimCommand cmd in anim.m_commands)
                    {
                        AnimEditorEntry entry = new AnimEditorEntry();

                        entry.m_lineNo = lineNo;

                        if (cmd.m_frameNo < 0)
                        {
                            entry.m_cmd = (AnimEditorEntry.cmd) (cmd.m_command+1);
                            entry.m_arg = cmd.GetArgString();
                        }
                        else
                        {
                            entry.m_cmd = AnimEditorEntry.cmd.Sprite;
                            entry.m_arg = String.Format("{0}",cmd.m_frameNo);
                        }

                        m_anim.Add(entry);

                        lineNo++;
                    }
                }

                cmdListView.SetObjects(m_anim);
            }
        }

        void OnAnimDoubleClicked(object sender, System.EventArgs e)
        {
            AnimEntry ae = animListView.SelectedObject as AnimEntry;

            if (null != ae)
            {
                ShowAnimEditor(true);
                Console.WriteLine("Double Clicked {0} {1}", ae.m_animNo, ae.m_name);

                m_animNo = ae.m_animNo;
                PaintAnimEditor();
            }
        }

        void OnAnimSelectionChanged(object sender, System.EventArgs e)
        {
            AnimEntry ae = animListView.SelectedObject as AnimEntry;

            if (null != ae)
            {
                Console.WriteLine("{0} {1}", ae.m_animNo, ae.m_name);
            }
        }

        void OnFrameSelectionChanged(object sender, System.EventArgs e)
        {
            ImageEntry ie = objectFramesView.SelectedObject as ImageEntry;

            if (null != ie)
                m_frameNo = ie.m_frameNo;

            PaintSprite();
        }

        void OnCommandSelectionChanged(object sender, System.EventArgs e)
        {
            AnimEditorEntry aee = cmdListView.SelectedObject as AnimEditorEntry;

            if (null != aee)
            {
                if (AnimEditorEntry.cmd.Sprite == aee.m_cmd)
                {
                    if (int.TryParse(aee.m_arg, out m_frameNo))
                    {
                        PaintSprite();
                    }
                }
            }
        }
        
        // MouseMove Handler
        void Wedit_MouseMove(object obj, MouseEventArgs e)
        {
            m_canvas_mouse_x = e.X;
            m_canvas_mouse_y = e.Y;

            bool canvas_mouse_button_left = (MouseButtons.Left == e.Button);

            m_canvas_mousedown_left = false;
            m_canvas_mouseup_left = false;

            if (canvas_mouse_button_left != m_canvas_mouse_button_left)
            {
                m_canvas_mouse_button_left = canvas_mouse_button_left;
                if (canvas_mouse_button_left)
                {
                    m_canvas_mousedown_left = true;
                    // On mouse down, set the anchor
                    m_canvas_mouse_anchor_x = e.X;
                    m_canvas_mouse_anchor_y = e.Y;
                }
                else
                {
                    m_canvas_mouseup_left = true;
                }
            }

            if (canvas_mouse_button_left)
            {
                //Console.WriteLine("Mouse {0},{1}", e.X, e.Y);

                Panel p = splitContainer1.Panel2;

                int width = p.Size.Width;
                int height = p.Size.Height;
                
                switch (m_mode)
                {
                case AppMode.VIEW:
                    {
                        // Cheap-O recenter the origin based on mouse position
                        int cx = width / 2;
                        int cy = height / 2;

                        m_canvas_offset_x = e.X - cx;
                        m_canvas_offset_y = e.Y - cy;

                        PaintSprite();
                        break;
                    }
                case AppMode.IMPORT_FRAMES:
                    {
                        break;
                    }

                default:
                    break;

                }
            }

            if (m_canvas_mouseup_left)
            {
                switch (m_mode)
                {
                case AppMode.IMPORT_FRAMES:
                    {
                        SetImportRect(m_canvas_mouse_anchor_x,
                                    m_canvas_mouse_anchor_y,
                                    m_canvas_mouse_x,
                                    m_canvas_mouse_y);
                        break;
                    }
                }
            }

            this.mouseXY.Text = String.Format("{0}, {1}", e.X, e.Y);

            switch (m_mode)
            {
            case AppMode.IMPORT_FRAMES:
                PaintImport();
                break;
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

        private void PaintSprite()
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

        private void PaintImport()
        {
            if (null == m_importImage)
                return;

            Panel p = splitContainer1.Panel2;
            int width  = p.Size.Width;
            int height = p.Size.Height;
            Size s = new Size(width, height);
            pictureBox.Size = s;

            Color bgColor = m_BackColor;
            Color fgColor = Color.FromArgb(128, m_GridColor.R, m_GridColor.G, m_GridColor.B);

            Bitmap bmp = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage(bmp);

            gr.Clear(bgColor);

            if (null != m_importImage)
            {
                int zoom = 1 << m_zoom;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gr.DrawImage(m_importImage, 0, 0,
                    m_importImage.Width << m_zoom, m_importImage.Height << m_zoom);

                // Draw Cross Hairs
                Pen penCross = new Pen(fgColor);

                penCross.Width = zoom;

                int x = m_canvas_mouse_x;
                int y = m_canvas_mouse_y;

                if (zoom > 1)
                {
                    x &= ~(zoom - 1);
                    y &= ~(zoom - 1);
                }

                x += (zoom >> 1);
                y += (zoom >> 1);

                if (m_canvas_mouse_button_left)
                {
                    // Draw a selection rubber-band
                    int anchor_x = m_canvas_mouse_anchor_x;
                    int anchor_y = m_canvas_mouse_anchor_y;

                    if (zoom > 1)
                    {
                        anchor_x &= ~(zoom - 1);
                        anchor_y &= ~(zoom - 1);
                    }

                    anchor_x += (zoom >> 1);
                    anchor_y += (zoom >> 1);

                    gr.DrawLine(penCross, x, y, anchor_x, y);
                    gr.DrawLine(penCross, anchor_x, y, anchor_x, anchor_y);
                    gr.DrawLine(penCross, anchor_x, anchor_y, x, anchor_y);
                    gr.DrawLine(penCross, x, y, x, anchor_y);
                }
                else
                {
                    if (!m_importRect.IsEmpty)
                    {
                        int x0 = m_importRect.Left;
                        int y0 = m_importRect.Top;
                        int x1 = m_importRect.Right;
                        int y1 = m_importRect.Bottom;

                        x0 <<= m_zoom; x0 += (1 << m_zoom >> 1);
                        y0 <<= m_zoom; y0 += (1 << m_zoom >> 1);
                        x1 <<= m_zoom; x1 += (1 << m_zoom >> 1);
                        y1 <<= m_zoom; y1 += (1 << m_zoom >> 1);

                        gr.DrawLine(penCross, x0, y0, x1, y0);
                        gr.DrawLine(penCross, x1, y0, x1, y1);
                        gr.DrawLine(penCross, x1, y1, x0, y1);
                        gr.DrawLine(penCross, x0, y1, x0, y0);
                    }

                    gr.DrawLine(penCross, 0, y, width, y);
                    gr.DrawLine(penCross, x, 0, x, height);
                }
                penCross.Dispose();
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

                if (null != m_importImage)
                {
                    int zoom = 1 << m_zoom;
                    gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    gr.DrawImage(m_importImage, 0, 0,
                        m_importImage.Width << m_zoom, m_importImage.Height << m_zoom);

                    // Draw Cross Hairs
                    Pen penCross = new Pen(fgColor);
                    
                    penCross.Width = zoom;
                    //penCross.Alignment = System.Drawing.Drawing2D.PenAlignment.Right;

                    int x = m_canvas_mouse_x;
                    int y = m_canvas_mouse_y;

                    if (zoom > 1)
                    {
                        x &= ~(zoom - 1);
                        y &= ~(zoom - 1);
                    }

                    x += (zoom >> 1);
                    y += (zoom >> 1);
                    
                    gr.DrawLine(penCross, 0, y, width, y);
                    gr.DrawLine(penCross, x, 0, x, height);
                    penCross.Dispose();
                }
                else if (null != m_bitmap)
                {
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
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

        //
        // Make sure controls reflect what is happening
        // with the the m_spriteFile
        //
        private void SpriteFileUpdate()
        {
            m_frameNo = 0;

            if ((null != m_spriteFile) /*&& !m_spriteFile.IsEmpty()*/)
            {
                ImageList images = new ImageList();
                images.ImageSize = new Size(64, 64);
                int numFrames = m_spriteFile.NumFrames();

                List<ImageEntry> frames = new List<ImageEntry>();

                // Load up the image list with the BMPs
                for (int idx = 0; idx < numFrames; ++idx)
                {
                    Bitmap bmp = m_spriteFile.BitmapFromFrame(idx);
                    images.Images.Add(bmp);
                    ImageEntry ie = new ImageEntry();
                    ie.m_frameNo = idx;
                    ie.m_name = String.Format("Frame {0}", idx + 1);
                    frames.Add(ie);
                }

                objectFramesView.SmallImageList = images;
                //cmdListView.SmallImageList = images;
                objectFramesView.LargeImageList = images;
                objectFramesView.SetObjects(frames);

                // Anims
                List<AnimEntry> anims = new List<AnimEntry>();

                for (int idx = 0; idx<m_spriteFile.NumAnims(); ++idx)
                {
                    AnimEntry entry = new AnimEntry();

                    spAnim anm = m_spriteFile.GetAnim(idx);

                    entry.m_animNo = idx;
                    entry.m_name   = anm.m_name;

                    anims.Add(entry);
                }

                animListView.SetObjects(anims);
            }

            PaintSprite();
        }

        private void loadSpriteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openSpriteFileDialog.ShowDialog();

            if (DialogResult.OK == result)
            {
                m_spriteFile = new spData(openSpriteFileDialog.FileName);

                SpriteFileUpdate();
            }
        }

        private void openSpriteFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Wedit_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("key {0}", e.KeyChar);
            //e.Handled = true;

            // Hack to keep spacebar from pushing down on buttons
             if (this.ActiveControl is Button
               && e.KeyChar == (char)Keys.Space) 
               {
                  var button = this.ActiveControl;
                  button.Enabled = false;
                  Application.DoEvents();
                  button.Enabled = true;
                  button.Focus();
               }

            switch (e.KeyChar)
            {
                case (char)'4':
                //case (char)Keys.NumPad4:
                //case (char)Keys.Left:
                    m_frameNo -= 1;
                    if (m_frameNo < 0)
                    {
                        m_frameNo = 0;
                    }
                    PaintSprite();
                    e.Handled = true;
                    break;
                case (char)'6':
                //case (char)Keys.NumPad6:
                //case (char)Keys.Right:

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
                    PaintSprite();
                    e.Handled = true;
                    break;
                case (char)'-':
                    //case (char)Keys.Subtract:
                    zoomOutButton_Click(null, null);
                    e.Handled = true;
                    break;
                case (char)'+':
                    //case (char)Keys.Add:
                    zoomInButton_Click(null, null);
                    e.Handled = true;
                    break;
                case (char)27: // Escape
                    e.Handled = true;
                    SetMode( AppMode.VIEW );
                    handCheckBox_Pressed(null, null);
                    if (null != m_importImage)
                    {
                        m_importImage.Dispose();
                        m_importImage = null;
                    }
                    PaintSprite();
                    break;
                case (char)' ': // Space
                    if ( (!m_importRect.IsEmpty) &&
                         (null != m_importImage) )
                    {
                        ImportFrame();
                        PaintImport();
                    }
                break;
            }
        }

        private void RedButton_Click(object sender, EventArgs e)
        {
            m_BackColor = RedButton.BackColor;
            m_GridColor = Color.White;
            PaintSprite();
        }

        private void GreenButton_Click(object sender, EventArgs e)
        {
            m_BackColor = GreenButton.BackColor;
            m_GridColor = Color.White;
            PaintSprite();
        }

        private void BlueButton_Click(object sender, EventArgs e)
        {
            m_BackColor = BlueButton.BackColor;
            m_GridColor = Color.White;
            PaintSprite();
        }

        private void PurpleButton_Click(object sender, EventArgs e)
        {
            m_BackColor = PurpleButton.BackColor;
            m_GridColor = Color.White;
            PaintSprite();
        }

        private void WhiteButton_Click(object sender, EventArgs e)
        {
            m_BackColor = WhiteButton.BackColor;
            m_GridColor = Color.Black;
            PaintSprite();
        }

        private void animButton_Click(object sender, EventArgs e)
        {
            ShowAnimEditor(false);
        }

        private void buttonNextAnim_Click(object sender, EventArgs e)
        {
            if (null != m_spriteFile)
            {
                m_animNo++;

                if (m_animNo >= m_spriteFile.NumAnims())
                    m_animNo = 0;

                PaintAnimEditor();
            }
        }

        private void buttonPrevAnim_Click(object sender, EventArgs e)
        {
            if (null != m_spriteFile)
            {
                m_animNo--;

                if (m_animNo < 0)
                {
                    m_animNo = m_spriteFile.NumAnims()-1;
                    if (m_animNo < 0)
                        m_animNo = 0;
                }

                PaintAnimEditor();
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (!m_animPlaying)
            {
                m_animPlaying = true;
                m_animCmdIndex = 0;
                m_animSpeed = 0x100;
                m_animTime  = 0x100;
                m_animTimer.Start();
                m_animStopWatch = System.Diagnostics.Stopwatch.StartNew();
                m_tickTimer = C_TICKTIME;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (m_animPlaying)
            {
                m_animPlaying = false;
                m_animTimer.Stop();
            }
        }

        private void animTimerEventProcessor(Object myObject,
                                            EventArgs myEventArgs)
        {
            if (m_animPlaying)
            {
                m_tickTimer -= (double)m_animStopWatch.ElapsedMilliseconds;
                m_animStopWatch = System.Diagnostics.Stopwatch.StartNew();

                if (m_tickTimer <= 0)
                {
                    //Console.WriteLine("tick");
                    m_tickTimer += C_TICKTIME;

                    m_animTime -= m_animSpeed;

                    if (m_animTime <= 0)
                    {
                        m_animTime += m_animRate;    // Base Rate
                        animNextFrame();
                    }
                }
            }
        }

        private void animNextFrame()
        {
            // Single Step down the animation
            AnimEditorEntry aee = m_anim[ m_animCmdIndex ];

            m_animCmdIndex++;
            if (m_animCmdIndex >= m_anim.Count)
            {
                m_animCmdIndex = m_anim.Count-1;
                if (m_animCmdIndex < 0)
                    m_animCmdIndex = 0;
            }

            switch (aee.m_cmd)
            {
            case AnimEditorEntry.cmd.Sprite:
                m_frameNo = int.Parse(aee.m_arg);
                PaintSprite();
                break;
            case AnimEditorEntry.cmd.End:
                stopButton_Click(null,null);
                break;
            case AnimEditorEntry.cmd.Loop:
                m_animCmdIndex = 0;
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.Goto:
                m_animCmdIndex = int.Parse(aee.m_arg);
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.GotoSeq:
                m_animCmdIndex = 0;
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.Pause:
                break;
            case AnimEditorEntry.cmd.SetRate:
                m_animRate = ParseSpeed(aee.m_arg);
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.SetSpeed:
                m_animSpeed = ParseSpeed(aee.m_arg);
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.MultiOp:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.Delete:
                stopButton_Click(null,null);
                break;
            case AnimEditorEntry.cmd.SetFlag:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.Sound:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.HFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.VFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.Nop:
                break;
            case AnimEditorEntry.cmd.Process:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.ClearFlag:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.GotoLast:
                m_animCmdIndex = 0;
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.Blank:
                m_frameNo = -1;
                PaintSprite();
                break;
            case AnimEditorEntry.cmd.RndPause:
                break;
            case AnimEditorEntry.cmd.SetHFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.ClrHFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.SetVFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.ClrVFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.HVFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.SetHVFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.ClrHVFlip:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.ExtSprite:
                m_frameNo = int.Parse(aee.m_arg);
                PaintSprite();
                break;
            case AnimEditorEntry.cmd.Brk:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.OnBrk:
                animNextFrame();
                break;
            case AnimEditorEntry.cmd.DynSound:
                animNextFrame();
                break;
            default:
                break;
            }
        }

        //
        // Parse $ style string
        //
        private int ParseSpeed(string arg)
        {
            int result = 0;

            if ("$" == arg.Substring(0,1))
            {
                result = int.Parse(arg.Substring(1), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                result = int.Parse(arg);
            }

            return result;
        }

        private void importFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openImageFileDialog.ShowDialog();

            if (DialogResult.OK == result)
            {
                m_importImage = new Bitmap(openImageFileDialog.FileName);

                if (null != m_importImage)
                {
                    SetMode( AppMode.IMPORT_FRAMES );
                }

                PaintSprite();
            }
        }

        private void SetImportRect(int x0, int y0, int x1, int y1)
        {
            // Attempt to shrink rect, and set the results out
            // to the m_importRect, so that the rubberband is rendered
            // (and we can wait for the spacebar to tell us to cut)
            if (x1 < x0)
            {
                int temp = x0;
                x0 = x1;
                x1 = temp;
            }
            if (y1 < y0)
            {
                int temp = y0;
                y0 = y1;
                y1 = temp;
            }

            MouseXYToImportXY(ref x0, ref y0);
            MouseXYToImportXY(ref x1, ref y1);

            m_importRect = new Rectangle(x0, y0, (x1 - x0)/*+ 1*/, (y1 - y0)/*+ 1*/);

            //if (null != m_importImage)
            //{
            //    if (PixelFormat.Format8bppIndexed == m_importImage.PixelFormat)
            //    {
            //        Console.WriteLine(String.Format("Pixel Format = {0}", m_importImage.PixelFormat));
            //    }
            //}

            ShrinkRect(ref m_importRect);

        }

        //
        // Take shrink the rectangle on to the m_importImage 
        //
        private void ShrinkRect(ref Rectangle r)
        {
            if (null != m_importImage)
            {
                Color transparent = m_importImage.GetPixel(r.Left, r.Top);
                m_transparent = transparent;

                int top, bottom, left, right;
                top = bottom = left = right = -1;

                // Get the top
                for (int y = r.Top; y <= r.Bottom; ++y)
                {
                    if (CheckHLine(transparent,r.Left,r.Right,y))
                    {
                        top = y;
                        break;
                    }
                }
                // Get the left
                for (int x = r.Left; x <= r.Right; ++x)
                {
                    if (CheckVLine(transparent,r.Top,r.Bottom,x))
                    {
                        left = x;
                        break;
                    }
                }
                // Get the bottom
                for (int y = r.Bottom; y >= r.Top; --y)
                {
                    if (CheckHLine(transparent,r.Left,r.Right,y))
                    {
                        bottom = y;
                        break;
                    }
                }
                // Get the right
                for (int x = r.Right; x >= r.Left; --x)
                {
                    if (CheckVLine(transparent,r.Top,r.Bottom,x))
                    {
                        right = x;
                        break;
                    }
                }

                if ((top >= 0) && (bottom >= 0) &&
                    (left >=0) && (right >= 0) )
                {
                    r = new Rectangle(left, top, right - left/*+ 1*/, bottom - top/*+ 1*/);
                }
                else
                {
                    r = new Rectangle();
                }
            }
        }

        //
        // Return true if we encounter a non-transparent pixel
        //
        private bool CheckHLine(Color transparent, int x0, int x1, int y)
        {
            bool result = false;

            for (int x = x0; x <= x1; ++x)
            {
                Color p = m_importImage.GetPixel(x , y);

                if (p != transparent)
                {
                    return true;
                }
            }

            return result;
        }

        //
        // Return true if we encounter a non-transparent pixel
        //
        private bool CheckVLine(Color transparent, int y0, int y1, int x)
        {
            bool result = false;

            for (int y = y0; y <= y1; ++y)
            {
                Color p = m_importImage.GetPixel(x, y);

                if (p != transparent)
                {
                    return true;
                }
            }

            return result;
        }

        //
        // Actually Clip out the frame we want to import
        //
        private void ImportFrame()
        {
            if ( (!m_importRect.IsEmpty) &&
                 (null != m_importImage) )
            {
                // For now, we're giving up a color (on the GS)
                // We want to map these colors into an index from 0-15
                // onto one of the palettes thats available

                // For now, we'll just support palette 0, because
                // more than 1 palette just isn't practical on the GS
                // and if you're going to use more than 1, it doesn't
                // really make any difference in this tool

                // Clone looks like it's not inclusive, so we have to inflate the importRect by 1
                Rectangle importRect = new Rectangle(m_importRect.Left, m_importRect.Top, m_importRect.Width+1, m_importRect.Height+1);

                Bitmap clipped = m_importImage.Clone(importRect, m_importImage.PixelFormat);

                if (null != clipped)
                {
                    if (null == m_spriteFile)
                    {
                        m_spriteFile = new spData();
                    }
                }

                // Add the frame to the sprite catalog
                m_spriteFile.AddFrame(m_transparent, clipped);

                // We're done, so erase the old image
                for (int y = m_importRect.Top; y <= m_importRect.Bottom; ++y)
                {
                    for (int x = m_importRect.Left; x <= m_importRect.Right; ++x)
                    {
                        m_importImage.SetPixel(x,y,m_transparent);
                    }
                }
                
                // Clear the import Rectangle
                m_importRect = new Rectangle();

                SpriteFileUpdate();
            }
        }


        // Convert Mouse Canvas Coordinates
        // into local import bitmap coordinates
        private void MouseXYToImportXY(ref int x, ref int y)
        {
            int zoom = m_zoom;

            x >>= zoom;
            y >>= zoom;

            if (null != m_importImage)
            {
                if (x < 0) x = 0;
                if (x >= m_importImage.Width) x = m_importImage.Width-1;
                if (y < 0) y = 0;
                if (y >= m_importImage.Height) y = m_importImage.Height-1;
            }

        }

        //
        // Hand Tool Check Box
        // Radio Button
        //
        private void handCheckBox_Pressed(object sender, EventArgs e)
        {
            // If this is enabled, make sure we disable the other tools
            // update the default cursor in the editor picture panel
            // set the editor mode
            handCheckBox.Checked = true;
            selectCheckBox.Checked = false;
            anchorCheckBox.Checked = false;
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void selectCheckBox_Pressed(object sender, EventArgs e)
        {
            selectCheckBox.Checked = true;
            handCheckBox.Checked = false;
            anchorCheckBox.Checked = false;
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
        }

        private void anchorCheckBox_Pressed(object sender, EventArgs e)
        {
            SetMode(AppMode.ANCHOR);
            anchorCheckBox.Checked = true;
            handCheckBox.Checked = false;
            selectCheckBox.Checked = false;
            this.pictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
        }

        private void pinCheckBox_Pressed(object sender, EventArgs e)
        {
            Color backColor = anchorCheckBox.BackColor;
            Color checkedColor = Color.FromArgb(255, 204, 228, 247);
            Color doubleColor  = Color.FromArgb(255, 152, 228, 183);

            //pinCheckBox.FlatAppearance.CheckedBackColor = Color.Red;

            switch (pinCheckBox.CheckState)
            {
            case CheckState.Checked:
                //pinCheckBox.Checked = true;
                pinCheckBox.CheckState = CheckState.Indeterminate;
                pinCheckBox.BackColor = doubleColor;
                //pinCheckBox.FlatAppearance.CheckedBackColor = Color.Red;
                break;
            case CheckState.Indeterminate:
                pinCheckBox.CheckState = CheckState.Unchecked;
                pinCheckBox.BackColor = backColor;
                //pinCheckBox.FlatAppearance.CheckedBackColor = checkedColor;
                break;
            case CheckState.Unchecked:
                pinCheckBox.CheckState = CheckState.Checked;
                pinCheckBox.BackColor = checkedColor;
                //pinCheckBox.FlatAppearance.CheckedBackColor = checkedColor;
                break;
            default:
                break;
            }
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            m_zoom++;
            if (m_zoom > 4)
            {
                m_zoom = 4;
            }
            PaintSprite();
            PaintImport();
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            m_zoom--;
            if (m_zoom < 0)
            {
                m_zoom = 0;
            }
            PaintSprite();
            PaintImport();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear out the current file
            m_spriteFile = new spData();

            ImageList images = new ImageList();
            images.ImageSize = new Size(64, 64);
            int numFrames = m_spriteFile.NumFrames();

            List<ImageEntry> frames = new List<ImageEntry>();

            objectFramesView.SmallImageList = images;
            objectFramesView.LargeImageList = images;
            objectFramesView.SetObjects(frames);

            // Anims
            List<AnimEntry> anims = new List<AnimEntry>();
            animListView.SetObjects(anims);

            m_bitmap = null;

            SpriteFileUpdate();          
        }

        private void AddAnimButton_Click(object sender, EventArgs e)
        {
            // Add a new animation to the list
            if (null == m_spriteFile)
                m_spriteFile = new spData();

            m_spriteFile.AddAnim();
            ShowAnimEditor(false);
            SpriteFileUpdate();
        }

        private void importPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == m_spriteFile)
                newToolStripMenuItem_Click(null, null);

            DialogResult result = openPaletteFileDialog.ShowDialog();

            if (DialogResult.OK == result)
            {
                // Read in the palette, file, and replace the initial palette in the m_spriteFile
                m_spriteFile.ImportPalette(openPaletteFileDialog.FileName);

                PaintSprite();
            }
        }

        private void saveSpriteFileAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveSpriteFileDialog.ShowDialog();

            if (DialogResult.OK == result)
            {
                // Save the sprite file
                m_spriteFile.Save(saveSpriteFileDialog.FileName);
            }
        }
    }
}
