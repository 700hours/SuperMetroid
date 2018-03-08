using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items.Armors
{
	[AutoloadEquip(EquipType.Body)]
	public class BlueSuitBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Blue Suit Breastplate");
			Tooltip.SetDefault("This shouldn't be in your inventory");
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
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == mod.ItemType("BlueSuitHelmet") && legs.type == mod.ItemType("BlueSuitGreaves");
		}
	}
}