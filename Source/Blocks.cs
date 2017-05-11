using Chunks.Geometry;
using Chunks.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinecraftMapReader.Source
{
    class Blocks
    {
        public Dictionary<int, Block> blocks = new Dictionary<int, Block>
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
            { 23, Plugin.GetResource<Block>("MinecraftTextures.dispenser") },
            { 24, Plugin.GetResource<Block>("MinecraftTextures.sandstone") },
            { 25, Plugin.GetResource<Block>("MinecraftTextures.noteblock") },
            { 26, Plugin.GetResource<Block>("MinecraftTextures.bed_head_top") },
            { 27, Plugin.GetResource<Block>("MinecraftTextures.rail_golden") },
            { 28, Plugin.GetResource<Block>("MinecraftTextures.rail_detector") },
            { 29, Plugin.GetResource<Block>("MinecraftTextures.sticky_piston") },
            { 30, Plugin.GetResource<Block>("MinecraftTextures.web") },
            { 31, Plugin.GetResource<Block>("MinecraftTextures.tallgrass") },
            { 32, Plugin.GetResource<Block>("MinecraftTextures.deadbush") },
            { 33, Plugin.GetResource<Block>("MinecraftTextures.piston") },
            { 34, Plugin.GetResource<Block>("MinecraftTextures.piston_head") },
            { 35, Plugin.GetResource<Block>("MinecraftTextures.wool_colored_white") },
            //36 technical piston block, invisible & untouchable
            { 37, Plugin.GetResource<Block>("MinecraftTextures.flower_dandelion") },
            { 38, Plugin.GetResource<Block>("MinecraftTextures.flower_rose") },
            { 39, Plugin.GetResource<Block>("MinecraftTextures.mushroom_brown") },
            { 40, Plugin.GetResource<Block>("MinecraftTextures.mushroom_red") },
            { 41, Plugin.GetResource<Block>("MinecraftTextures.gold_block") },
            { 42, Plugin.GetResource<Block>("MinecraftTextures.iron_block") },
            { 43, Plugin.GetResource<Block>("MinecraftTextures.double_stone_slab") },
            { 44, Plugin.GetResource<Block>("MinecraftTextures.stone_slab") },
            { 45, Plugin.GetResource<Block>("MinecraftTextures.brick") },
            { 46, Plugin.GetResource<Block>("MinecraftTextures.tnt") },
            { 47, Plugin.GetResource<Block>("MinecraftTextures.bookshelf") },
            { 48, Plugin.GetResource<Block>("MinecraftTextures.cobblestone_mossy") },
            { 49, Plugin.GetResource<Block>("MinecraftTextures.obsidian") },
            { 50, Plugin.GetResource<Block>("MinecraftTextures.torch_on") },
            { 51, Plugin.GetResource<Block>("MinecraftTextures.fire") },
            { 52, Plugin.GetResource<Block>("MinecraftTextures.mob_spawner") },
            { 53, Plugin.GetResource<Block>("MinecraftTextures.oak_stairs") },
            { 54, Plugin.GetResource<Block>("MinecraftTextures.chest") },
            { 55, Plugin.GetResource<Block>("MinecraftTextures.redstone_wire") },
            { 56, Plugin.GetResource<Block>("MinecraftTextures.diamond_ore") },
            { 57, Plugin.GetResource<Block>("MinecraftTextures.diamond_block") },
            { 58, Plugin.GetResource<Block>("MinecraftTextures.crafting_table") },
            { 59, Plugin.GetResource<Block>("MinecraftTextures.wheat_stage_7") },
            { 60, Plugin.GetResource<Block>("MinecraftTextures.farmland") },
            { 61, Plugin.GetResource<Block>("MinecraftTextures.furnace") },
            { 62, Plugin.GetResource<Block>("MinecraftTextures.lit_furnace") },
            //63 standing_sign
            { 64, Plugin.GetResource<Block>("MinecraftTextures.door_wood_lower") },
            { 65, Plugin.GetResource<Block>("MinecraftTextures.ladder") },
            { 66, Plugin.GetResource<Block>("MinecraftTextures.rail_normal") },
            { 67, Plugin.GetResource<Block>("MinecraftTextures.stone_stairs") },
            //68 wall_sign
            { 69, Plugin.GetResource<Block>("MinecraftTextures.lever") },
            //70 stone_pressure_plate
            { 71, Plugin.GetResource<Block>("MinecraftTextures.door_iron_lower") },
            //72 wooden_pressure_plate
            { 73, Plugin.GetResource<Block>("MinecraftTextures.redstone_ore") },
            { 74, Plugin.GetResource<Block>("MinecraftTextures.lit_redstone_ore") },
            { 75, Plugin.GetResource<Block>("MinecraftTextures.redstone_torch_off") },
            { 76, Plugin.GetResource<Block>("MinecraftTextures.redstone_torch_on") },
            //77 stone_button
            { 78, Plugin.GetResource<Block>("MinecraftTextures.snow_layer") },
            { 79, Plugin.GetResource<Block>("MinecraftTextures.ice") },
            { 80, Plugin.GetResource<Block>("MinecraftTextures.snow") },
            { 81, Plugin.GetResource<Block>("MinecraftTextures.cactus") },
            { 82, Plugin.GetResource<Block>("MinecraftTextures.clay") },
            { 83, Plugin.GetResource<Block>("MinecraftTextures.reeds") },
            { 84, Plugin.GetResource<Block>("MinecraftTextures.jukebox") },
            { 85, Plugin.GetResource<Block>("MinecraftTextures.fence") },
            { 86, Plugin.GetResource<Block>("MinecraftTextures.pumpkin") },
            { 87, Plugin.GetResource<Block>("MinecraftTextures.netherrack") },
            { 88, Plugin.GetResource<Block>("MinecraftTextures.soul_sand") },
            { 89, Plugin.GetResource<Block>("MinecraftTextures.glowstone") },
            { 90, Plugin.GetResource<Block>("MinecraftTextures.portal") },
            { 91, Plugin.GetResource<Block>("MinecraftTextures.lit_pumpkin") },
            { 92, Plugin.GetResource<Block>("MinecraftTextures.cake") },
            { 93, Plugin.GetResource<Block>("MinecraftTextures.repeater_off") },
            { 94, Plugin.GetResource<Block>("MinecraftTextures.repeater_on") },
            { 95, Plugin.GetResource<Block>("MinecraftTextures.stained_glass") },
            { 96, Plugin.GetResource<Block>("MinecraftTextures.trapdoor") },
            { 97, Plugin.GetResource<Block>("MinecraftTextures.monster_egg") },
            { 98, Plugin.GetResource<Block>("MinecraftTextures.stonebrick") },
            { 99, Plugin.GetResource<Block>("MinecraftTextures.mushroom_block_skin_brown") },
            { 100, Plugin.GetResource<Block>("MinecraftTextures.mushroom_block_skin_red") },
            { 101, Plugin.GetResource<Block>("MinecraftTextures.iron_bars") },
            { 102, Plugin.GetResource<Block>("MinecraftTextures.glass_pane") },
            { 103, Plugin.GetResource<Block>("MinecraftTextures.melon_block") },
            { 104, Plugin.GetResource<Block>("MinecraftTextures.pumpkin_stem") },
            { 105, Plugin.GetResource<Block>("MinecraftTextures.melon_stem") },
            { 106, Plugin.GetResource<Block>("MinecraftTextures.vine") },
            { 107, Plugin.GetResource<Block>("MinecraftTextures.fence_gate") },
            { 108, Plugin.GetResource<Block>("MinecraftTextures.brick_stairs") },
            { 109, Plugin.GetResource<Block>("MinecraftTextures.stone_brick_stairs") },
            { 110, Plugin.GetResource<Block>("MinecraftTextures.mycelium") },
            { 111, Plugin.GetResource<Block>("MinecraftTextures.waterlily") },
            { 112, Plugin.GetResource<Block>("MinecraftTextures.nether_brick") },
            { 113, Plugin.GetResource<Block>("MinecraftTextures.nether_brick_fence") },
            { 114, Plugin.GetResource<Block>("MinecraftTextures.nether_brick_stairs") },
            { 115, Plugin.GetResource<Block>("MinecraftTextures.nether_wart_stage_2") },
            //116 enchating_table
            //117 brewing_stand
            //118 cauldron
            //119 end_portal
            //120 end_portal_frame
            { 121, Plugin.GetResource<Block>("MinecraftTextures.end_stone") },
            { 122, Plugin.GetResource<Block>("MinecraftTextures.dragon_egg") },
            { 123, Plugin.GetResource<Block>("MinecraftTextures.redstone_lamp_off") },
            { 124, Plugin.GetResource<Block>("MinecraftTextures.redstone_lamp_on") },
            { 125, Plugin.GetResource<Block>("MinecraftTextures.double_wooden_slab") },
            { 126, Plugin.GetResource<Block>("MinecraftTextures.wooden_slab") },
            { 127, Plugin.GetResource<Block>("MinecraftTextures.cocoa_stage_0") },
            { 128, Plugin.GetResource<Block>("MinecraftTextures.sandstone_stairs") },
            { 129, Plugin.GetResource<Block>("MinecraftTextures.emerald_ore") },
            //130 ender_chest
            { 131, Plugin.GetResource<Block>("MinecraftTextures.trip_wire_source") },
            { 132, Plugin.GetResource<Block>("MinecraftTextures.trip_wire") },
            { 133, Plugin.GetResource<Block>("MinecraftTextures.emerald_block") },
            { 134, Plugin.GetResource<Block>("MinecraftTextures.spruce_stairs") },
            { 135, Plugin.GetResource<Block>("MinecraftTextures.birch_stairs") },
            { 136, Plugin.GetResource<Block>("MinecraftTextures.jungle_stairs") },
            //137 command_block
            { 138, Plugin.GetResource<Block>("MinecraftTextures.beacon") },
            { 139, Plugin.GetResource<Block>("MinecraftTextures.cobblestone_wall") },
            { 140, Plugin.GetResource<Block>("MinecraftTextures.flower_pot") },
            { 141, Plugin.GetResource<Block>("MinecraftTextures.carrots_stage_3") },
            { 142, Plugin.GetResource<Block>("MinecraftTextures.potatoes_stage_3") },
            //143 wooden_button
            //144 mob_head
            //145 anvil
            //146 traped_chest
            //147 light_weighted_pressure_plate
            //148 heavy_weighted_pressure_plate
            { 149, Plugin.GetResource<Block>("MinecraftTextures.comparator_off") },
            { 150, Plugin.GetResource<Block>("MinecraftTextures.comparator_on") },
            //151 daylight_detector
            { 152, Plugin.GetResource<Block>("MinecraftTextures.redstone_block") },
            { 153, Plugin.GetResource<Block>("MinecraftTextures.quartz_ore") },
            //154 hopper
            { 155, Plugin.GetResource<Block>("MinecraftTextures.quartz_block") },
            { 156, Plugin.GetResource<Block>("MinecraftTextures.quartz_stairs") },
            { 157, Plugin.GetResource<Block>("MinecraftTextures.rail_activator") },
            { 158, Plugin.GetResource<Block>("MinecraftTextures.dropper") },
            { 159, Plugin.GetResource<Block>("MinecraftTextures.hardened_clay_stained_white") },
            { 160, Plugin.GetResource<Block>("MinecraftTextures.stained_glass_pane)") },
            { 161, Plugin.GetResource<Block>("MinecraftTextures.leaves_acacia") },
            { 162, Plugin.GetResource<Block>("MinecraftTextures.log2") },
            { 163, Plugin.GetResource<Block>("MinecraftTextures.acacia_stairs") },
            { 164, Plugin.GetResource<Block>("MinecraftTextures.dark_oak_stairs") },
            { 165, Plugin.GetResource<Block>("MinecraftTextures.slime") },
            { 166, Block.Empty },// barrier
            { 167, Plugin.GetResource<Block>("MinecraftTextures.iron_trapdoor") },
            { 168, Plugin.GetResource<Block>("MinecraftTextures.prismarine") },
            { 169, Plugin.GetResource<Block>("MinecraftTextures.sea_lantern") },
            { 170, Plugin.GetResource<Block>("MinecraftTextures.hay_block") },
            { 171, Plugin.GetResource<Block>("MinecraftTextures.carpet") },
            { 172, Plugin.GetResource<Block>("MinecraftTextures.hardened_clay") },
            { 173, Plugin.GetResource<Block>("MinecraftTextures.coal_block") },
            { 174, Plugin.GetResource<Block>("MinecraftTextures.ice_packed") },
            { 175, Plugin.GetResource<Block>("MinecraftTextures.double_plant_sunflower_bottom") },
            //176 standing_banner
            //177 wall_banner
            //178 daylight_detector_inverted
            { 179, Plugin.GetResource<Block>("MinecraftTextures.red_sandstone") },
            { 180, Plugin.GetResource<Block>("MinecraftTextures.red_sandstone_stairs") },
            { 181, Plugin.GetResource<Block>("MinecraftTextures.double_stone_slab2") },
            { 182, Plugin.GetResource<Block>("MinecraftTextures.stone_slab2") },
            { 183, Plugin.GetResource<Block>("MinecraftTextures.spruce_fence_gate") },
            { 184, Plugin.GetResource<Block>("MinecraftTextures.birch_fence_gate") },
            { 185, Plugin.GetResource<Block>("MinecraftTextures.jungle_fence_gate") },
            { 186, Plugin.GetResource<Block>("MinecraftTextures.dark_oak_fence_gate") },
            { 187, Plugin.GetResource<Block>("MinecraftTextures.acacia_fence_gate") },
            { 188, Plugin.GetResource<Block>("MinecraftTextures.spruce_fence") },
            { 189, Plugin.GetResource<Block>("MinecraftTextures.birch_fence") },
            { 190, Plugin.GetResource<Block>("MinecraftTextures.jungle_fence") },
            { 191, Plugin.GetResource<Block>("MinecraftTextures.dark_oak_fence") },
            { 192, Plugin.GetResource<Block>("MinecraftTextures.acacia_fence") },
            { 193, Plugin.GetResource<Block>("MinecraftTextures.door_spruce_lower") },
            { 194, Plugin.GetResource<Block>("MinecraftTextures.door_birch_lower") },
            { 195, Plugin.GetResource<Block>("MinecraftTextures.door_jungle_lower") },
            { 196, Plugin.GetResource<Block>("MinecraftTextures.door_acacia_lower") },
            { 197, Plugin.GetResource<Block>("MinecraftTextures.door_dark_oak_lower") },
            { 198, Plugin.GetResource<Block>("MinecraftTextures.end_rod") },
            { 199, Plugin.GetResource<Block>("MinecraftTextures.chorus_plant") },
            { 200, Plugin.GetResource<Block>("MinecraftTextures.chorus_flower") },
            { 201, Plugin.GetResource<Block>("MinecraftTextures.purpur_block") },
            { 202, Plugin.GetResource<Block>("MinecraftTextures.purpur_pillar") },
            { 203, Plugin.GetResource<Block>("MinecraftTextures.purpur_stairs") },
            { 204, Plugin.GetResource<Block>("MinecraftTextures.purpur_double_slab") },
            { 205, Plugin.GetResource<Block>("MinecraftTextures.purpur_slab") },
            { 206, Plugin.GetResource<Block>("MinecraftTextures.end_bricks") },
            { 207, Plugin.GetResource<Block>("MinecraftTextures.beetroots_stage_3") },
            { 208, Plugin.GetResource<Block>("MinecraftTextures.grass_path") },
            //209 end_gateway
            //210 repeating_command_block
            //211 chain_command_block
            { 212, Plugin.GetResource<Block>("MinecraftTextures.frosted_ice_0") },
            { 213, Plugin.GetResource<Block>("MinecraftTextures.magma") },
            { 214, Plugin.GetResource<Block>("MinecraftTextures.nether_wart_block") },
            { 215, Plugin.GetResource<Block>("MinecraftTextures.red_nether_brick") },
            { 216, Plugin.GetResource<Block>("MinecraftTextures.bone_block") },
            //217 structure_void
            { 218, Plugin.GetResource<Block>("MinecraftTextures.observer") },
            //219 white_shulker_box
            //..
            //234 black_shulker_box
            //235 white_glazed_terracotta
            //..
            //250 black_glazed_terracotta
            //251 concrete
            //252 concrete_powder
            { 338, Plugin.GetResource<Block>("MinecraftTextures.reeds") },
            { 389, Plugin.GetResource<Block>("MinecraftTextures.itemframe_background") },
        };
    }
}
