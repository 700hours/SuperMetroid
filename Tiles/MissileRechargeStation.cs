using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles
{
	public class MissileRechargeStation : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= false;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= false;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= false;
			Main.tileNoAttach[Type] 		= true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Full Recharge Station");
			AddMapEntry(new Color(200, 200, 200), name);
			disableSmartCursor = true;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			player.showItemIconText = "Stand near, and right click to use";
		}
		int x, y;
		int missileT = 0;
		int m = 0;
		int m1 = 0;
		int m2 = 0;
		int m3 = 0;
		int mCount = 0;
		int RefillSound = 0;
		public override void RightClick(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			for(int k = 0; k < player.inventory.Length-1; k++)
			{
				if(player.inventory[k].type == mod.ItemType("Missile")) mCount += player.inventory[k].stack;
			}
			
			var modPlayer = player.GetModPlayer<MetroidPlayer>(mod);
			
			int maxMissiles = modPlayer.missileUpg*5;
						
			if(Main.tile[(int)player.position.X/16, (int)player.position.Y/16+1].type == mod.TileType("MissileRechargeStation"))
			{
				RefillSound++;
				if(mCount < maxMissiles)
				{
					for(int k = 0; k < maxMissiles - mCount; k++){
						Item.NewItem(i*16,j*16,32,32,(int)mod.ItemType("Missile"),1,false);
					}
				}
				else RefillSound = 0;
				if(RefillSound > 0)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/FullRefill"), new Vector2(i*16, j*16));
					RefillSound = 0;
				}
			}
		}
	}
}