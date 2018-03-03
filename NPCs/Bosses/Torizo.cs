using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using SuperMetroid;

namespace SuperMetroid.NPCs.Bosses
{
	public class Torizo : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "SuperMetroid/NPCs/Bosses/Torizo";
			}
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
			Main.npcFrameCount[npc.type] = 22;
		}
		
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 160;
			npc.damage = 10;
			npc.defense = 3;
			npc.knockBackResist = 0f;
			npc.width = 90;
			npc.height = 93;
		//	npc.value = Item.buyPrice(0, 5, 0, 0);
			npc.npcSlots = 10f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Item/Nothing");
			npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Death");
			npc.buffImmune[24] = true;
		//	music = mod.GetLegacySoundSlot(SoundType.Music, "Sounds/Music/RidleyTorizoTheme");
		}
		int shootProjectiles = 0;
		int frames = 0;
		int bombs = 0;
		int AItimer = 0;
		int AIevent = 2;
		int num = 1;
		int Alpha = 0;
		bool DeathSequence = false;
		public override void FindFrame(int frameHeight)
		{
			if(frames >= 0)
			{
				frames++;
			}
			if(!Main.dedServ)
			{
				num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
			}
			//npc frame advance
			if(frames%6 == 0)
			{	
				npc.frame.Y = (int)num*(frames/6);
			}
			//turning around
			if(npc.collideX && npc.velocity.Y <= 0 && AIevent == 2)
			{
				npc.spriteDirection = npc.spriteDirection*(-1);
			}
			//walking
			if(npc.spriteDirection == -1 && AIevent == 2)
			{
			//moving 1-4
			if(frames <= 6)	npc.velocity.X -= 0.16f;
			//stationary 5
			if(frames >= 30 && frames < 36)	npc.velocity.X = 0.0f;
			//moving 6-7
			if(frames >= 36 && frames < 54)	npc.velocity.X -= 0.25f;
			//stationary 9
			if(frames >= 54 && frames < 60) npc.velocity.X = 0.0f;
			//moving 10-11
			if(frames >= 60 && frames < 72) npc.velocity.X -= 0.185f;
			if(frames == 71) frames = 1;
			}
			if(npc.spriteDirection == 1 && AIevent == 2)
			{
			//moving 1-4
			if(frames <= 6)	npc.velocity.X += 0.16f; 
			//stationary 5
			if(frames >= 30 && frames < 36)	npc.velocity.X = 0.0f; 
			//moving 6-7
			if(frames >= 36 && frames < 54)	npc.velocity.X += 0.25f; 
			//stationary 9
			if(frames >= 54 && frames < 60) npc.velocity.X = 0.0f; 
			//moving 10-11
			if(frames >= 60 && frames < 72)	npc.velocity.X += 0.185f; 
			if(frames == 71) frames = 1;
			}

			if(frames == 6)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/TorizoStep"), npc.position);
			}
			if(frames == 36)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/TorizoStep"), npc.position);
			}
		}
		public override void AI()
		{
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest(true);
			}
			if(AItimer >= 0)
			{
				AItimer++;
			}
			Player plr = Main.player[npc.target];
			if(plr.position.X > npc.position.X && plr.position.X < npc.position.X+npc.width)
			{
				if(Main.rand.Next(60) == 0){
					AIevent = 3;
				}
			}
		#region beams
			if(AItimer == 600)
			{
				AIevent = 3;
			}
			if(AIevent == 3)
			{	
				AIevent = 0;
				frames = 72;
				if(npc.spriteDirection == -1)
				{
					npc.velocity.X += 5.0f;
					npc.velocity.Y -= 5.0f;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Torizo"), npc.position);
				}
				if(npc.spriteDirection == 1)
				{
					npc.velocity.X -= 5.0f;
					npc.velocity.Y -= 5.0f;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Torizo"), npc.position);
				}
			}
			if(frames > 80 && frames < 110 && npc.collideY)
			{
				npc.velocity.X = 0f;
			}
			if(frames >= 90 && AItimer%20 == 0 && AItimer >= 600 && AItimer < 700)
			{
				frames = 92;
				if(Main.rand.Next(3) == 0)
				{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/TorizoBeam"), npc.position);
				
				Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height));
				int damage = 12;
				int type = mod.ProjectileType("TorizoBeam");
				int num54 = Projectile.NewProjectile(vector8.X, vector8.Y-64, 6.0f*npc.spriteDirection, 0, type, damage, 0f, Main.myPlayer, -1, -1);
				Main.projectile[num54].tileCollide=true;
				Main.projectile[num54].timeLeft = 240;
				Main.projectile[num54].rotation = 90*npc.spriteDirection;
				Main.projectile[num54].aiStyle=-1;
				}
				else
				{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/TorizoBeam"), npc.position);
				
				Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height));
				int damage = 12;
				int type = mod.ProjectileType("TorizoBeam");
				int num54 = Projectile.NewProjectile(vector8.X, vector8.Y-32, 6.0f*npc.spriteDirection, 0, type, damage, 0f, Main.myPlayer, -1, -1);
				Main.projectile[num54].tileCollide=true;
				Main.projectile[num54].timeLeft = 240;
				Main.projectile[num54].rotation = 90*npc.spriteDirection;
				Main.projectile[num54].aiStyle=-1;
				}
			}
			if(AItimer == 704 && AIevent == 0)
			{
				AIevent = 2;
				frames = 1;
			}
		#endregion
		#region bombs
			if(AItimer == 900)
			{
				AItimer = 0;
				AIevent = 1;
			}
			if(AIevent == 1)
			{
				AIevent = 0;
				bombs = 0;
				frames = 72;
				if(npc.spriteDirection == -1)
				{
					npc.velocity.X += 5.0f;
					npc.velocity.Y -= 5.0f;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Torizo"), npc.position);
				}
				if(npc.spriteDirection == 1)
				{
					npc.velocity.X -= 5.0f;
					npc.velocity.Y -= 5.0f;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Torizo"), npc.position);
				}
			}
			if(frames == 126 && AIevent == 0 && AItimer < 600)
			{
				AIevent = 2;
				frames = 1;
			}
			if(frames == 90)
			{
				npc.frame.Y = num*10;
			}
			if(frames >= 90 && AIevent == 0 && AItimer < 600)
			{
				bombs++;
				if(bombs%4 == 0 && bombs <= 32)
				{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/BeamStart"), npc.position);
						
				Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height / 2));
				int damage = 8;
				int type = mod.ProjectileType("TorizoBomb");
				int num54 = Projectile.NewProjectile(vector8.X, vector8.Y-30,(Main.rand.Next(4)+1)*2*npc.spriteDirection,Main.rand.Next(3), type, damage, 0.5f, Main.myPlayer);
				Main.projectile[num54].timeLeft = 200;
				Main.projectile[num54].tileCollide=true;
				Main.projectile[num54].aiStyle=1;
				Main.projectile[num54].friendly=false;
				Main.projectile[num54].hostile=true;
				}
			}
		#endregion
			if(npc.life <= 30)
			{
				int dustID = 31;
				int dustCount = 1;
				float dustScale = 1.0f;
				for(int i = 0; i < dustCount; i++)
				{
					Dust.NewDust(npc.position, (int)(npc.width/1.4f)*(npc.spriteDirection), npc.height/4+16, dustID, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, dustScale);
				}
			}
			if(Main.rand.Next(30) == 0 && npc.life >= 20 && npc.life < 45)
			{
				npc.life -= 2;
			}
			if(npc.active)
			{
				npc.width = 60;
				npc.height = npc.height;
				npc.damage = 0;
			}
			Rectangle torizo = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height - 24);
			Rectangle player = new Rectangle((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
			if(torizo.Intersects(player))
			{
				PlayerDeathReason PDR = new PlayerDeathReason();
				PDR = PlayerDeathReason.ByCustomReason(" was stomped by Torizo");
				Main.player[npc.target].Hurt(PDR, 8+(Main.player[npc.target].statDefense/2)+(Main.rand.Next(2)-4), (int)npc.velocity.X*npc.spriteDirection, false, false, false, -1);
			}
			
			if(DeathSequence)
			{
				npc.alpha = Alpha;
				Alpha++;
				AIevent = 2;
				AItimer = 0;
				if(Alpha >= 255)
				{
				//	int PostBoss = NPC.NewNPC(0, 0, "PostBoss music");
				//	if (PostBoss < 0 || PostBoss >= 300)
				//	return;
				//	Main.npc[PostBoss].target = npc.target;
					
				//	WorldGen.TripWire((int)npc.position.X/16,(int)npc.position.Y/16);
					npc.active = false;
					DeathSequence = false;
				}
				if(Alpha%25 == 0)
				{
					Vector2 vector8 = new Vector2(npc.position.X + 42, npc.position.Y + (npc.height / 2));
					float speedX = npc.velocity.X;
					float speedY = npc.velocity.Y;
					int damage = 5;
					int type = mod.ProjectileType("Explosionsmall");
					int a = Projectile.NewProjectile(vector8.X - Main.rand.Next(-20,20), vector8.Y - Main.rand.Next(-20,20), speedX, speedY, type, damage, 4f, Main.myPlayer);
					Main.projectile[a].aiStyle = 0;
					Main.projectile[a].timeLeft = 60;
					
					int b = Projectile.NewProjectile(vector8.X - Main.rand.Next(-40,40), vector8.Y - Main.rand.Next(-40,40), speedX, speedY, type, damage, 4f, Main.myPlayer);
					Main.projectile[b].aiStyle = 0;
					Main.projectile[b].timeLeft = 60;
					
					int c = Projectile.NewProjectile(vector8.X - Main.rand.Next(-30,30), vector8.Y - Main.rand.Next(-30,30), speedX, speedY, type, damage, 4f, Main.myPlayer);
					Main.projectile[c].aiStyle = 0;
					Main.projectile[c].timeLeft = 60;
				}
			}
		}
	//	public override bool CheckDead()
	//	{
	//		return false;
	//	}
		public override void NPCLoot()
		{
			DeathSequence = true;
			npc.active = true;
			npc.life = 100;
			npc.dontTakeDamage = true;
			npc.damage = 0;
			
			float x = npc.position.X + (npc.width/2);
			float y = npc.position.Y + (npc.height/2);
			int missile1 = Main.player[Main.myPlayer].inventory[44].stack; 
			int missile2 = Main.player[Main.myPlayer].inventory[45].stack;
			int missile3 = Main.player[Main.myPlayer].inventory[46].stack;
			int missile4 = Main.player[Main.myPlayer].inventory[47].stack;
			int maxMissiles = GlobalPlayer.missileUpg;
			if(missile1+missile2+missile3+missile4 < maxMissiles*5)
			{
				Item.NewItem((int)x,(int)y,32,54,(int)mod.ItemType("Missile"),1,false);
				Item.NewItem((int)x,(int)y,32,54,(int)mod.ItemType("Missile"),1,false);
				Item.NewItem((int)x,(int)y,32,54,(int)mod.ItemType("Missile"),1,false);
				Item.NewItem((int)x,(int)y,32,54,(int)mod.ItemType("Missile"),1,false);
				Item.NewItem((int)x,(int)y,32,54,(int)mod.ItemType("Missile"),1,false);
			}
		}
	}
}