using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SuperMetroid.Tiles.Extensions
{
	public class ReserveTank : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= false;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Reserve Tank");
			AddMapEntry(new Color(200, 125, 50), name);
			disableSmartCursor = true;
			animationFrameHeight = 18;
		}
		public static bool modPlayer.isLighted = false;
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<MetroidPlayer>(mod);
			
			if(modPlayer.isLighted)
			{
				r = 0.2f;
				g = 0.05f;
				b = 0.0f;
			}
		}
	}
}