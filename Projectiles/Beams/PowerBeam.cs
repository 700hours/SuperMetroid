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
	public class PowerBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Beam");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 8800;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
	}
}