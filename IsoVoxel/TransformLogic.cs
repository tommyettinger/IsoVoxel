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
        BackBack = 0x10000,
        BackBackTop = 0x20000,
        BackBackBottom = 0x40000,
        RearBrightTop = 0x80000,
        RearDimTop = 0x100000,
        RearBrightBottom = 0x200000,
        RearDimBottom = 0x400000,

        BrightDimTopThick = 0x42,
        BrightDimBottomThick = 0x202,
        BrightTopBackThick = 0x1002,
        BrightBottomBackThick = 0x4002,
        DimTopBackThick = 0x2002,
        DimBottomBackThick = 0x8002,
        BackBackTopThick = 0x20002,
        BackBackBottomThick = 0x40002,
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
        public FaceVoxel(int x, int y, int z, int color, Slope slp)
        {
            vox = new MagicaVoxelData(x, y, z, color);
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
        public static byte[,,] RunThinningCA(byte[,,] voxelData, int smoothLevel)
        {
            if(smoothLevel <= 1)
                return voxelData;
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            //Dictionary<byte, int> colorCount = new Dictionary<byte, int>();
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
                            int emptyCount = 0;
                            if(x == 0 || y == 0 || z == 0 || x == xSize - 1 || y == ySize - 1 || z == zSize - 1)
                            {

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
                                        }
                                    }
                                }
                            }
                            if(emptyCount >= 18)
                                vs[v][x, y, z] = 0;
                            else
                            {
                                vs[v][x, y, z] = vs[v - 1][x, y, z];
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

            byte[,,] vls;
            switch(amount / 90)
            {
                case 1:
                    vls = new byte[ySize, xSize, zSize];
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
                    vls = new byte[xSize, ySize, zSize];
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
                    vls = new byte[ySize, xSize, zSize];
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
                default:
                    vls = colors.Replicate();
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


        public static byte[,,] ScalePartial(byte[,,] colors, int scale)
        {
            if(scale == 1) return colors.Replicate();
            return ScalePartial(colors, scale, scale, scale);
        }
        public static byte[,,] ScalePartial(byte[,,] colors, int xScale, int yScale, int zScale)
        {
            int xSize = colors.GetLength(0), ySize = colors.GetLength(1), zSize = colors.GetLength(2);

            if(xScale <= 0 || yScale <= 0 || zScale <= 0)
                return colors;
            byte[,,] vls = new byte[(xSize * xScale), (ySize * yScale), (zSize * zScale)];

            for(int z = 0; z < zSize; z++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int x = 0; x < xSize; x++)
                    {
                        for(int xsc = 0; xsc < xScale; xsc++)
                        {
                            for(int ysc = 0; ysc < yScale; ysc++)
                            {
                                for(int zsc = 0; zsc < zScale; zsc++)
                                {
                                    //int tempX = (x - (xSize / 2));
                                    //int tempY = (y - (ySize / 2));
                                    //int tempZ = (z - (zSize / 2));
                                    //int x2 = ((xScale * tempX) + (xScale * xSize / 2) + ((tempX < 0) ? xsc : -xsc));
                                    //int y2 = ((yScale * tempY) + (yScale * ySize / 2) + ((tempY < 0) ? ysc : -ysc));
                                    //int z2 = ((zScale * tempZ) + (zScale * zSize / 2) + ((tempZ < 0) ? zsc : -zsc));
                                    int x2 = xScale * x + xsc;
                                    int y2 = yScale * y + ysc;
                                    int z2 = zScale * z + zsc;
                                    if (colors[x, y, z] > 0 && x2 >= 0 && y2 >= 0 && z2 >= 0 && x2 < xSize * xScale && y2 < ySize * yScale && z2 < zSize * zScale)
                                        vls[x2, y2, z2] = colors[x, y, z];
                                }
                            }
                        }
                    }
                }
            }
            return vls;
        }
        public static byte NearbyColor(byte[,,] voxelData, int x, int y, int z)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            byte[] nearby = new byte[27], counts = new byte[27];
            int running = 0;
            for(int i = Math.Max(0, x - 1); i <= x + 1 && i < xSize; i++)
            {
                for(int j = Math.Max(0, y - 1); j <= y + 1 && j < ySize; j++)
                {
                    for(int k = Math.Max(0, z - 1); k <= z + 1 && k < zSize; k++)
                    {
                        for(int c = 0; c <= running; c++)
                        {
                            if(nearby[c] == voxelData[i, j, k] && voxelData[i, j, k] != 0 && voxelData[i, j, k] != 255)
                            {
                                counts[c]++;
                                break;
                            }
                            if(c == running && voxelData[i, j, k] != 0)
                            {
                                nearby[c] = voxelData[i, j, k];
                                counts[c]++;
                                running++;
                                break;
                            }
                        }
                    }
                }
            }
            byte best = 0, bestCount = 0;
            for(int c = 0; c < running; c++)
            {
                if(counts[c] > bestCount || (counts[c] >= bestCount && best == 255))
                {
                    best = nearby[c];
                    bestCount = counts[c];
                }
            }
            if(best == 255)
                return 0;
            return best;
        }
        public static byte[,,] MarkInterior(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            byte[,,] voxels = new byte[xSize, ySize, zSize];
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {

                        if(
                            x == 0 || x == xSize - 1 ||
                            y == 0 || y == ySize - 1 ||
                            z == 0 || z == zSize - 1 ||
                            voxelData[x, y, z] == 0 ||
                            voxelData[x - 1, y, z] == 0 ||
                            voxelData[x + 1, y, z] == 0 ||
                            voxelData[x, y - 1, z] == 0 ||
                            voxelData[x, y + 1, z] == 0 ||
                            voxelData[x, y, z - 1] == 0 ||
                            voxelData[x, y, z + 1] == 0 ||

                            voxelData[x - 1, y, z + 1] == 0 ||
                            voxelData[x + 1, y, z + 1] == 0 ||
                            voxelData[x, y - 1, z + 1] == 0 ||
                            voxelData[x, y + 1, z + 1] == 0 ||

                            voxelData[x - 1, y, z - 1] == 0 ||
                            voxelData[x + 1, y, z - 1] == 0 ||
                            voxelData[x, y - 1, z - 1] == 0 ||
                            voxelData[x, y + 1, z - 1] == 0 ||

                            voxelData[x - 1, y - 1, z] == 0 ||
                            voxelData[x + 1, y - 1, z] == 0 ||

                            voxelData[x - 1, y + 1, z] == 0 ||
                            voxelData[x + 1, y + 1, z] == 0 ||

                            voxelData[x - 1, y + 1, z + 1] == 0 ||
                            voxelData[x + 1, y - 1, z + 1] == 0 ||
                            voxelData[x - 1, y - 1, z + 1] == 0 ||
                            voxelData[x + 1, y + 1, z + 1] == 0 ||

                            voxelData[x - 1, y + 1, z - 1] == 0 ||
                            voxelData[x + 1, y - 1, z - 1] == 0 ||
                            voxelData[x - 1, y - 1, z - 1] == 0 ||
                            voxelData[x + 1, y + 1, z - 1] == 0)

                            voxels[x, y, z] = voxelData[x, y, z];
                        else
                            voxels[x, y, z] = 255;

                    }
                }
            }
            return voxels;
        }
        public static byte[,,] FillInterior(byte[,,] voxelData)
        {
            return voxelData;
            /*
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);

            byte[,,] voxels = MarkInterior(voxelData), vox2 = new byte[xSize,ySize,zSize];
            for(int x = 0; x < xSize; x++)
            {
                for(int y = 0; y < ySize; y++)
                {
                    for(int z = 0; z < zSize; z++)
                    {
                        if(voxels[x, y, z] == 255)
                            vox2[x, y, z] = NearbyColor(voxels, x, y, z);
                        else
                            vox2[x, y, z] = voxels[x, y, z];
                    }
                }
            }*/
            //return voxels;
        }
    }
    public class FaceLogic
    {
        public static FaceVoxel[,,] GetFaces(byte[,,] voxelData)
        {
            int xSize = voxelData.GetLength(0), ySize = voxelData.GetLength(1), zSize = voxelData.GetLength(2);
            FaceVoxel[,,] data = new FaceVoxel[Math.Max(xSize, ySize), Math.Max(xSize, ySize), zSize];

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

        public static bool AllArray(bool[] checks)
        {
            for(int i = 0; i < checks.Length; i++)
            {
                if(!checks[i])
                    return false;
            }
            return true;
        }

        public static FaceVoxel[,,] AddAll(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel[,,] faces2 = new FaceVoxel[xSize, ySize, zSize];
            byte mvd;
            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize; x++)
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

                            if(zdown && !zup)
                            {
                                mvd = faces[x, y, z - 1].vox.color;
                                
                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightTop);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimTop);
                                }
                                else if(!xdown && !yup && xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearBrightTop);
                                }
                                else if(!xdown && !yup && !xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearDimTop);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightDimTopThick);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightTopBackThick);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimTopBackThick);
                                }
                                else if(!xdown && !yup && xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BackBackTopThick);
                                }
                            }
                            else if(zup)
                            {

                                mvd = faces[x, y, z + 1].vox.color;
                                
                                if(!xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightBottom);
                                }
                                else if(xdown && !yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimBottom);
                                }
                                else if(!xdown && !yup && xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearBrightBottom);
                                }
                                else if(!xdown && !yup && !xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.RearDimBottom);
                                }
                                else if(xdown && yup && !xup && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightDimBottomThick);
                                }
                                else if(xup && yup && !xdown && !ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightBottomBackThick);
                                }
                                else if(!xup && !yup && xdown && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimBottomBackThick);
                                }
                                else if(!xdown && !yup && xup && ydown)
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BackBackBottomThick);
                                }
                            }
                            else if(xdown)
                            {

                                mvd = faces[x - 1, y, z].vox.color;

                                if(yup && !xup && !ydown) // && xdown
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightDim);
                                }
                                else if(!xup && !yup && ydown) // && xdown
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.DimBack);
                                }
                            }
                            else if(xup)
                            {

                                mvd = faces[x + 1, y, z].vox.color;
                                
                                if(!xdown && !yup && ydown) // && xup
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BackBack);
                                }
                                else if(yup && !xdown && !ydown) // && xup
                                {
                                    faces2[x, y, z] = new FaceVoxel(new MagicaVoxelData { x = (byte)x, y = (byte)y, z = (byte)(z), color = mvd }, Slope.BrightBack);
                                }
                            }
                        }
                    }
                }
            }
            return faces2;
        }

        /*
        public static FaceVoxel[,,] AddAll(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel[,,] faces2 = new FaceVoxel[xSize, ySize, zSize];

            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize; x++)
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
        */

        public static bool IsSlopeThick(Slope s)
        {
            return s == Slope.Cube || s == Slope.BackBackBottomThick || s == Slope.BackBackTopThick || s == Slope.BrightBottomBackThick || s == Slope.BrightDimBottomThick || s == Slope.BrightDimTopThick
                 || s == Slope.BrightTopBackThick || s == Slope.DimBottomBackThick || s == Slope.DimTopBackThick;
        }

        public static byte[,,] FaceArrayToByteArray(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);

            byte[,,] b = new byte[xSize, ySize, zSize];

            for(byte x = 0; x < xSize; x++)
            {
                for(byte y = 0; y < ySize; y++)
                {
                    for(byte z = 0; z < zSize; z++)
                    {
                        if(faces[x, y, z] != null && IsSlopeThick(faces[x, y, z].slope))
                            b[x, y, z] = faces[x, y, z].vox.color;
                    }
                }
            }
            return b;
        }

        public static FaceVoxel[,,] DoubleSize(FaceVoxel[,,] faces)
        {
            int xSize = faces.GetLength(0), ySize = faces.GetLength(1), zSize = faces.GetLength(2);
            FaceVoxel[,,] result = new FaceVoxel[xSize * 2, ySize * 2, zSize * 2];


            for(int z = zSize - 1; z >= 0; z--)
            {
                for(int x = 0; x < xSize; x++)
                {
                    for(int y = ySize - 1; y >= 0; y--)
                    {
                        if(faces[x, y, z] != null)
                        {
                            FaceVoxel fv = faces[x, y, z];

                            switch(fv.slope)
                            {
                                case Slope.Cube:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    break;
                                case Slope.DimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTop);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTop);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.RearBrightTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.RearBrightTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearBrightTop);
                                    break;
                                case Slope.RearDimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.RearDimTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightDim:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDim);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    break;
                                case Slope.BrightDimTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTop); //cube
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTop);
                                    break;
                                case Slope.BrightBottom:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimBottom);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottom);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottom);
                                    break;
                                case Slope.RearBrightBottom:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.RearBrightBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearBrightBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.RearDimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.RearDimBottom);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.RearDimBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.RearDimTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearDimBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.RearDimBottom);
                                    break;
                                case Slope.BrightDimBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom); // cube
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimBottom);
                                    break;
                                case Slope.BrightBack:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTopBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTopBack); // newest
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightTopBack); // cube
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBack);
                                    break;
                                case Slope.DimTopBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimTopBack); // cube
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBack); //newest
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTopBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightBottomBack:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightBottomBack); //cube
                                    break;
                                case Slope.DimBottomBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottomBack);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottomBack); //cube
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBack);
                                    break;
                                case Slope.BackBack:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBack);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBack);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBack);
                                    break;
                                case Slope.BackBackTop:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BackBackTop); //cube
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    break;
                                case Slope.BackBackBottom:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackBottom); //cube
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottom);
                                    break;
                                case Slope.BrightDimTopThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightDimTopThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimTopThick);
                                    //result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightDimTopThick);
                                    break;
                                case Slope.BrightDimBottomThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightDimBottomThick);
                                    //result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightDimBottomThick);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDimBottomThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightTopBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightTopBackThick); // newest
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube); // cube
                                    //result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightDim);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightTopBackThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BrightTopBackThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimTopBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube); // cube
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.DimTopBackThick); //newest
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.DimTopBackThick);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimTopBackThick);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.BrightBottomBackThick:
                                    //result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BrightBack);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BrightBottomBackThick);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                                case Slope.DimBottomBackThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.DimBottomBackThick);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.DimBottomBackThick);
                                    //result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.DimBottomBackThick);
                                    break;
                                case Slope.BackBackTopThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackTopThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.BackBackTopThick);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTop);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackTopThick);
                                    break;
                                case Slope.BackBackBottomThick:
                                    result[x * 2, y * 2, z * 2] = new FaceVoxel(x * 2, y * 2, z * 2, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2 + 1, y * 2, z * 2] = new FaceVoxel(x * 2 + 1, y * 2, z * 2, fv.vox.color, Slope.Cube);
                                    //result[x * 2, y * 2 + 1, z * 2] = new FaceVoxel(x * 2, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottom);
                                    result[x * 2 + 1, y * 2 + 1, z * 2] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2, y * 2, z * 2 + 1] = new FaceVoxel(x * 2, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2 + 1, y * 2, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    result[x * 2, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.BackBackBottomThick);
                                    result[x * 2 + 1, y * 2 + 1, z * 2 + 1] = new FaceVoxel(x * 2 + 1, y * 2 + 1, z * 2 + 1, fv.vox.color, Slope.Cube);
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }

    }
}
