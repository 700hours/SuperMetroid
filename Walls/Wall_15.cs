using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SuperMetroid.Walls
{
	public class Wall_15 : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = mod.DustType("Sparkle");
			drop = mod.ItemType("Wall_15");
			AddMapEntry(new Color(150, 150, 150));
		}

	/*	public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.4f;
			g = 0.4f;
			b = 0.4f;
		}	*/
	}
}