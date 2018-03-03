using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace SuperMetroid.Projectiles
{
	public class PowerBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb");
			Main.projFrames[projectile.type] = 3;
		}
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.timeLeft = 45;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1.5f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		int explode = 0;
		float BombRadius = 200f;
		public override void AI()
		{
		#region frames
				this.explode++;
				this.projectile.frameCounter++;
				if (this.projectile.frameCounter > 3)
				{
					this.projectile.frame++;
					this.projectile.frameCounter = 0;
				}
				if (this.projectile.frame >= 3)
				{
					this.projectile.frame = 0;
				}
		#endregion
		if(this.explode == 44)
			{
				Projectile.NewProjectile(projectile.Center.X+144,projectile.Center.Y,0,0,mod.ProjectileType("powerbombExplosion"),0,0,projectile.owner);
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Power Pomb"), projectile.position);
			}
		}
		public override void Kill(int timeleft)
		{
			float DTX = 500f; //detonation radius x
			float DTY = 300f; //detonation radius y
			for (int num70 = 0; num70 < 45; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-DTY, this.projectile.position.Y-DTY), this.projectile.width+(int)DTY*2, this.projectile.height+(int)DTY*2, 57, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-DTY, this.projectile.position.Y-DTY), this.projectile.width+(int)DTY*2, this.projectile.height+(int)DTY*2, 57, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			projectile.active = false;
		}
	}
}