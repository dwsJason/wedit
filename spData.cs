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

    public class spPixels
    {
        public int m_width;
        public int m_height;
        public int m_offset_x;
        public int m_offset_y;

        public List<byte> m_pixels = new List<byte>();

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
                            while (0 != length)
                            {
                                b.ReadByte();
                                length--;
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
