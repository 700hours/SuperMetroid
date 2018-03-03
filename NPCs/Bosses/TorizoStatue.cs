using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SuperMetroid.NPCs.Bosses
{
	public class TorizoStatue : ModNPC
	{
		public override string Texture
		{
			get
			{
				return "SuperMetroid/NPCs/Bosses/TorizoStatue";
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
			Main.npcFrameCount[npc.type] = 17;
		}
		
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 600;
			npc.damage = 0;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.width = 100;
			npc.height = 93;
			npc.value = Item.buyPrice(0, 20, 0, 0);
			npc.npcSlots = 10f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.dontTakeDamage = true;
			npc.HitSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Nothing");
			npc.DeathSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Nothing");
		//	music = mod.GetLegacySoundSlot(SoundType.Music, "Sounds/Music/ChozoStatueAwakens");
		}
		public void CustomInit()
		{
			npc.position.X = Main.player[Main.myPlayer].position.X-8 + (Main.player[Main.myPlayer].width - npc.width) * 0.5f;
			npc.position.Y = Main.player[Main.myPlayer].position.Y - npc.height+40;
		}
		int frames = 0;
		int num = 1;
		bool statueCrumble = false;
		bool musicStart = false;
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
			if(frames >= 0 && frames < 120)
			{	npc.frame.Y = num*1;		}
			if(frames == 120)
			{	npc.frame.Y = num*2;			
				statueCrumble = true;		} //spawn bits
			if(frames == 121)
			{	npc.frame.Y = num*3;		}
			if(frames == 135)
			{	npc.frame.Y = num*4;		}
		#region crumble frame counts
		/*bits falling to the ground
		frame 21, bit 3,2 falls
		frame 53, bit 3,2 hits
		frame 61, bit 3,3
		frame 69, bit 2,1
		frame 75, bit 2,2
		frame 77, 3 bits hit ground
		frame 81, bit 2,3
		bits death 3 @ 80 frames
		bits death 2 @ 85 frames
		bits death 1 @ 90 frames*/
		#endregion
			if(frames == 223)
			{	npc.frame.Y = num*5;		}
			if(frames == 260)
			{	npc.frame.Y = num*5;		}
			if(frames == 262)
			{	npc.frame.Y = num*5;		}
			if(frames == 264)
			{	npc.frame.Y = num*4;		}
			if(frames == 266)
			{	npc.frame.Y = num*3;		}
			if(frames == 272)
			{	npc.frame.Y = num*4;		}
			if(frames == 274)
			{	npc.frame.Y = num*5;		}
			if(frames == 276)
			{	npc.frame.Y = num*5;		}
			if(frames == 282)
			{	npc.frame.Y = num*5;		}
			if(frames == 284)
			{	npc.frame.Y = num*4;		}
			if(frames == 286)
			{	npc.frame.Y = num*3;		}
			if(frames == 292)
			{	npc.frame.Y = num*4;		}
			if(frames == 296)
			{	npc.frame.Y = num*5;		}
			if(frames == 300)
			{	npc.frame.Y = num*5;		}
			if(frames == 304)
			{	npc.frame.Y = num*5;		}
			if(frames == 310)
			{	npc.frame.Y = num*4;		}
			if(frames == 315)
			{	npc.frame.Y = num*5;		}
			if(frames == 320)
			{	npc.frame.Y = num*6;		}
			if(frames == 350)
			{	npc.frame.Y = num*7;		}
			if(frames == 364)
			{	npc.frame.Y = num*8;		}
			if(frames == 374)
			{	npc.frame.Y = num*9;		}
			if(frames == 382)
			{	npc.frame.Y = num*10;		}
			if(frames == 390)				
			{	npc.frame.Y = num*11;		}
			if(frames == 398)				//a step
			{	npc.frame.Y = num*12;		
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Custom/TorizoStep"), npc.position);
			}
			if(frames == 406)				//color changing begins
			{	npc.frame.Y = num*13;		}
			if(frames == 416)
			{	npc.frame.Y = num*14;		}
			if(frames == 414)
			{	npc.frame.Y = num*15;		}
			if(frames == 424)
			{	npc.frame.Y = num*16;		}
			if(frames == 430)
			{	npc.frame.Y = num*16;		}
			if(frames == 460)				//despawn
			{	
				npc.active = false;
				NPC.NewNPC((int) (npc.position.X+npc.width/2) + 16, (int) (npc.position.Y+npc.height/1.3f), mod.NPCType("Torizo"), npc.whoAmI);
			}
		}
		public override void AI()
		{
			if(statueCrumble)
			{
			/*	statueCrumble = false;
				NPC.NewNPC((int) npc.position.X+68, (int) npc.position.Y+92, "Torizo_1 2", npc.whoAmI);
				NPC.NewNPC((int) npc.position.X+84, (int) npc.position.Y+92, "Torizo_1 3", npc.whoAmI);
				NPC.NewNPC((int) npc.position.X+52, (int) npc.position.Y+76, "Torizo_2 1", npc.whoAmI);
				NPC.NewNPC((int) npc.position.X+68, (int) npc.position.Y+76, "Torizo_2 2", npc.whoAmI);
				NPC.NewNPC((int) npc.position.X+84, (int) npc.position.Y+76, "Torizo_2 3", npc.whoAmI);
				NPC.NewNPC((int) npc.position.X+68, (int) npc.position.Y+60, "Torizo_3 2", npc.whoAmI);
				NPC.NewNPC((int) npc.position.X+84, (int) npc.position.Y+60, "Torizo_3 3", npc.whoAmI);
			*/
			}	
		}
	}
}