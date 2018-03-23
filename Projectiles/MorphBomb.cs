using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using SuperMetroid.Tiles.Breakables;

namespace SuperMetroid.Projectiles
{
	public class MorphBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Bomb");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.timeLeft = 45;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1.5f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		public override void AI()
		{
		float scalez = 0.2f;
		Lighting.AddLight((int)((projectile.position.X + (float)(projectile.width / 2)) / 16f), (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f), scalez, scalez, scalez);  
		#region frames
					this.projectile.frameCounter++;
					if (this.projectile.frameCounter > 3)
					{
						this.projectile.frame++;
						this.projectile.frameCounter = 0;
					}
					if (this.projectile.frame >= 6)
					{
						this.projectile.frame = 0;
					}
		#endregion
			
			Vector2 tilev = new Vector2(projectile.position.X/16, projectile.position.Y/16);
			int radius = 1;
			int type = mod.TileType("CrackedBlock");
			Tile T = Main.tile[(int)tilev.X, (int)tilev.Y];
			for(int i = -1; i < 1; i++){
				bool correctTile = (type == Main.tile[(int)tilev.X + i, (int)tilev.Y].type);
				if(correctTile) 
				{
					KillBlock((int)tilev.X + i, (int)tilev.Y);
					Kill(0);
					projectile.active = false;
				}
			}
			for(int j = -1; j < 1; j++){
				bool correctTile = (type == Main.tile[(int)tilev.X, (int)tilev.Y + j].type);
				if(correctTile) 
				{
					KillBlock((int)tilev.X, (int)tilev.Y + j);
					Kill(0);
					projectile.active = false;
				}
			}
		}
		
		public override void Kill(int timeleft)
		{
			projectile.active = false;
		//	ModPlayer.grappled = false;
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Bomb"), projectile.position);
			
				float Xthreshold = 10f; //max speed
				float BombRadius = 50f; //max speed
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-BombRadius, this.projectile.position.Y-BombRadius), this.projectile.width+(int)BombRadius*2, this.projectile.height+(int)BombRadius*2, 59, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-BombRadius, this.projectile.position.Y-BombRadius), this.projectile.width+(int)BombRadius*2, this.projectile.height+(int)BombRadius*2, 61, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			foreach (NPC n in Main.npc)
			if (n.active && !n.friendly && !n.dontTakeDamage)
			{
				Vector2 MyVec = SMDV2(n,projectile);
				float num197 = Vector2.Distance(MyVec,Vector2.Zero);
				if (num197 < BombRadius)
				{
					n.velocity+=MyVec;
					n.StrikeNPC(this.projectile.damage, this.projectile.knockBack, this.projectile.direction, false, false);
					
				if (n.velocity.X > Xthreshold)
				{
					n.velocity.X = Xthreshold;
				}
				if (n.velocity.X < -Xthreshold)
				{
					n.velocity.X = -Xthreshold;
				}
				if (n.velocity.Y > Xthreshold)
				{
					n.velocity.Y = Xthreshold;
				}
				if (n.velocity.Y < -Xthreshold)
				{
					n.velocity.Y = -Xthreshold;
				}
				}
			}
			foreach (Player n in Main.player)
			if (n.active && n.hostile && n.team != Main.player[projectile.owner].team)
			{
				Vector2 MyVec = SMDV2(n,projectile);
				float num197 = Vector2.Distance(MyVec,Vector2.Zero);
				PlayerDeathReason PDR = new PlayerDeathReason();
				PDR = PlayerDeathReason.ByCustomReason(" was blown to pieces");
				if (num197 < BombRadius)
				{
					n.velocity+=MyVec;
					n.Hurt(PDR, (int)projectile.damage, (int)projectile.knockBack,true,false,false,-1);
				
				if (n.velocity.X > Xthreshold)
				{
					n.velocity.X = Xthreshold;
				}
				if (n.velocity.X < -Xthreshold)
				{
					n.velocity.X = -Xthreshold;
				}
				if (n.velocity.Y > Xthreshold)
				{
					n.velocity.Y = Xthreshold;
				}
				if (n.velocity.Y < -Xthreshold)
				{
					n.velocity.Y = -Xthreshold;
				}
				}
			}
			Vector2 MyVe1c = SMDV2(Main.player[projectile.owner],projectile);
			float num1917 = Vector2.Distance(MyVe1c,Vector2.Zero);
			if (num1917 < BombRadius)
			{
					Main.player[projectile.owner].velocity+=MyVe1c;
				
				if (Main.player[projectile.owner].velocity.X > Xthreshold)
				{
					Main.player[projectile.owner].velocity.X = Xthreshold;
				}
				if (Main.player[projectile.owner].velocity.X < -Xthreshold)
				{
					Main.player[projectile.owner].velocity.X = -Xthreshold;
				}
				if (Main.player[projectile.owner].velocity.Y > Xthreshold)
				{
					Main.player[projectile.owner].velocity.Y = Xthreshold;
				}
				if (Main.player[projectile.owner].velocity.Y < -Xthreshold)
				{
					Main.player[projectile.owner].velocity.Y = -Xthreshold;
				}
			}
			
		/*	for(int i = (int)(projectile.position.X-BombRadius)/16; i < (int)(projectile.position.X+BombRadius)/16; i++) {
				for(int j = (int)(projectile.position.Y-BombRadius)/16; j < (int)(projectile.position.Y+BombRadius)/16; j++) {
					if(Main.tile[i, j].type == mod.TileType("CrackedBlock"))
					{
						CrackedBlock.KillBlock(i, j);
					}
				//	Codable.RunTileMethod(false, new Vector2(i,j), Main.tile[i, j].type, "KillBBlock", i, j, null);
				//	Codable.RunTileMethod(false, new Vector2(i,j), Main.tile[i, j].type, "KillBlock", i, j, null);
				}
			}	*/
		}


		public Vector2 SMDV2(object var1,object var2)
		{
		#region returns vector2 of the distance
			float dist = 0;
			Vector2[] A1 = new Vector2[2];
			object varz = var1;
			for (int i = 0; i < 2; i++)
			{
			if (i == 0)
				varz = var1;
			if (i == 1)
				varz = var2;
			if (varz is Player)
			{
				Player pl = (Player)varz;
				A1[i] = new Vector2(pl.position.X+(pl.width/2),pl.position.Y+(pl.height/2));
			}
			if (varz is Projectile)
			{
				Projectile pl = (Projectile)varz;
				A1[i] = new Vector2(pl.position.X+(pl.width/2),pl.position.Y+(pl.height/2));
			}
			if (varz is NPC)
			{
				NPC pl = (NPC)varz;
				A1[i] = new Vector2(pl.position.X+(pl.width/2),pl.position.Y+(pl.height/2));
			}	 
			}
			
			return A1[0]-A1[1];
		#endregion
		}
		public void KillBlock(int x,int y)
		{
			GlobalPlayer.tileTime = 300;
			int type = mod.TileType("CrackedBlock");
			int type2 = mod.TileType("EmptyBlock");
			bool correctTile = (type == Main.tile[x, y].type);
			if(correctTile) Main.tile[x, y].type = (ushort)type2;
			Projectile.NewProjectile(x,y,0,0,mod.ProjectileType("Crumble"),0,0,Main.myPlayer);
		}
	}
}