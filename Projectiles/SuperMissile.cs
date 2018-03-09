using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Projectiles
{
	public class SuperMissile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile");
		}
		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 15;
			projectile.aiStyle = 1;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1.5f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		bool soundInit = false;
		public override void AI()
		{
			if(!soundInit) {
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/SMissile"), projectile.position);
				soundInit = true;
			}
		
		#region tile functions
			Vector2 tilev = new Vector2(projectile.position.X/16, projectile.position.Y/16);
			
		//	blocks
			int block = mod.TileType("CrackedBlock");
			int block2 = mod.TileType("smBlock");
		//	doors
			int type = 0;
			int type2 = mod.TileType("BlueSwitchleft");
			int type3 = mod.TileType("BlueSwitchright");
			int type4 = mod.TileType("vBlueDoor");
			int type5 = mod.TileType("ChozoDoor");
			int type6 = mod.TileType("MissileDoor");
			int type7 = mod.TileType("vMissileDoor");
			int type8 = mod.TileType("SuperMissileDoor");
			int type9 = mod.TileType("vSuperMissileDoor");
			int type10 = mod.TileType("GreenSwitchleft");
			int type11 = mod.TileType("GreenSwitchright");
			
			Tile T = Main.tile[(int)tilev.X, (int)tilev.Y];
		//	collision	
			bool collision = Main.tile[(int)tilev.X, (int)tilev.Y].active() && (Main.tileSolid[T.type] == true);
			bool correctTile = (block == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (block2 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool shutterSwitch = (type2 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type3 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type10 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type11 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool chozoDoor = (type4 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type5 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool missileDoor = (type6 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type7 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type8 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type9 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			if(collision)
			{
				PreKill(0);
				Kill(0);
				if(correctTile)
				{
					KillBlock((int)tilev.X, (int)tilev.Y);
				}
				if(shutterSwitch)
				{
					ShutterSwitch((int)tilev.X, (int)tilev.Y);
				}
				if(chozoDoor)
				{
					DoorToggle((int)tilev.X, (int)tilev.Y);
				}
				if(missileDoor)
				{
					DoorToggle((int)tilev.X, (int)tilev.Y);
				}
				projectile.active = false;
			}
		#endregion
		
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 6, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = true;
			
		/*	foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
			//	array about npcs to nullify gravity effects
			//	npcs affected "Geemer", "Owtch", "Sova","Sciser","Work Robot","Zero"
				Rectangle projrec = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X, (int)projectile.position.Y+(int)projectile.velocity.Y, projectile.width, projectile.height);
				Rectangle nrec = new Rectangle((int)N.position.X, (int)N.position.Y, (int)N.width,(int)N.height);
				if(projrec.Intersects(nrec))
				{
		//			N.AddBuff("Shaken", 10800, false);
					if(N.type == mod.NPCType("GoldTorizo"))
					{
						projectile.damage = 0;
					//	ModPlayer.catchMissile = true;
						projectile.active = false;
					}
					else
					{ 	projectile.damage = 60;		}
				}
			}*/
		}
		
		float Radius = 20f;
		public override void Kill(int timeleft)
		{
			Main.PlaySound(mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/SMBurst"), projectile.position);
			
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 6, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 60, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
		}
		
		public void KillBlock(int x,int y)
		{
		//	modPlayer.tileTime = 300;
		//	Main.tile[x, y].type = (ushort)type;
			WorldGen.KillTile(x, y,	false, false, true);
			Projectile.NewProjectile(x,y,0,0,mod.ProjectileType("Crumble"),0,0,Main.myPlayer);
		}
		public void ShutterSwitch(int x,int y)
		{
			var modPlayer = Main.player[projectile.owner].GetModPlayer<MetroidPlayer>(mod);
			
			modPlayer.tileTime = 300;
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ShutterDoor"), projectile.position);
			int type = mod.TileType("EmptyShutter");
			if(Main.tile[x, y].type == mod.TileType("BlueSwitchleft"))
			{
				for(int i = 0; i < 4; i++) Main.tile[x+1, (y+1)+i].type = (ushort)type;
			}
			if(Main.tile[x, y].type == mod.TileType("BlueSwitchright"))
			{
				for(int i = 0; i < 4; i++) Main.tile[x-1, (y+1)+i].type = (ushort)type;
			}
			if(Main.tile[x, y].type == mod.TileType("GreenSwitchleft"))
			{
				for(int i = 0; i < 4; i++) Main.tile[x+1, (y+1)+i].type = (ushort)type;
			}
			if(Main.tile[x, y].type == mod.TileType("GreenSwitchright"))
			{
				for(int i = 0; i < 4; i++) Main.tile[x-1, (y+1)+i].type = (ushort)type;
			}
		}
		public void DoorToggle(int x, int y)
		{
			var modPlayer = Main.player[projectile.owner].GetModPlayer<MetroidPlayer>(mod);
		
		//	to open entire door at once
			Wiring.TripWire(x, y, 1, 1);
				
			modPlayer.tileTime = 300;
			
			int type = mod.TileType("vBlueDoor");
			int type2 = mod.TileType("ChozoDoor");
			int type3 = mod.TileType("MissileDoor");
			int type4 = mod.TileType("vMissileDoor");
			int type5 = mod.TileType("SuperMissileDoor");
			int type6 = mod.TileType("vSuperMissileDoor");
			
			int transform = mod.TileType("vEmptyDoor");
			int transform2 = mod.TileType("ChozoDoorOpening");
			int transform3 = mod.TileType("EmptyChozoDoor");
						
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/DoorOpening"), projectile.position);
			
			if(Main.tile[x, y].type == (ushort)type || Main.tile[x, y].type == (ushort)type4 || Main.tile[x, y].type == (ushort)type6) Main.tile[x, y].type = (ushort)transform;
			if(Main.tile[x, y].type == (ushort)type2) Main.tile[x, y].type = (ushort)transform3;
			if(Main.tile[x, y].type == (ushort)type3 || Main.tile[x, y].type == (ushort)type5) Main.tile[x, y].type = (ushort)transform3;
		}
	}
}