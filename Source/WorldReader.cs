using Chunks;
using Chunks.Geometry;
using System.IO;
using System;
using Ionic.Zlib;
using Chunks.Plugins;

namespace MinecraftMapReader.Source
{
    public class WorldReader : ChunkGenerator
    {
        private enum TAG
        {
            End = 0,
            Byte = 1,
            Short = 2,
            Int = 3,
            Long = 4,
            Float = 5,
            Double = 6,
            Byte_Array = 7,
            String = 8,
            List = 9,
            Compound = 10,
            Int_Array = 11
        }

        static string worldfolder;// = "C:\\Users\\Chris\\AppData\\LocalLow\\Facepunch Studios\\Chunks\\Worlds\\world";
        [ThreadStatic]
        static byte[] chunkData;
        [ThreadStatic]
        static byte chunkVersion;
        protected override void GenerateChunkColumn(ChunkColumn chunks)
        {
            worldfolder = WorldProperties.SaveDirectory;
            if (!File.Exists(worldfolder + "\\level.dat"))
                Debug.Log("Couldn't locate Minecraft World to import.");

            chunkData = new byte[] { 0x00 };
            chunkVersion = 0x00;
            if (ChunkExistsAtCoords(chunks.Min.X, chunks.Min.Z, out chunkData, out chunkVersion))
            {
                var uncompressedChunk = decompressChunk(chunkData, chunkVersion);
                uncompressedChunk.Seek(0, SeekOrigin.Begin);
                var br = new BinaryReader(uncompressedChunk);
                
                #region readData
                int Tag, payloadSize, TagNameLength;
                char[] TagName;
                payloadSize = 0;
                int payloadbyte = 0;
                do
                {
                    Tag = uncompressedChunk.ReadByte();
                    if ((TAG)Tag != TAG.End)
                        TagNameLength = Helpers.ReadInt16BE(br);
                    else
                        TagNameLength = 0;
                    var tagreader = new StreamReader(uncompressedChunk);
                    TagName = new char[TagNameLength];
                    TagName = br.ReadChars(TagNameLength);
                    switch ((TAG)Tag)
                    {
                        case TAG.Compound:
                            break;
                        case TAG.End:
                            break;
                        case TAG.Int:
                            var payloadint = Helpers.ReadInt32BE(br);
                            break;
                        case TAG.Long:
                            var payloadlong = Helpers.ReadInt64BE(br);
                            break;
                        case TAG.Byte:
                            payloadbyte = uncompressedChunk.ReadByte();
                            break;
                        case TAG.Byte_Array:
                            payloadSize =  Helpers.ReadInt32BE(br);
                            uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                            break;
                        case TAG.Int_Array:
                           payloadSize = Helpers.ReadInt32BE(br);
                            uncompressedChunk.Seek(payloadSize*4, SeekOrigin.Current);
                            break;
                        case TAG.List:
                            var tagId = uncompressedChunk.ReadByte();
                            payloadSize = Helpers.ReadInt32BE(br);
                            break;
                        case TAG.String:
                            payloadSize = Helpers.ReadInt16BE(br);
                            uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                            break;
                        default:
                            Debug.Log(chunks.Min.X + " " +chunks.Min.Z + "ERRORTAG: UNKOWN TAG : " + (TAG)Tag);
                            break;
                    }
                } while (new string(TagName) != "Sections" && uncompressedChunk.Position < uncompressedChunk.Length);
                var count = payloadSize;
                int yLevel = 0;
                byte[] blocks = { 0 };
                for (var i = 0; i < count; i++)//payloadSize is the amount of sections avaiable
                {
                    do
                    {
                        Tag = uncompressedChunk.ReadByte();
                        if ((TAG)Tag != TAG.End)
                            TagNameLength = Helpers.ReadInt16BE(br);
                        else
                            TagNameLength = 0;
                        var tagreader = new StreamReader(uncompressedChunk);
                        TagName = new char[TagNameLength];
                        TagName = br.ReadChars(TagNameLength);
                        switch ((TAG)Tag)
                        {
                            case TAG.Compound:
                                break;
                            case TAG.End:
                                break;
                            case TAG.Int:
                                var payloadint = Helpers.ReadInt32BE(br);
                                break;
                            case TAG.Long:
                                var payloadlong = Helpers.ReadInt64BE(br);
                                break;
                            case TAG.Byte:
                                payloadbyte = uncompressedChunk.ReadByte();
                                break;
                            case TAG.Byte_Array:
                                payloadSize = Helpers.ReadInt32BE(br);
                                if(new string(TagName)!= "Blocks")
                                    uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                                break;
                            case TAG.Int_Array:
                                payloadSize = Helpers.ReadInt32BE(br);
                                uncompressedChunk.Seek(payloadSize * 4, SeekOrigin.Current);
                                break;
                            case TAG.List:
                                var tagId = uncompressedChunk.ReadByte();
                                payloadSize = Helpers.ReadInt32BE(br);
                                break;
                            case TAG.String:
                                payloadSize = Helpers.ReadInt16BE(br);
                                uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                                break;
                            default:
                                Debug.Log(chunks.Min.X + " " + chunks.Min.Z + "ERRORTAG: UNKOWN TAG : " + (TAG)Tag);
                                break;
                        }
                    } while (new string(TagName) != "Blocks" && uncompressedChunk.Position < uncompressedChunk.Length);
                    blocks = br.ReadBytes(payloadSize);
                    do
                    {
                        Tag = uncompressedChunk.ReadByte();
                        if ((TAG)Tag != TAG.End)
                            TagNameLength = Helpers.ReadInt16BE(br);
                        else
                            TagNameLength = 0;
                        var tagreader = new StreamReader(uncompressedChunk);
                        TagName = new char[TagNameLength];
                        TagName = br.ReadChars(TagNameLength);
                        switch ((TAG)Tag)
                        {
                            case TAG.Compound:
                                break;
                            case TAG.End:
                                break;
                            case TAG.Int:
                                var payloadint = Helpers.ReadInt32BE(br);
                                break;
                            case TAG.Long:
                                var payloadlong = Helpers.ReadInt64BE(br);
                                break;
                            case TAG.Byte:
                                payloadbyte = uncompressedChunk.ReadByte();
                                break;
                            case TAG.Byte_Array:
                                payloadSize = Helpers.ReadInt32BE(br);
                                if (new string(TagName) != "Blocks")
                                    uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                                break;
                            case TAG.Int_Array:
                                payloadSize = Helpers.ReadInt32BE(br);
                                uncompressedChunk.Seek(payloadSize * 4, SeekOrigin.Current);
                                break;
                            case TAG.List:
                                var tagId = uncompressedChunk.ReadByte();
                                 payloadSize = Helpers.ReadInt32BE(br);
                                break;
                            case TAG.String:
                                payloadSize = Helpers.ReadInt16BE(br);
                                uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                                break;
                            default:
                                Debug.Log(chunks.Min.X + " " + chunks.Min.Z + "ERRORTAG: UNKOWN TAG : " + (TAG)Tag);
                                break;
                        }
                    } while (new string(TagName) != "Y" && uncompressedChunk.Position < uncompressedChunk.Length);
                    yLevel = payloadbyte;
                     do
                    {
                        Tag = uncompressedChunk.ReadByte();
                        if ((TAG)Tag != TAG.End)
                            TagNameLength = Helpers.ReadInt16BE(br);
                        else
                            TagNameLength = 0;
                        var tagreader = new StreamReader(uncompressedChunk);
                        TagName = new char[TagNameLength];
                         TagName = br.ReadChars(TagNameLength);
                        switch ((TAG)Tag)
                        {
                            case TAG.Compound:
                                break;
                            case TAG.End:
                                break;
                            case TAG.Int:
                                var payloadint = Helpers.ReadInt32BE(br);
                                break;
                            case TAG.Long:
                               var payloadlong = Helpers.ReadInt64BE(br);
                                break;
                            case TAG.Byte:
                                payloadbyte = uncompressedChunk.ReadByte();
                                break;
                            case TAG.Byte_Array:
                                payloadSize = Helpers.ReadInt32BE(br);
                                if (new string(TagName) != "Blocks")
                                    uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                                break;
                            case TAG.Int_Array:
                                payloadSize = Helpers.ReadInt32BE(br);
                                uncompressedChunk.Seek(payloadSize * 4, SeekOrigin.Current);
                                break;
                            case TAG.List:
                                var tagId = uncompressedChunk.ReadByte();
                                 payloadSize = Helpers.ReadInt32BE(br);
                                break;
                            case TAG.String:
                                payloadSize = Helpers.ReadInt16BE(br);
                                uncompressedChunk.Seek(payloadSize, SeekOrigin.Current);
                                break;
                            default:
                                Debug.Log(chunks.Min.X + " " + chunks.Min.Z + "ERRORTAG: UNKOWN TAG : " + (TAG)Tag);
                                break;
                        }
                    } while ((new string(TagName) != "" && uncompressedChunk.Position < uncompressedChunk.Length) || Tag == -1);
                    var ran = CreateRandom(chunks);
                    for(var x=0; x< 16; x++)
                        for (var z = 0; z < 16; z++)
                        { 
                            for (var y = 0; y < 16; y++)
                            {
                                int BlockPos = y*16*16+z*16+x;
                                byte BlockID_a = blocks[BlockPos];
                                /*if(BlockID_a != 0)
                                {
                                    var grassBlock = GetBlock("Grass", ((float)ran.NextDouble() - 0.5f) * 0.25f);
                                    chunks.Set(new IntVector(x+chunks.Min.X, y+(16*yLevel), z+chunks.Min.Z), grassBlock);
                                }*/
                                var block = Plugin.GetResource<Block>("Core.Test");
                                switch (BlockID_a)
                                {
                                    case 1://Stone
                                        block = GetBlock("Stone", 1);
                                        break;
                                    case 2://Grass
                                        block = GetBlock("Grass", 1);
                                        break;
                                    case 3://Dirt
                                        block = GetBlock("Dirt", 1);
                                        break;
                                    case 5://Plank
                                        block = Plugin.GetResource<Block>("BasicTerrain.Test");
                                        break;
                                    case 7://Bedrock
                                        block = GetBlock("Bedrock", 1);
                                        break;
                                    case 17://Log
                                        block = GetBlock("Trunk", 1);
                                        break;
                                    case 18://Leaves
                                        block = GetBlock("Leaves", 1);
                                        break;
                                    default:
                                        break;
                                }
                                if (BlockID_a != 0)
                                    chunks.Set(new IntVector(x + chunks.Min.X, y + (16 * yLevel), (15-z) + chunks.Min.Z), block);
                            }
                        }
                }
                #endregion
            }
            else //No chunk exists at given coorinates
            {
                chunkData = new byte[] { 0x00 };
                var rando = CreateRandom(chunks);
                
                for (var x = chunks.Min.X; x < chunks.Max.X; ++x)
                    for (var z = chunks.Min.Z; z < chunks.Max.Z; ++z)
                    {
                        var grassBlock = GetBlock("Grass", ((float)rando.NextDouble() - 0.5f) * 0.25f);
                        chunks.Set(new IntVector(x, 0, z), grassBlock);
                    }
            }
            /*
            //Standing Platform to reach debug console, can be deleted later on 
            var rand = CreateRandom(chunks);
            for (var x = chunks.Min.X; x < chunks.Max.X; ++x)
                for (var z = chunks.Min.Z; z < chunks.Max.Z; ++z)
                {
                    var grassBlock = GetBlock("Grass", ((float)rand.NextDouble() - 0.5f) * 0.25f);
                    if (x > -20 && x < 20 && z > -20 && z < 20)
                    {
                        chunks.Set(new IntVector(x, 30, z), grassBlock);
                    }
                }
             */
        }

