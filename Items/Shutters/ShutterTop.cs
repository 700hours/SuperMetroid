using Terraria;
using Terraria.ModLoader;

namespace SuperMetroid.Items.Shutters
{
	public class ShutterTop : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shutter Top");
			Tooltip.SetDefault("Retractable door");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 99;
			item.useTime = 10;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.value = 100;
			item.rare = 1;
			item.autoReuse = true;
			item.useTurn = true;
			item.consumable = false;
			item.noMelee = true;
			item.createTile = mod.TileType("ShutterTop");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}