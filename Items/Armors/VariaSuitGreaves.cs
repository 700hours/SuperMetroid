using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items.Armors
{
	[AutoloadEquip(EquipType.Legs)]
	public class VariaSuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Varia Suit Greaves");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 100;
			item.rare = 4;
			item.defense = 3;
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