        private static MemoryStream decompressChunk(byte[] chunkData, int version)
        {
            if (version == 2)//We can only deal with the compression type of 2, zlib
            {
                //Inflate chunkData
                using (MemoryStream ms = new MemoryStream(chunkData))
                {
                    MemoryStream msInner = new MemoryStream();
                    ms.Seek(2, SeekOrigin.Begin); //Probby not needed (zlib header skip) //well, it does seem to be needed, how foolish
                    using (DeflateStream z = new DeflateStream(ms, CompressionMode.Decompress))
                    {
                        MissingExtensions.CopyTo(z, msInner);
                    }
                    return msInner;
                }
            }
            else
            {
                Debug.Log("Error mc-chunk compression version: " +version.ToString());
                return null;
            }
        }
        
        private static bool ChunkExistsAtCoords(int x, int z, out byte[] chunkData, out byte chunkVersion)
        {
            int chunkX = x / 16;
            int chunkZ = -z / 16;
            int regionX = (int)Math.Floor(chunkX / 32.0);
            int regionZ = (int)Math.Floor(chunkZ / 32.0);
            string regionPath = worldfolder + "\\region\\r." + regionX.ToString() + "." + regionZ + ".mca";
            if (File.Exists(regionPath))
            {
                using (BinaryReader br = new BinaryReader(new FileStream(regionPath, FileMode.Open, FileAccess.Read)))
                {
                    br.BaseStream.Position = 4 * ((chunkX & 31) + (chunkZ & 31) * 32);
                    var offset = br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte();
                    var sectorCount = br.ReadByte();
                
                    if (offset==0)//No chunk here :C
                    {
                        chunkVersion = 0;
                        chunkData = new byte[] { 0x00 }; 
                        return false;
                    }
                    //Chunk exists, get the version and data
                    br.BaseStream.Position = 4096 * offset;
                    var chunkLength = br.ReadByte() << 24 | br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte();
                    chunkVersion = br.ReadByte();
                    chunkData = br.ReadBytes(chunkLength - 1);
                    return true;
                }
            }
            chunkData = new byte[] { 0x00 };
            chunkVersion = 0;
            return false;
        }

