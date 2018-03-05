using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Doors
{
	public class DoorCorridor : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= false;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Door Corridor");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
	}
}