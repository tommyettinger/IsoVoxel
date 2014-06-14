using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace IsoVoxel
{
    /// <summary>
    /// This is just magic right now.  It expects weird .vox files.
    /// </summary>
    class MultiColorDraw
    {
        private static float[][] colors = new float[][]
        {
            //tires, tread
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            
            //tire hubcap, tread core
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.4F,0.4F,0.4F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            
            //gun barrel
            new float[] {0.4F,0.35F,0.5F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            
            //gun peripheral (sights, trigger)
            new float[] {0.3F,0.25F,0.2F,1F},
            new float[] {0.6F,0.8F,1F,1F},
            new float[] {0.3F,0.25F,0.2F,1F},
            new float[] {0.3F,0.25F,0.2F,1F},
            new float[] {0.3F,0.25F,0.2F,1F},
            new float[] {0.3F,0.25F,0.2F,1F},
            new float[] {0.3F,0.25F,0.2F,1F},
            new float[] {0.3F,0.25F,0.2F,1F},
            
            //main paint
            new float[] {0.3F,0.3F,0.3F,1F},     //black
            new float[] {1.15F,1.15F,1.15F,1F},  //white
            new float[] {1F,0.1F,0F,1F},         //red
            new float[] {0.9F,0.35F,0.25F,1F},      //orange
            new float[] {1.1F,1.1F,0.2F,1F},         //yellow
            new float[] {0.25F,0.9F,0.2F,1F},    //green
            new float[] {0.2F,0.3F,1F,1F},       //blue
            new float[] {0.7F,0.3F,0.8F,1F},       //purple
            
            //doors
            new float[] {0.8F,0F,0F,1F},         //black
            new float[] {0.6F,1F,1F,1.3F},         //white
            new float[] {0.6F,0.1F,0F,1F},       //red
            new float[] {0.8F,0.5F,-0.1F,1F},       //orange
            new float[] {0.85F,0.8F,0.1F,1F},   //yellow
            new float[] {0.2F,0.7F,0.1F,1F},     //green
            new float[] {0.1F,0.1F,0.7F,1F},       //blue
            new float[] {0.6F,0.1F,0.55F,1F},       //purple
            
            //cockpit
            new float[] {0.5F,0.5F,0.4F,1F},     //black
            new float[] {0.75F,0.75F,0.9F,1F},   //white
            new float[] {0.9F,0.5F,0.4F,1F},     //red
            new float[] {0.82F,0.4F,0.1F,1F},    //orange
            new float[] {0.9F,0.9F,0.4F,1F},     //yellow
            new float[] {0.5F,0.9F,0.4F,1F},     //green
            new float[] {0.4F,0.5F,0.9F,1F},     //blue
            new float[] {0.9F,0.3F,0.85F,1F},   //purple

            //helmet
            new float[] {0.8F,0.4F,0.2F,1F},     //black
            new float[] {0.9F,0.95F,1F,1F},       //white
            new float[] {0.9F,0.3F,0.2F,1F},     //red
            new float[] {1F,0.5F,0.3F,1F},       //orange
            new float[] {0.7F,0.65F,0.5F,1F},         //yellow
            new float[] {0.35F,0.85F,0.5F,1F},       //green
            new float[] {0.2F,0.3F,1F,1F},       //blue
            new float[] {0.9F,0.2F,1F,1F},       //purple
            
            //flesh
            new float[] {1.1F,0.89F,0.55F,1F},  //black
            new float[] {0.9F,1.2F,0F,1F},      //white
            new float[] {1.1F,0.89F,0.55F,1F},  //red
            new float[] {1.1F,0.89F,0.55F,1F},  //orange
            new float[] {1.1F,0.89F,0.55F,1F},  //yellow
            new float[] {1.1F,0.89F,0.55F,1F},  //green
            new float[] {1.1F,0.89F,0.55F,1F},  //blue
            new float[] {1.1F,0.89F,0.55F,1F},  //purple
            
            //bullets
            new float[] {0.7F,0.45F,0.35F,1F},   //black
            new float[] {0.5F,0.8F,1F,1F},       //white
            new float[] {0.7F,0.45F,0.35F,1F},   //red
            new float[] {0.7F,0.45F,0.35F,1F},   //orange
            new float[] {0.7F,0.45F,0.35F,1F},   //yellow
            new float[] {0.7F,0.45F,0.35F,1F},   //green
            new float[] {0.7F,0.45F,0.35F,1F},   //blue
            new float[] {0.7F,0.45F,0.35F,1F},   //purple

            //lights
            new float[] {1.2F,1.2F,0.75F,1F},        //black
            new float[] {1.5F,0.8F,1.5F,1F},         //white
            new float[] {1.4F,1.2F,0.75F,1F},        //red
            new float[] {1.3F,1.3F,0.75F,1F},        //orange
            new float[] {1.2F,0.7F,0.4F,1F},        //yellow
            new float[] {1.3F,1.2F,0.75F,1F},        //green
            new float[] {1.2F,1.3F,0.75F,1F},        //blue
            new float[] {1.3F,1.3F,0.8F,1F},        //purple

            //windows
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.3F,1.2F,1.2F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.45F,0.4F,0.4F,1F},
            new float[] {0.45F,0.4F,0.4F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},

            //shadow (HAS ALPHA)
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},

            //128 garbage
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            //4 final garbage
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
        };
        private struct MagicaVoxelData
        {
            public byte x;
            public byte y;
            public byte z;
            public byte color;

            public MagicaVoxelData(BinaryReader stream, bool subsample)
            {
                x = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                y = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                z = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                color = stream.ReadByte();
            }
        }
        private static int sizex = 0, sizey = 0, sizez = 0;
        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <param name="overrideColors">Optional color lookup table for converting RGB values into my internal engine color format.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        private static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];

            MagicaVoxelData[] voxelData = null;

            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            if (magic == "VOX ")
            {
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    // each chunk has an ID, size and child chunks
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    // there are only 2 chunks we only care about, and they are SIZE and XYZI
                    if (chunkName == "SIZE")
                    {
                        sizex = stream.ReadInt32();
                        sizey = stream.ReadInt32();
                        sizez = stream.ReadInt32();

                        if (sizex > 32 || sizey > 32) subsample = true;

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        // XYZI contains n voxels
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        // each voxel has x, y, z and color index values
                        voxelData = new MagicaVoxelData[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelData(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
                        //colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }

                if (voxelData.Length == 0) return voxelData; // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;

                    // use the voxColors array by default, or overrideColor if it is available
                    int voxel = (voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128);
                    //data[voxel] = (colors == null ? voxColors[voxelData[i].color - 1] : colors[voxelData[i].color - 1]);
                }
            }

            return voxelData;
        }


        private static Bitmap drawPixelsSE(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80, 80,PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, 80 - 3 - 32 - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsSW(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80, 80,PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY) + 10);
                vls[i].y = (byte)((tempX * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, 80 - 3 - 32 - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNE(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80, 80,PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY * -1) + 10 - 1);
                vls[i].y = (byte)(tempX + 10);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, 80 - 3 - 32 - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNW(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80, 80,PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempX * -1) + 10 - 1);
                vls[i].y = (byte)((tempY * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, 80 - 3 - 32 - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static void processUnitBasic(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

            for (int i = 0; i < 8; i++)
            {
                System.IO.Directory.CreateDirectory(u);
                
                Bitmap bSE = drawPixelsSE(parsed, i);
                bSE.Save(u + "/color" + i + "_" + u + "_default_SE" + ".png", ImageFormat.Png);
                Bitmap bSW = drawPixelsSW(parsed, i);
                bSW.Save(u + "/color" + i + "_" + u + "_default_SW" + ".png", ImageFormat.Png);
                Bitmap bNW = drawPixelsNW(parsed, i);
                bNW.Save(u + "/color" + i + "_" + u + "_default_NW" + ".png", ImageFormat.Png);
                Bitmap bNE = drawPixelsNE(parsed, i);
                bNE.Save(u + "/color" + i + "_" + u + "_default_NE" + ".png", ImageFormat.Png);
                
            }
            bin.Close();
            
        }
        /*
        private static void processBases()
        {
            BinaryReader[] powers = new BinaryReader[8];
            BinaryReader[] speeds = new BinaryReader[8];
            BinaryReader[] techniques = new BinaryReader[8];


            MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

            for (int i = 0; i < 8; i++)
            {
                powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                basepowers[i] = FromMagica(powers[i]);
                speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                basespeeds[i] = FromMagica(speeds[i]);
                techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                basetechniques[i] = FromMagica(techniques[i]);

            }

            System.IO.Directory.CreateDirectory("Power");
            System.IO.Directory.CreateDirectory("Speed");
            System.IO.Directory.CreateDirectory("Technique");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    power.Save("Power/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    speed.Save("Speed/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    technique.Save("Technique/color" + i + "_frame_" + j + ".png", ImageFormat.Png);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                powers[i].Close();
                speeds[i].Close();
                techniques[i].Close();
            }
        }
        
        private static void processUnit(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

            BinaryReader[] bases = {
                                   new BinaryReader(File.Open("Base_Power.vox", FileMode.Open)),
                                   new BinaryReader(File.Open("Base_Speed.vox", FileMode.Open)),
                                   new BinaryReader(File.Open("Base_Technique.vox", FileMode.Open))
            };
            BinaryReader[] powers = new BinaryReader[8];
            BinaryReader[] speeds = new BinaryReader[8];
            BinaryReader[] techniques = new BinaryReader[8];


            MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

            for (int i = 0; i < 8; i++)
            {
                powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                basepowers[i] = FromMagica(powers[i]);
                speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                basespeeds[i] = FromMagica(speeds[i]);
                techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                basetechniques[i] = FromMagica(techniques[i]);

            }
            for (int i = 0; i < 8; i++)
            {
                System.IO.Directory.CreateDirectory(u);
                System.IO.Directory.CreateDirectory(u + "color" + i);
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NE");
                Bitmap bSE = drawPixelsSE(parsed, i);
                bSE.Save(u + "/color" + i + "_" + u + "_default_SE" + ".png", ImageFormat.Png);
                Bitmap bSW = drawPixelsSW(parsed, i);
                bSW.Save(u + "/color" + i + "_" + u + "_default_SW" + ".png", ImageFormat.Png);
                Bitmap bNW = drawPixelsNW(parsed, i);
                bNW.Save(u + "/color" + i + "_" + u + "_default_NW" + ".png", ImageFormat.Png);
                Bitmap bNE = drawPixelsNE(parsed, i);
                bNE.Save(u + "/color" + i + "_" + u + "_default_NE" + ".png", ImageFormat.Png);
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bSE, 0, 0);
                    power.Save(u + "color" + i + "/power/SE/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bSE, 0, 0);
                    speed.Save(u + "color" + i + "/speed/SE/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bSE, 0, 0);
                    technique.Save(u + "color" + i + "/technique/SE/" + j + ".png", ImageFormat.Png);
                }
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bSW, 0, 0);
                    power.Save(u + "color" + i + "/power/SW/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bSW, 0, 0);
                    speed.Save(u + "color" + i + "/speed/SW/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bSW, 0, 0);
                    technique.Save(u + "color" + i + "/technique/SW/" + j + ".png", ImageFormat.Png);
                }
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bNE, 0, 0);
                    power.Save(u + "color" + i + "/power/NE/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bNE, 0, 0);
                    speed.Save(u + "color" + i + "/speed/NE/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bNE, 0, 0);
                    technique.Save(u + "color" + i + "/technique/NE/" + j + ".png", ImageFormat.Png);
                }
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bNW, 0, 0);
                    power.Save(u + "color" + i + "/power/NW/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bNW, 0, 0);
                    speed.Save(u + "color" + i + "/speed/NW/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bNW, 0, 0);
                    technique.Save(u + "color" + i + "/technique/NW/" + j + ".png", ImageFormat.Png);
                }
                ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\ImageMagick-6.8.9-Q16\convert.EXE");
                startInfo.UseShellExecute = false;
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SE/* " + u + "/color" + i + "_" + u + "_power_SE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SE/* " + u + "/color" + i + "_" + u + "_speed_SE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SE/* " + u + "/color" + i + "_" + u + "_technique_SE.gif";
                Process.Start(startInfo).WaitForExit();

                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SW/* " + u + "/color" + i + "_" + u + "_power_SW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SW/* " + u + "/color" + i + "_" + u + "_speed_SW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SW/* " + u + "/color" + i + "_" + u + "_technique_SW.gif";
                Process.Start(startInfo).WaitForExit();

                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NW/* " + u + "/color" + i + "_" + u + "_power_NW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NW/* " + u + "/color" + i + "_" + u + "_speed_NW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NW/* " + u + "/color" + i + "_" + u + "_technique_NW.gif";
                Process.Start(startInfo).WaitForExit();

                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NE/* " + u + "/color" + i + "_" + u + "_power_NE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NE/* " + u + "/color" + i + "_" + u + "_speed_NE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NE/* " + u + "/color" + i + "_" + u + "_technique_NE.gif";
                Process.Start(startInfo).WaitForExit();

            }
            bin.Close();
            bases[0].Close();
            bases[1].Close();
            bases[2].Close();
            for (int i = 0; i < 8; i++)
            {
                powers[i].Close();
                speeds[i].Close();
                techniques[i].Close();
            }
        }
        */
        private static Bitmap[] processFloor(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + ".vox", FileMode.Open));
            PaletteDraw.MagicaVoxelData[] parsed = PaletteDraw.FromMagica(bin);


            System.IO.Directory.CreateDirectory(u);
            Bitmap[] bits = new Bitmap[] {
                PaletteDraw.renderSE(parsed, 20, 20, 20),
                PaletteDraw.renderSW(parsed, 20, 20, 20),
                PaletteDraw.renderNW(parsed, 20, 20, 20),
                PaletteDraw.renderNE(parsed, 20, 20, 20)
            };
            /*Random r = new Random();
            Bitmap b = new Bitmap(80,40);
            Graphics tiling = Graphics.FromImage(b);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 0, 0);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, 20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, 20);*/
            bits[0].Save(u + "/" + u + "_default_SE" + ".png", ImageFormat.Png);
            bits[1].Save(u + "/" + u + "_default_SW" + ".png", ImageFormat.Png);
            bits[2].Save(u + "/" + u + "_default_NW" + ".png", ImageFormat.Png);
            bits[3].Save(u + "/" + u + "_default_NE" + ".png", ImageFormat.Png);
            //b.Save(u + "/tiled.png", ImageFormat.Png);

            bin.Close();
            return bits;
        }
        static Bitmap makeTiling()
        {

            Bitmap[] tilings = new Bitmap[16];
            processFloor("Grass").CopyTo(tilings, 0);
            processFloor("Grass").CopyTo(tilings, 4);
            processFloor("Forest").CopyTo(tilings, 8);
            processFloor("Jungle").CopyTo(tilings, 12);


            Random r = new Random();
            Bitmap b = new Bitmap(240, 120);
            Graphics tiling = Graphics.FromImage(b);

            tiling.DrawImageUnscaled(tilings[r.Next(4)], -40, -20);
            tiling.DrawImageUnscaled(tilings[r.Next(4)], 40, -20);
            tiling.DrawImageUnscaled(tilings[r.Next(4)], 120, -20);
            tiling.DrawImageUnscaled(tilings[r.Next(4)], 200, -20);
            for (int x = 0; x <= 160; x += 80)
            {
                tiling.DrawImageUnscaled(tilings[r.Next(12)], x, 0);
            }

            tiling.DrawImageUnscaled(tilings[r.Next(4)], -40, 20);

            tiling.DrawImageUnscaled(tilings[r.Next(14)], 40, 20);
            tiling.DrawImageUnscaled(tilings[r.Next(14)], 120, 20);

            tiling.DrawImageUnscaled(tilings[r.Next(4)], 200, 20);

            for (int x = 0; x <= 160; x += 80)
            {
                tiling.DrawImageUnscaled(tilings[r.Next(16)], x, 40);
            }

            tiling.DrawImageUnscaled(tilings[r.Next(4)], -40, 60);

            tiling.DrawImageUnscaled(tilings[r.Next(14)], 40, 60);
            tiling.DrawImageUnscaled(tilings[r.Next(16)], 120, 60);

            tiling.DrawImageUnscaled(tilings[r.Next(4)], 200, 60);

            for (int x = 0; x <= 160; x += 80)
            {
                tiling.DrawImageUnscaled(tilings[r.Next(16)], x, 80);
            }
            tiling.DrawImageUnscaled(tilings[r.Next(4)], -40, 100);
            tiling.DrawImageUnscaled(tilings[r.Next(4)], 40, 100);
            tiling.DrawImageUnscaled(tilings[r.Next(4)], 120, 100);
            tiling.DrawImageUnscaled(tilings[r.Next(4)], 200, 100);

            return b;
        }
        static void Main(string[] args)
        {
            processUnitBasic("Tank");

            /*
            processUnitBasic("Infantry");
            processUnitBasic("Artillery");
            processUnitBasic("Supply");
            
            processUnitBasic("Helicopter");
            processUnitBasic("Plane");

            processFloor("Grass");
            processFloor("Forest");
            processFloor("Jungle");

            processBases();*/
            /*
            Bitmap[] randomTilings = new Bitmap[9];
            for(int i = 0; i<9;i++)
            {
                randomTilings[i] = makeTiling();
            }
            Bitmap b = new Bitmap(720,360);
            Graphics g = Graphics.FromImage(b);
            for(int i = 0; i<9;i++)
            {
                g.DrawImageUnscaled(randomTilings[i], 240 * (i % 3), 120 * (i / 3));
            }
            b.Save("tiling_large.png", ImageFormat.Png);

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\ImageMagick-6.8.9-Q16\convert.EXE");
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "tiling_large.png -modulate 100,60 tiling_large.png";
            Process.Start(startInfo).WaitForExit();
            */
        }
    }
}
