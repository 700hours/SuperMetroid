using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;

namespace SuperMetroid.Items.Extensions
{
	public class SpaceJump : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Speed Booster");
			Tooltip.SetDefault("Time a jump mid-air to jump again");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
			item.consumable = false;
			item.value = 100;
			item.rare = 5;
			item.scale = 1f;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
				
		int jumpTimer = 0;
		public override void UpdateAccessory(Player player, bool hideVisuals)
		{	
			if(player.velocity.Y > 0)
			{
				jumpTimer++;
			}
			else
			{
				jumpTimer = 0;
			}
			if(player.velocity.X != 0/* && !ModPlayer.ballstate*/)
			{
				if(player.controlJump && player.releaseJump && player.velocity.Y != 0 && player.velocity.Y >= 0.5f && jumpTimer < 30)
				{
					jumpTimer = 0;
					player.velocity.Y = -Player.jumpSpeed * player.gravDir;
					player.jump = Player.jumpHeight;
				}
			}
			var modPlayer = player.GetModPlayer<MetroidPlayer>(mod);
			
			if(modPlayer.PowerSuit && !modPlayer.VariaSuit && !modPlayer.GravitySuit)
			{
				UpdatePowerSuit(player);
			}
			if(modPlayer.PowerSuit && modPlayer.VariaSuit && !modPlayer.GravitySuit || (!modPlayer.PowerSuit && modPlayer.VariaSuit && !modPlayer.GravitySuit))
			{
				UpdateVariaSuit(player);
			}
			if((modPlayer.PowerSuit && modPlayer.GravitySuit) || (modPlayer.PowerSuit && modPlayer.VariaSuit && modPlayer.GravitySuit) || (!modPlayer.PowerSuit && modPlayer.VariaSuit && modPlayer.GravitySuit) || (!modPlayer.PowerSuit && !modPlayer.VariaSuit && modPlayer.GravitySuit))
			{
				UpdateGravitySuit(player);
			}
		}
		//----------------//
		#region suit effects
		bool Attacking;
		int sa;
		public void UpdatePowerSuit(Player player)
		{
		#region spin
			if(player.controlRight || player.velocity.X > 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
				{
					Attacking = false;
				}
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("PowerSpaceJump"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 0;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("PowerSpaceJump"),damage,0,player.whoAmI);
					player.invis = true;
				}	
			}

			if(player.controlLeft || player.velocity.X < 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
				{
					Attacking = false;
				}
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("PowerSpaceJump"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 0;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("PowerSpaceJump"),damage,0,player.whoAmI);
					player.invis = true;
				}	
			}
		#endregion

			if(Attacking && player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
			{
				player.head = -1;
				player.body = -1;
				player.legs = -1;
				player.wings = 0;	
			
				Color color = Color.Red;
				int dust = Dust.NewDust(player.position, player.width, player.height, 61, 0, 0, 200, color, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
		}
		public void UpdateVariaSuit(Player player)
		{
		#region spin
			if(player.controlRight || player.velocity.X > 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
				{
					Attacking = false;
				}
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("VariaSpaceJump"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 0;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("VariaSpaceJump"),damage,0,player.whoAmI);
				}	
			}

			if(player.controlLeft || player.velocity.X < 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
				{
					Attacking = false;
				}
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("VariaSpaceJump"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 0;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("VariaSpaceJump"),damage,0,player.whoAmI);
				}	
			}
		#endregion

			if(Attacking && player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
			{
				player.head = -1;
				player.body = -1;
				player.legs = -1;
				player.wings = 0;	
			
				Color color = Color.Red;
				int dust = Dust.NewDust(player.position, player.width, player.height, 61, 0, 0, 200, color, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
		}
		public void UpdateGravitySuit(Player player)
		{
		#region spin
			if(player.controlRight || player.velocity.X > 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
				{
					Attacking = false;
				}
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("GravitySpaceJump"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 0;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("GravitySpaceJump"),damage,0,player.whoAmI);
				}	
			}

			if(player.controlLeft || player.velocity.X < 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
				{
					Attacking = false;
				}
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("GravitySpaceJump"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 0;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("GravitySpaceJump"),damage,0,player.whoAmI);
				}	
			}
		#endregion

			if(Attacking && player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
			{
				player.head = -1;
				player.body = -1;
				player.legs = -1;
				player.wings = 0;	
			
				Color color = Color.Red;
				int dust = Dust.NewDust(player.position, player.width, player.height, 61, 0, 0, 200, color, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
		}
		#endregion 	
		public override bool OnPickup(Player player)
		{
			var modPlayer = player.GetModPlayer<MetroidPlayer>(mod);
			if(modPlayer.ffTimer <= 0)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ItemFanfare"), player.position);
				modPlayer.ffTimer = 3600;
			}
			return true;
		}
	}
}