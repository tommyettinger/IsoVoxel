using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace IsoVoxel
{

    class PaletteDraw
    {
        private static float[][] colors = null;
        public struct MagicaVoxelData
        {
            public byte x;
            public byte y;
            public byte z;
            public byte color;

            public MagicaVoxelData(BinaryReader stream, bool subsample)
            {
                x = stream.ReadByte(); //(byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                y = stream.ReadByte(); //(byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                z = stream.ReadByte(); //(byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                color = stream.ReadByte();
            }
        }


        
        private static int sizex = 0, sizey = 0, sizez = 0;

        private static Bitmap image;

        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static MagicaVoxelData[] FromMagica(BinaryReader stream)
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
//                        Console.WriteLine("x is " + sizex + ", y is " + sizey + ", z is " + sizez);
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
                        colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                            colors[i] = new float[] { r / 256.0f, g / 256.0f, b / 256.0f, a / 256.0f};
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
        /// <summary>
        /// Render voxel chunks in a MagicaVoxelData[] to a Bitmap with X pointing Southeast.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap renderSE(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize)
        {
            int bWidth = (xSize + ySize) * 2;
            int bHeight = (xSize + ySize) + zSize * 2;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube.png");
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

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[vx.color - 1][0],  0,  0,  0, 0},
   new float[] {0,  colors[vx.color - 1][1],  0,  0, 0},
   new float[] {0,  0,  colors[vx.color - 1][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        /// <summary>
        /// Render voxel chunks in a MagicaVoxelData[] to a Bitmap with X pointing Southwest.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap renderSW(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize)
        {
            int bWidth = (xSize + ySize) * 2;
            int bHeight = (xSize + ySize) + zSize * 2;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,   0, 1F,  0, 0},
   new float[] {0,   0,  0, 1F, 0},
   new float[] {0,   0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - (xSize / 2));
                byte tempY = (byte)(voxels[i].y - (ySize / 2));
                vls[i].x = (byte)((tempY) + (ySize / 2));
                vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;
            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
            {

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[vx.color - 1][0],  0,  0,  0, 0},
   new float[] {0,  colors[vx.color - 1][1],  0,  0, 0},
   new float[] {0,  0,  colors[vx.color - 1][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                    //(3 * zSize - 2)
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        /// <summary>
        /// Render voxel chunks in a MagicaVoxelData[] to a Bitmap with X pointing Northwest.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap renderNW(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize)
        {
            int bWidth = (xSize + ySize) * 2;
            int bHeight = (xSize + ySize) + zSize * 2;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,   0, 1F,  0, 0},
   new float[] {0,   0,  0, 1F, 0},
   new float[] {0,   0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - (xSize / 2));
                byte tempY = (byte)(voxels[i].y - (ySize / 2));
                vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;
            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
            {

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[vx.color - 1][0],  0,  0,  0, 0},
   new float[] {0,  colors[vx.color - 1][1],  0,  0, 0},
   new float[] {0,  0,  colors[vx.color - 1][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        /// <summary>
        /// Render voxel chunks in a MagicaVoxelData[] to a Bitmap with X pointing Northeast.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap renderNE(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize)
        {
            int bWidth = (xSize + ySize) * 2;
            int bHeight = (xSize + ySize) + zSize * 2;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,   0, 1F,  0, 0},
   new float[] {0,   0,  0, 1F, 0},
   new float[] {0,   0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - (xSize / 2));
                byte tempY = (byte)(voxels[i].y - (ySize / 2));
                vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                vls[i].y = (byte)(tempX + (xSize / 2));
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;
            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
            {

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[vx.color - 1][0],  0,  0,  0, 0},
   new float[] {0,  colors[vx.color - 1][1],  0,  0, 0},
   new float[] {0,  0,  colors[vx.color - 1][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        static void Main(string[] args)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream imageStream = assembly.GetManifestResourceStream("IsoVoxel.cube.png");
            image = new Bitmap(imageStream);
            string voxfile = "Tank.vox";
            if (args.Length >= 1)
                voxfile = args[0];
            byte x = 20, y = 20, z = 20;
            try
            {
                if (args.Length >= 2)
                    x = byte.Parse(args[1]);
                if (args.Length >= 3)
                    y = byte.Parse(args[2]);
                if (args.Length >= 4)
                    z = byte.Parse(args[3]);
            }catch(Exception)
            {
                Console.WriteLine("Args: 'file x y z'. file is a MagicaVoxel .vox file, x y z are optional sizes.");
                Console.WriteLine("Defaults: runs on Tank.vox with x, y, z set to 20, 20, 20.");
            }
            BinaryReader bin = new BinaryReader(File.Open(voxfile, FileMode.Open));
            MagicaVoxelData[] mvd = FromMagica(bin);
            renderSE(mvd, x, y, z).Save(voxfile.Substring(0, voxfile.Length - 4) + "_SE.png", ImageFormat.Png);
            renderSW(mvd, x, y, z).Save(voxfile.Substring(0, voxfile.Length - 4) + "_SW.png", ImageFormat.Png);
            renderNE(mvd, x, y, z).Save(voxfile.Substring(0, voxfile.Length - 4) + "_NE.png", ImageFormat.Png);
            renderNW(mvd, x, y, z).Save(voxfile.Substring(0, voxfile.Length - 4) + "_NW.png", ImageFormat.Png);
            bin.Close();
        }
    }
}
