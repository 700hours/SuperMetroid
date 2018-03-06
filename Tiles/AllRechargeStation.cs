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
	public class AllRechargeStation : ModTile
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
		int missileT = 0;
		int mCount = 0;
		int m1 = 0;
		int m2 = 0;
		int m3 = 0;
		int smissileT = 0;
		int smCount = 0;
		int sm1 = 0;
		int sm2 = 0;
		int sm3 = 0;
		int RefillSound = 0;
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			player.showItemIconText = "Right click to use";
		}
		public override void RightClick(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			for(int k = 0; k < player.inventory.Length; k++)
			{
				if(player.inventory[k].type == mod.ItemType("Missile")) mCount += player.inventory[k].stack;
				if(player.inventory[k].type == mod.ItemType("SuperMissile")) smCount += player.inventory[k].stack;
			}
		#region ammo count
/*			if(player.inventory[44].type == mod.ItemType("Missile") && player.inventory[44].stack > 0)
			{
				m = player.inventory[44].stack;
			}
			if(player.inventory[45].type == mod.ItemType("Missile") && player.inventory[45].stack > 0)
			{
				m1 = player.inventory[45].stack;
			}
			if(player.inventory[46].type == mod.ItemType("Missile") && player.inventory[46].stack > 0)
			{
				m2 = player.inventory[46].stack;
			}
			if(player.inventory[47].type == mod.ItemType("Missile") && player.inventory[47].stack > 0)
			{
				m3 = player.inventory[47].stack;
			}
			if(player.inventory[44].type == mod.ItemType("SuperMissile") && player.inventory[44].stack > 0)
			{
				sm = player.inventory[44].stack;
			}
			if(player.inventory[45].type == mod.ItemType("SuperMissile") && player.inventory[45].stack > 0)
			{
				sm1 = player.inventory[45].stack;
			}
			if(player.inventory[46].type == mod.ItemType("SuperMissile") && player.inventory[46].stack > 0)
			{
				sm2 = player.inventory[46].stack;
			}
			if(player.inventory[47].type == mod.ItemType("SuperMissile")  && player.inventory[47].stack > 0)
			{
				sm3 = player.inventory[47].stack;
			}	*/
		#endregion
			if(RefillSound >= 0) RefillSound++;
			if(RefillSound >= 1800) RefillSound = 0;
			
			int maxMissiles = GlobalPlayer.missileUpg*5;
			int maxSMissiles = GlobalPlayer.smissileUpg*5;
			int type = mod.TileType("AllRechargeStation");
			Vector2 playerv = new Vector2(player.position.X/16, player.position.Y/16);
			if(Main.tile[(int)player.position.X/16, (int)player.position.Y/16+1].type == mod.TileType("AllRechargeStation"))
			{
				if(mCount < maxMissiles)
				{
					for(int k = 0; k < maxMissiles-mCount; k++)
					{
						Item.NewItem((int)i*16,(int)j*16,32,32,(int)mod.ItemType("Missile"),1,false);
					}
				}
				if(smCount < maxSMissiles)
				{
					for(int k = 0; k < maxSMissiles-smCount; k++)
					{
						Item.NewItem((int)i*16,(int)j*16,32,32,(int)mod.ItemType("SuperMissile"),1,false);
					}
				}
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/FullRefill"), new Vector2(i*16, j*16));
			}
		}
	}
}