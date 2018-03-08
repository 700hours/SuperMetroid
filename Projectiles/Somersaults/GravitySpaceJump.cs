using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace SuperMetroid.Projectiles.Somersaults
{
	public class GravitySpaceJump : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Space Jump");
			Main.projFrames[projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.aiStyle = 0;
			projectile.timeLeft = 9000;
			projectile.friendly = true;
			projectile.penetrate = 500;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		
		int DelayTime = 0;
		int SpaceJumpStart = 0;
		public override void AI()
		{
			Player P = Main.player[projectile.owner];
			projectile.position.X=(P.position.X-P.width/3)-6;
			projectile.position.Y=(P.position.Y+P.height/4)-12;
			projectile.direction = P.direction;
			DelayTime++;
			SpaceJumpStart++;
			if((float)DelayTime > 16.2)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Spin"), projectile.position);
				DelayTime = 0;
			}
			if(SpaceJumpStart > 3 && SpaceJumpStart < 5)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Spin"), projectile.position);
				SpaceJumpStart = 6;
				if(SpaceJumpStart > 6)
				{
					SpaceJumpStart = 6;
				}
			}
			if(P.velocity.Y == 0 || P.velocity.X == 0 || P.height == 14 || P.itemAnimation != 0 || P.controlHook || P.grapCount != 0/* || ModPlayer.grappled*/)
			{
				projectile.Kill();
			}
			foreach(Projectile Pr in Main.projectile) if (Pr!= null)
			{
				if(Pr.active && Pr.type == mod.ProjectileType("ShineSpark") || Pr.active && Pr.type == mod.ProjectileType("ScrewAttack"))
				{
					  projectile.Kill();
				}
			}

			projectile.frameCounter++;
			if(projectile.frameCounter >= 3)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= 4)
			{
				projectile.frame = 0;
			}
		}
		
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = Main.projectileTexture[projectile.type];
			SpriteEffects effects = SpriteEffects.None;
			effects = SpriteEffects.FlipHorizontally;
			
			int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int startY = frameHeight * projectile.frame;
			
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size();
			
			if(player.velocity.X < 0)
			{
				spriteBatch.Draw(Main.projectileTexture[projectile.type], 
					new Vector2(projectile.position.X + 38, projectile.position.Y + 48) - Main.screenPosition, 
					sourceRectangle, Color.White, projectile.rotation, origin, projectile.scale, effects, 0f);
			}
			if(player.velocity.X > 0)
			{
				spriteBatch.Draw(Main.projectileTexture[projectile.type], 
					new Vector2(projectile.position.X + 38, projectile.position.Y + 48) - Main.screenPosition, 
					sourceRectangle, Color.White, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
			}	
			return false;
		}
	}
}