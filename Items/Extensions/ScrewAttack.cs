
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SuperMetroid;

namespace SuperMetroid.Items.Extensions
{
	public class ScrewAttack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Speed Booster");
			Tooltip.SetDefault("Damages enemies while somersaulting."
				+ 	"\nIncreased invincibility effect when airborne.");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
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
		
		bool Attacking;
		int sa;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		#region ScrewAttack
			if(player.controlRight || player.velocity.X > 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
				{
					Attacking = false;
				}
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("ScrewAttack"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 150;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("ScrewAttack"),damage,0,player.whoAmI);
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
					if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("ScrewAttack"))
					{
						Attacking = true;
						player.invis = true;
						break;
					}
				}		
				if(!Attacking)
				{
					int damage = 150;
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("ScrewAttack"),damage,0,player.whoAmI);
				}	
			}
		#endregion

			if(Attacking && player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && !MetroidPlayer.grappled)
			{
				player.head = -1;
				player.body = -1;
				player.legs = -1;
				player.wings = 0;	
			
				Color color = new Color();
				int dust = Dust.NewDust(player.position, player.width, player.height, 61, 0, 0, 200, color, 1.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = false;
			}
		}
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