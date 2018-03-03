using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid
{
	public class SuperMetroid : Mod
	{
		public void SetModInfo(out string name, ref ModProperties properties)
		{
			name = "Super Metroid";
			properties.Autoload = true;
			properties.AutoloadGores = true;
			properties.AutoloadSounds = true;
		}
	}
}
