using Chunks;
using Chunks.Geometry;
using System.IO;
using System;
using Ionic.Zlib;
using Chunks.Plugins;
using System.Linq;
using System.Collections.Generic;

namespace MinecraftMapReader.Source
{
    public class WorldReader : ChunkGenerator
    {
        
        static string worldfolder;
        [ThreadStatic]
        static byte[] chunkData;
        [ThreadStatic]
        static byte chunkVersion;
        [Initialization]
        private static void OnPluginInitialize()
        {
            WorldManagement.WorldInitialized += OnWorldInitialized;
        }

        private static void OnWorldInitialized(IWorld world)
        {
            //Teleport the player and Spectator camera to the last saved position in level.dat
            worldfolder = world.Properties.SaveDirectory;
            if (Directory.Exists(worldfolder + "\\playerdata"))
            {
                //byte[] leveldatcompressed = File.ReadAllBytes(worldfolder + "\\level.dat");
                var playerdatafolder = new DirectoryInfo(worldfolder + "\\playerdata");
                byte[] leveldatcompressed = File.ReadAllBytes(playerdatafolder.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
                Debug.Log(playerdatafolder.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
                byte[] leveldat = Decompress(leveldatcompressed);
                var nbt = NBTReader.readNBT(leveldat);
                var positionVector = new Vector(
                    ((float)nbt.tree["Pos"][0]) * world.BlockSize,
                    ((float)nbt.tree["Pos"][1]+1) * world.BlockSize,
                    (-(float)nbt.tree["Pos"][2]+16) * world.BlockSize);
                /*((float)nbt.tree["Data"]["Player"]["Pos"][0]) * world.BlockSize,
                    ((float)nbt.tree["Data"]["Player"]["Pos"][1]+1) * world.BlockSize,
                    ((float)nbt.tree["Data"]["Player"]["Pos"][2]+15) * world.BlockSize);*/

                world.CameraRig.Transform.Position = positionVector;
                world.SpectatorCamera.Transform.Position = positionVector;
                //if(world.GetAllComponents<SpectatorCameraController>().FirstOrDefault()!=null) //optional json tag needs to be fixed
                //    world.GetAllComponents<SpectatorCameraController>().FirstOrDefault().Transform.Position = positionVector;
            }
        }

        protected override void GenerateChunkColumn(ChunkColumn chunks)
        {
            //chunks.SaveOnGenerate = true;
            worldfolder = WorldProperties.SaveDirectory;
            if (!File.Exists(worldfolder + "\\level.dat"))
                Debug.Log("Couldn't locate Minecraft world to import.");

            chunkData = new byte[] { 0x00 };
            chunkVersion = 0x00;
            if (ChunkExistsAtCoords(chunks.Min.X, chunks.Min.Z, out chunkData, out chunkVersion))
            {
                var uncompressedChunk = decompressChunk(chunkData, chunkVersion);
                uncompressedChunk.Seek(0, SeekOrigin.Begin);
                var br = new BinaryReader(uncompressedChunk);
                var nbt = NBTReader.readNBT(uncompressedChunk.ToArray());
                var sections = (NBTListTag)nbt.tree["Level"]["Sections"];
                var sectionscount = sections.value.Count();
                int yLevel = 0;
                byte[] blocks = { 0 };

                for (var i = 0; i < sectionscount; i++)
                {
                    blocks = (byte[])sections[i]["Blocks"];
                    yLevel = (int)sections[i]["Y"];

                    var ran = CreateRandom(chunks);
                    for (var x = 0; x < 16; x++)
                        for (var z = 0; z < 16; z++)
                        {
                            for (var y = 0; y < 16; y++)
                            {
                                int BlockPos = y * 16 * 16 + z * 16 + x;
                                byte BlockID_a = blocks[BlockPos];
                                Block block;
                                Blocks.blocks.TryGetValue(BlockID_a, out block);
                                if (block == default(Block))
                                    block = Plugin.GetResource<Block>("MinecraftTextures.dirt"); //Core.Test
                                if (BlockID_a != 0)
                                    chunks.Set(new IntVector(x + chunks.Min.X, y + (16 * yLevel), (15 - z) + chunks.Min.Z), block);
                            }
                        }
                }
            }
            else //No chunk exists at given coorinates, generate a simple flat terrain
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
        }

        static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
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
                Debug.Log("Error mc-chunk compression version: " + version.ToString());
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

                    if (offset == 0)//No chunk here :C
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
}