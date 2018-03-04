using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace SuperMetroid.Projectiles.Mechanics
{
	public class Crumble : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 24;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.netUpdate = true;
		}
		int ticks = 0;
		public override void AI()
		{
		//	projectile.AI(true);
		#region frames
			if(ticks < 24) ticks++;
			if (ticks%4 == 0) this.projectile.frame++;
			if (this.projectile.frame >= 6) projectile.active = false;
		#endregion
		}
	}
}