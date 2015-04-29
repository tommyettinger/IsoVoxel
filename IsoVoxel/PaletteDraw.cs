using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IsoVoxel
{
    enum Outlining
    {
        Full, Light, Partial, None
    }
    enum Direction
    {
        SE, SW, NW, NE
    }
    enum OrthoDirection
    {
        S, W, N, E
    }
    class PaletteDraw
    {
        private static float[][] colors = new float[][]
        {
            new float[]{1.0F, 1.0F, 1.0F, 1.0F},
            new float[]{1.0F, 1.0F, 0.8F, 1.0F},
            new float[]{1.0F, 1.0F, 0.6F, 1.0F},
            new float[]{1.0F, 1.0F, 0.4F, 1.0F},
            new float[]{1.0F, 1.0F, 0.2F, 1.0F},
            new float[]{1.0F, 1.0F, 0.0F, 1.0F},
            new float[]{1.0F, 0.8F, 1.0F, 1.0F},
            new float[]{1.0F, 0.8F, 0.8F, 1.0F},
            new float[]{1.0F, 0.8F, 0.6F, 1.0F},
            new float[]{1.0F, 0.8F, 0.4F, 1.0F},
            new float[]{1.0F, 0.8F, 0.2F, 1.0F},
            new float[]{1.0F, 0.8F, 0.0F, 1.0F},
            new float[]{1.0F, 0.6F, 1.0F, 1.0F},
            new float[]{1.0F, 0.6F, 0.8F, 1.0F},
            new float[]{1.0F, 0.6F, 0.6F, 1.0F},
            new float[]{1.0F, 0.6F, 0.4F, 1.0F},
            new float[]{1.0F, 0.6F, 0.2F, 1.0F},
            new float[]{1.0F, 0.6F, 0.0F, 1.0F},
            new float[]{1.0F, 0.4F, 1.0F, 1.0F},
            new float[]{1.0F, 0.4F, 0.8F, 1.0F},
            new float[]{1.0F, 0.4F, 0.6F, 1.0F},
            new float[]{1.0F, 0.4F, 0.4F, 1.0F},
            new float[]{1.0F, 0.4F, 0.2F, 1.0F},
            new float[]{1.0F, 0.4F, 0.0F, 1.0F},
            new float[]{1.0F, 0.2F, 1.0F, 1.0F},
            new float[]{1.0F, 0.2F, 0.8F, 1.0F},
            new float[]{1.0F, 0.2F, 0.6F, 1.0F},
            new float[]{1.0F, 0.2F, 0.4F, 1.0F},
            new float[]{1.0F, 0.2F, 0.2F, 1.0F},
            new float[]{1.0F, 0.2F, 0.0F, 1.0F},
            new float[]{1.0F, 0.0F, 1.0F, 1.0F},
            new float[]{1.0F, 0.0F, 0.8F, 1.0F},
            new float[]{1.0F, 0.0F, 0.6F, 1.0F},
            new float[]{1.0F, 0.0F, 0.4F, 1.0F},
            new float[]{1.0F, 0.0F, 0.2F, 1.0F},
            new float[]{1.0F, 0.0F, 0.0F, 1.0F},
            new float[]{0.8F, 1.0F, 1.0F, 1.0F},
            new float[]{0.8F, 1.0F, 0.8F, 1.0F},
            new float[]{0.8F, 1.0F, 0.6F, 1.0F},
            new float[]{0.8F, 1.0F, 0.4F, 1.0F},
            new float[]{0.8F, 1.0F, 0.2F, 1.0F},
            new float[]{0.8F, 1.0F, 0.0F, 1.0F},
            new float[]{0.8F, 0.8F, 1.0F, 1.0F},
            new float[]{0.8F, 0.8F, 0.8F, 1.0F},
            new float[]{0.8F, 0.8F, 0.6F, 1.0F},
            new float[]{0.8F, 0.8F, 0.4F, 1.0F},
            new float[]{0.8F, 0.8F, 0.2F, 1.0F},
            new float[]{0.8F, 0.8F, 0.0F, 1.0F},
            new float[]{0.8F, 0.6F, 1.0F, 1.0F},
            new float[]{0.8F, 0.6F, 0.8F, 1.0F},
            new float[]{0.8F, 0.6F, 0.6F, 1.0F},
            new float[]{0.8F, 0.6F, 0.4F, 1.0F},
            new float[]{0.8F, 0.6F, 0.2F, 1.0F},
            new float[]{0.8F, 0.6F, 0.0F, 1.0F},
            new float[]{0.8F, 0.4F, 1.0F, 1.0F},
            new float[]{0.8F, 0.4F, 0.8F, 1.0F},
            new float[]{0.8F, 0.4F, 0.6F, 1.0F},
            new float[]{0.8F, 0.4F, 0.4F, 1.0F},
            new float[]{0.8F, 0.4F, 0.2F, 1.0F},
            new float[]{0.8F, 0.4F, 0.0F, 1.0F},
            new float[]{0.8F, 0.2F, 1.0F, 1.0F},
            new float[]{0.8F, 0.2F, 0.8F, 1.0F},
            new float[]{0.8F, 0.2F, 0.6F, 1.0F},
            new float[]{0.8F, 0.2F, 0.4F, 1.0F},
            new float[]{0.8F, 0.2F, 0.2F, 1.0F},
            new float[]{0.8F, 0.2F, 0.0F, 1.0F},
            new float[]{0.8F, 0.0F, 1.0F, 1.0F},
            new float[]{0.8F, 0.0F, 0.8F, 1.0F},
            new float[]{0.8F, 0.0F, 0.6F, 1.0F},
            new float[]{0.8F, 0.0F, 0.4F, 1.0F},
            new float[]{0.8F, 0.0F, 0.2F, 1.0F},
            new float[]{0.8F, 0.0F, 0.0F, 1.0F},
            new float[]{0.6F, 1.0F, 1.0F, 1.0F},
            new float[]{0.6F, 1.0F, 0.8F, 1.0F},
            new float[]{0.6F, 1.0F, 0.6F, 1.0F},
            new float[]{0.6F, 1.0F, 0.4F, 1.0F},
            new float[]{0.6F, 1.0F, 0.2F, 1.0F},
            new float[]{0.6F, 1.0F, 0.0F, 1.0F},
            new float[]{0.6F, 0.8F, 1.0F, 1.0F},
            new float[]{0.6F, 0.8F, 0.8F, 1.0F},
            new float[]{0.6F, 0.8F, 0.6F, 1.0F},
            new float[]{0.6F, 0.8F, 0.4F, 1.0F},
            new float[]{0.6F, 0.8F, 0.2F, 1.0F},
            new float[]{0.6F, 0.8F, 0.0F, 1.0F},
            new float[]{0.6F, 0.6F, 1.0F, 1.0F},
            new float[]{0.6F, 0.6F, 0.8F, 1.0F},
            new float[]{0.6F, 0.6F, 0.6F, 1.0F},
            new float[]{0.6F, 0.6F, 0.4F, 1.0F},
            new float[]{0.6F, 0.6F, 0.2F, 1.0F},
            new float[]{0.6F, 0.6F, 0.0F, 1.0F},
            new float[]{0.6F, 0.4F, 1.0F, 1.0F},
            new float[]{0.6F, 0.4F, 0.8F, 1.0F},
            new float[]{0.6F, 0.4F, 0.6F, 1.0F},
            new float[]{0.6F, 0.4F, 0.4F, 1.0F},
            new float[]{0.6F, 0.4F, 0.2F, 1.0F},
            new float[]{0.6F, 0.4F, 0.0F, 1.0F},
            new float[]{0.6F, 0.2F, 1.0F, 1.0F},
            new float[]{0.6F, 0.2F, 0.8F, 1.0F},
            new float[]{0.6F, 0.2F, 0.6F, 1.0F},
            new float[]{0.6F, 0.2F, 0.4F, 1.0F},
            new float[]{0.6F, 0.2F, 0.2F, 1.0F},
            new float[]{0.6F, 0.2F, 0.0F, 1.0F},
            new float[]{0.6F, 0.0F, 1.0F, 1.0F},
            new float[]{0.6F, 0.0F, 0.8F, 1.0F},
            new float[]{0.6F, 0.0F, 0.6F, 1.0F},
            new float[]{0.6F, 0.0F, 0.4F, 1.0F},
            new float[]{0.6F, 0.0F, 0.2F, 1.0F},
            new float[]{0.6F, 0.0F, 0.0F, 1.0F},
            new float[]{0.4F, 1.0F, 1.0F, 1.0F},
            new float[]{0.4F, 1.0F, 0.8F, 1.0F},
            new float[]{0.4F, 1.0F, 0.6F, 1.0F},
            new float[]{0.4F, 1.0F, 0.4F, 1.0F},
            new float[]{0.4F, 1.0F, 0.2F, 1.0F},
            new float[]{0.4F, 1.0F, 0.0F, 1.0F},
            new float[]{0.4F, 0.8F, 1.0F, 1.0F},
            new float[]{0.4F, 0.8F, 0.8F, 1.0F},
            new float[]{0.4F, 0.8F, 0.6F, 1.0F},
            new float[]{0.4F, 0.8F, 0.4F, 1.0F},
            new float[]{0.4F, 0.8F, 0.2F, 1.0F},
            new float[]{0.4F, 0.8F, 0.0F, 1.0F},
            new float[]{0.4F, 0.6F, 1.0F, 1.0F},
            new float[]{0.4F, 0.6F, 0.8F, 1.0F},
            new float[]{0.4F, 0.6F, 0.6F, 1.0F},
            new float[]{0.4F, 0.6F, 0.4F, 1.0F},
            new float[]{0.4F, 0.6F, 0.2F, 1.0F},
            new float[]{0.4F, 0.6F, 0.0F, 1.0F},
            new float[]{0.4F, 0.4F, 1.0F, 1.0F},
            new float[]{0.4F, 0.4F, 0.8F, 1.0F},
            new float[]{0.4F, 0.4F, 0.6F, 1.0F},
            new float[]{0.4F, 0.4F, 0.4F, 1.0F},
            new float[]{0.4F, 0.4F, 0.2F, 1.0F},
            new float[]{0.4F, 0.4F, 0.0F, 1.0F},
            new float[]{0.4F, 0.2F, 1.0F, 1.0F},
            new float[]{0.4F, 0.2F, 0.8F, 1.0F},
            new float[]{0.4F, 0.2F, 0.6F, 1.0F},
            new float[]{0.4F, 0.2F, 0.4F, 1.0F},
            new float[]{0.4F, 0.2F, 0.2F, 1.0F},
            new float[]{0.4F, 0.2F, 0.0F, 1.0F},
            new float[]{0.4F, 0.0F, 1.0F, 1.0F},
            new float[]{0.4F, 0.0F, 0.8F, 1.0F},
            new float[]{0.4F, 0.0F, 0.6F, 1.0F},
            new float[]{0.4F, 0.0F, 0.4F, 1.0F},
            new float[]{0.4F, 0.0F, 0.2F, 1.0F},
            new float[]{0.4F, 0.0F, 0.0F, 1.0F},
            new float[]{0.2F, 1.0F, 1.0F, 1.0F},
            new float[]{0.2F, 1.0F, 0.8F, 1.0F},
            new float[]{0.2F, 1.0F, 0.6F, 1.0F},
            new float[]{0.2F, 1.0F, 0.4F, 1.0F},
            new float[]{0.2F, 1.0F, 0.2F, 1.0F},
            new float[]{0.2F, 1.0F, 0.0F, 1.0F},
            new float[]{0.2F, 0.8F, 1.0F, 1.0F},
            new float[]{0.2F, 0.8F, 0.8F, 1.0F},
            new float[]{0.2F, 0.8F, 0.6F, 1.0F},
            new float[]{0.2F, 0.8F, 0.4F, 1.0F},
            new float[]{0.2F, 0.8F, 0.2F, 1.0F},
            new float[]{0.2F, 0.8F, 0.0F, 1.0F},
            new float[]{0.2F, 0.6F, 1.0F, 1.0F},
            new float[]{0.2F, 0.6F, 0.8F, 1.0F},
            new float[]{0.2F, 0.6F, 0.6F, 1.0F},
            new float[]{0.2F, 0.6F, 0.4F, 1.0F},
            new float[]{0.2F, 0.6F, 0.2F, 1.0F},
            new float[]{0.2F, 0.6F, 0.0F, 1.0F},
            new float[]{0.2F, 0.4F, 1.0F, 1.0F},
            new float[]{0.2F, 0.4F, 0.8F, 1.0F},
            new float[]{0.2F, 0.4F, 0.6F, 1.0F},
            new float[]{0.2F, 0.4F, 0.4F, 1.0F},
            new float[]{0.2F, 0.4F, 0.2F, 1.0F},
            new float[]{0.2F, 0.4F, 0.0F, 1.0F},
            new float[]{0.2F, 0.2F, 1.0F, 1.0F},
            new float[]{0.2F, 0.2F, 0.8F, 1.0F},
            new float[]{0.2F, 0.2F, 0.6F, 1.0F},
            new float[]{0.2F, 0.2F, 0.4F, 1.0F},
            new float[]{0.2F, 0.2F, 0.2F, 1.0F},
            new float[]{0.2F, 0.2F, 0.0F, 1.0F},
            new float[]{0.2F, 0.0F, 1.0F, 1.0F},
            new float[]{0.2F, 0.0F, 0.8F, 1.0F},
            new float[]{0.2F, 0.0F, 0.6F, 1.0F},
            new float[]{0.2F, 0.0F, 0.4F, 1.0F},
            new float[]{0.2F, 0.0F, 0.2F, 1.0F},
            new float[]{0.2F, 0.0F, 0.0F, 1.0F},
            new float[]{0.0F, 1.0F, 1.0F, 1.0F},
            new float[]{0.0F, 1.0F, 0.8F, 1.0F},
            new float[]{0.0F, 1.0F, 0.6F, 1.0F},
            new float[]{0.0F, 1.0F, 0.4F, 1.0F},
            new float[]{0.0F, 1.0F, 0.2F, 1.0F},
            new float[]{0.0F, 1.0F, 0.0F, 1.0F},
            new float[]{0.0F, 0.8F, 1.0F, 1.0F},
            new float[]{0.0F, 0.8F, 0.8F, 1.0F},
            new float[]{0.0F, 0.8F, 0.6F, 1.0F},
            new float[]{0.0F, 0.8F, 0.4F, 1.0F},
            new float[]{0.0F, 0.8F, 0.2F, 1.0F},
            new float[]{0.0F, 0.8F, 0.0F, 1.0F},
            new float[]{0.0F, 0.6F, 1.0F, 1.0F},
            new float[]{0.0F, 0.6F, 0.8F, 1.0F},
            new float[]{0.0F, 0.6F, 0.6F, 1.0F},
            new float[]{0.0F, 0.6F, 0.4F, 1.0F},
            new float[]{0.0F, 0.6F, 0.2F, 1.0F},
            new float[]{0.0F, 0.6F, 0.0F, 1.0F},
            new float[]{0.0F, 0.4F, 1.0F, 1.0F},
            new float[]{0.0F, 0.4F, 0.8F, 1.0F},
            new float[]{0.0F, 0.4F, 0.6F, 1.0F},
            new float[]{0.0F, 0.4F, 0.4F, 1.0F},
            new float[]{0.0F, 0.4F, 0.2F, 1.0F},
            new float[]{0.0F, 0.4F, 0.0F, 1.0F},
            new float[]{0.0F, 0.2F, 1.0F, 1.0F},
            new float[]{0.0F, 0.2F, 0.8F, 1.0F},
            new float[]{0.0F, 0.2F, 0.6F, 1.0F},
            new float[]{0.0F, 0.2F, 0.4F, 1.0F},
            new float[]{0.0F, 0.2F, 0.2F, 1.0F},
            new float[]{0.0F, 0.2F, 0.0F, 1.0F},
            new float[]{0.0F, 0.0F, 1.0F, 1.0F},
            new float[]{0.0F, 0.0F, 0.8F, 1.0F},
            new float[]{0.0F, 0.0F, 0.6F, 1.0F},
            new float[]{0.0F, 0.0F, 0.4F, 1.0F},
            new float[]{0.0F, 0.0F, 0.2F, 1.0F},
            new float[]{0.9333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.8666666666666667F, 0.0F, 0.0F, 1.0F},
            new float[]{0.7333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.6666666666666666F, 0.0F, 0.0F, 1.0F},
            new float[]{0.5333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.4666666666666667F, 0.0F, 0.0F, 1.0F},
            new float[]{0.3333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.26666666666666666F, 0.0F, 0.0F, 1.0F},
            new float[]{0.13333333333333333F, 0.0F, 0.0F, 1.0F},
            new float[]{0.06666666666666667F, 0.0F, 0.0F, 1.0F},
            new float[]{0.0F, 0.9333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.8666666666666667F, 0.0F, 1.0F},
            new float[]{0.0F, 0.7333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.6666666666666666F, 0.0F, 1.0F},
            new float[]{0.0F, 0.5333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.4666666666666667F, 0.0F, 1.0F},
            new float[]{0.0F, 0.3333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.26666666666666666F, 0.0F, 1.0F},
            new float[]{0.0F, 0.13333333333333333F, 0.0F, 1.0F},
            new float[]{0.0F, 0.06666666666666667F, 0.0F, 1.0F},
            new float[]{0.0F, 0.0F, 0.9333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.8666666666666667F, 1.0F},
            new float[]{0.0F, 0.0F, 0.7333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.6666666666666666F, 1.0F},
            new float[]{0.0F, 0.0F, 0.5333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.4666666666666667F, 1.0F},
            new float[]{0.0F, 0.0F, 0.3333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.26666666666666666F, 1.0F},
            new float[]{0.0F, 0.0F, 0.13333333333333333F, 1.0F},
            new float[]{0.0F, 0.0F, 0.06666666666666667F, 1.0F},
            new float[]{0.9333333333333333F, 0.9333333333333333F, 0.9333333333333333F, 1.0F},
            new float[]{0.8666666666666667F, 0.8666666666666667F, 0.8666666666666667F, 1.0F},
            new float[]{0.7333333333333333F, 0.7333333333333333F, 0.7333333333333333F, 1.0F},
            new float[]{0.6666666666666666F, 0.6666666666666666F, 0.6666666666666666F, 1.0F},
            new float[]{0.5333333333333333F, 0.5333333333333333F, 0.5333333333333333F, 1.0F},
            new float[]{0.4666666666666667F, 0.4666666666666667F, 0.4666666666666667F, 1.0F},
            new float[]{0.3333333333333333F, 0.3333333333333333F, 0.3333333333333333F, 1.0F},
            new float[]{0.26666666666666666F, 0.26666666666666666F, 0.26666666666666666F, 1.0F},
            new float[]{0.13333333333333333F, 0.13333333333333333F, 0.13333333333333333F, 1.0F},
            new float[]{0.06666666666666667F, 0.06666666666666667F, 0.06666666666666667F, 1.0F},
            new float[]{0.0F, 0.0F, 0.0F, 0.0F},
        };
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

        private static Bitmap cube, ortho;

        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below

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

                            colors[i] = new float[] { r / 256.0f, g / 256.0f, b / 256.0f, a / 256.0f };
                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }

                if (voxelData.Length == 0) return voxelData; // failed to read any valid voxel data
                /*
                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
//                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;
                    
                    // use the voxColors array by default, or overrideColor if it is available
//                    int voxel = (voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128);
                    //data[voxel] = (colors == null ? voxColors[voxelData[i].color - 1] : colors[voxelData[i].color - 1]);
                }*/
            }

            return voxelData;
        }
        static public int colorcount = 254;

        public static byte[][] rendered, renderedOrtho;

        private static byte[][] storeColorCubes()
        {
            colorcount = colors.Length;
            byte[,] cubes = new byte[colorcount, 80];

            Image image = cube;
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 5;
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

            for (int current_color = 0; current_color < colorcount; current_color++)
            {
                Bitmap b =
                new Bitmap(width, height, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage((Image)b);

                if (colors[current_color][3] == 0F)
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);
                g.DrawImage(image,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        cubes[current_color, i * 4 + j * 4 * width + 0] = c.B;
                        cubes[current_color, i * 4 + j * 4 * width + 1] = c.G;
                        cubes[current_color, i * 4 + j * 4 * width + 2] = c.R;
                        cubes[current_color, i * 4 + j * 4 * width + 3] = c.A;
                    }
                }
            }
            byte[][] cubes2 = new byte[colorcount][];
            for (int c = 0; c < colorcount; c++)
            {
                cubes2[c] = new byte[width * height * 4];
                for (int j = 0; j < width * height * 4; j++)
                {
                    cubes2[c][j] = cubes[c, j];
                }
            }

            return cubes2;
        }

        private static byte[][] storeColorCubesOrtho()
        {
            colorcount = colors.Length;
            byte[,] cubes = new byte[colorcount, 80];

            Image image = ortho;
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 3;
            int height = 5;
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

            for (int current_color = 0; current_color < colorcount; current_color++)
            {
                Bitmap b =
                new Bitmap(width, height, PixelFormat.Format32bppArgb);

                Graphics g = Graphics.FromImage((Image)b);

                if (colors[current_color][3] == 0F)
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 0},
   new float[] {0,  0,  0,  0, 1F}});
                }
                else
                {
                    colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {0.22F+colors[current_color][0],  0,  0,  0, 0},
   new float[] {0,  0.251F+colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  0.31F+colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});
                }
                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(image,
                   new Rectangle(0, 0,
                       width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Color c = b.GetPixel(i, j);
                        cubes[current_color, i * 4 + j * 4 * width + 0] = c.B;
                        cubes[current_color, i * 4 + j * 4 * width + 1] = c.G;
                        cubes[current_color, i * 4 + j * 4 * width + 2] = c.R;
                        cubes[current_color, i * 4 + j * 4 * width + 3] = c.A;
                    }
                }
            }

            byte[][] cubes2 = new byte[colorcount][];
            for (int c = 0; c < colorcount; c++)
            {
                cubes2[c] = new byte[width * height * 4];
                for (int j = 0; j < width * height * 4; j++)
                {
                    cubes2[c][j] = cubes[c, j];
                }
            }

            return cubes2;
        }

        /// <summary>
        /// Render voxel chunks in a MagicaVoxelData[] to a Bitmap with X pointing in any direction.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="dir">The Direction enum that specifies which way the X-axis should point.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap render(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, Direction dir)
        {
            int bWidth = (xSize + ySize) * 2;
            int bHeight = (xSize + ySize) + zSize * 3;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 4;

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
            switch (dir)
            {
                case Direction.SE:
                    vls = voxels;
                    break;
                case Direction.SW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;

                case Direction.NW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case Direction.NE:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
            {
                int currentColor = vx.color - 1;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[currentColor][0],  0,  0,  0, 0},
   new float[] {0,  colors[currentColor][1],  0,  0, 0},
   new float[] {0,  0,  colors[currentColor][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   cube,
                    //(3 * zSize - 2)
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 3 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }
        /*
        /// <summary>
        /// Render outline chunks in a MagicaVoxelData[] to a Bitmap with X pointing in any direction.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="dir">The Direction enum that specifies which way the X-axis should point.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        public static Bitmap renderOutline(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, Direction dir)
        {
            int bWidth = (xSize + ySize) * 2 + 4;
            int bHeight = (xSize + ySize) + zSize * 3 + 4;
            Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage((Image)b);
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 8;
            int height = 8;

            float[][] colorMatrixElements = { 
   new float[] {1.2F,  0,  0,  0, 0},
   new float[] {0,  1.2F,  0,  0, 0},
   new float[] {0,   0, 1.2F,  0, 0},
   new float[] {0,   0,    0, 1F, 0},
   new float[] {0,   0,    0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            switch (dir)
            {
                case Direction.SE:
                    vls = voxels;
                    break;
                case Direction.SW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY) + (ySize / 2));
                        vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;

                case Direction.NW:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                        vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
                case Direction.NE:
                    for (int i = 0; i < voxels.Length; i++)
                    {
                        byte tempX = (byte)(voxels[i].x - (xSize / 2));
                        byte tempY = (byte)(voxels[i].y - (ySize / 2));
                        vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                        vls[i].y = (byte)(tempX + (xSize / 2));
                        vls[i].z = voxels[i].z;
                        vls[i].color = voxels[i].color;
                    }
                    break;
            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128))
            {
                g.DrawImage(
                   outline,
                    //(3 * zSize - 2)
                   new Rectangle((vx.x + vx.y) * 2, (bHeight - xSize - 2) - vx.y + vx.x - 3 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }
        */
        private static int voxelToPixel(int innerX, int innerY, int x, int y, int z, int current_color, int stride, byte xSize, byte ySize, byte zSize)
        {
            return 4 * ((x + y) * 2 + 4)
                + innerX +
                stride * (((xSize + ySize) + zSize * 3 + 0) - (Math.Max(xSize, ySize)) - y + x - z * 3 + innerY); //(xSize + ySize) * 2
        }
        private static int voxelToPixelOrtho(int innerX, int innerY, int x, int y, int z, int current_color, int stride, byte xSize, byte ySize, byte zSize)
        {
            /*
            4 * (vx.y * 3 + 6 + ((current_color == 136) ? jitter - 1 : 0))
                             + i +
                           bmpData.Stride * (308 - 60 - 8 + vx.x - vx.z * 3 - ((VoxelLogic.xcolors[current_color + faction][3] == VoxelLogic.flat_alpha) ? -3 : jitter) + j)
             */
            return 4 * (y * 3 + 4 + ySize / 2)
                 + innerX +
                stride * ((xSize * 2 / 3 + zSize * 3 - 1) + x - z * 3 + innerY);
        }
        /// <summary>
        /// Render outline chunks in a MagicaVoxelData[] to a Bitmap with X pointing in any direction.
        /// </summary>
        /// <param name="voxels">The result of calling FromMagica.</param>
        /// <param name="xSize">The bounding X size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="ySize">The bounding Y size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="zSize">The bounding Z size, in voxels, of the .vox file or of other .vox files that should render at the same pixel size.</param>
        /// <param name="dir">The Direction enum that specifies which way the X-axis should point.</param>
        /// <returns>A Bitmap view of the voxels in isometric pixel view.</returns>
        private static Bitmap renderSmart(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, Direction dir, Outlining o, bool shrink)
        {

            if (xSize <= sizex) xSize = (byte)(sizex);
            if (ySize <= sizey) ySize = (byte)(sizey);
            if (zSize <= sizez) zSize = (byte)(sizez);
            if (xSize % 2 == 1) xSize++;
            if (ySize % 2 == 1) ySize++;
            if (zSize % 2 == 1) zSize++;


            byte tsx = (byte)sizex, tsy = (byte)sizey;
            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = (xSize + ySize) * 2 + 8;
            int bHeight = (xSize + ySize) + zSize * 3 + 8;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            byte[] bareValues = new byte[numBytes];
            bareValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];

            switch (dir)
            {
                case Direction.SE:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            vls[i].x = (byte)(voxels[i].x + (hSize - tsx) / 2);
                            vls[i].y = (byte)(voxels[i].y + (hSize - tsy) / 2);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.SW:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY) + (ySize / 2));
                            vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.NW:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case Direction.NE:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].y = (byte)(tempX + (xSize / 2));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
            }
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * xSize * 2 - v.y + v.z * xSize * ySize * 4 - (((253 - v.color) / 4 == 25) ? xSize * ySize * zSize * 8 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int p = 0;
                int mod_color = vx.color - 1;
                /*
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[currentColor][0],  0,  0,  0, 0},
   new float[] {0,  colors[currentColor][1],  0,  0, 0},
   new float[] {0,  0,  colors[currentColor][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);*/



                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        p = voxelToPixel(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, xSize, ySize, zSize);

                        if (argbValues[p] == 0)
                        {
                            zbuffer[p] = vx.z + vx.x - vx.y;
                            argbValues[p] = rendered[mod_color][i + j * 16];
                            if (outlineValues[p] == 0)
                                outlineValues[p] = rendered[mod_color][i + 64]; //(argbValues[p] * 1.2 + 2 < 255) ? (byte)(argbValues[p] * 1.2 + 2) : (byte)255;

                        }
                    }
                }

            }
            switch(o)
            {
                case Outlining.Full:
                    {
                        for (int i = 3; i < numBytes; i += 4)
                        {
                            if (argbValues[i] > 0)
                            {

                                if (i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if (i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if (i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if (i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if (i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if (i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if (i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if (i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if (i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if (i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if (i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if (i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if (i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if (i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if (i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if (i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if (i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if (i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if (i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if (i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for (int i = 3; i < numBytes; i += 4)
                        {
                            if (argbValues[i] > 0)
                            {

                                if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if (i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if (i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for (int i = 3; i < numBytes; i += 4)
                        {
                            if (argbValues[i] > 0)
                            {

                                if (i + 4 >= 0 && i + 4 < argbValues.Length && argbValues[i + 4] == 0) {} else if (i + 4 >= 0 && i + 4 < argbValues.Length && barePositions[i + 4] == false && zbuffer[i] - 2 > zbuffer[i + 4]) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if (i - 4 >= 0 && i - 4 < argbValues.Length && argbValues[i - 4] == 0) {} else if (i - 4 >= 0 && i - 4 < argbValues.Length && barePositions[i - 4] == false && zbuffer[i] - 2 > zbuffer[i - 4]) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && argbValues[i + bmpData.Stride] == 0) {} else if (i + bmpData.Stride >= 0 && i + bmpData.Stride < argbValues.Length && barePositions[i + bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride]) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && argbValues[i - bmpData.Stride] == 0) {} else if (i - bmpData.Stride >= 0 && i - bmpData.Stride < argbValues.Length && barePositions[i - bmpData.Stride] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride]) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && argbValues[i + bmpData.Stride + 4] == 0) {} else if (i + bmpData.Stride + 4 >= 0 && i + bmpData.Stride + 4 < argbValues.Length && barePositions[i + bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 4]) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && argbValues[i - bmpData.Stride - 4] == 0) {} else if (i - bmpData.Stride - 4 >= 0 && i - bmpData.Stride - 4 < argbValues.Length && barePositions[i - bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 4]) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && argbValues[i + bmpData.Stride - 4] == 0) {} else if (i + bmpData.Stride - 4 >= 0 && i + bmpData.Stride - 4 < argbValues.Length && barePositions[i + bmpData.Stride - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 4]) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && argbValues[i - bmpData.Stride + 4] == 0) {} else if (i - bmpData.Stride + 4 >= 0 && i - bmpData.Stride + 4 < argbValues.Length && barePositions[i - bmpData.Stride + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 4]) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if (i + 8 >= 0 && i + 8 < argbValues.Length && argbValues[i + 8] == 0) {} else if (i + 8 >= 0 && i + 8 < argbValues.Length && barePositions[i + 8] == false && zbuffer[i] - 2 > zbuffer[i + 8]) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if (i - 8 >= 0 && i - 8 < argbValues.Length && argbValues[i - 8] == 0) {} else if (i - 8 >= 0 && i - 8 < argbValues.Length && barePositions[i - 8] == false && zbuffer[i] - 2 > zbuffer[i - 8]) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && argbValues[i + bmpData.Stride * 2] == 0) {} else if (i + bmpData.Stride * 2 >= 0 && i + bmpData.Stride * 2 < argbValues.Length && barePositions[i + bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2]) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && argbValues[i - bmpData.Stride * 2] == 0) {} else if (i - bmpData.Stride * 2 >= 0 && i - bmpData.Stride * 2 < argbValues.Length && barePositions[i - bmpData.Stride * 2] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2]) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && argbValues[i + bmpData.Stride + 8] == 0) {} else if (i + bmpData.Stride + 8 >= 0 && i + bmpData.Stride + 8 < argbValues.Length && barePositions[i + bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride + 8]) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && argbValues[i - bmpData.Stride + 8] == 0) {} else if (i - bmpData.Stride + 8 >= 0 && i - bmpData.Stride + 8 < argbValues.Length && barePositions[i - bmpData.Stride + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride + 8]) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && argbValues[i + bmpData.Stride - 8] == 0) {} else if (i + bmpData.Stride - 8 >= 0 && i + bmpData.Stride - 8 < argbValues.Length && barePositions[i + bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride - 8]) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && argbValues[i - bmpData.Stride - 8] == 0) {} else if (i - bmpData.Stride - 8 >= 0 && i - bmpData.Stride - 8 < argbValues.Length && barePositions[i - bmpData.Stride - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride - 8]) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 8] == 0) {} else if (i + bmpData.Stride * 2 + 8 >= 0 && i + bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 8]) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 + 4] == 0) {} else if (i + bmpData.Stride * 2 + 4 >= 0 && i + bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 + 4]) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 4] == 0) {} else if (i + bmpData.Stride * 2 - 4 >= 0 && i + bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 4]) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i + bmpData.Stride * 2 - 8] == 0) {} else if (i + bmpData.Stride * 2 - 8 >= 0 && i + bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i + bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i + bmpData.Stride * 2 - 8]) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 8] == 0) {} else if (i - bmpData.Stride * 2 + 8 >= 0 && i - bmpData.Stride * 2 + 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 8]) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 + 4] == 0) {} else if (i - bmpData.Stride * 2 + 4 >= 0 && i - bmpData.Stride * 2 + 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 + 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 + 4]) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 4] == 0) {} else if (i - bmpData.Stride * 2 - 4 >= 0 && i - bmpData.Stride * 2 - 4 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 4] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 4]) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && argbValues[i - bmpData.Stride * 2 - 8] == 0) {} else if (i - bmpData.Stride * 2 - 8 >= 0 && i - bmpData.Stride * 2 - 8 < argbValues.Length && barePositions[i - bmpData.Stride * 2 - 8] == false && zbuffer[i] - 2 > zbuffer[i - bmpData.Stride * 2 - 8]) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 0) // && argbValues[i] <= 255 * VoxelLogic.flat_alpha
                    argbValues[i] = 255;
                if (outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            if (!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 2, bHeight / 2, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 2, bHeight / 2);
                g2.Dispose();
                return b2;
            }
        }


        private static Bitmap renderSmartOrtho(MagicaVoxelData[] voxels, byte xSize, byte ySize, byte zSize, OrthoDirection dir, Outlining o, bool shrink)
        {

            if (xSize <= sizex) xSize = (byte)(sizex);
            if (ySize <= sizey) ySize = (byte)(sizey);
            if (zSize <= sizez) zSize = (byte)(sizez);
            if (xSize % 2 == 1) xSize++;
            if (ySize % 2 == 1) ySize++;
            if (zSize % 2 == 1) zSize++;


            byte tsx = (byte)sizex, tsy = (byte)sizey;
            
            byte hSize = Math.Max(ySize, xSize);

            xSize = hSize;
            ySize = hSize;

            int bWidth = (xSize + ySize) * 2 + 8;
            int bHeight = (xSize + ySize) + zSize * 3 + 8;
            Bitmap bmp = new Bitmap(bWidth, bHeight, PixelFormat.Format32bppArgb);

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] argbValues = new byte[numBytes];
            argbValues.Fill<byte>(0);
            byte[] outlineValues = new byte[numBytes];
            outlineValues.Fill<byte>(0);
            bool[] barePositions = new bool[numBytes];
            barePositions.Fill<bool>(false);
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];

            switch (dir)
            {
                case OrthoDirection.S:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            vls[i].x = (byte)(voxels[i].x + (hSize - tsx) / 2);
                            vls[i].y = (byte)(voxels[i].y + (hSize - tsy) / 2);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.W:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY) + (ySize / 2));
                            vls[i].y = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.N:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempX * -1) + (xSize / 2) - 1);
                            vls[i].y = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
                case OrthoDirection.E:
                    {
                        for (int i = 0; i < voxels.Length; i++)
                        {
                            byte tempX = (byte)(voxels[i].x + ((hSize - tsx) / 2) - (xSize / 2));
                            byte tempY = (byte)(voxels[i].y + ((hSize - tsy) / 2) - (ySize / 2));
                            vls[i].x = (byte)((tempY * -1) + (ySize / 2) - 1);
                            vls[i].y = (byte)(tempX + (xSize / 2));
                            vls[i].z = voxels[i].z;
                            vls[i].color = voxels[i].color;
                        }
                    }
                    break;
            } 

            int[] xbuffer = new int[numBytes];
            xbuffer.Fill<int>(-999);
            int[] zbuffer = new int[numBytes];
            zbuffer.Fill<int>(-999);

            foreach (MagicaVoxelData vx in vls.OrderByDescending(v => v.x * 128 + v.y + v.z * 128 * 128 - ((v.color == 249 - 96) ? 128 * 128 * 128 : 0))) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int p = 0;
                int mod_color = vx.color - 1;

                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        p = voxelToPixelOrtho(i, j, vx.x, vx.y, vx.z, mod_color, bmpData.Stride, xSize, ySize, zSize);
                        if (argbValues[p] == 0)
                        {
                            argbValues[p] = renderedOrtho[mod_color][i + j * 12];
                            zbuffer[p] = vx.z;
                            xbuffer[p] = vx.x;
                            if (outlineValues[p] == 0)
                                outlineValues[p] = renderedOrtho[mod_color][i + 48];
                        }
                    }
                }

            }
            switch (o)
            {
                case Outlining.Full:
                    {
                        for (int i = 3; i < numBytes; i += 4)
                        {
                            if (argbValues[i] > 0)
                            {

                                if (argbValues[i + 4] == 0) { outlineValues[i + 4] = 255; } else if ((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - 4] == 0) { outlineValues[i - 4] = 255; } else if ((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride] == 0) { outlineValues[i + bmpData.Stride] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride] == 0) { outlineValues[i - bmpData.Stride] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride + 4] == 0) { outlineValues[i + bmpData.Stride + 4] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride - 4] == 0) { outlineValues[i - bmpData.Stride - 4] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride - 4] == 0) { outlineValues[i + bmpData.Stride - 4] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride + 4] == 0) { outlineValues[i - bmpData.Stride + 4] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if (argbValues[i + 8] == 0) { outlineValues[i + 8] = 255; } else if ((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - 8] == 0) { outlineValues[i - 8] = 255; } else if ((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2] == 0) { outlineValues[i + bmpData.Stride * 2] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2] == 0) { outlineValues[i - bmpData.Stride * 2] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride + 8] == 0) { outlineValues[i + bmpData.Stride + 8] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride + 8] == 0) { outlineValues[i - bmpData.Stride + 8] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride - 8] == 0) { outlineValues[i + bmpData.Stride - 8] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride - 8] == 0) { outlineValues[i - bmpData.Stride - 8] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 + 8] == 0) { outlineValues[i + bmpData.Stride * 2 + 8] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 + 4] == 0) { outlineValues[i + bmpData.Stride * 2 + 4] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 - 4] == 0) { outlineValues[i + bmpData.Stride * 2 - 4] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 - 8] == 0) { outlineValues[i + bmpData.Stride * 2 - 8] = 255; } else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 + 8] == 0) { outlineValues[i - bmpData.Stride * 2 + 8] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 + 4] == 0) { outlineValues[i - bmpData.Stride * 2 + 4] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 - 4] == 0) { outlineValues[i - bmpData.Stride * 2 - 4] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 - 8] == 0) { outlineValues[i - bmpData.Stride * 2 - 8] = 255; } else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Light:
                    {
                        for (int i = 3; i < numBytes; i += 4)
                        {
                            if (argbValues[i] > 0)
                            {

                                if ((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if ((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 4) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
                case Outlining.Partial:
                    {
                        for (int i = 3; i < numBytes; i += 4)
                        {
                            if (argbValues[i] > 0)
                            {

                                if (argbValues[i + 4] == 0) {} else if ((zbuffer[i] - zbuffer[i + 4]) > 1 || (xbuffer[i] - xbuffer[i + 4]) > 3) { argbValues[i + 4] = 255; argbValues[i + 4 - 1] = outlineValues[i - 1]; argbValues[i + 4 - 2] = outlineValues[i - 2]; argbValues[i + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - 4] == 0) {} else if ((zbuffer[i] - zbuffer[i - 4]) > 1 || (xbuffer[i] - xbuffer[i - 4]) > 3) { argbValues[i - 4] = 255; argbValues[i - 4 - 1] = outlineValues[i - 1]; argbValues[i - 4 - 2] = outlineValues[i - 2]; argbValues[i - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride]) > 3) { argbValues[i + bmpData.Stride] = 255; argbValues[i + bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride]) <= 0)) { argbValues[i - bmpData.Stride] = 255; argbValues[i - bmpData.Stride - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride + 4] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride + 4]) > 3) { argbValues[i + bmpData.Stride + 4] = 255; argbValues[i + bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride - 4] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 4]) <= 0)) { argbValues[i - bmpData.Stride - 4] = 255; argbValues[i - bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride - 4] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride - 4]) > 3) { argbValues[i + bmpData.Stride - 4] = 255; argbValues[i + bmpData.Stride - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride + 4] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 4]) <= 0)) { argbValues[i - bmpData.Stride + 4] = 255; argbValues[i - bmpData.Stride + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 4 - 3] = outlineValues[i - 3]; }

                                if (argbValues[i + 8] == 0) {} else if ((zbuffer[i] - zbuffer[i + 8]) > 1 || (xbuffer[i] - xbuffer[i + 8]) > 3) { argbValues[i + 8] = 255; argbValues[i + 8 - 1] = outlineValues[i - 1]; argbValues[i + 8 - 2] = outlineValues[i - 2]; argbValues[i + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - 8] == 0) {} else if ((zbuffer[i] - zbuffer[i - 8]) > 1 || (xbuffer[i] - xbuffer[i - 8]) > 3) { argbValues[i - 8] = 255; argbValues[i - 8 - 1] = outlineValues[i - 1]; argbValues[i - 8 - 2] = outlineValues[i - 2]; argbValues[i - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2]) > 3) { argbValues[i + bmpData.Stride * 2] = 255; argbValues[i + bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2]) <= 0)) { argbValues[i - bmpData.Stride * 2] = 255; argbValues[i - bmpData.Stride * 2 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride + 8] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride + 8]) > 3) { argbValues[i + bmpData.Stride + 8] = 255; argbValues[i + bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride + 8] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride + 8]) <= 0)) { argbValues[i - bmpData.Stride + 8] = 255; argbValues[i - bmpData.Stride + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride - 8] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride - 8]) > 3) { argbValues[i + bmpData.Stride - 8] = 255; argbValues[i + bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride - 8] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) > 1 || ((xbuffer[i] - xbuffer[i - bmpData.Stride - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride - 8]) <= 0)) { argbValues[i - bmpData.Stride - 8] = 255; argbValues[i - bmpData.Stride - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 + 8] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 8]) > 5) { argbValues[i + bmpData.Stride * 2 + 8] = 255; argbValues[i + bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 + 4] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 + 4]) > 5) { argbValues[i + bmpData.Stride * 2 + 4] = 255; argbValues[i + bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 - 4] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 4]) > 5) { argbValues[i + bmpData.Stride * 2 - 4] = 255; argbValues[i + bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i + bmpData.Stride * 2 - 8] == 0) {} else if ((zbuffer[i] - zbuffer[i + bmpData.Stride * 2 - 8]) > 5) { argbValues[i + bmpData.Stride * 2 - 8] = 255; argbValues[i + bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i + bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i + bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 + 8] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 8] = 255; argbValues[i - bmpData.Stride * 2 + 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 8 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 + 4] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 + 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 + 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 + 4] = 255; argbValues[i - bmpData.Stride * 2 + 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 + 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 + 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 - 4] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 4]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 4]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 4] = 255; argbValues[i - bmpData.Stride * 2 - 4 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 4 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 4 - 3] = outlineValues[i - 3]; }
                                if (argbValues[i - bmpData.Stride * 2 - 8] == 0) {} else if ((zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) > 0 || ((xbuffer[i] - xbuffer[i - bmpData.Stride * 2 - 8]) > 2 && (zbuffer[i] - zbuffer[i - bmpData.Stride * 2 - 8]) <= 0)) { argbValues[i - bmpData.Stride * 2 - 8] = 255; argbValues[i - bmpData.Stride * 2 - 8 - 1] = outlineValues[i - 1]; argbValues[i - bmpData.Stride * 2 - 8 - 2] = outlineValues[i - 2]; argbValues[i - bmpData.Stride * 2 - 8 - 3] = outlineValues[i - 3]; }
                            }
                        }
                    }
                    break;
            }

            for (int i = 3; i < numBytes; i += 4)
            {
                if (argbValues[i] > 0) // && argbValues[i] <= 255 * VoxelLogic.flat_alpha
                    argbValues[i] = 255;
                if (outlineValues[i] == 255) argbValues[i] = 255;
            }

            Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            if (!shrink)
            {
                return bmp;
            }
            else
            {
                Graphics g = Graphics.FromImage(bmp);
                Bitmap b2 = new Bitmap(bWidth / 2, bHeight / 2, PixelFormat.Format32bppArgb);
                Graphics g2 = Graphics.FromImage(b2);
                g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp.Clone(new Rectangle(0, 0, bWidth, bHeight), bmp.PixelFormat), 0, 0, bWidth / 2, bHeight / 2);
                g2.Dispose();
                return b2;
            }
        }



        /*
        public static Bitmap processSingleOutlined(MagicaVoxelData[] parsed, byte xSize, byte ySize, byte zSize, Direction dir)
        {
            Graphics g;
            Bitmap b, o;
            b = render(parsed, xSize, ySize, zSize, dir);
            o = renderOutline(parsed, xSize, ySize, zSize, dir);
            g = Graphics.FromImage(o);
            g.DrawImage(b, 2,6);
            return o;
        }
        private static void processUnitOutlined(MagicaVoxelData[] parsed, string u, byte xSize, byte ySize, byte zSize)
        {
            u = u.Substring(0, u.Length - 4);
            System.IO.Directory.CreateDirectory(u);

            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.SE).Save(u + "/" + u + "_outline_SE" + ".png", ImageFormat.Png); //se
            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.SW).Save(u + "/" + u + "_outline_SW" + ".png", ImageFormat.Png); //sw
            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.NW).Save(u + "/" + u + "_outline_NW" + ".png", ImageFormat.Png); //nw
            processSingleOutlined(parsed, xSize, ySize, zSize, Direction.NE).Save(u + "/" + u + "_outline_NE" + ".png", ImageFormat.Png); //ne

        }
        private static void processUnit(MagicaVoxelData[] parsed, string u, byte xSize, byte ySize, byte zSize)
        {
            u = u.Substring(0, u.Length - 4);
            System.IO.Directory.CreateDirectory(u);

            render(parsed, xSize, ySize, zSize, Direction.SE).Save(u + "/" + u + "_SE" + ".png", ImageFormat.Png); //se
            render(parsed, xSize, ySize, zSize, Direction.SW).Save(u + "/" + u + "_SW" + ".png", ImageFormat.Png); //sw
            render(parsed, xSize, ySize, zSize, Direction.NW).Save(u + "/" + u + "_NW" + ".png", ImageFormat.Png); //nw
            render(parsed, xSize, ySize, zSize, Direction.NE).Save(u + "/" + u + "_NE" + ".png", ImageFormat.Png); //ne

        }
        */
        public static void processUnitSmart(MagicaVoxelData[] parsed, string u, byte xSize, byte ySize, byte zSize, Outlining o)
        {
            u = u.Substring(0, u.Length - 4);
            System.IO.Directory.CreateDirectory(u);

            renderSmart(parsed, xSize, ySize, zSize, Direction.SE, o, false).Save(u + "/" + u + "_Big_SE" + ".png", ImageFormat.Png); //se
            renderSmart(parsed, xSize, ySize, zSize, Direction.SW, o, false).Save(u + "/" + u + "_Big_SW" + ".png", ImageFormat.Png); //sw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NW, o, false).Save(u + "/" + u + "_Big_NW" + ".png", ImageFormat.Png); //nw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NE, o, false).Save(u + "/" + u + "_Big_NE" + ".png", ImageFormat.Png); //ne

            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.S, o, false).Save(u + "/" + u + "_Big_S" + ".png", ImageFormat.Png); //s
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.W, o, false).Save(u + "/" + u + "_Big_W" + ".png", ImageFormat.Png); //w
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.N, o, false).Save(u + "/" + u + "_Big_N" + ".png", ImageFormat.Png); //n
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.E, o, false).Save(u + "/" + u + "_Big_E" + ".png", ImageFormat.Png); //e

            renderSmart(parsed, xSize, ySize, zSize, Direction.SE, o, true).Save(u + "/" + u + "_SE" + ".png", ImageFormat.Png); //se
            renderSmart(parsed, xSize, ySize, zSize, Direction.SW, o, true).Save(u + "/" + u + "_SW" + ".png", ImageFormat.Png); //sw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NW, o, true).Save(u + "/" + u + "_NW" + ".png", ImageFormat.Png); //nw
            renderSmart(parsed, xSize, ySize, zSize, Direction.NE, o, true).Save(u + "/" + u + "_NE" + ".png", ImageFormat.Png); //ne

            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.S, o, true).Save(u + "/" + u + "_S" + ".png", ImageFormat.Png); //s
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.W, o, true).Save(u + "/" + u + "_W" + ".png", ImageFormat.Png); //w
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.N, o, true).Save(u + "/" + u + "_N" + ".png", ImageFormat.Png); //n
            renderSmartOrtho(parsed, xSize, ySize, zSize, OrthoDirection.E, o, true).Save(u + "/" + u + "_E" + ".png", ImageFormat.Png); //e
        }
        static void Main(string[] args)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream imageStream = assembly.GetManifestResourceStream("IsoVoxel.cube_soft.png");
            cube = new Bitmap(imageStream);
            imageStream = assembly.GetManifestResourceStream("IsoVoxel.cube_ortho.png");
            ortho = new Bitmap(imageStream);
            string voxfile = "Truck.vox";
            if (args.Length >= 1)
            {
                voxfile = args[0];
            }
            else
            {
                Console.WriteLine("Args: 'file x y z o'. file is a MagicaVoxel .vox file, x y z are sizes,");
                Console.WriteLine("o must be one of the following, changing how outlines are drawn (default full):");
                Console.WriteLine("  outline=full    Draw a black outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=light   Draw a shaded outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=partial Draw no outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=none    Draw no outlines.");
                Console.WriteLine("x y z o are all optional, but o must be the last if present.");
                Console.WriteLine("Defaults: runs on Truck.vox with x y z set by the model, o is full.");
                Console.WriteLine("Given no arguments, running on Truck.vox ...");
            }
            byte x = 0, y = 0, z = 0;
            Outlining o = Outlining.Full;
            int al = args.Length;
            if (al >= 2 && args.Last().StartsWith("outline", StringComparison.OrdinalIgnoreCase))
            {
                o = GetOutlining(args.Last().ToLowerInvariant().Split('=').Last());
                --al;
            }
            try
            {
                if (al >= 2)
                {
                    x = byte.Parse(args[1]);
                }
                if (al >= 3)
                {
                    y = byte.Parse(args[2]);
                }
                if (al >= 4)
                {
                    z = byte.Parse(args[3]);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Args: 'file x y z o'. file is a MagicaVoxel .vox file, x y z are sizes,");
                Console.WriteLine("o must be one of the following, changing how outlines are drawn (default full):");
                Console.WriteLine("  outline=full    Draw a black outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=light   Draw a shaded outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=partial Draw no outer outline and shaded inner outlines.");
                Console.WriteLine("  outline=none    Draw no outlines.");
                Console.WriteLine("x y z o are all optional, but o must be the last if present.");
                Console.WriteLine("Defaults: runs on Truck.vox with x y z set by the model, o is full.");
            }
            BinaryReader bin = new BinaryReader(File.Open(voxfile, FileMode.Open));
            MagicaVoxelData[] mvd = FromMagica(bin);
            rendered = storeColorCubes();
            renderedOrtho = storeColorCubesOrtho();
            processUnitSmart(mvd, voxfile, x, y, z, o);
            bin.Close();
        }

        private static Outlining GetOutlining(string s)
        {
            switch(s)
            {
                case "full": return Outlining.Full;
                case "light": return Outlining.Light;
                case "partial": return Outlining.Partial;
                case "none": return Outlining.None;
            }
            return Outlining.Full;
        }
    }
}
