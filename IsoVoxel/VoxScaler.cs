using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IsoVoxel
{
    class VoxScaler
    {

        /// <summary>
        /// Write a MagicaVoxel .vox format file from a List of MagicaVoxelData and a palette from this program to use.
        /// </summary>
        /// <param name="filename">Name of the file to write.</param>
        /// <param name="voxelData">The voxels in indexed-color mode.</param>
        /// <param name="paletteKind">Currently 'X', 'W', "K_ALLY", or "K_OTHER", referring to the different styles of indexed color to use.</param>
        /// <param name="palette">Which palette to use.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static void WriteVOX(string filename, byte[,,] voxelData)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below

            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            Stream stream = File.OpenWrite(filename);
            BinaryWriter bin = new BinaryWriter(stream);
            bool[,,] taken = new bool[xSize, ySize, zSize];

            List<byte> voxelsRaw = new List<byte>();

            byte[] colors = new byte[1024];
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        if(x < xSize && y < ySize && z < zSize && !taken[x, y, z] && voxelData[x, y, z] != 0)
                        {
                            voxelsRaw.Add((byte)x);
                            voxelsRaw.Add((byte)y);
                            voxelsRaw.Add((byte)z);
                            voxelsRaw.Add(voxelData[x, y, z]);
                            taken[x, y, z] = true;
                        }
                    }
                }
            }
            for(int i = 1; i < 255; i++)
            {

                colors[(i) * 4] = PaletteDraw.byteColors[i][0];
                colors[(i) * 4 + 1] = PaletteDraw.byteColors[i][1];
                colors[(i) * 4 + 2] = PaletteDraw.byteColors[i][2];
                colors[(i) * 4 + 3] = PaletteDraw.byteColors[i][3];
                /*
                colors[(i - 1) * 4 + 0] = PaletteDraw.renderedFace[i][0][2 + 4 * 2];
                colors[(i - 1) * 4 + 1] = PaletteDraw.renderedFace[i][0][1 + 4 * 2];
                colors[(i - 1) * 4 + 2] = PaletteDraw.renderedFace[i][0][0 + 4 * 2];
                colors[(i - 1) * 4 + 3] = PaletteDraw.renderedFace[i][0][3 + 4 * 2];
                */
            }
            

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            bin.Write("VOX ".ToCharArray());
            // current version?
            bin.Write((int)150);

            bin.Write("MAIN".ToCharArray());
            bin.Write((int)0);
            bin.Write((int)12 + 12 + 12 + 4 + voxelsRaw.Count + 12 + 1024);

            bin.Write("SIZE".ToCharArray());
            bin.Write((int)12);
            bin.Write((int)0);
            bin.Write(xSize);
            bin.Write(ySize);
            bin.Write(zSize);

            bin.Write("RGBA".ToCharArray());
            bin.Write((int)1024);
            bin.Write((int)0);
            bin.Write(colors);

            bin.Write("XYZI".ToCharArray());
            bin.Write((int)(4 + voxelsRaw.Count));
            bin.Write((int)0);
            bin.Write((int)(voxelsRaw.Count / 4));
            bin.Write(voxelsRaw.ToArray());

            bin.Flush();
            bin.Close();
        }

        /*
        
                    VoxelLogic.WriteVOX(folder + moniker + "_palette" + palette + "_" + i + ".vox",
                        TransformLogic.FillInterior(
                                //TransformLogic.RunSurfaceCA(
                                TransformLogic.RunCA(
                                    FaceLogic.FaceArrayToByteArray(
                                        //FaceLogic.DoubleSize(
                                        FaceLogic.GetFaces(
                                                TransformLogic.RunThinningCA(
                                                    posed[(int)(frames[i][0])]
                                                    .Interpolate(posed[(int)(frames[i][1])], frames[i][2])
                                                    .Translate(10 * OrthoSingle.multiplier, 10 * OrthoSingle.multiplier, 0, (alt) ? "Left_Lower_Leg" : "Left_Leg")
                                                    .Finalize(),
                                                2, true)//1 + OrthoSingle.multiplier * OrthoSingle.bonus / 2)
                                            )
                                        
                                    ),
                                2, true)
                        ),
                        "W", palette);
        */
        public static byte[,,] Scale(MagicaVoxelData[] voxels)
        {
            return TransformLogic.FillInterior(
                                    FaceLogic.FaceArrayToByteArray(
                                        FaceLogic.DoubleSize(
                                            FaceLogic.GetFaces(voxels, PaletteDraw.sizex, PaletteDraw.sizey, PaletteDraw.sizez)
                                        )
                                    )
                        );
            /*
            
            return TransformLogic.FillInterior(
                                //TransformLogic.RunSurfaceCA(
                                TransformLogic.RunCA(
                                    FaceLogic.FaceArrayToByteArray(
                                        //FaceLogic.DoubleSize(
                                        FaceLogic.GetFaces(
                                                TransformLogic.RunThinningCA(
                                                    TransformLogic.ScalePartial(TransformLogic.VoxListToArray(voxels, PaletteDraw.sizex, PaletteDraw.sizey, PaletteDraw.sizez), 2.0),
                                                2)//1 + OrthoSingle.multiplier * OrthoSingle.bonus / 2)
                                            )

                                    ),
                                2)
                        );
            */
        }


        static void Main(string[] args)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream imageStream = assembly.GetManifestResourceStream("IsoVoxel.white.png");
            string voxfile = "Zombie.vox";
            Console.WriteLine("This program will double the size of a .vox model.");
            Console.WriteLine("It will edit the model in place, so have a backup or copy!");
            Console.WriteLine("It can increase a model to a large, non-standard size (over 126x126x126).");
            Console.WriteLine("Drag and drop a file on this window, then hit enter: ");
            voxfile = Console.ReadLine();
            voxfile = voxfile.Trim('"');
            Console.WriteLine("Processing file " + voxfile + " and will edit that same file.");
            PaletteDraw.white = new Bitmap(imageStream);
            BinaryReader bin = new BinaryReader(File.Open(voxfile, FileMode.Open));
            MagicaVoxelData[] mvd = PaletteDraw.FromMagica(bin);
            PaletteDraw.storeColorCubesFaces();
            bin.Close();
            WriteVOX(voxfile, Scale(mvd));

        }
    }
}
