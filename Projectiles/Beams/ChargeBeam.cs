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
	public class ChargeBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Beam");
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
		public override void Kill(int timeleft)
		{
			for(int i = 0; i < 20; i++) 
			{
				int num54 = Projectile.NewProjectile(this.projectile.position.X, this.projectile.position.Y,Main.rand.Next(10)-5,Main.rand.Next(10)-5,mod.ProjectileType("DiffusionBeam"),1,0.1f,this.projectile.owner);
				Main.projectile[num54].timeLeft = 15;
				Main.projectile[num54].tileCollide = false;
			}
		}
	}
}