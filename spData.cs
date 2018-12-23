using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

//
// Load / Save Ancient SP
// WEDIT Files from back in the day
//
// Have to back engineer the format, but I think it looks
// I lot like AIFF, so hopefully won't be too terrible
//

namespace wedit
{
    public class spPalette
    {
        public List<Color> colors = new List<Color>();
    }

    public class spRect
    {
        public int m_frameNo;
        public int m_attrib;
        public int m_xOffset;
        public int m_yOffset;
        public int m_width;
        public int m_height;
    }

    public class spPixels
    {
        public int m_width;
        public int m_height;
        public int m_offset_x;
        public int m_offset_y;

        public List<byte> m_pixels = new List<byte>();

        public List<spRect> m_rects = new List<spRect>();

        public spPixels(int width, int height, List<byte> pixels)
        {
            m_width  = width;
            m_height = height;
            m_pixels = pixels;
        }
    }

    // WEDIT SP File Serializer / Class
    public class spData
    {
        List<spPalette> m_palettes = new List<spPalette>();
        List<spPixels>  m_frames   = new List<spPixels>();

        public int NumFrames()
        {
            return m_frames.Count;
        }

        public spData(string pathName)
        {
            int animNo = 0;

            Console.WriteLine("import: {0}", pathName);

            using (BinaryReader b = new BinaryReader(
                File.Open(pathName, FileMode.Open)))
            {
                // Use BaseStream.
                long fileLength = b.BaseStream.Length;
                UInt32 header = b.ReadUInt32();

                UInt16 v0 = 0;
                UInt16 v1 = 0;


                while (0xFFFFFFFF != header)
                {
                    UInt16 length = b.ReadUInt16();

                    Console.WriteLine("0x{0:X8} {1}{2}{3}{4} - {5}",
                                header,
                                (char) ((header >> 0) & 0xFF),
                                (char) ((header >> 8) & 0xFF),
                                (char) ((header >>16) & 0xFF),
                                (char) ((header >>24) & 0xFF),
                                length
                                );

                    switch (header)
                    {
                        // SPRITE FILE HEADER
                        case 0x41454853: // 'SHEA'
                            Debug.Assert(4 == length);
                            v0 = b.ReadUInt16();
                            v1 = b.ReadUInt16();
                            Console.WriteLine("v = {0:X4}", v0);
                            Console.WriteLine("v = {0:X4}", v1);
                        break;
                    // SPRITE RECT
                    case 0x54435253: // 'SRCT'
                        Debug.Assert(12 == length);
                        {
                            UInt16 frameNo   = b.ReadUInt16();
                            UInt16 attribute = b.ReadUInt16();
                            Int16  xOffset   = b.ReadInt16();
                            Int16  yOffset   = b.ReadInt16();
                            Int16  width     = b.ReadInt16();
                            Int16  height    = b.ReadInt16();

                            Console.WriteLine("{0:X4},{1:X4} - {2},{3},{4},{5}",
                                              frameNo, attribute,
                                              xOffset, yOffset,
                                              width, height);

                            spRect r = new spRect();

                            r.m_frameNo = frameNo;
                            r.m_attrib  = attribute;
                            r.m_xOffset = xOffset;
                            r.m_yOffset = yOffset;
                            r.m_width   = width;
                            r.m_height  = height;

                            m_frames[ frameNo ].m_rects.Add(r);

                        }
                        break;

                        // SPRITE PALETTE ENTRIES
                    case 0x45505353: // 'SSPE'
                        {
                            // 50
                            // 99

                            // byte pal num
                            // 16 color (3 bytes each)
                            // pal num = 0xFF end

                            while (true)
                            {
                                byte palNo = b.ReadByte();

                                if (0xFF == palNo)
                                    break;

                                Console.WriteLine("Read palNo {0}", palNo);

                                spPalette pal = new spPalette();

                                byte red;
                                byte green;
                                byte blue;

                                for (int idx = 0; idx < 16; ++idx)
                                {
                                    red   = b.ReadByte();
                                    green = b.ReadByte();
                                    blue  = b.ReadByte();

                                    pal.colors.Add( Color.FromArgb(255,red,green,blue) );
                                }

                                m_palettes.Add( pal );
                            }
                        }
                            break;

                       // SPRITE PACKED PIXELS
                    case 0x50505353: // 'SSPP'
                        {
                            // 10 byte header
                            // b 0
                            // b 0
                            // b width in bytes (2x this for pixels)
                            // b height in bytes
                            // b 0
                            // w xoff
                            // w yoff
                            // b 0

                            byte ub = 0;

                            ub = b.ReadByte(); // Discard
                            Debug.Assert(0 == ub);

                            ub = b.ReadByte(); // Discard
                            Debug.Assert(0 == ub);

                            byte width  = b.ReadByte();
                            byte height = b.ReadByte();
                            b.ReadByte(); // Discard

                            Int16 xoff = b.ReadInt16();
                            Int16 yoff = b.ReadInt16();

                            if (v1 > 0x100) {
                                ub = b.ReadByte(); // Discard
                            }
                            //Debug.Assert(0 == ub);

                            Console.WriteLine("     {0}x{1} {2},{3}",
                                              width, height,
                                              xoff, yoff
                                              );

                            if (v1 > 0x100) {
                                length -= 10;
                            }
                            else
                            {
                                length -= 9;
                            }

                            // Read in the pixels
                            List<byte> pixels = new List<byte>();

                            // pixel data (w x h)
                            while (0 != length)
                            {
                                pixels.Add(b.ReadByte());
                                length--;
                            }

                            // Create a pixel frame
                            spPixels pixelFrame = new spPixels(width, height, pixels);

                            pixelFrame.m_offset_x = xoff;
                            pixelFrame.m_offset_y = yoff;

                            // Add it to the list
                            m_frames.Add( pixelFrame );

                        }
                        break;


                    case 0x4D4E4153:  // 'SANM'
                                      /* 
                                    	da	opend
                                    	da	oploop
                                    	da	opgoto
                                    	da	opgotoseq
                                    	da	oppause
                                    	da	opsetrate
                                    	da	opsetdefrate
                                    	da	opmultiop
                                    	da	opdelete
                                    	da 	opsetflag
                                    	da	opsound
                                    	da	opfliph
                                    	da	opflipv
                                    	da	opnop
                                    	da	opprocess
                                    	da	opclearflag
                                    	da	opgotolast
                                    	da	opblank
                                    	da	oprndpause
                                    	da	opsethflip
                                    	da	opresethflip
                                    	da	opsetvflip
                                    	da	opresetvflip
                                    	da	ophvflip
                                    	da	opsethvflip
                                    	da	opresethvflip
                                    	da	opextspr
                                    	da	opbrk
                                    	da	oponbrk
                                    	da 	opdynsound
                                       
                                      dw  <size in bytes> ; including these 2 bytes
                                      db  #of lines in the seqence

                                      db <frameNo> or 0xFF means command
                                       
                                      ; End 
                                      00 
                                      ; Loop - back to index 0 on the anim
                                      01
                                      ; Goto
                                      02, targetLo, TargetHi
                                      ; GotoSeq 
                                      03   db <seqNo>
                                      ; Pause 
                                      04 
                                      ; SetRate 
                                      05 
                                      ; SetDefaultSpeed
                                      06, speedlo, speed hi 
                                      ; Loop
                                      0A
                                      ; Process
                                      0E, <byte>processNum
                                      ; NOP
                                      0D, NOP
                                      ; GotoLast (return)
                                      10

                                      GotoSeq
                                      MultiOp
                                      Sound XX
                                      HFlip
                                      VFlip
                                      SetFlag
                                      BRK
                                      */
                        {
                            Console.WriteLine("Animation #{0}",animNo);
                            animNo++;
                            int lineNo = 1;
                            UInt16 u16;
                            byte u8;

                            while (0 != length)
                            {
                                byte cmd = b.ReadByte();
                                length--;

                                if ((0xFF == cmd) && (0 != length))
                                {
                                    cmd = b.ReadByte();
                                    length--;

                                    switch (cmd)
                                    {
                                    case 0:
                                        Console.WriteLine("({0})  End", lineNo);
                                        break;
                                    case 1:
                                        Console.WriteLine("({0})  Loop",lineNo);
                                        break;
                                    case 2:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  Goto {1}", lineNo, u16);
                                        break;
                                    case 3:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  GotoSeq {1}", lineNo, u8);
                                        break;
                                    case 4:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  Pause {1}", lineNo, u8);
                                        break;
                                    case 5:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  SetRate ${1:X4}", lineNo, u16);
                                        break;
                                    case 6:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  SetSpeed ${1:X4}", lineNo, u16);
                                        break;
                                    case 7:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  MultiOp {1}", lineNo, u8);
                                        break;
                                    case 8:
                                        Console.WriteLine("({0})  Delete", lineNo);
                                        break;
                                    case 9:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  SetFlag {1}", lineNo, u8);
                                        break;
                                    case 10:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  Sound {1}", lineNo, u8);
                                        break;
                                    case 11:
                                        Console.WriteLine("({0})  HFlip", lineNo);
                                        break;
                                    case 12:
                                        Console.WriteLine("({0})  VFlip", lineNo);
                                        break;
                                    case 13:
                                        Console.WriteLine("({0})  Nop", lineNo);
                                        break;
                                    case 14:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  Process {1}", lineNo, u8);
                                        break;
                                    case 15:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  ClearFlag {1}", lineNo, u8);
                                        break;
                                    case 16:
                                        Console.WriteLine("({0})  GotoLast", lineNo);
                                        break;
                                    case 17:
                                        Console.WriteLine("({0})  Blank", lineNo);
                                        break;
                                    case 18:
                                        {
                                            byte min = b.ReadByte(); length--;
                                            byte max = b.ReadByte(); length--;
                                            Console.WriteLine("({0})  RndPause {1},{2}", lineNo, min, max);
                                        }
                                        break;
                                    case 19:
                                        Console.WriteLine("({0})  Set HFlip", lineNo);
                                        break;
                                    case 20:
                                        Console.WriteLine("({0})  Clr HFlip", lineNo);
                                        break;
                                    case 21:
                                        Console.WriteLine("({0})  Set VFlip", lineNo);
                                        break;
                                    case 22:
                                        Console.WriteLine("({0})  Clr VFlip", lineNo);
                                        break;
                                    case 23:
                                        Console.WriteLine("({0})  HVFlip", lineNo);
                                        break;
                                    case 24:
                                        Console.WriteLine("({0})  Set HVFlip", lineNo);
                                        break;
                                    case 25:
                                        Console.WriteLine("({0})  Clr HVFlip", lineNo);
                                        break;
                                    case 26:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  ExtSprite {1}", lineNo, u16);
                                        break;
                                    case 27:
                                        Console.WriteLine("({0})  Brk", lineNo);
                                        break;
                                    case 28:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  OnBrk {1}", lineNo, u8);
                                        break;
                                    case 29:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  DynSound {1}", lineNo, u8);
                                        break;
                                    default:
                                        Console.WriteLine("({0})  UNKNOWN CMD {1}, len={2}", lineNo, cmd, length);
                                        Debug.Assert(false);
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("({0})  Sprite {1}",lineNo,cmd);
                                }
                                lineNo++;
                            }
                        }
                        break;

                    default:
                        {
                            // Skip the Chunk, since we don't know what it is
                            while (0 != length)
                            {
                                b.ReadByte();
                                length--;
                            }
                            break;
                        }

                    }

                    header = b.ReadUInt32();
                }

                #if false
                switch (header)
                {
                // SPRITE FILE HEADER
                case 'SHEA':
                    break;
                case 'SSPE':
                    break;
                case 'SSPP':
                case 'SOBJ':
                case 'SANM':
                case 'SRCT':

                }
                #endif
            }
        }

        public bool IsEmpty()
        {
            bool bResult = true;

            if ((m_palettes.Count > 0) &&
                (m_frames.Count > 0))
            {
                bResult = false;
            }

            return bResult;
        }

        public Bitmap BitmapFromFrame(int frameNo)
        {
            Bitmap bmp = null;

            if (!IsEmpty() && (frameNo >= 0) && (frameNo < m_frames.Count))
            {
                spPalette pal = m_palettes[0];
                spPixels  pix = m_frames[ frameNo ];

                int width  = pix.m_width * 2;
                int height = pix.m_height;

                bmp = new Bitmap(width, height);

                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        byte px = pix.m_pixels[(y * pix.m_width) + (x>>1)];
                        if (0 == (x&1))
                        {
                            px >>= 4;
                        }

                        px &= 0xF;

                        Color c = pal.colors[px];
                        
                        if (0==px)
                        {
                            c = Color.FromArgb(0, c.R, c.G, c.B);
                        }

                        bmp.SetPixel(x,y,c);
                    }
                }

#if false
                //
                // Draw the rectangles in white
                //
                int anchor_x = 0; //pix.m_offset_x;
                int anchor_y = 0; //pix.m_offset_y;

                foreach (spRect r in pix.m_rects)
                {
                    int x0,y0,x1,y1;

                    x0 = r.m_xOffset + anchor_x;
                    x1 = x0 + r.m_width;

                    y0 = r.m_yOffset + anchor_y;
                    y1 = y0 + r.m_height;

                    // Horizontal Lines
                    for (int x = x0; x < x1; ++x)
                    {
                        if (x >= 0 && x < width)
                        {
                            if (y0 >= 0 && y0 < height)
                            bmp.SetPixel(x,y0,Color.White);
                            if (y1 >= 0 && y1 < height)
                            bmp.SetPixel(x,y1,Color.White);
                        }
                    }
                    // Vertical Lines
                    for (int y = y0; y < y1; ++y)
                    {
                        if (y >= 0 && y < height)
                        {
                            if (x0 >= 0 && x0 < width)
                            bmp.SetPixel(x0,y,Color.White);
                            if (x1 >= 0 && x1 < width)
                            bmp.SetPixel(x1,y,Color.White);
                        }
                    }
                }
#endif
            }
            return bmp;
        }

        public spPixels GetFrame(int frameNo)
        {
            spPixels pix = null;

            if (!IsEmpty() && (frameNo >= 0) && (frameNo < m_frames.Count))
            {
                pix = m_frames[ frameNo ];
            }

            return pix;
        }
    }
}
