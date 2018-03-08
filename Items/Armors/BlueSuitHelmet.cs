using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items.Armors
{
	[AutoloadEquip(EquipType.Head)]
	public class BlueSuitHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Blue Suit Helmet");
			Tooltip.SetDefault("This shouldn't be in your inventory");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 100;
			item.rare = 4;
			item.defense = 1;
		}
	}
}