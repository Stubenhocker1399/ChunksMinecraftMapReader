/*ing Chunks;
using Chunks.Geometry;
using Substrate;
using Substrate.Core;
using Substrate.Nbt;
using System.IO;


namespace MinecraftMapReader.Source
{
    class WorldReader : ChunkGenerator
    {
        NbtWorld srcWorld;
        protected override void GenerateChunkColumn(ChunkColumn chunks)
        {
            var worldfolder = "C:\\Users\\Chris\\AppData\\LocalLow\\Facepunch Studios\\Chunks\\Worlds\\world";
            if (!Directory.Exists(worldfolder))
                //Directory.CreateDirectory(dst);
                Debug.Log("Couldn't locate Minecraft World to import.");
            //AlphaWorld betasrcWorld = AlphaWorld.Open(worldfolder);
            srcWorld = NbtWorld.Open(worldfolder);
            IChunkManager cm = srcWorld.GetChunkManager();
            IBlockManager bm = srcWorld.GetBlockManager();
            //AlphaChunkManager cm = betasrcWorld.GetChunkManager();
            if (cm.ChunkExists(chunks.Min.X % 16, chunks.Min.Y % 16))
            {
                Debug.Log("Chunk exists! At %d %d", chunks.Min.X % 16, chunks.Min.Y % 16);
                for (var x = chunks.Min.X; x < chunks.Max.X; ++x)
                    for (var z = chunks.Min.Z; z < chunks.Max.Z; ++z)
                        for(var y = 0; y <= 256; ++y)
                        {
                            if(bm.GetID(x,y,z) != Substrate.BlockType.AIR)
                            {
                                var randi = CreateRandom(chunks);
                                var grassBlock = GetBlock("Grass", ((float)randi.NextDouble() - 0.5f) * 0.25f);
                                chunks.Set(new IntVector(x,y,z), grassBlock);
                            }
                        }
            }
            else //No chunk exists at given coorinates
            {
                var rando = CreateRandom(chunks);
                var grassBlock = GetBlock("Grass", ((float)rando.NextDouble() - 0.5f) * 0.25f);
                
                //Standing Platform to reach debug console
                for (var x = chunks.Min.X; x < chunks.Max.X; ++x)
                    for (var z = chunks.Min.Z; z < chunks.Max.Z; ++z)
                    {
                        chunks.Set(new IntVector(x, 0, z), grassBlock);
                        if (x > -20 && x < 20 && z > -20 && z < 20)
                        {
                            chunks.Set(new IntVector(x, 30, z), grassBlock);
                        }
                    }
                /*for (var y = 0; y <= 256; ++y)
                {

                    var block = GetBlockManager().GetBlock(x, y, z);
                    var block = chunk
                    if (block.ID != 0)
                        chunks.Set(new IntVector(x, y, z), grassBlock);
                }*//*
                Debug.Log(chunks.Min.X % 16);
            }
            var rand = CreateRandom(chunks);
            for (var x = chunks.Min.X; x < chunks.Max.X; ++x)
                for (var z = chunks.Min.Z; z < chunks.Max.Z; ++z)
                {
                    var grassBlock = GetBlock("Grass", ((float)rand.NextDouble() - 0.5f) * 0.25f);
                    //Standing Platform to reach debug console
                    if (x > -20 && x < 20 && z > -20 && z < 20)
                    {
                        chunks.Set(new IntVector(x, 30, z), grassBlock);
                    }
                }
        }
    }
}
    */