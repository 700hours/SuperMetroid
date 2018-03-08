using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Projectiles
{
	public class ScrewAttackDiffusion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screw Attack Diffusion");
		}
		public override void SetDefaults()
		{
			projectile.width = 3;
			projectile.height = 3;
			projectile.aiStyle = 0;
			projectile.timeLeft = 200;
			projectile.friendly = true;
			projectile.penetrate = 500;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1.6f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		
		public void AI()
		{
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 6, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = true;
		}
	}
}