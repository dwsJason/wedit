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
                            byte frameCount = b.ReadByte();
                            length--;
                            byte frameCount2 = b.ReadByte();
                            length--;

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

    }
}
