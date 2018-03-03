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
	public class TorizoBombDiffusion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo Bomb Diffusion");
		}
		public override void SetDefaults()
		{
			projectile.width = 1;
			projectile.height = 1;
			projectile.aiStyle = 1;
			projectile.timeLeft = 200;
			projectile.friendly = false;
			projectile.penetrate = 500;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1.6f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		public override void AI()
		{
		//	projectile.AI(true);
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 64, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = true;
		}
	}
}