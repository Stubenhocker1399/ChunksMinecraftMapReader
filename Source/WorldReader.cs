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
        Dictionary<int, Block> Blocks = new Dictionary<int, Block>
        {
            { 1, Plugin.GetResource<Block>("MinecraftTextures.stone") },
            { 2, Plugin.GetResource<Block>("MinecraftTextures.grass") },
            { 3, Plugin.GetResource<Block>("MinecraftTextures.dirt") },
            { 4, Plugin.GetResource<Block>("MinecraftTextures.cobblestone") },
            { 5, Plugin.GetResource<Block>("MinecraftTextures.planks_oak") },
            { 6, Plugin.GetResource<Block>("MinecraftTextures.sapling_oak") },
            { 7, Plugin.GetResource<Block>("MinecraftTextures.bedrock") },
            { 8, Plugin.GetResource<Block>("MinecraftTextures.water_still") },
            { 9, Plugin.GetResource<Block>("MinecraftTextures.water_still") },
            { 10, Plugin.GetResource<Block>("MinecraftTextures.lava_still") },
            { 11, Plugin.GetResource<Block>("MinecraftTextures.lava_still") },
            { 12, Plugin.GetResource<Block>("MinecraftTextures.sand") },
            { 13, Plugin.GetResource<Block>("MinecraftTextures.gravel") },
            { 14, Plugin.GetResource<Block>("MinecraftTextures.gold_ore") },
            { 15, Plugin.GetResource<Block>("MinecraftTextures.iron_ore") },
            { 16, Plugin.GetResource<Block>("MinecraftTextures.coal_ore") },
            { 17, Plugin.GetResource<Block>("MinecraftTextures.log_oak") },
            { 18, Plugin.GetResource<Block>("MinecraftTextures.leaves_oak") },
            { 19, Plugin.GetResource<Block>("MinecraftTextures.sponge") },
            { 20, Plugin.GetResource<Block>("MinecraftTextures.glass") },
            { 21, Plugin.GetResource<Block>("MinecraftTextures.lapis_ore") },
            { 22, Plugin.GetResource<Block>("MinecraftTextures.lapis_block") },
            { 25, Plugin.GetResource<Block>("MinecraftTextures.noteblock") },
            { 27, Plugin.GetResource<Block>("MinecraftTextures.rail_golden") },
            { 28, Plugin.GetResource<Block>("MinecraftTextures.rail_detector") },
            { 30, Plugin.GetResource<Block>("MinecraftTextures.web") },
            { 31, Plugin.GetResource<Block>("MinecraftTextures.tallgrass") },
            { 32, Plugin.GetResource<Block>("MinecraftTextures.deadbush") },
            { 35, Plugin.GetResource<Block>("MinecraftTextures.wool_colored_white") },
            { 37, Plugin.GetResource<Block>("MinecraftTextures.flower_dandelion") },
            { 38, Plugin.GetResource<Block>("MinecraftTextures.flower_rose") },
            { 39, Plugin.GetResource<Block>("MinecraftTextures.mushroom_brown") },
            { 40, Plugin.GetResource<Block>("MinecraftTextures.mushroom_red") },
            { 41, Plugin.GetResource<Block>("MinecraftTextures.gold_block") },
            { 42, Plugin.GetResource<Block>("MinecraftTextures.iron_block") },
            { 45, Plugin.GetResource<Block>("MinecraftTextures.brick") },
            { 48, Plugin.GetResource<Block>("MinecraftTextures.cobblestone_mossy") },
            { 49, Plugin.GetResource<Block>("MinecraftTextures.obsidian") },
            { 50, Plugin.GetResource<Block>("MinecraftTextures.torch_on") },
            { 52, Plugin.GetResource<Block>("MinecraftTextures.mob_spawner") },
            { 56, Plugin.GetResource<Block>("MinecraftTextures.diamond_ore") },
            { 57, Plugin.GetResource<Block>("MinecraftTextures.diamond_block") },
            { 59, Plugin.GetResource<Block>("MinecraftTextures.wheat_stage_0") },
            { 64, Plugin.GetResource<Block>("MinecraftTextures.door_wood_lower") },
            { 65, Plugin.GetResource<Block>("MinecraftTextures.ladder") },
            { 66, Plugin.GetResource<Block>("MinecraftTextures.rail_normal") },
            { 69, Plugin.GetResource<Block>("MinecraftTextures.lever") },
            { 71, Plugin.GetResource<Block>("MinecraftTextures.door_iron_lower") },
            { 73, Plugin.GetResource<Block>("MinecraftTextures.redstone_ore") },
            { 75, Plugin.GetResource<Block>("MinecraftTextures.redstone_torch_off") },
            { 76, Plugin.GetResource<Block>("MinecraftTextures.redstone_torch_on") },
            { 79, Plugin.GetResource<Block>("MinecraftTextures.ice") },
            { 80, Plugin.GetResource<Block>("MinecraftTextures.snow") },
            { 82, Plugin.GetResource<Block>("MinecraftTextures.clay") },
            { 87, Plugin.GetResource<Block>("MinecraftTextures.netherrack") },
            { 88, Plugin.GetResource<Block>("MinecraftTextures.soul_sand") },
            { 89, Plugin.GetResource<Block>("MinecraftTextures.glowstone") },
            { 93, Plugin.GetResource<Block>("MinecraftTextures.repeater_off") },
            { 94, Plugin.GetResource<Block>("MinecraftTextures.repeater_on") },
            { 96, Plugin.GetResource<Block>("MinecraftTextures.trapdoor") },
            { 98, Plugin.GetResource<Block>("MinecraftTextures.stonebrick") },
            { 99, Plugin.GetResource<Block>("MinecraftTextures.mushroom_block_skin_brown") },
            { 100, Plugin.GetResource<Block>("MinecraftTextures.mushroom_block_skin_red") },
            { 101, Plugin.GetResource<Block>("MinecraftTextures.iron_bars") },
            { 105, Plugin.GetResource<Block>("MinecraftTextures.melon_stem_disconnected") },
            { 106, Plugin.GetResource<Block>("MinecraftTextures.vine") },
            { 111, Plugin.GetResource<Block>("MinecraftTextures.waterlily") },
            { 112, Plugin.GetResource<Block>("MinecraftTextures.nether_brick") },
            { 115, Plugin.GetResource<Block>("MinecraftTextures.nether_wart_stage_0") },
            { 121, Plugin.GetResource<Block>("MinecraftTextures.end_stone") },
            { 122, Plugin.GetResource<Block>("MinecraftTextures.dragon_egg") },
            { 123, Plugin.GetResource<Block>("MinecraftTextures.redstone_lamp_off") },
            { 124, Plugin.GetResource<Block>("MinecraftTextures.redstone_lamp_on") },
            { 127, Plugin.GetResource<Block>("MinecraftTextures.cocoa_stage_0") },
            { 129, Plugin.GetResource<Block>("MinecraftTextures.emerald_ore") },
            { 131, Plugin.GetResource<Block>("MinecraftTextures.trip_wire_source") },
            { 132, Plugin.GetResource<Block>("MinecraftTextures.trip_wire") },
            { 133, Plugin.GetResource<Block>("MinecraftTextures.emerald_block") },
            { 138, Plugin.GetResource<Block>("MinecraftTextures.beacon") },
            { 140, Plugin.GetResource<Block>("MinecraftTextures.flower_pot") },
            { 141, Plugin.GetResource<Block>("MinecraftTextures.carrots_stage_0") },
            { 142, Plugin.GetResource<Block>("MinecraftTextures.potatoes_stage_0") },
            { 149, Plugin.GetResource<Block>("MinecraftTextures.comparator_off") },
            { 150, Plugin.GetResource<Block>("MinecraftTextures.comparator_on") },
            { 152, Plugin.GetResource<Block>("MinecraftTextures.redstone_block") },
            { 153, Plugin.GetResource<Block>("MinecraftTextures.quartz_ore") },
            { 157, Plugin.GetResource<Block>("MinecraftTextures.rail_activator") },
            { 159, Plugin.GetResource<Block>("MinecraftTextures.hardened_clay_stained_white") },
            { 161, Plugin.GetResource<Block>("MinecraftTextures.leaves_acacia") },
            { 165, Plugin.GetResource<Block>("MinecraftTextures.slime") },
            { 167, Plugin.GetResource<Block>("MinecraftTextures.iron_trapdoor") },
            { 172, Plugin.GetResource<Block>("MinecraftTextures.hardened_clay") },
            { 173, Plugin.GetResource<Block>("MinecraftTextures.coal_block") },
            { 174, Plugin.GetResource<Block>("MinecraftTextures.ice_packed") },
            { 175, Plugin.GetResource<Block>("MinecraftTextures.double_plant_sunflower_bottom") },
            { 193, Plugin.GetResource<Block>("MinecraftTextures.door_spruce_lower") },
            { 194, Plugin.GetResource<Block>("MinecraftTextures.door_birch_lower") },
            { 195, Plugin.GetResource<Block>("MinecraftTextures.door_jungle_lower") },
            { 196, Plugin.GetResource<Block>("MinecraftTextures.door_acacia_lower") },
            { 197, Plugin.GetResource<Block>("MinecraftTextures.door_dark_oak_lower") },
            { 198, Plugin.GetResource<Block>("MinecraftTextures.end_rod") },
            { 199, Plugin.GetResource<Block>("MinecraftTextures.chorus_plant") },
            { 200, Plugin.GetResource<Block>("MinecraftTextures.chorus_flower") },
            { 201, Plugin.GetResource<Block>("MinecraftTextures.purpur_block") },
            { 206, Plugin.GetResource<Block>("MinecraftTextures.end_bricks") },
            { 207, Plugin.GetResource<Block>("MinecraftTextures.beetroots_stage_0") },
            { 212, Plugin.GetResource<Block>("MinecraftTextures.frosted_ice_0") },
            { 213, Plugin.GetResource<Block>("MinecraftTextures.magma") },
            { 214, Plugin.GetResource<Block>("MinecraftTextures.nether_wart_block") },
            { 215, Plugin.GetResource<Block>("MinecraftTextures.red_nether_brick") },
            { 338, Plugin.GetResource<Block>("MinecraftTextures.reeds") },
            { 389, Plugin.GetResource<Block>("MinecraftTextures.itemframe_background") },
        };
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
            chunks.SaveOnGenerate = true;
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
                                Blocks.TryGetValue(BlockID_a, out block);
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