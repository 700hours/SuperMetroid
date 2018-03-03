using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items
{
	public class SuperMissileLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile Launcher");
			Tooltip.SetDefault("Firing Super Missiles"
				+ "\nPress R to switch");
		}
		public override void SetDefaults()
		{
			item.width = 19;
			item.height = 13;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.value = 100;
			item.rare = 3;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Nothing");
			item.autoReuse = false;
			item.consumable = false;
			item.noMelee = true;
			item.ranged = true;
			item.shoot = mod.ProjectileType("SuperMissile");
			item.useAmmo = mod.ItemType("SuperMissile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public void UseStyle(Player P)
		{
			float PlayerCentreX = P.position.X + P.width/2;
			float PlayerCentreY = P.position.Y + P.height/2;
			float HalfPi = (float)(Math.PI/2f);
			if (Main.MouseWorld.X-P.position.X > 0) P.direction = 1;
			else P.direction = -1;
			P.itemRotation = (float)Math.Atan2((Main.MouseWorld.Y-PlayerCentreY)*P.direction,(Main.MouseWorld.X-PlayerCentreX)*P.direction);
			P.itemLocation.X-=P.direction*(P.width/8)*(1f-(float)Math.Abs(P.itemRotation)/HalfPi);
		}
	}
}