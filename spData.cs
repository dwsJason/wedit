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
        // optional alpha mask (color index 0, can be substituted)
        // This optional mask allows 16 color + mask on the GS
        public List<byte> m_mask = new List<byte>();

        public List<spRect> m_rects = new List<spRect>();

        public spPixels(int width, int height, List<byte> pixels)
        {
            m_width  = width;
            m_height = height;
            m_pixels = pixels;
        }
    }

    public class spAnimCommand
    {
        public enum cmd
        {
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

            None = -1
        };

        public int m_frameNo;   // Is a frame or command
        public int m_command;   // Animation command
        public int m_arg;       // Arguments

        public string GetArgString()
        {
            string result = "";

            switch (m_command)
            {
            case 0: //End
                break;
            case 1: //Loop
                break;
            case 2: //Goto
                result = String.Format("{0}", m_arg);
                break;
            case 3: //GotoSeq
                result = String.Format("{0}", m_arg);
                break;
            case 4: //Pause
                result = String.Format("{0}", m_arg);
                break;
            case 5: //SetRate
                result = String.Format("${0:X4}", m_arg);
                break;
            case 6: //SetSpeed
                result = String.Format("${0:X4}", m_arg);
                break;
            case 7: //MultiOp
                result = String.Format("{0}", m_arg);
                break;
            case 8: //Delete
                break;
            case 9: //SetFlag
                result = String.Format("{0}", m_arg);
                break;
            case 10: //Sound
                result = String.Format("{0}", m_arg);
                break;
            case 11: //HFlip
                break;
            case 12: //VFlip
                break;
            case 13: //Nop
                break;
            case 14: //Process
                result = String.Format("{0}", m_arg);
                break;
            case 15: //ClearFlag
                result = String.Format("{0}", m_arg);
                break;
            case 16:  //GotoLast
                break;
            case 17:  //Blank
                break;
            case 18: //RndPause
                {
                    int min = m_arg & 0xFF;
                    int max = (m_arg >> 8) & 0xFF;
                    result = String.Format("{0},{1}", min, max);
                }
                break;
            case 19: //SetHFlip
                break;
            case 20: //ClrHFlip
                break;
            case 21: //SetVFlip
                break;
            case 22: //ClrVFlip
                break;
            case 23: //HVFlip
                break;
            case 24: //SetHVFlip
                break;
            case 25: //ClrHVFlip
                break;
            case 26: //ExtSprite
                result = String.Format("{0}", m_arg);
                break;
            case 27: //Brk
                break;
            case 28: //OnBrk
                result = String.Format("{0}", m_arg);
                break;
            case 29: //DynSound
                result = String.Format("{0}", m_arg);
                break;

            default:
                result = "?";
                break;
            }

            return result;
        }
    }

    public class spAnim
    {
        public string m_name;
        public List<spAnimCommand> m_commands = new List<spAnimCommand>();
    }

    // WEDIT SP File Serializer / Class
    public class spData
    {
        List<spPalette> m_palettes = new List<spPalette>();
        List<spPixels>  m_frames   = new List<spPixels>();
        List<spAnim>    m_anims    = new List<spAnim>();

        public int NumFrames()
        {
            return m_frames.Count;
        }

        public int NumAnims()
        {
            return m_anims.Count;
        }

        // Create an "empty" spData
        public spData()
        {
            // Add a default palette
            spPalette pal = new spPalette();

            // Default to a DB16 Palette
            pal.colors.Add( Color.FromArgb(255,0x14,0x0C,0x1C) );
            pal.colors.Add( Color.FromArgb(255,0x44,0x24,0x34) );
            pal.colors.Add( Color.FromArgb(255,0x30,0x34,0x6D) );
            pal.colors.Add( Color.FromArgb(255,0x4E,0x4A,0x4E) );
            pal.colors.Add( Color.FromArgb(255,0x85,0x4C,0x30) );
            pal.colors.Add( Color.FromArgb(255,0x34,0x65,0x24) );
            pal.colors.Add( Color.FromArgb(255,0xD0,0x46,0x48) );
            pal.colors.Add( Color.FromArgb(255,0x75,0x71,0x61) );
            pal.colors.Add( Color.FromArgb(255,0x59,0x7D,0xCE) );
            pal.colors.Add( Color.FromArgb(255,0xD2,0x7D,0x2C) );
            pal.colors.Add( Color.FromArgb(255,0x85,0x95,0xA1) );
            pal.colors.Add( Color.FromArgb(255,0x6D,0xAA,0x2C) );
            pal.colors.Add( Color.FromArgb(255,0xD2,0xAA,0x99) );
            pal.colors.Add( Color.FromArgb(255,0x6D,0xC2,0xCA) );
            pal.colors.Add( Color.FromArgb(255,0xDA,0xD4,0x5E) );
            pal.colors.Add( Color.FromArgb(255,0xDE,0xEE,0xD6) );

            m_palettes.Add( pal );

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
                            //Debug.Assert(0 == ub);

                            ub = b.ReadByte(); // Discard
                            //Debug.Assert(0 == ub);

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

                       // SPRITE PIXEL MASK
                    case 0x4D505353: // 'SSPM'
                        {
                            // 2 byte header
                            // w frameNo
                            // mask pixels

                            UInt16 frameNo = b.ReadUInt16();
                            length -= 2;

                            Console.WriteLine("     Mask frameNo {0}",
                                              frameNo
                                              );

                            // Read in the pixels
                            List<byte> mask = new List<byte>();

                            // pixel data (w x h)
                            while (0 != length)
                            {
                                mask.Add(b.ReadByte());
                                length--;
                            }

                            // Create a pixel frame
                            // if the frame doesn't exist, the file
                            // is broken, so let it crash
                            spPixels pixelFrame = m_frames[ frameNo ];
                            pixelFrame.m_mask = mask;
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

                            UInt16 len2 = b.ReadUInt16();
                            Debug.Assert(length == len2);
                            length-=2;

                            // Number of frames in the animation
                            // (I think we don't need this, as it
                            //  can be computed later)
                            //if (v1 > 0x100)
                            //{
                                byte frameCount = b.ReadByte();
                                length--;
                                byte frameCount2 = b.ReadByte();
                                length--;
                            //}


                            animNo++;
                            int lineNo = 1;
                            UInt16 u16;
                            byte u8;

                            spAnim anim = new spAnim();

                            anim.m_name = "";

                            while (0 != length)
                            {
                                spAnimCommand animCmd = new spAnimCommand();
                                animCmd.m_frameNo =  0;
                                animCmd.m_command = (int)spAnimCommand.cmd.None;
                                animCmd.m_arg     =  0;

                                byte cmd = b.ReadByte();
                                length--;

                                if ((0xFF == cmd) && (0 != length))
                                {
                                    animCmd.m_frameNo = -1;
                                    cmd = b.ReadByte();
                                    length--;

                                    switch (cmd)
                                    {
                                    case 0:
                                        Console.WriteLine("({0})  End", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.End;
                                        break;
                                    case 1:
                                        Console.WriteLine("({0})  Loop",lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Loop;
                                        break;
                                    case 2:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  Goto {1}", lineNo, u16);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Goto;
                                        animCmd.m_arg = u16;
                                        break;
                                    case 3:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  GotoSeq {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.GotoSeq;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 4:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  Pause {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Pause;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 5:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  SetRate ${1:X4}", lineNo, u16);
                                        animCmd.m_command = (int)spAnimCommand.cmd.SetRate;
                                        animCmd.m_arg = u16;
                                        break;
                                    case 6:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  SetSpeed ${1:X4}", lineNo, u16);
                                        animCmd.m_command = (int)spAnimCommand.cmd.SetSpeed;
                                        animCmd.m_arg = u16;
                                        break;
                                    case 7:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  MultiOp {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.MultiOp;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 8:
                                        Console.WriteLine("({0})  Delete", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Delete;
                                        break;
                                    case 9:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  SetFlag {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.SetFlag;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 10:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  Sound {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Sound;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 11:
                                        Console.WriteLine("({0})  HFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.HFlip;
                                        break;
                                    case 12:
                                        Console.WriteLine("({0})  VFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.VFlip;
                                        break;
                                    case 13:
                                        Console.WriteLine("({0})  Nop", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Nop;
                                        break;
                                    case 14:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  Process {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Process;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 15:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  ClearFlag {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.ClearFlag;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 16:
                                        Console.WriteLine("({0})  GotoLast", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.GotoLast;
                                        break;
                                    case 17:
                                        Console.WriteLine("({0})  Blank", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Blank;
                                        break;
                                    case 18:
                                        {
                                            byte min = b.ReadByte(); length--;
                                            byte max = b.ReadByte(); length--;
                                            Console.WriteLine("({0})  RndPause {1},{2}", lineNo, min, max);
                                            animCmd.m_command = (int)spAnimCommand.cmd.RndPause;
                                            animCmd.m_arg   = max;
                                            animCmd.m_arg <<= 8;
                                            animCmd.m_arg  |= min;
                                        }
                                        break;
                                    case 19:
                                        Console.WriteLine("({0})  Set HFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.SetHFlip;
                                        break;
                                    case 20:
                                        Console.WriteLine("({0})  Clr HFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.ClrHFlip;
                                        break;
                                    case 21:
                                        Console.WriteLine("({0})  Set VFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.SetVFlip;
                                        break;
                                    case 22:
                                        Console.WriteLine("({0})  Clr VFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.ClrVFlip;
                                        break;
                                    case 23:
                                        Console.WriteLine("({0})  HVFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.HVFlip;
                                        break;
                                    case 24:
                                        Console.WriteLine("({0})  Set HVFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.SetHVFlip;
                                        break;
                                    case 25:
                                        Console.WriteLine("({0})  Clr HVFlip", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.ClrHVFlip;
                                        break;
                                    case 26:
                                        u16 = b.ReadUInt16(); length-=2;
                                        Console.WriteLine("({0})  ExtSprite {1}", lineNo, u16);
                                        animCmd.m_command = (int)spAnimCommand.cmd.ExtSprite;
                                        animCmd.m_arg = u16;
                                        break;
                                    case 27:
                                        Console.WriteLine("({0})  Brk", lineNo);
                                        animCmd.m_command = (int)spAnimCommand.cmd.Brk;
                                        break;
                                    case 28:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  OnBrk {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.OnBrk;
                                        animCmd.m_arg = u8;
                                        break;
                                    case 29:
                                        u8 = b.ReadByte(); length--;
                                        Console.WriteLine("({0})  DynSound {1}", lineNo, u8);
                                        animCmd.m_command = (int)spAnimCommand.cmd.DynSound;
                                        animCmd.m_arg = u8;
                                        break;
                                    default:
                                        Console.WriteLine("({0})  UNKNOWN CMD {1}, len={2}", lineNo, cmd, length);
                                        Debug.Assert(false);
                                        break;
                                    }

                                    anim.m_commands.Add(animCmd);
                                }
                                else
                                {
                                    animCmd.m_frameNo = cmd;
                                    anim.m_commands.Add(animCmd);
                                    Console.WriteLine("({0})  Sprite {1}",lineNo,cmd);
                                }
                                lineNo++;
                            }

                            m_anims.Add(anim);
                        }
                        break;

                        // Animation Name
                    case 0x4E4E4153:  // 'SANN'
                        {
                            UInt16 animNom = b.ReadUInt16();

                            spAnim anim = m_anims[ animNom ];

                            string name = "";

                            length -= 2;

                            while (length > 0)
                            {
                                length--;
                                byte c = b.ReadByte();
                                name += Convert.ToChar(c);
                            }

                            anim.m_name = name;
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

        // Take an input bitmap, convret into an spPixels
        // object, and add it to the frame list 
        public void AddFrame(Color trans, Bitmap bmp)
        {
            if (null == bmp)
                return;

            List<byte> pixels = new List<byte>();
            List<byte> mask   = new List<byte>();

            for (int y = 0; y < bmp.Height; ++y)
            {
                for (int x = 0; x < bmp.Width; ++x)
                {
                    Color px = bmp.GetPixel(x,y);
                    byte colorIndex = GetPaletteIndex(px);
                    byte maskIndex = 0xF;

                    // If the pixel is Transparent
                    // it's index 0
                    if (trans == px)
                    {
                        colorIndex = 0;
                        maskIndex = 0;
                    }

                    if (0 == (x & 1))
                    {
                        // Even
                        colorIndex <<= 4;
                        maskIndex  <<= 4;

                        pixels.Add(colorIndex);
                        mask.Add(maskIndex);
                    }
                    else
                    {
                        // Odd
                        colorIndex &= 0xF;
                        maskIndex  &= 0xF;
                        pixels[ pixels.Count - 1 ] |= colorIndex;
                        mask[ mask.Count - 1 ] |= maskIndex;
                    }
                }
            }

            spPixels pixelFrame = new spPixels((bmp.Width+1)>>1,bmp.Height, pixels);

            pixelFrame.m_offset_x = pixelFrame.m_width;
            pixelFrame.m_offset_y = pixelFrame.m_height>>1;
            pixelFrame.m_mask = mask;

            m_frames.Add( pixelFrame );
        }

        //
        //  Get the Closest Matching palette index
        //
        byte GetPaletteIndex(Color p)
        {
            byte result_index = 0;

            if (m_palettes.Count > 0)
            {
                spPalette pal = m_palettes[0];
                List<float> dist = new List<float>();

                for (int idx = 0; idx < pal.colors.Count; ++idx)
                {
                    float delta = ColorDelta(pal.colors[idx], p);
                    dist.Add(delta);

                    // Make sure the result_index is the one
                    // with the least amount of error
                    if (dist[idx] < dist[result_index])
                    {
                        result_index = (byte)idx;
                    }
                }
            }

            return result_index;
        }

        float ColorDelta(Color c0, Color c1)
        {
            //  Y=0.2126R+0.7152G+0.0722B
            float r = (c0.R-c1.R);
            r = r * r;
            r *= 0.2126f;

            float g = (c0.G-c1.G);
            g = g * g;
            g *= 0.7152f;

            float b = (c0.B-c1.B);
            b = b * b;
            b *= 0.0722f;

            return r + g + b;
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

                if (pix.m_mask.Count > 0)
                {
                    // Use a 16 color sprite + mask
                    for (int y = 0; y < height; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            byte px = pix.m_pixels[(y * pix.m_width) + (x>>1)];
                            byte mx = pix.m_mask[(y * pix.m_width) + (x>>1)];
                            if (0 == (x&1))
                            {
                                px >>= 4;
                                mx >>= 4;
                            }

                            px &= 0xF;
                            mx &= 0xF;

                            Color c = pal.colors[px];
                            
                            if (0==mx)
                            {
                                c = Color.FromArgb(0, c.R, c.G, c.B);
                            }

                            bmp.SetPixel(x,y,c);
                        }
                    }

                }
                else
                {
                    // 15 Color + Color Index 0 is transparent
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

        public spAnim GetAnim(int animNo)
        {
            spAnim anim = null;

            if ((animNo >= 0) && (animNo < m_anims.Count))
            {
                anim = m_anims[ animNo ];
            }

            return anim;
        }

        public void AddAnim()
        {
            spAnim anim = new spAnim();
            anim.m_name = "New Anim";
            m_anims.Add( anim );
        }

        public void ImportPalette(string pathName)
        {
            Console.WriteLine("import: {0}", pathName);

            using (BinaryReader b = new BinaryReader(
                File.Open(pathName, FileMode.Open)))
            {
                // Use BaseStream.
                long fileLength = b.BaseStream.Length;

                spPalette pal = new spPalette();
                byte red;
                byte green;
                byte blue;
                byte alpha;

                switch (fileLength)
                {
                    // 48 byte palettes
                case 48:
                case 768:
                    // Read in first 16 colors, R8G8B8, ProMotion .PAL files
                    for (int idx = 0; idx < 16; ++idx)
                    {
                        red   = b.ReadByte();
                        green = b.ReadByte();
                        blue  = b.ReadByte();

                        pal.colors.Add( Color.FromArgb(255,red,green,blue) );
                    }

                    break;

                     // 64 byte palettes
                case 64:
                case 1024:
                    // Read in first 16 colors, R8G8B8A8
                    for (int idx = 0; idx < 16; ++idx)
                    {
                        red   = b.ReadByte();
                        green = b.ReadByte();
                        blue  = b.ReadByte();
                        alpha = b.ReadByte();

                        pal.colors.Add( Color.FromArgb(alpha,red,green,blue) );
                    }

                    break;
                default:
                    Console.WriteLine("Invalid PAL File");
                    break;
                }

                // If it looks like a good palette, replace
                // the existing one
                if (16 == pal.colors.Count)
                {
                    if (m_palettes.Count > 0)
                    {
                        m_palettes[0] = pal;
                    }
                    else
                    {
                        m_palettes.Add( pal );
                    }
                }

            }
        }

        public void Save(string pathName)
        {
            Console.WriteLine("save: {0}", pathName);
            uint length = 0;

            using (BinaryWriter b = new BinaryWriter(
                File.Open(pathName, FileMode.Create)))
            {
                // File Header
                b.Write((uint)0x41454853); // 'SHEA'
                b.Write((ushort)4); // hard coded header length 4 bytes
                b.Write((ushort)0x0100); // version 0
                b.Write((ushort)0x0101); // version 1

                // Palettes
                //$$JGA for now Hardcoded to a single palette
                b.Write((uint)0x45505353);      // 'SSPE'
                b.Write((ushort)(1 + 48 + 1));  // length

                spPalette pal = m_palettes[0];
                b.Write((byte) 0);  // palette #0

                for (int idx = 0; idx < 16; ++idx)
                {
                    b.Write((byte)pal.colors[idx].R);
                    b.Write((byte)pal.colors[idx].G);
                    b.Write((byte)pal.colors[idx].B);
                }

                b.Write((byte)0xFF); // palette #255, done

                // Pixels
                for (int frameNo = 0; frameNo < m_frames.Count; ++frameNo)
                {
                    // For Each Frame
                    spPixels pixelFrame = m_frames[ frameNo ];

                    int width  = pixelFrame.m_width;
                    int height = pixelFrame.m_height;

                    b.Write((uint)0x50505353);  // 'SSPP'
                    length = 10;                // header
                    length += (uint)(width * height); // pixel byte length
                    b.Write((ushort) length);   // length

                    b.Write((ushort) 0);    // discards

                    b.Write((byte)pixelFrame.m_width);  // width
                    b.Write((byte)pixelFrame.m_height); // height
                    
                    b.Write((byte)0); // discard

                    b.Write((short)pixelFrame.m_offset_x); // offset X
                    b.Write((short)pixelFrame.m_offset_y); // offset Y

                    b.Write((byte)0); // discard

                    // write the pixels
                    b.Write(pixelFrame.m_pixels.ToArray());
                }

                // Masks
                for (int frameNo = 0; frameNo < m_frames.Count; ++frameNo)
                {
                    // For Each Frame
                    spPixels pixelFrame = m_frames[ frameNo ];

                    if (0 == pixelFrame.m_mask.Count)
                        continue;

                    int width  = pixelFrame.m_width;
                    int height = pixelFrame.m_height;

                    b.Write((uint)0x4D505353);  // 'SSPM'
                    length = 2;                 // header size
                    length += (uint)(width * height); // pixel byte length
                    b.Write((ushort) length);   // length

                    // write the frame #
                    b.Write((ushort)frameNo);

                    // write the pixels
                    b.Write(pixelFrame.m_mask.ToArray());
                }

                // Animations
                for (int animNo = 0; animNo < m_anims.Count; ++animNo)
                {
                    spAnim anim = m_anims[ animNo ];

                    // Export Wedit compatible Anim
                    b.Write((uint)0x4D4E4153); // 'SANM'
                    long position = b.Seek(0, System.IO.SeekOrigin.Current);  // Get current position in the Stream
                    b.Write((uint)0); // placeholder for len16, len16

                    // This for sanity?
                    b.Write((ushort)anim.m_commands.Count); // number of lines

                    for (int cmdIdx = 0; cmdIdx < anim.m_commands.Count; ++cmdIdx)
                    {
                        // Write out the commands
                        spAnimCommand animCmd = anim.m_commands[ cmdIdx ];

                        int lineNo = cmdIdx + 1;

                        if (animCmd.m_frameNo < 0)
                        {
                            byte u8;
                            ushort u16;
                            // It's a command
                            b.Write((byte)0xFF); // command
                            b.Write((byte)animCmd.m_command);

                            switch ((spAnimCommand.cmd)animCmd.m_command)
                            {
                            case spAnimCommand.cmd.End:
                                Console.WriteLine("({0})  End", lineNo);
                                break;
                            case spAnimCommand.cmd.Loop:
                                Console.WriteLine("({0})  Loop",lineNo);
                                break;
                            case spAnimCommand.cmd.Goto:
                                u16 = (ushort) animCmd.m_arg;
                                Console.WriteLine("({0})  Goto {1}", lineNo, u16);
                                b.Write((ushort)u16);
                                break;
                            case spAnimCommand.cmd.GotoSeq:
                                u8 = (byte) animCmd.m_arg;
                                Console.WriteLine("({0})  GotoSeq {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.Pause:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  Pause {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.SetRate:
                                u16 = (ushort)animCmd.m_arg;
                                Console.WriteLine("({0})  SetRate ${1:X4}", lineNo, u16);
                                b.Write((ushort)u16);
                                break;
                            case spAnimCommand.cmd.SetSpeed:
                                u16 = (ushort)animCmd.m_arg;
                                Console.WriteLine("({0})  SetSpeed ${1:X4}", lineNo, u16);
                                b.Write((ushort)u16);
                                break;
                            case spAnimCommand.cmd.MultiOp:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  MultiOp {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.Delete:
                                Console.WriteLine("({0})  Delete", lineNo);
                                break;
                            case spAnimCommand.cmd.SetFlag:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  SetFlag {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.Sound:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  Sound {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.HFlip:
                                Console.WriteLine("({0})  HFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.VFlip:
                                Console.WriteLine("({0})  VFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.Nop:
                                Console.WriteLine("({0})  Nop", lineNo);
                                break;
                            case spAnimCommand.cmd.Process:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  Process {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.ClearFlag:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  ClearFlag {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.GotoLast:
                                Console.WriteLine("({0})  GotoLast", lineNo);
                                break;
                            case spAnimCommand.cmd.Blank:
                                Console.WriteLine("({0})  Blank", lineNo);
                                break;
                            case spAnimCommand.cmd.RndPause:
                                {
                                    u16 = (ushort)animCmd.m_arg;
                                    byte min = (byte)(u16 & 0xFF);
                                    byte max = (byte)(u16 >> 8);
                                    Console.WriteLine("({0})  RndPause {1},{2}", lineNo, min, max);
                                    b.Write((ushort)u16);
                                }
                                break;
                            case spAnimCommand.cmd.SetHFlip:
                                Console.WriteLine("({0})  Set HFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.ClrHFlip:
                                Console.WriteLine("({0})  Clr HFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.SetVFlip:
                                Console.WriteLine("({0})  Set VFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.ClrVFlip:
                                Console.WriteLine("({0})  Clr VFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.HVFlip:
                                Console.WriteLine("({0})  HVFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.SetHVFlip:
                                Console.WriteLine("({0})  Set HVFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.ClrHVFlip:
                                Console.WriteLine("({0})  Clr HVFlip", lineNo);
                                break;
                            case spAnimCommand.cmd.ExtSprite:
                                u16 = (ushort)animCmd.m_arg;
                                Console.WriteLine("({0})  ExtSprite {1}", lineNo, u16);
                                b.Write((ushort)u16);
                                break;
                            case spAnimCommand.cmd.Brk:
                                Console.WriteLine("({0})  Brk", lineNo);
                                break;
                            case spAnimCommand.cmd.OnBrk:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  OnBrk {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;
                            case spAnimCommand.cmd.DynSound:
                                u8 = (byte)animCmd.m_arg;
                                Console.WriteLine("({0})  DynSound {1}", lineNo, u8);
                                b.Write((byte)u8);
                                break;

                            default:
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("({0})  Frame {1}", lineNo, (byte)animCmd.m_frameNo);
                            // It's an 8 bit frameNo
                            b.Write((byte)animCmd.m_frameNo);
                        }
                    }

                    // Back patch the length
                    long lastpos = b.Seek(0, System.IO.SeekOrigin.Current);  // Get current position in the Stream
                    b.Seek((int)position, System.IO.SeekOrigin.Begin);

                    ushort delta = (ushort)(lastpos - position);
                    delta -= 2;

                    b.Write((ushort)delta);
                    b.Write((ushort)delta);

                    b.Seek((int)lastpos, System.IO.SeekOrigin.Begin);

                    // Export Extra Anim Data that I want

                    if (anim.m_name.Count() > 0)
                    {
                        // Save out the extra anim data
                        b.Write((uint)0x4E4E4153); // 'SANN'

                        length = (ushort)anim.m_name.Count();
                        length+=2;

                        b.Write((ushort)length);

                        b.Write((ushort)animNo);

                        for (int idx = 0; idx < anim.m_name.Count(); ++idx)
                        {
                            b.Write((byte)anim.m_name[idx]);
                        }
                    }

                }

                // Terminate File
                b.Write((uint)0xFFFFFFFF);
            }
        }

        public void SetAnimName(int animNo, string animName)
        {
            if ((animNo >= 0) &&
                (animNo < m_anims.Count))
            {
                m_anims[ animNo ].m_name = animName;
            }
        }

        //----------------------------------------------------------------------

        public class CompiledData
        {
            // Use Dictionary as a map.
            public Dictionary<byte, List<int>>   byte_map = new Dictionary<byte, List<int>>();
            public Dictionary<ushort, List<int>> short_map = new Dictionary<ushort, List<int>>();

            // ... Add some keys and values.
            //map.Add("cat", "orange");
            //map.Add("dog", "brown");

            // ... Loop over the map.
            //foreach (var pair in map)
            //{
            //    string key = pair.Key;
            //    string value = pair.Value;
            //    Console.WriteLine(key + "/" + value);
            //}

            // ... Get value at a known key.
            //string result = map["cat"];
            //Console.WriteLine(result);

            // ... Use TryGetValue to safely look up a value in the map.
            //string mapValue;
            //if (map.TryGetValue("dog", out mapValue))
            //{
            //    Console.WriteLine(mapValue);
            //}

            public CompiledData(ref List<byte> source_canvas,ref List<byte> dest_canvas)
            {
                // hard-coded
                // size of a IIgs SHR Display Buffer
                if ((source_canvas.Count == dest_canvas.Count) &&
                    (source_canvas.Count == (160*200)))
                {
                    // This could miss the last byte in the buffer
                    // but we don't care, because our sprite should never be there
                    // and it helps simple the logic

                    for (int index = 0; index < (source_canvas.Count-1); ++index)
                    {
                        // Nothing to compile if the data is identical
                        if (source_canvas[ index ] == dest_canvas[ index ])
                            continue;

                        // Definitely we have a byte, but could it be a word?
                        if (source_canvas[ index + 1] == dest_canvas[ index + 1 ])
                        {
                            // We have a byte!
                            // Record the change in the byte_map

                            byte token = dest_canvas[ index ];

                            List<int> offsets = new List<int>();

                            if (byte_map.TryGetValue(token, out offsets))
                            {
                                offsets.Add(index);
                                byte_map.Remove(token);
                            }
                            else
                            {
                                offsets = new List<int>();
                            }

                            offsets.Add(index);
                            byte_map.Add(token, offsets);

                            index+=1;           // save time by skipping this one

                        }
                        else
                        {
                            // We have a word!
                            // Record the change in the word_map
                            ushort token = dest_canvas[ index+1 ];
                            token <<= 8;
                            token |= dest_canvas[ index ];

                            List<int> offsets = new List<int>();

                            if (short_map.TryGetValue(token, out offsets))
                            {
                                offsets.Add(index);
                                short_map.Remove(token);
                            }
                            else
                            {
                                offsets = new List<int>();
                            }

                            offsets.Add(index);
                            short_map.Add(token, offsets);

                            index+=1; // skip processed byte
                        }
                    }
                }
            }

        }


        //
        // Hard Coded SHR Display Buffer, take image and
        // change it into a collision mask
        // 
        // color index 0x1 is the background, and color index 6
        // is our colliding color
        //
        public void dxGenPixelCollision(ref List<byte> canvas)
        {
            if (canvas.Count != (160*200))
                return;

            int top_y = -1;
            int bottom_y = -1;
            int last_y = -1;
            for (int y = 0; y < 200; ++y)
            {
                int buffer_index = y * 160;
                int left_x = -1;
                int right_x = -1;

                // find left
                for (int x = 0; x < 160;  ++x)
                {
                    if (0x11 != canvas[ buffer_index + x ])
                    {
                        left_x = x;
                        break;
                    }
                }

                // Only find the right, if there's a left
                if (left_x >= 0)
                {
                    if (top_y < 0)  top_y = y;
                    last_y = y;     // last y line with edges

                    for (int x = 159; x >= left_x; --x)
                    {
                        if (0x11 != canvas[ buffer_index + x ])
                        {
                            right_x = x;
                            break;
                        }
                    }

                    for (int x = left_x; x <= right_x; ++x)
                    {
                        byte pixel = 0x11;

                        // Top line, let's fill the whole thing
                        if (y == top_y)
                        {
                            pixel = 0x66;
                        }

                        // Bottom Line, fill the whole thing
                        if (y == bottom_y)
                        {
                            pixel = 0x66;
                        }

                        if (left_x == x)
                        {
                            pixel &= 0x0F;
                            pixel |= 0x60;
                        }
                        if (right_x == x)
                        {
                            pixel &= 0xF0;
                            pixel |= 0x06;
                        }


                        canvas[ buffer_index + x ] = pixel;
                    }
                }
                else if (bottom_y < 0)
                {
                    // there's no edge, but was there an edge before?
                    if ((last_y >= 0) && (last_y == (y-1)))
                    {
                        bottom_y = last_y;  // signal bottom line
                        y = last_y - 1;     // get for loop to go back in time
                    }
                }
            }
        }


        //
        // Hard Coded to plot into an array, the size of a IIgs
        // SHR Display Buffer
        //
        public void dxPlot(ref List<byte> canvas, spPixels sprite, int pos_x, int pos_y)
        {
            if (canvas.Count != (160*200))
                return;


            // Render a sprite, as if it was being rendered to the SHR video
            // buffer on a IIgs (yes, this is weird)

            // In pixels
            int canvas_width = 320;
            //int canvas_height = 200;

            spPixels pix = sprite;

            // Sprite pixels
            int width  = pix.m_width * 2;
            int height = pix.m_height;

            int canvas_x = pos_x - pix.m_offset_x;
            int canvas_y = pos_y - pix.m_offset_y;

            if (pix.m_mask.Count > 0)
            {
                // Use a 16 color sprite + mask
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        byte px = pix.m_pixels[(y * pix.m_width) + (x>>1)];
                        byte mx = pix.m_mask[(y * pix.m_width) + (x>>1)];
                        mx ^= 0xFF;
                        if (0xFF == mx)
                            continue;

                        if (0 == (x & 1))
                        {
                            px >>= 4;
                            mx >>= 4;
                        }

                        px &= 0xF;
                        mx &= 0xF;
                        mx |= 0xF0;
                         
                        //------------------------------------------------------
                        // plot the thing
                        int canvas_index = (canvas_y + y) * canvas_width / 2;
                        canvas_index += ((canvas_x + x)/2);

                        if (0 == ((canvas_x + x) & 0x1))
                        {
                            // it's an even position
                            px <<= 4;
                            mx <<= 4;
                            mx |= 0xF;
                        }

                        if ((canvas_index >= 0) &&
                            (canvas_index < canvas.Count))
                        {
                            if (0x11 == canvas[canvas_index]) {
                                canvas[canvas_index] = 0;
                            }
                            canvas[canvas_index] &= mx;
                            canvas[canvas_index] |= px;
                        }
                    }
                }

            }
            else
            {
                // 15 Color + Color Index 0 is transparent
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
                        //------------------------------------------------------
                        // plot the thing
                        int canvas_index = (canvas_y + y) * canvas_width / 2;
                        canvas_index += ((canvas_x + x)/2);

                        if (0 == ((canvas_x + x) & 0x1))
                        {
                            // it's an even position
                            px <<= 4;
                        }

                        if ((canvas_index >= 0) &&
                            (canvas_index < canvas.Count))
                        {
                            canvas[ canvas_index ] |= px;
                        }
                    }
                }
            }



        }

        //----------------------------------------------------------------------
        //
        // Save a sprite Sheet, in dxSprite Format
        //
        public void ExportDxSprite(string pathName)
        {
            String fileName = Path.GetFileName( pathName );
            String baseName = Path.GetFileNameWithoutExtension( pathName );
            String dirName = Path.GetDirectoryName(pathName);

            // I want everything Mr Sprite Export is going to give me
            ExportMrSprite(dirName + baseName + ".gif");

            // I also want more...
            // I want to export source code for compiled sprites specifically
            // for the the Journey Game
            // which means they will include a pre-compiled "Erase"
            // and they will include transition codes for movement / and 
            // animations (where only the pixels that change, get changed)
            // dx == delta, where the name comes from

            // loop through, and grab a C1 of each Frame
            // then generate draws for each C1
            //    Evens
            //    -- 0- +-
            //    -0 00 +0
            //    -+ 0+ ++

            //    Odds
            //    -- 0- +-
            //    -0 00 +0
            //    -+ 0+ ++


            //------------------------------------------------------------------
            //   00 Full Draw
            //   00 Full Erase
            //   00 Collision ring
            //   00 Collision Erase
            //   -- 0- +-
            //   -0    +0
            //   -+ 0+ ++
            //------------------------------------------------------------------


            // The delta to other frames can come later

            // spPixels, texel format is already the same as a GS!, that seems
            // random to me, but maybe SP did that on purpose

            List<byte> source_canvas = new List<byte>( new byte[160*200] );
            List<byte> dest_canvas   = new List<byte>( new byte[160*200] );

            // a List of compiled results
            List<CompiledData> data = new List<CompiledData>();

            for (int frame_index = 0; frame_index < m_frames.Count; ++frame_index)
            {
                for (int even_odd = 0; even_odd < 2; ++even_odd)
                {
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 160+even_odd, 100);

                    // Full Frame
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));
                    // Full Erase
                    data.Add( new CompiledData( ref dest_canvas, ref source_canvas));

                    //--------------------------------------------------------------

                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 160+even_odd, 100);

                    dxGenPixelCollision(ref dest_canvas);

                    // Collision Data
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    // Collision Erase
                    data.Add( new CompiledData( ref dest_canvas, ref source_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 159+even_odd, 99);

                    // dx -1 dy = -1
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 160+even_odd, 99);

                    // dx 0 dy = -1
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 161+even_odd, 99);

                    // dx +1 dy = -1
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 159+even_odd, 100);

                    // dx = -1, dy = 0
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 161+even_odd, 100);

                    // dx = +1, dy = 0
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 159+even_odd, 101);

                    // dx = -1, dy = +1
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 160+even_odd, 101);

                    // dx = 0, dy = +1
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));

                    //--------------------------------------------------------------
                    // Blank Canvas
                    for (int idx = 0; idx < source_canvas.Count; ++idx) {
                        source_canvas[ idx ] = 0x11;
                        dest_canvas[ idx ] = 0x11;
                    }

                    dxPlot(ref source_canvas, m_frames[ frame_index ], 160+even_odd, 100);
                    dxPlot(ref dest_canvas, m_frames[ frame_index ], 161+even_odd, 101);

                    // dx = +1, dy = +1
                    data.Add( new CompiledData( ref source_canvas, ref dest_canvas ));
                    //--------------------------------------------------------------

                }
            }

            Console.WriteLine(" Export dxSprite Support Files");
            Console.WriteLine(" Export {0}", pathName);

            try
            {
                System.IO.TextWriter t = new StreamWriter(pathName);

                for (int dataidx = 0; dataidx < data.Count; ++dataidx)
                {
                    CompiledData compiled_data = data[ dataidx ];

                    //--------------------------------------------------------------
                    // How many clocks

                    int cycles = 6;  // +6 RTS
                    int size_bytes = 1; // +1 RTS

                    if (compiled_data.byte_map.Count > 0)
                    {
                        cycles += 6; // +3 SEP, +3 REP
                        size_bytes += 4; // +2 SEP, +2 REP

                        foreach (var pair in compiled_data.byte_map)
                        {
                            if (0x00 == pair.Key)
                            {
                                // STZ Case
                                List<int> offsets = pair.Value;
                                cycles += (offsets.Count*5); // +5 STZ |$1234,x
                                size_bytes += (offsets.Count*3); // +3 STZ |$1234,x
                            }
                            else if (0x11 == pair.Key)
                            {
                                // ERASE Case
                                List<int> offsets = pair.Value;
                                cycles += (offsets.Count*5); // +5 LDA >$011234,x
                                cycles += (offsets.Count*5); // +5 STA |$1234,x
                                size_bytes += (offsets.Count * 7);  //+4 +3
                            }
                            else
                            {
                                cycles += 2;    // +2 LDA #$12
                                size_bytes += 2; // +2 LDA #$12
                                List<int> offsets = pair.Value;
                                cycles += (offsets.Count*5); // +5 STA |$1234,x
                                size_bytes += (offsets.Count *3); // +3 STA |$1234,x
                            }
                        }
                    }

                    foreach (var pair in compiled_data.short_map)
                    {
                        if (0x0000 == pair.Key)
                        {
                            List<int> offsets = pair.Value;
                            cycles += (offsets.Count*6); // +6 STZ |$1234,x
                            size_bytes += (offsets.Count*3); // +3 STZ |$1234,x
                        }
                        else if (0x1111 == pair.Key)
                        {
                            List<int> offsets = pair.Value;
                            cycles += (offsets.Count*6); // +6 LDA >$011234,x
                            cycles += (offsets.Count*6); // +6 STA |$1234,x
                            size_bytes += (offsets.Count * 7);  //+4 +3
                        }
                        else
                        {
                            cycles += 3;  // +3 LDA #$1234
                            size_bytes += 3; // +3 LDA #$1234
                            List<int> offsets = pair.Value;
                            cycles += (offsets.Count*6); // +6 STA |$1234,x
                            size_bytes += (offsets.Count *3); // +3 STA |$1234,x
                        }
                    }

                    //------------------------------------------------------------------
                    //   00 Full Draw
                    //   00 Full Erase
                    //   00 Collision ring
                    //   00 Collision Erase
                    //   -- 0- +-
                    //   -0    +0
                    //   -+ 0+ ++
                    //------------------------------------------------------------------

                    int frame_no = dataidx / 24;
                    int even_odd = dataidx / 12;
                    t.WriteLine(String.Format("data_{0}_{1}_{2}\t; cycles = {3}, scanlines = {4}, bytes={5}",
                                              frame_no, even_odd,
                                              dataidx % 12,
                                              cycles, cycles / 65,
                                              size_bytes ));

                    //--------------------------------------------------------------

                    // A9 - LDA #
                    // 9D - STA |NNNN,x ; 5/6
                    // C2 - REP #
                    // E2 - SEP #
                    // 60 - RTS, 6B - RTL (6)

                    if (compiled_data.byte_map.Count > 0) {

                        t.WriteLine("\tSEP\t#$20\t; mx=10   cyc=3");

                        foreach (var pair in compiled_data.byte_map)
                        {
                            if (0x00 == pair.Key)
                            {
                                List<int> offsets = pair.Value;

                                for (int offIdx = 0; offIdx < offsets.Count; ++offIdx)
                                {
                                    t.WriteLine("\tSTZ\t${0:x4},x\t; cyc=5", offsets[offIdx]);
                                }
                            }
                            else if (0x11 == pair.Key)
                            {
                                List<int> offsets = pair.Value;

                                for (int offIdx = 0; offIdx < offsets.Count; ++offIdx)
                                {
                                    t.WriteLine("\tLDA\t>$01{0:x4},x\t; cyc=5", offsets[offIdx]);
                                    t.WriteLine("\tSTA\t${0:x4},x\t; cyc=5", offsets[offIdx]);
                                }
                            }
                            else
                            {   
                                t.WriteLine("\tLDA\t#${0:x2}\t; cyc=2", pair.Key);
                                List<int> offsets = pair.Value;

                                for (int offIdx = 0; offIdx < offsets.Count; ++offIdx)
                                {
                                    t.WriteLine("\tSTA\t${0:x4},x\t; cyc=5", offsets[offIdx]);
                                }
                            }
                        }

                        t.WriteLine("\tREP\t#$31\t; mxc=000  cyc=3");
                    }


                    foreach (var pair in compiled_data.short_map)
                    {
                        if (0x0000 == pair.Key)
                        {
                            List<int> offsets = pair.Value;
                            for (int offIdx = 0; offIdx < offsets.Count; ++offIdx)
                            {
                                t.WriteLine("\tSTZ\t${0:x4},x\t; cyc=6", offsets[offIdx]);
                            }

                        }
                        else if (0x1111 == pair.Key)
                        {
                            List<int> offsets = pair.Value;
                            for (int offIdx = 0; offIdx < offsets.Count; ++offIdx)
                            {
                                t.WriteLine("\tLDA\t>$01{0:x4},x\t; cyc=6", offsets[offIdx]);
                                t.WriteLine("\tSTA\t${0:x4},x\t; cyc=6", offsets[offIdx]);
                            }
                        }
                        else
                        {
                            t.WriteLine("\tLDA\t#${0:x4}\t; cyc=3", pair.Key);
                            List<int> offsets = pair.Value;
                            for (int offIdx = 0; offIdx < offsets.Count; ++offIdx)
                            {
                                t.WriteLine("\tSTA\t${0:x4},x\t; cyc=6", offsets[offIdx]);
                            }
                        }
                    }

                    t.WriteLine("\tRTS\t\t; cyc=6");


                    t.WriteLine(";-----------------------------------------------");
                }

                t.Flush();
                t.Close();
                t = null;
            }
            catch (IOException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }


        }

        //----------------------------------------------------------------------
        //
        // Save a sprite Sheet, in Mr.Sprite Format
        //
        public void ExportMrSprite(string pathName)
        {
            // Mr.Sprite needs a GIF Sprite Sheet
            // we also need two colors that are not in the Sprite
            // A "Frame" color, and a "Transparent" color for the mask
            //
            // Also kick out a ".bat" file that you can use to invoke
            // Mr.Sprite
            // Also spit out an .asm file with the animation definitions
            //

            if (m_frames.Count > 0)
            {
                // we need the union of the frame bounds
                // for all the frames
                spPixels pix = m_frames[0];

                // To gather pixel accurate width, and height of each frame
                // (legacy wedit files will push dimensions out to a multiple of 8 or 16)
                // Even new files could be off by 1 on the width
                // For clipping we want to be perfect (so we aren't drawing something,
                // that you can't see)
                List<int> frames_width  = new List<int>();
                List<int> frames_height = new List<int>(); 

#if true   // shrink each sprite, depend on offsets

                int maxWidth  = pix.m_width;
                int maxHeight = pix.m_height;
                int bmpHeight = pix.m_height;

                for (int idx = 1; idx < m_frames.Count; ++idx)
                {
                    pix = m_frames[ idx ];

                    if (maxWidth < pix.m_width) maxWidth = pix.m_width;
                    if (maxHeight < pix.m_height) maxHeight = pix.m_height;

                    bmpHeight += pix.m_height;
                }

                // Basically the max frame extents
                int outWidth  = maxWidth * 2;
                int outHeight = maxHeight;

                // Dimensions of the output bitmap
                int bmpWidth = outWidth+2;
                bmpHeight += 4 * m_frames.Count;

                Bitmap outBmp = new Bitmap(bmpWidth, bmpHeight);
                
                // Create a Graphics interface, so we can draw
                // into the output using accelerated commands
                Graphics gr = Graphics.FromImage(outBmp);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                Color backColor = GetBackColor(m_palettes[0]);
                Color frameColor = GetFrameColor(m_palettes[0]);

                Pen penFrame = new Pen(frameColor);

                gr.Clear(backColor);

                int current_y = 0;

                // Draw Frames
                for(int idx = 0; idx < m_frames.Count; ++idx)
                {
                    pix = m_frames[ idx ];

                    int y = current_y;
                    int y2 = y + pix.m_height + 1;
                    int x = 0;
                    int x2 = x + 1 + (pix.m_width * 2);

                    gr.DrawLine(penFrame, x, y, x2, y);
                    gr.DrawLine(penFrame, x, y2, x2, y2);
                    gr.DrawLine(penFrame, x, y, x, y2);
                    gr.DrawLine(penFrame, x2, y, x2, y2);

                    Bitmap bmp = BitmapFromFrame(idx);

                    frames_width.Add(GetRealWidth(ref bmp));
                    frames_height.Add(GetRealHeight(ref bmp));

                    gr.DrawImage(bmp, 1,
                                 y + 1,
                                 bmp.Width, bmp.Height);

                    current_y += 4 + pix.m_height;
                }

                penFrame.Dispose();
                gr.Dispose();
#endif

#if false  // Unioned fixed Size Frames
                int minX = -pix.m_offset_x;
                int maxX = minX + (pix.m_width*2);
                int minY = -pix.m_offset_y;
                int maxY = minY + pix.m_height;

                for (int idx = 1; idx < m_frames.Count; ++idx)
                {
                    pix = m_frames[ idx ];

                    int mnX = -pix.m_offset_x;
                    int mxX = mnX + (pix.m_width*2);
                    int mnY = -pix.m_offset_y;
                    int mxY = mnY + pix.m_height;

                    if (mnX < minX) minX = mnX;
                    if (mxX > maxX) maxX = mxX;
                    if (mnY < minY) minY = mnY;
                    if (mxY > maxY) maxY = mxY;
                }

                // Basically the max frame extents
                int outWidth  = maxX - minX;
                int outHeight = maxY - minY;

                // Dimensions of the output bitmap
                int bmpWidth = outWidth+2;
                int bmpHeight = (outHeight+4)*m_frames.Count;

                Bitmap outBmp = new Bitmap(bmpWidth, bmpHeight);
                
                frames_width.Add(GetRealWidth(bmp));
                frames_height.Add(GetRealHeight(bmp));

                // Create a Graphics interface, so we can draw
                // into the output using accelerated commands
                Graphics gr = Graphics.FromImage(outBmp);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                Color backColor = GetBackColor(m_palettes[0]);
                Color frameColor = GetFrameColor(m_palettes[0]);

                Pen penFrame = new Pen(frameColor);

                gr.Clear(backColor);

                // Draw Frames
                for(int idx = 0; idx < m_frames.Count; ++idx)
                {
                    int y = (outHeight+4) * idx;
                    int y2 = y + outHeight + 1;

                    gr.DrawLine(penFrame, 0, y, bmpWidth, y);
                    gr.DrawLine(penFrame, 0, y2, bmpWidth, y2);
                    gr.DrawLine(penFrame, 0, y, 0, y2);
                    gr.DrawLine(penFrame, bmpWidth-1, y, bmpWidth-1, y2);

                    pix = m_frames[ idx ];

                    Bitmap bmp = BitmapFromFrame(idx);

                    gr.DrawImage(bmp, 1 - minX - pix.m_offset_x,
                                 y + 1 - minY - pix.m_offset_y,
                                 bmp.Width, bmp.Height);
                }

                penFrame.Dispose();
                gr.Dispose();
#endif

                //--------------------------------------------------------------
                // GIF I need to manually set the pixels
                // or .NET uses a dumb palette
                Bitmap indexBmp = new Bitmap(bmpWidth, bmpHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                System.Drawing.Imaging.ColorPalette palette = indexBmp.Palette;
                Color[] entries = palette.Entries;

                // Setup the palette in the indexBmp
                entries[16] = backColor;
                entries[17] = frameColor;

                for (int idx = 0; idx < 16; ++idx)
                {
                    entries[idx] = m_palettes[0].colors[idx];
                }

                // Fill in the last colors
                for (int idx = 18; idx < 256; ++idx)
                {
                    entries[ idx ] = backColor;
                }

                indexBmp.Palette = palette;
                //--------------------------------------------------------------

                // I have to manually poke the colors into the color indexed
                // bitmap

                var data = indexBmp.LockBits(new Rectangle(0, 0, indexBmp.Width, indexBmp.Height),
                                             System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                             System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                var bytes = new byte[data.Height * data.Stride];
                System.Runtime.InteropServices.Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

                for (int pixelY = 0; pixelY < bmpHeight; ++pixelY)
                {
                    for (int pixelX = 0; pixelX < bmpWidth; ++pixelX)
                    {
                        // Source True Color
                        Color pixel = outBmp.GetPixel(pixelX, pixelY);

                        // Match the color to an index
                        byte index = 0;

                        for (int idx = 0; idx < 18; ++idx)
                        {
                            if (pixel == entries[ idx ])
                            {
                                index = (byte) idx;
                                break;
                            }
                        }

                        // Poke out the index
                        bytes[(pixelY * data.Stride) + pixelX] = index;
                        //indexBmp.SetPixel(pixelX, pixelY, outBmp.GetPixel(pixelX, pixelY) );
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
                indexBmp.UnlockBits(data);
                //--------------------------------------------------------------
                
                //outBmp.Save(pathName, System.Drawing.Imaging.ImageFormat.Png);
                indexBmp.Save(pathName, System.Drawing.Imaging.ImageFormat.Gif);

                //--------------------------------------------------------------
                // Convert the path into a "root", so we can export some support files
                // to go with this sprite
                String basePath = pathName.Remove(pathName.Length-4);
                String batPath = basePath + ".bat";
                String asmPath = basePath + ".asm";

                Console.WriteLine("Export Mr.Sprite Support Files");
                Console.WriteLine("Export {0}", batPath);

                String fileName = Path.GetFileName( pathName );
                String baseName = Path.GetFileNameWithoutExtension( pathName );

                try
                {
                    System.IO.TextWriter t = new StreamWriter(batPath);

                    t.WriteLine("rem");
                    t.WriteLine("rem Process a spritesheet GIF");
                    t.WriteLine("rem into compiled Mr.Sprite Data");
                    t.WriteLine("rem");
                    t.WriteLine("");

                    String work_dir = baseName+"_work";
                    t.WriteLine("rem Recreate a work directory");
                    t.Write("rmdir /S /Q ");
                    t.WriteLine(work_dir);
                    t.Write("mkdir ");
                    t.WriteLine(work_dir);
                    t.WriteLine("");

                    t.WriteLine("");
                    t.WriteLine("rem Extract the Sprite Frames");
                    t.WriteLine("");
                    t.Write("copy "); t.Write(fileName);
                    t.Write(" "); t.WriteLine(work_dir);
                    t.Write("cd "); t.WriteLine(work_dir);
                    t.Write("MrSprite EXTRACT ");
                    t.Write(fileName);
                    t.Write(" ");
                    t.Write( ColorToString( backColor ) );
                    t.Write(" ");
                    t.WriteLine( ColorToString( frameColor ) );
                    t.Write("del "); t.WriteLine(fileName);
                    t.WriteLine("cd ..");
                    t.WriteLine("");

                    t.WriteLine("");
                    t.WriteLine("rem Generate Odd positions");
                    t.WriteLine("");
                    t.Write("MrSprite ODD ");
                    t.Write(work_dir);
                    t.Write("\\* ");
                    t.WriteLine( ColorToString( backColor ) );
                    t.WriteLine("");

                    t.WriteLine("");
                    t.WriteLine("rem Generate HFlips");
                    t.WriteLine("");
                    t.Write("MrSprite MIRROR ");
                    t.Write(work_dir);
                    t.Write("\\* ");
                    t.WriteLine( ColorToString( backColor ) );
                    t.WriteLine("");

                    t.WriteLine("");
                    t.WriteLine("rem Generate Compiled Code");
                    t.WriteLine("");
                    t.Write("MrSprite CODE -V ");
                    t.Write(work_dir);
                    t.Write("\\* ");
                    t.Write( ColorToString( backColor ) );
                    for (int idx = 0; idx < 16; ++idx)
                    {
                        t.Write(" ");
                        t.Write( ColorToString( entries[ idx ] ) );
                    }
                    t.WriteLine("");
                    t.WriteLine("");

                    t.WriteLine("");
                    t.WriteLine("rem Generate Sprite Banks");
                    t.WriteLine("");
                    t.Write("MrSprite BANK ");
                    t.Write(work_dir);
                    t.Write("\\*.txt ");
                    t.WriteLine(baseName);
                    t.WriteLine("");

                    t.Flush();
                    t.Close();
                    t = null;
                }
                catch (IOException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }

                //--------------------------------------------------------------
                // Support ASM Files
                //
                Console.WriteLine("Export {0}", asmPath);

                try
                {
                    System.IO.TextWriter t = new StreamWriter(asmPath);

                    t.WriteLine("***********************************************");
                    t.WriteLine("* Mr. Sprite Support Data");
                    t.WriteLine("*");
                    t.WriteLine("* - Offset / Render Clipping Data");
                    t.WriteLine("* - Animation Definitions");
                    t.WriteLine("*");
                    t.WriteLine("***********************************************");
                    t.WriteLine("");
                    t.WriteLine(String.Format("{0}_hs_offset_x", baseName));
                    int column = 0;
                    for (int idx = 0; idx < m_frames.Count; ++idx)
                    {
                        pix = m_frames[idx];
                        WriteHex(ref t, ref column, -pix.m_offset_x);
                    }
                    t.WriteLine("");


                    t.WriteLine("");
                    t.WriteLine(String.Format("{0}_hs_offset_y", baseName));
                    column = 0;
                    for (int idx = 0; idx < m_frames.Count; ++idx)
                    {
                        pix = m_frames[idx];
                        WriteHex(ref t, ref column, -pix.m_offset_y);
                    }
                    t.WriteLine("");

                    t.WriteLine("");
                    t.WriteLine(String.Format("{0}_width", baseName));
                    column = 0;
                    for (int idx = 0; idx < m_frames.Count; ++idx)
                    {
                        WriteHex(ref t, ref column, frames_width[ idx ]);
                    }
                    t.WriteLine("");

                    t.WriteLine("");
                    t.WriteLine(String.Format("{0}_height", baseName));
                    column = 0;
                    for (int idx = 0; idx < m_frames.Count; ++idx)
                    {
                        WriteHex(ref t, ref column, frames_height[ idx ]);
                    }
                    t.WriteLine("");

                    // Kick out Animation definition Table
                    t.WriteLine("");
                    t.WriteLine(String.Format("{0}_anim", baseName));
                    column = 0;
                    for (int idx = 0; idx < m_anims.Count; ++idx)
                    {
                        WriteString(ref t, ref column, String.Format(":{0}", idx));
                    }
                    t.WriteLine("");
                    t.WriteLine("");

                    // Kickout the Animations
                    for (int animIdx = 0; animIdx < m_anims.Count; ++animIdx)
                    {
                        spAnim anim = m_anims[ animIdx ];
                        t.WriteLine(String.Format(":{0}", animIdx));
                        column = 0;
                        for (int cmdIdx = 0; cmdIdx < anim.m_commands.Count; ++cmdIdx)
                        {
                            spAnimCommand animCmd = anim.m_commands[ cmdIdx ];

                            int lineNo = cmdIdx + 1;

                            if (animCmd.m_frameNo < 0)
                            {
                                byte u8;
                                ushort u16;
                                // It's a command
                                WriteString(ref t, ref column, String.Format("${0:x2}",(byte)0xFF));      // command
                                WriteString(ref t, ref column, String.Format("${0:x2}",(byte)animCmd.m_command));

                                switch ((spAnimCommand.cmd)animCmd.m_command)
                                {
                                case spAnimCommand.cmd.End:
                                case spAnimCommand.cmd.Loop:
                                    break;
                                case spAnimCommand.cmd.Goto:
                                    u16 = (ushort) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16&0xFF)));
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16>>8)));
                                    break;
                                case spAnimCommand.cmd.GotoSeq:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.Pause:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.SetRate:
                                    u16 = (ushort) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16&0xFF)));
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16>>8)));
                                    break;
                                case spAnimCommand.cmd.SetSpeed:
                                    u16 = (ushort) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16&0xFF)));
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16>>8)));
                                    break;
                                case spAnimCommand.cmd.MultiOp:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.Delete:
                                    break;
                                case spAnimCommand.cmd.SetFlag:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.Sound:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.HFlip:
                                case spAnimCommand.cmd.VFlip:
                                case spAnimCommand.cmd.Nop:
                                    break;
                                case spAnimCommand.cmd.Process:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.ClearFlag:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.GotoLast:
                                case spAnimCommand.cmd.Blank:
                                    break;
                                case spAnimCommand.cmd.RndPause:
                                    u16 = (ushort) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16&0xFF)));
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16>>8)));
                                    break;
                                case spAnimCommand.cmd.SetHFlip:
                                case spAnimCommand.cmd.ClrHFlip:
                                case spAnimCommand.cmd.SetVFlip:
                                case spAnimCommand.cmd.ClrVFlip:
                                case spAnimCommand.cmd.HVFlip:
                                case spAnimCommand.cmd.SetHVFlip:
                                case spAnimCommand.cmd.ClrHVFlip:
                                    break;
                                case spAnimCommand.cmd.ExtSprite:
                                    u16 = (ushort) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16&0xFF)));
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)(u16>>8)));
                                    break;
                                case spAnimCommand.cmd.Brk:
                                    break;
                                case spAnimCommand.cmd.OnBrk:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;
                                case spAnimCommand.cmd.DynSound:
                                    u8 = (byte) animCmd.m_arg;
                                    WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                                    break;

                                default:
                                    break;
                                }
                            }
                            else
                            {
                                byte u8 = (byte) animCmd.m_frameNo;
                                WriteString(ref t, ref column, String.Format("${0:x2}",(byte)u8));
                            }
                        }
                        t.WriteLine("");
                    }




                    t.WriteLine("***********************************************");
                    t.WriteLine("");

                    t.Flush();
                    t.Close();
                    t = null;
                }
                catch (IOException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }

            }

        }

        void WriteHex(ref System.IO.TextWriter t, ref int column, int value)
        {
            if (0 == column)
            {
                t.Write(String.Format("\tdw\t${0:x4}", value & 0xFFFF));
            }
            else
            {
                t.Write(String.Format(",${0:x4}", value & 0xFFFF));
            }
            column++;
            if (16 == column)
            {
                column = 0;
                t.WriteLine("");
            }
        }

        void WriteString(ref System.IO.TextWriter t, ref int column, String str)
        {
            if (0 == column)
            {
                t.Write(String.Format("\tdw\t{0}", str));
            }
            else
            {
                t.Write(String.Format(",{0}", str));
            }
            column++;
            if (16 == column)
            {
                column = 0;
                t.WriteLine("");
            }
        }

        // This more accurately gets a "trimmed" width
        int GetRealWidth( ref Bitmap bmp )
        {
            int width  = bmp.Width;
            int height = bmp.Height;

            for (int x = width - 1; x >=0; --x)
            {
                bool bPixelFound = false;

                for (int y = 0; y < height; ++y)
                {
                    Color pix = bmp.GetPixel(x, y);

                    if (255 == pix.A)
                    {
                        bPixelFound = true;
                        break;
                    }
                }

                if (bPixelFound)
                {
                    width = x + 1;
                    break;
                }
            }

            return width;
        }


        // This more accurately gets a "trimmed" height
        int GetRealHeight( ref Bitmap bmp )
        {
            int width  = bmp.Width;
            int height = bmp.Height;

            for (int y = height - 1; y >=0; --y)
            {
                bool bPixelFound = false;

                for (int x = 0; x < width; ++x)
                {
                    Color pix = bmp.GetPixel(x, y);

                    if (255 == pix.A)
                    {
                        bPixelFound = true;
                        break;
                    }
                }

                if (bPixelFound)
                {
                    height = y + 1;
                    break;
                }
            }

            return height;
        }

        String ColorToString( Color color )
        {
            return String.Format("{0:x2}{1:x2}{2:x2}",
                                 color.R, color.G, color.B
                                 );
        }

        Color GetBackColor(spPalette excludes)
        {
            // Potential Background Colors, 17 of them
            // Because there are 16 input colors
            List<Color> colors = new List<Color>();
            colors.Add( Color.FromArgb(255,0,0,0) );
            colors.Add( Color.FromArgb(255,0x00,0x10,0x00) );
            colors.Add( Color.FromArgb(255,0x10,0x00,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0x00,0x10) );
            colors.Add( Color.FromArgb(255,0x10,0x10,0x00) );

            colors.Add( Color.FromArgb(255,0x00,0x10,0x10) );
            colors.Add( Color.FromArgb(255,0x10,0x00,0x10) );
            colors.Add( Color.FromArgb(255,0x20,0x00,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0x20,0x00) );

            colors.Add( Color.FromArgb(255,0x00,0x00,0x20) );
            colors.Add( Color.FromArgb(255,0x20,0x20,0x00) );
            colors.Add( Color.FromArgb(255,0x20,0x00,0x20) );
            colors.Add( Color.FromArgb(255,0x00,0x20,0x20) );

            colors.Add( Color.FromArgb(255,0x30,0x00,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0x30,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0x00,0x30) );
            colors.Add( Color.FromArgb(255,0x30,0x00,0x30) );

            Color result = Color.FromArgb(255, 255, 255, 255);

            for (int idx = 0; idx < colors.Count; ++idx)
            {
                bool bMatch = false;

                for (int exIdx = 0; exIdx < excludes.colors.Count; ++exIdx)
                {
                    if ( (colors[idx].R == excludes.colors[ exIdx ].R) &&
                         (colors[idx].G == excludes.colors[ exIdx ].G) &&
                         (colors[idx].B == excludes.colors[ exIdx ].B) )
                    {
                        bMatch = true;
                        break;
                    }
                }

                if (bMatch) continue;

                // No Match
                result = colors[ idx ];
                break;
            }
            
            return result;
        }

        Color GetFrameColor(spPalette excludes)
        {
            // Potential Background Colors, 17 of them
            // Because there are 16 input colors
            List<Color> colors = new List<Color>();
            colors.Add( Color.FromArgb(255,0xFF,0xFF,0xFF) );
            colors.Add( Color.FromArgb(255,0x00,0xF0,0x00) );
            colors.Add( Color.FromArgb(255,0xF0,0x00,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0x00,0xF0) );

            colors.Add( Color.FromArgb(255,0xF0,0xF0,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0xF0,0xF0) );
            colors.Add( Color.FromArgb(255,0xF0,0x00,0xF0) );
            colors.Add( Color.FromArgb(255,0xE0,0x00,0x00) );

            colors.Add( Color.FromArgb(255,0x00,0xE0,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0x00,0xE0) );
            colors.Add( Color.FromArgb(255,0xE0,0xE0,0x00) );
            colors.Add( Color.FromArgb(255,0xE0,0x00,0xE0) );

            colors.Add( Color.FromArgb(255,0x00,0xE0,0xE0) );
            colors.Add( Color.FromArgb(255,0xF8,0x00,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0xF8,0x00) );
            colors.Add( Color.FromArgb(255,0x00,0x00,0xF8) );
            colors.Add( Color.FromArgb(255,0xF8,0x00,0xF8) );

            Color result = Color.FromArgb(255,255,255,255);

            for (int idx = 0; idx < colors.Count; ++idx)
            {
                bool bMatch = false;

                for (int exIdx = 0; exIdx < excludes.colors.Count; ++exIdx)
                {
                    if ( (colors[idx].R == excludes.colors[ exIdx ].R) &&
                         (colors[idx].G == excludes.colors[ exIdx ].G) &&
                         (colors[idx].B == excludes.colors[ exIdx ].B) )
                    {
                        bMatch = true;
                        break;
                    }
                }

                if (bMatch) continue;

                // No Match
                result = colors[ idx ];
                break;
            }
            
            return result;
        }
        //----------------------------------------------------------------------

    }
}
