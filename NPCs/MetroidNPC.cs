using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SuperMetroid;

namespace SuperMetroid.NPCs
{
	public class MetroidNPC : GlobalNPC
	{
		public void PostAI(NPC npc)
		{
		//	var modPlayer = Main.LocalPlayer.GetModPlayer<MetroidPlayer>(mod);
			if(MetroidPlayer.VariaSuit)
			{
				npc.damage = (int) (npc.damage*0.5f);
			}
			if(MetroidPlayer.VariaSuit && MetroidPlayer.GravitySuit)
			{
				npc.damage = (int) (npc.damage*0.25f);
			}
		/*	NPC N = npc;
			foreach(Projectile P in Main.projectile){
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
				if(N.boss) continue;
				if(P.type == mod.ProjectileType("ScrewAttack")){
				Rectangle PB = new Rectangle((int)P.position.X,(int)P.position.Y,P.width,P.height);
				Rectangle NB = new Rectangle((int)npc.position.X,(int)npc.position.Y,npc.width,npc.height);
					if(PB.Intersects(NB)){
						for(int i = 0; i < 5; i++) {
						int a = Projectile.NewProjectile(npc.position.X, npc.position.Y, 5, 5,mod.ProjectileType("ScrewAttackDiffusion"),0,0.1f,Main.myPlayer);
						Main.projectile[a].timeLeft = 15;
						Main.projectile[a].tileCollide = false;
						int b = Projectile.NewProjectile(npc.position.X, npc.position.Y, -5, 5,mod.ProjectileType("ScrewAttackDiffusion"),0,0.1f,Main.myPlayer);
						Main.projectile[b].timeLeft = 15;
						Main.projectile[b].tileCollide = false;
						int c = Projectile.NewProjectile(npc.position.X, npc.position.Y, 5, -5,mod.ProjectileType("ScrewAttackDiffusion"),0,0.1f,Main.myPlayer);
						Main.projectile[c].timeLeft = 15;
						Main.projectile[c].tileCollide = false;
						int d = Projectile.NewProjectile(npc.position.X, npc.position.Y, -5, -5,mod.ProjectileType("ScrewAttackDiffusion"),0,0.1f,Main.myPlayer);
						Main.projectile[d].timeLeft = 15;
						Main.projectile[d].tileCollide = false;
						}
					}
				}
			}	*/
		}
	}
}