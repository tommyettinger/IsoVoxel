using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsoVoxel
{

    public enum Slope
    {
        Cube = 0x1,
        BrightTop = 0x8,
        DimTop = 0x10,
        BrightDim = 0x20,
        BrightDimTop = 0x40,
        BrightBottom = 0x80,
        DimBottom = 0x100,
        BrightDimBottom = 0x200,
        BrightBack = 0x400,
        DimBack = 0x800,
        BrightTopBack = 0x1000,
        DimTopBack = 0x2000,
        BrightBottomBack = 0x4000,
        DimBottomBack = 0x8000,
        BackBack = 0x10000
    }
    public class FaceVoxel
    {
        public MagicaVoxelData vox;
        public Slope slope;

        public FaceVoxel(MagicaVoxelData v, Slope slp)
        {
            vox = v;
            slope = slp;
        }
    }
    public class TransformLogic
    {
        public static byte[,,] VoxListToArray(IEnumerable<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize, ySize, zSize];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                data[mvd.x, mvd.y, mvd.z] = mvd.color;
            }
            return data;
        }
        public static List<MagicaVoxelData> VoxArrayToList(byte[,,] voxelData)
        {
            List<MagicaVoxelData> vlist = new List<MagicaVoxelData>(voxelData.Length);
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        if(voxelData[x, y, z] > 0)
                            vlist.Add(new MagicaVoxelData { x = x, y = y, z = z, color = voxelData[x, y, z] });
                    }
                }
            }
            return vlist;
        }

        public static byte[,,] VoxListToLargerArray(IEnumerable<MagicaVoxelData> voxelData, int multiplier, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize * multiplier, ySize * multiplier, zSize * multiplier];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                if(mvd.color == 0)
                    continue;
                for(int x = 0; x < multiplier; x++)
                {
                    for(int y = 0; y < multiplier; y++)
                    {
                        for(int z = 0; z < multiplier; z++)
                        {
                            if(mvd.x * multiplier + x < xSize * multiplier && mvd.y * multiplier + y < ySize * multiplier && mvd.z * multiplier + z < zSize * multiplier)
                               data[mvd.x * multiplier + x, mvd.y * multiplier + y, mvd.z * multiplier + z] = mvd.color;
                        }
                    }
                }
            }
            return data;
        }
        public static byte[,,] VoxListToLargerArray(IEnumerable<MagicaVoxelData> voxelData, int multiplier, int originalX, int originalY, int xSize, int ySize, int zSize)
        {
            byte[,,] data = new byte[xSize * multiplier, ySize * multiplier, zSize * multiplier];
            foreach(MagicaVoxelData mvd in voxelData)
            {
                for(int x = 0; x < multiplier; x++)
                {
                    for(int y = 0; y < multiplier; y++)
                    {
                        for(int z = 0; z < multiplier; z++)
                        {
                            if(mvd.color != 0 &&
                                (mvd.x + (xSize - originalX) / 2) * multiplier + x < xSize * multiplier &&
                                (mvd.y + (ySize - originalY) / 2) * multiplier + y < ySize * multiplier &&
                                mvd.z * multiplier + z < zSize * multiplier)
                                data[(mvd.x + (xSize - originalX) / 2) * multiplier + x, (mvd.y + (ySize - originalY) / 2) * multiplier + y, mvd.z * multiplier + z] = mvd.color;
                        }
                    }
                }
            }
            return data;
        }
        public static byte[,,] Translate(byte[,,] voxelData, int xMove, int yMove, int zMove)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            byte[,,] vs = new byte[xSize, ySize, zSize];

            int xmin = 0, ymin = 0, zmin = 0,
                xmax = xSize, ymax = ySize, zmax = zSize;
            if(xMove < 0)
                xmin -= xMove;
            else if(xMove > 0)
                xmax -= xMove;
            if(yMove < 0)
                ymin -= yMove;
            else if(yMove > 0)
                ymax -= yMove;
            if(zMove < 0)
                zmin -= zMove;
            else if(zMove > 0)
                zmax -= zMove;

            for(int x = xmin; x < xmax; x++)
            {
                for(int y = ymin; y < ymax; y++)
                {
                    for(int z = zmin; z < zmax; z++)
                    {
                        vs[x + xMove, y + yMove, z + zMove] = voxelData[x, y, z];
                    }
                }
            }

            return vs;

        }
        public static byte[,,] ExpandBounds(byte[,,] voxelData, int newXSize, int newYSize, int newZSize)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            byte[,,] vs = new byte[newXSize, newYSize, newZSize];

            for(int x = 0; x < xSize; x++)
            {
                if(x + (newXSize - xSize) / 2 >= 0 && x + (newXSize - xSize) / 2 < newXSize)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        if(y + (newYSize - ySize) / 2 >= 0 && y + (newYSize - ySize) / 2 < newYSize)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                if(z < newZSize)
                                {
                                    vs[x + (newXSize - xSize) / 2, y + (newYSize - ySize) / 2, z] = voxelData[x, y, z];
                                }
                            }
                        }
                    }

                }
            }

            return vs;

        }
        public static byte[,,] RunCA(byte[,,] voxelData, int smoothLevel)
        {
            if(smoothLevel <= 1)
                return voxelData;
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            //Dictionary<byte, int> colorCount = new Dictionary<byte, int>();
            int[] colorCount = new int[256];
            byte[][,,] vs = new byte[smoothLevel][,,];
            vs[0] = voxelData.Replicate();
            for(int v = 1; v < smoothLevel; v++)
            {
                vs[v] = new byte[xSize, ySize, zSize];
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        for(int z = 0; z < zSize; z++)
                        {
                            Array.Clear(colorCount, 0, 256);
                            int emptyCount = 0;
                            if(x == 0 || y == 0 || z == 0 || x == xSize - 1 || y == ySize - 1 || z == zSize - 1)
                            {
                                colorCount[vs[v - 1][x, y, z]] = 10000;
                            }
                            else
                            {
                                for(int xx = -1; xx < 2; xx++)
                                {
                                    for(int yy = -1; yy < 2; yy++)
                                    {
                                        for(int zz = -1; zz < 2; zz++)
                                        {
                                            byte smallColor = vs[v - 1][x + xx, y + yy, z + zz];
                                            if(smallColor == 0)
                                            {
                                                emptyCount++;
                                            }
                                            else {
                                                colorCount[smallColor]++;
                                            }
                                        }
                                    }
                                }
                            }
                            if(emptyCount >= 18)
                                vs[v][x, y, z] = 0;
                            else
                            {
                                int max = 0, cc = colorCount[0], tmp;
                                for(int idx = 1; idx < 256; idx++)
                                {
                                    tmp = colorCount[idx];
                                    if(tmp > cc)
                                    {
                                        cc = tmp;
                                        max = idx;
                                    }
                                }
                                vs[v][x, y, z] = (byte)max;
                            }
                        }
                    }
                }
            }
            return vs[smoothLevel - 1];
        }
        public static List<MagicaVoxelData> VoxArrayToListSmoothed(byte[,,] voxelData)
        {
            return VoxArrayToList(RunCA(voxelData, 3));
        }

        public static byte[,,] SealGaps(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte[,,] vls = new byte[xSize, ySize, zSize];// = voxelData.Replicate();
            for(int z = 0; z < zSize - 1; z++)
            {
                for(int y = 1; y < ySize - 1; y++)
                {
                    byte currentFill = 0;
                    for(int x = xSize - 2; x > 0; x--)
                    {
                        int clr = 253 - voxelData[x, y, z] / 4;

                        if(voxelData[x + 1, y, z] == 0 || voxelData[x - 1, y, z] == 0 || voxelData[x, y, z + 1] == 0)
                        {
                            //if(voxelData[x, y, z] > 253 - 57 * 4 || voxelData[x, y, z] == 0)
                            currentFill = voxelData[x, y, z];
                        }
                        vls[x, y, z] = currentFill;
                    }
                }
            }
            return vls;
        }


        public static byte[,,] Shrink(byte[,,] voxelData, int multiplier)
        {
            if(multiplier == 1) return voxelData;
            return Shrink(voxelData, multiplier, multiplier, multiplier);
        }
        public static byte[,,] Shrink(byte[,,] voxelData, int xmultiplier, int ymultiplier, int zmultiplier)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            Dictionary<byte, int> colorCount = new Dictionary<byte, int>();

            byte[,,] vfinal = new byte[xSize / xmultiplier, ySize / ymultiplier, zSize / zmultiplier];
            for(int x = 0; x < xSize / xmultiplier; x++)
            {
                for(int y = 0; y < ySize / ymultiplier; y++)
                {
                    for(int z = 0; z < zSize / zmultiplier; z++)
                    {
                        colorCount = new Dictionary<byte, int>();
                        int fullCount = 0;
                        int emptyCount = 0;
                        for(int xx = 0; xx < xmultiplier; xx++)
                        {
                            for(int yy = 0; yy < ymultiplier; yy++)
                            {
                                for(int zz = 0; zz < zmultiplier; zz++)
                                {
                                    if(x * xmultiplier + xx >= xSize || y * ymultiplier + yy < 0 || z * zmultiplier + zz >= zSize)
                                        continue;
                                    byte smallColor = voxelData[x * xmultiplier + xx, y * ymultiplier + yy, z * zmultiplier + zz];
                                    
                                    if(smallColor > 0)
                                    {
                                        if(colorCount.ContainsKey(smallColor))
                                        {
                                            colorCount[smallColor] = colorCount[smallColor] + 16;
                                        }
                                        else
                                        {
                                            colorCount[smallColor] = 16;
                                        }
                                        fullCount += 16;
                                    }
                                    else
                                    {
                                        emptyCount += 5;
                                    }
                                }
                            }
                        }
                        byte best = 0;
                        if(fullCount >= emptyCount)
                            best = colorCount.OrderByDescending(kv => kv.Value).First().Key;
                        if(best > 0)
                        {
                            vfinal[x, y, z] = best;
                        }
                    }
                }
            }
            return vfinal;
        }


        public static byte[,,] RotateYaw(byte[,,] colors, int amount)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];
            switch(amount / 90)
            {
                case 0:
                    vls = colors.Replicate();
                    break;
                case 1:
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                vls[y, xSize - x - 1, z] = colors[x, y, z];
                            }
                        }
                    }
                    break;
                case 2:
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                vls[xSize - x - 1, ySize - y - 1, z] = colors[x, y, z];
                            }
                        }
                    }
                    break;
                case 3:
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int y = 0; y < ySize; y++)
                        {
                            for(int z = 0; z < zSize; z++)
                            {
                                vls[ySize - y - 1, x, z] = colors[x, y, z];
                            }
                        }
                    }
                    break;
            }
            return vls;
        }
        public static byte[,,] RotateYawPartial(byte[,,] colors, int degrees)
        {
            if(degrees % 90 == 0)
                return RotateYaw(colors, degrees);
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            byte[,,] vls = new byte[xSize, ySize, zSize];

            double angle = (Math.PI / 180) * ((degrees + 720) % 360);
            double sn = Math.Sin(angle), cs = Math.Cos(angle);

            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    int tempX = (x - (xSize / 2));
                    int tempY = (y - (ySize / 2));
                    int x2 = (int)Math.Round((cs * tempX) + (sn * tempY) + (xSize / 2));
                    int y2 = (int)Math.Round((-sn * tempX) + (cs * tempY) + (ySize / 2));

                    for(int z = 0; z < zSize; z++)
                    {
                        if(x2 >= 0 && y2 >= 0 && x2 < xSize && y2 < ySize && colors[x, y, z] > 0)
                            vls[x2, y2, z] = colors[x, y, z];
                    }
                }
            }

            return vls;
        }


        public static byte[,,] ScalePartial(byte[,,] colors, double scale)
        {
            if(scale == 1.0) return colors;
            return ScalePartial(colors, scale, scale, scale);
        }
        public static byte[,,] ScalePartial(byte[,,] colors, double xScale, double yScale, double zScale)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            if(xScale <= 0 || yScale <= 0 || zScale <= 0)
                return colors;
            byte[,,] vls = new byte[(int)(xSize * xScale), (int)(ySize * yScale), (int)(zSize * zScale)];

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        for(double xsc = 0.0; xsc < xScale; xsc += 1.0)
                        {
                            for(double ysc = 0.0; ysc < yScale; ysc += 1.0)
                            {
                                for(double zsc = 0.0; zsc < zScale; zsc += 1.0)
                                {
                                    int tempX = (x - (xSize / 2));
                                    int tempY = (y - (ySize / 2));
                                    int tempZ = (z - (zSize / 2));
                                    int x2 = (int)Math.Round((xScale * tempX) + (xScale * xSize / 2) + ((tempX < 0) ? xsc : -xsc));
                                    int y2 = (int)Math.Round((yScale * tempY) + (yScale * ySize / 2) + ((tempY < 0) ? ysc : -ysc));
                                    int z2 = (int)Math.Round((zScale * tempZ) + (zScale * zSize / 2) + ((tempZ < 0) ? zsc : -zsc));

                                    if(colors[x, y, z] > 0 && x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < xSize * xScale && y2 < ySize * yScale && z2 < zSize * zScale)
                                        vls[x2, y2, z2] = colors[x, y, z];
                                }
                            }
                        }
                    }
                }
            }
            return vls;
        }
        
    }
    public class FaceLogic
    {
        public static FaceVoxel[,,] GetFaces(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];

            for(int z = 0; z < zSize; z++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = 0; y < ySize; y++)
                    {
                        byte mvd = voxelData[x, y, z];
                        if(mvd > 0)
                        {
                            data[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)z, color = mvd }, Slope.Cube);
                        }
                    }
                }
            }

            data = AddAll(data);

            return data;
        }

        public static FaceVoxel[,,] GetFaces(IEnumerable<MagicaVoxelData> voxelData, int xSize, int ySize, int zSize)
        {
            FaceVoxel[,,] data = new FaceVoxel[xSize, ySize, zSize];

            foreach(MagicaVoxelData mvd in voxelData)
            {
                data[mvd.x, mvd.y, mvd.z] = new FaceVoxel(mvd, Slope.Cube);
            }
            data = AddAll(data);

            return data;
        }

        public static FaceVoxel[,,] AddAll(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel[,,] faces2 = new FaceVoxel[xSize, ySize, zSize];

            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize - 1; x++)
                {
                    for(int y = ySize - 1; y >= 0; y--)
                    {
                        if(z == 0 || x == 0 || y == 0 || z == zSize - 1 || x == xSize - 1 || y == ySize - 1 || faces[x, y, z] != null)
                        {
                            faces2[x, y, z] = faces[x, y, z];
                        }
                        else
                        {
                            bool xup = faces[x + 1, y, z] != null,
                                 xdown = faces[x - 1, y, z] != null,
                                 yup = faces[x, y + 1, z] != null,
                                 ydown = faces[x, y - 1, z] != null,
                                 zup = faces[x, y, z + 1] != null,
                                 zdown = faces[x, y, z - 1] != null;

                            if(zdown)
                            {

                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.BrightTop);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.DimTop);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.BrightDimTop);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.BrightTopBack);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z - 1].vox.color }, Slope.DimTopBack);
                                }
                            }
                            else if(zup)
                            {

                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.BrightBottom);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.DimBottom);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.BrightDimBottom);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.BrightBottomBack);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x, y, z + 1].vox.color }, Slope.DimBottomBack);
                                }
                            }
                            else
                            {
                                if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x - 1, y, z].vox.color }, Slope.BrightDim);
                                }
                                else if(!xdown && !yup && xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x + 1, y, z].vox.color }, Slope.BackBack);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x + 1, y, z].vox.color }, Slope.BrightBack);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = faces[x - 1, y, z].vox.color }, Slope.DimBack);
                                }
                            }
                        }
                    }
                }
            }
            return faces2;
        }

    }
}
