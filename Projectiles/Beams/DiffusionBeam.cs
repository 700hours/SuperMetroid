using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;
using SuperMetroid.Tiles.Shutters;

namespace SuperMetroid.Projectiles.Beams
{
	public class DiffusionBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diffusion Beam");
		}
		public override void SetDefaults()
		{
			projectile.width = 1;
			projectile.height = 1;
			projectile.aiStyle = 1;
			projectile.timeLeft = 200;
			projectile.friendly = true;
			projectile.penetrate = 500;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1.6f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		public override void AI()
		{
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 11, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = true;
		}
	}
}