        private static int ThreeBytesToInt32BigEndian(byte[] buf, int i)
        {
            return (buf[i] << 24) | (buf[i + 1] << 16) | (buf[i + 2] << 8);
        }

        private static int FourBytesToInt32BigEndian(byte[] buf, int i)
        {
            return (buf[i] << 24) | (buf[i + 1] << 16) | (buf[i + 2] << 8) | buf[i + 3];
        }
    }
    public static class MissingExtensions
    {
        /// Only useful before .NET 4
        public static void CopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }
    public static class Helpers
    {
        // Note this MODIFIES THE GIVEN ARRAY then returns a reference to the modified array.
        public static byte[] Reverse(this byte[] b)
        {
            Array.Reverse(b);
            return b;
        }

        public static UInt16 ReadUInt16BE(this BinaryReader binRdr)
        {
            return BitConverter.ToUInt16(binRdr.ReadBytesRequired(sizeof(UInt16)).Reverse(), 0);
        }

        public static Int16 ReadInt16BE(this BinaryReader binRdr)
        {
            return BitConverter.ToInt16(binRdr.ReadBytesRequired(sizeof(Int16)).Reverse(), 0);
        }

        public static UInt32 ReadUInt32BE(this BinaryReader binRdr)
        {
            return BitConverter.ToUInt32(binRdr.ReadBytesRequired(sizeof(UInt32)).Reverse(), 0);
        }

        public static Int32 ReadInt32BE(this BinaryReader binRdr)
        {
            return BitConverter.ToInt32(binRdr.ReadBytesRequired(sizeof(Int32)).Reverse(), 0);
        }

        public static UInt64 ReadUInt64BE(this BinaryReader binRdr)
        {
            return BitConverter.ToUInt64(binRdr.ReadBytesRequired(sizeof(UInt64)).Reverse(), 0);
        }

        public static Int64 ReadInt64BE(this BinaryReader binRdr)
        {
            return BitConverter.ToInt64(binRdr.ReadBytesRequired(sizeof(Int64)).Reverse(), 0);
        }

        public static byte[] ReadBytesRequired(this BinaryReader binRdr, int byteCount)
        {
            var result = binRdr.ReadBytes(byteCount);

            if (result.Length != byteCount)
                throw new EndOfStreamException(string.Format("{0} bytes required from stream, but only {1} returned.", byteCount, result.Length));

            return result;
        }
    }
}
