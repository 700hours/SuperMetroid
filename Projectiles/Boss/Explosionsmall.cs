using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Projectiles.Boss
{
	public class Explosionsmall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Small Explosion");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = -1;
			projectile.timeLeft = 300;
			projectile.friendly = false;
			projectile.penetrate = 500;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		public void AI()
		{
		//	projectile.AI(true);
		#region frames
			this.projectile.frameCounter++;
			if (this.projectile.frameCounter > 5)
			{
				this.projectile.frame++;
				this.projectile.frameCounter = 0;
			}
			if (this.projectile.frame >= 6)
			{
				this.projectile.active = false;
			}
		#endregion
		}
	}
}