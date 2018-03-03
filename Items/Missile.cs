using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items
{
	public class Missile : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 20;
			item.ranged = true;
			item.width = 6;
			item.height = 15;
			item.maxStack = 250;
			item.consumable = true;
			item.knockBack = 2f;
			item.value = 15;
			item.rare = 2;
			item.scale = 1.5f;
			item.shoot = mod.ProjectileType("Missile");
			item.shootSpeed = 8f;
			item.ammo = item.type;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
}