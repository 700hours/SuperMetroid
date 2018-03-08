using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items.Armors
{
	[AutoloadEquip(EquipType.Body)]
	public class GravitySuitBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Gravity Suit Breastplate");
			Tooltip.SetDefault("Enables free movement in water");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.maxStack = 1;
			item.value = 100;
			item.rare = 4;
			item.defense = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == mod.ItemType("GravitySuitHelmet") && legs.type == mod.ItemType("GravitySuitGreaves");
		}
		
		bool Attacking;
		int sa;
		public override void UpdateArmorSet(Player player)
		{
		#region spin
			if(player.controlRight || player.velocity.X > 0 && player.velocity.Y != 0)
			{
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0/* && !ModPlayer.grappled*/)
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
				if(player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0/* && !ModPlayer.grappled*/)
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

			if(Attacking && player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0/* && !ModPlayer.grappled*/)
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
	}
}