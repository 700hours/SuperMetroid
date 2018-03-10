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
	public class IceWaveChargeLead : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Wave Charge Beam Lead");
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
		float CHARGE = 0;
		int Pindex = -1;
		int Pindex2 = -1;
		int Pindex3 = -1;
		int Pindex4 = -1;
		int Pindex5 = -1;
		int Pindex6 = -1;
		int Pindex7 = -1;
		public override void AI() 
		{
			CHARGE++;
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			if(CHARGE > 101) CHARGE = 101;

			float PlayerCentreX = O.position.X + O.width/2;
			float PlayerCentreY = O.position.Y + O.height/2;
			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			if (Main.myPlayer == P.owner/* && O.height == 42*/)
			{
				if (O.channel || O.controlUseItem)
				{
					float num119 = 0f;
					if (O.inventory[O.selectedItem].shoot == P.type)
					{
						num119 = O.inventory[O.selectedItem].shootSpeed * 17;
					}
					P.velocity.X = (float)Math.Cos(P.rotation)*num119;
					P.velocity.Y = (float)Math.Sin(P.rotation)*num119;
					P.timeLeft = 2;
				}
				else
				{
					ShootOnDeath(O,P);
				}
			}

			P.scale = CHARGE / 100;
			float targetrotation = (float)Math.Atan2((MY-PlayerCentreY),(MX-PlayerCentreX));
			P.rotation = targetrotation;
			O.itemTime = 2;
			O.itemAnimation = 2;
			P.position = new Vector2(O.itemLocation.X+(float)Math.Cos(targetrotation)*17+1,O.itemLocation.Y+(float)Math.Sin(targetrotation)*17-5);
			if(P.velocity.X < 0)
			{
				P.direction = -1;
			}
			else
			{
				P.direction = 1;
			}
			P.spriteDirection = P.direction;
			O.heldProj = P.whoAmI;
			O.direction = P.direction;
			O.itemRotation = (float)Math.Atan2((MY-PlayerCentreY)*O.direction,(MX-PlayerCentreX)*O.direction) -0.05f*O.direction;
		//	if(Pindex != -1 && Main.projectile[Pindex].active) Main.projectile[Pindex].RunMethod("TotalRotate",P.position-new Vector2(3f*O.direction,3f*O.gravDir),new Vector2((float)Math.Cos(targetrotation),(float)Math.Sin(targetrotation)));
		//	if(Pindex2 != -1 && Main.projectile[Pindex2].active) Main.projectile[Pindex2].RunMethod("TotalRotate",P.position-new Vector2(3f*O.direction,3f*O.gravDir),new Vector2((float)Math.Cos(targetrotation),(float)Math.Sin(targetrotation)));

			P.width = (int)(16f*P.scale);
			P.height = (int)(16f*P.scale);

		#region sounds and dust
			if(CHARGE > 9 && CHARGE < 11)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ChargeStartup"), projectile.position);
			}
			else if(CHARGE > 97 && CHARGE < 99)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ChargeMax"), projectile.position);
			}
			if(CHARGE > 99)
			{
				Color color = new Color();
				int dust = Dust.NewDust(P.position+P.velocity, P.width, P.height, 59, 0, 0, 100, color, 2.0f);
				Main.dust[dust].noGravity = true;
			}
		#endregion
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = projectile;
			P.active = false;
			Player O = Main.player[P.owner];
			float PlayerCentreX = O.position.X + O.width * 0.5f;
			float PlayerCentreY = O.position.Y + O.height * 0.5f;
			if(O.height == 14 && CHARGE > 99 && O.statMana > 9)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/LayBomb"), projectile.position);
				Pindex3 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,-5,-2,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex4 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,-3,-4,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex5 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,0,-5,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex6 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,3,-4,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex7 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,5,-2,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Main.projectile[Pindex3].timeLeft = 90;
				Main.projectile[Pindex4].timeLeft = 90;
				Main.projectile[Pindex5].timeLeft = 90;
				Main.projectile[Pindex6].timeLeft = 90;
				Main.projectile[Pindex7].timeLeft = 90;
				O.statMana -= 10;
				O.manaRegenDelay = (int)O.maxRegenDelay;
			}
			else if(O.height == 14 && CHARGE > 99)
			{
			}
		}
		public void ShootOnDeath(Player O,Projectile P)
		{
			float PlayerCentreX = O.position.X + O.width * 0.5f;
			float PlayerCentreY = O.position.Y + O.height * 0.5f;
			// angle from centre of player to mouse
			float Angle = (float)Math.Atan2(Main.screenPosition.Y + Main.mouseY - PlayerCentreY,
											Main.screenPosition.X + Main.mouseX - PlayerCentreX);
			if(CHARGE > 99 && O.statMana > (59 - (int)((float)O.inventory[O.selectedItem].mana*O.manaCost)) && O.height == 42)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/IceBeamChargeShot"), projectile.position);
				Pindex = Projectile.NewProjectile(0.0f, 0.0f, 0.0f, 0.0f, mod.ProjectileType("IceWaveChargeShot1"), 50*(int)O.rangedDamage, 3.0f, Main.myPlayer);
				Pindex2 = Projectile.NewProjectile(0.0f, 0.0f, 0.0f, 0.0f, mod.ProjectileType("IceWaveChargeShot2"), 50*(int)O.rangedDamage, 3.0f, Main.myPlayer);

				Main.projectile[Pindex].position.X = PlayerCentreX - Main.projectile[Pindex].width * 0.5f;
				Main.projectile[Pindex].position.Y = PlayerCentreY - Main.projectile[Pindex].height * 0.5f;
				Main.projectile[Pindex2].position.X = PlayerCentreX - Main.projectile[Pindex2].width * 0.5f;
				Main.projectile[Pindex2].position.Y = PlayerCentreY - Main.projectile[Pindex2].height * 0.5f;
				// for use in the projectile cs
				Main.projectile[Pindex].ai[0] = Angle;
				Main.projectile[Pindex].ai[1] = (float)Main.myPlayer;
				Main.projectile[Pindex2].ai[0] = Angle;
				Main.projectile[Pindex2].ai[1] = (float)Main.myPlayer;
				O.statMana -= 60 - (int)((float)O.inventory[O.selectedItem].mana*O.manaCost);
				O.manaRegenDelay = (int)O.maxRegenDelay;
			}
			else/* if(O.height == 42)*/
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/IceBeam"), projectile.position);
				Pindex = Projectile.NewProjectile(0.0f, 0.0f, 0.0f, 0.0f, mod.ProjectileType("IceWave"), 20*(int)O.rangedDamage, 3.0f, Main.myPlayer);
				Pindex2 = -1;

				Main.projectile[Pindex].position.X = PlayerCentreX - Main.projectile[Pindex].width * 0.5f;
				Main.projectile[Pindex].position.Y = PlayerCentreY - Main.projectile[Pindex].height * 0.5f;
				// for use in the projectile cs
				Main.projectile[Pindex].ai[0] = Angle;
				Main.projectile[Pindex].ai[1] = (float)Main.myPlayer;
			}
		}
	}
}