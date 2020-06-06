using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;
using Terraria.UI;
using System.Text;
using ReLogic.Graphics;

namespace Hsmod.NPCs
{
    class Soles : ModNPC
    {
        bool flag;
        int oldCnt;
        public override void SetDefaults()
        {
            //npc.name = "Soles";
            npc.width = 67;
            npc.height = 125;
            //npc.aiStyle = 84;
            npc.damage = 10;
            npc.defense = 42;
            npc.lifeMax = 8000;
            npc.knockBackResist = 0f;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.npcSlots = 10f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.value = 10000f;
            npc.boss = true;
            npc.netAlways = true;
            npc.timeLeft = NPC.activeTime * 30;
            Main.npcFrameCount[mod.NPCType("Soles")] = 2;
            flag = true;
            oldCnt = 0;

            base.SetDefaults();
        }
        public override void FindFrame(int frameHeight)
        {
            if(npc.ai[1] >= 100)
            {
                npc.frame.Y = 0;
            }
            else
            {
                npc.frame.Y = frameHeight;
            }
            //base.FindFrame(frameHeight);
        }
        public override void AI()
        {
            
            if (npc.ai[0] == 0)
            {
                ++npc.ai[1];
                if (npc.ai[1] >= 360)
                {
                    if (npc.ai[3] != -1)
                    {
                        Vector2 center1 = npc.Center;
                        Player player = Main.player[npc.target];
                        float num12 = (float)Math.Ceiling((player.Center + new Vector2(0, -100) - center1).Length() / 50);
                        if (num12 == 0)
                        {
                            num12 = 1;
                        }
                        int num13 = 0;

                        Vector2 center2 = npc.Center;
                        float num14 = (float)(((num13 + 2) / 2) * 6.28318548202515 * 0.400000005960464);
                        if (num13 % 2 == 1)
                        {
                            num14 *= -1;
                        }
                        num14 = 0;
                        Vector2 vector2_1 = new Vector2(0, -1).RotatedBy(num14, new Vector2()) * new Vector2(300, 200);
                        Vector2 vector2_2 = player.Center + vector2_1 - center2;
                        npc.ai[0] = 1;
                        npc.ai[1] = num12 * 2;
                        npc.velocity = vector2_2 / num12;
                        for(int i = 0; i < oldCnt; i++)
                        {
                            npc.oldPos[i] = Vector2.Zero;
                        }
                        oldCnt = 0;
                        //npc.velocity = new Vector2(1, 1);
                        npc.netUpdate = true;
                        ++num13;
                    }
                }
                else
                {
                    if(npc.ai[1] <= 150)
                    {
                        if (npc.ai[1] % 30 == 0)
                        {
                            Player player = Main.player[npc.target];

                            Vector2 vector2 = npc.Center;
                            Vector2 value7 = new Vector2(player.position.X, player.position.Y);
                            float num115 = value7.Y;
                            if (num115 > npc.Center.Y - 200)
                            {
                                num115 = npc.Center.Y - 200;
                            }

                            //float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                            //float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                            float num78 = player.Center.X - vector2.X;
                            float num79 = player.Center.Y - vector2.Y;

                            float num72 = 10;
                            for (int num116 = 0; num116 < 2; num116++)
                            {
                                vector2 = npc.Center + new Vector2(Main.rand.Next(0, 401) * npc.direction, -600);
                                vector2.Y -= 100 * num116;

                                Vector2 value8 = value7 - vector2;
                                if (value8.Y < 0)
                                {
                                    value8.Y *= -1;
                                }
                                if (value8.Y < 20)
                                {
                                    value8.Y = 20;
                                }
                                value8.Normalize();
                                value8 *= num72;
                                num78 = value8.X;
                                num79 = value8.Y;
                                float speedX5 = num78 + player.velocity.X / 2.5f;
                                float speedY6 = num79 + Main.rand.Next(-40, 41) * 0.02f;


                                Projectile.NewProjectile(
                                    vector2.X, vector2.Y,
                                    speedX5,
                                    speedY6,
                                    mod.ProjectileType("SolesSword"),
                                    10,
                                    0,
                                    byte.MaxValue,
                                    0,
                                    num115);
                            }
                        }
                    }
                }
            }
            else if(npc.ai[0] == 1)
            {
                npc.localAI[2] = 10;
                if(npc.ai[1] % 2 != 0 &&
                    npc.ai[1] != 1)
                {
                    Vector2 vector2 = npc.position - npc.velocity;
                    npc.position = vector2;
                    saveOldPos(npc.position);
                }
                --npc.ai[1];
                if(npc.ai[1] <= 0)
                {
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                    ++npc.ai[3];
                    npc.velocity = Vector2.Zero;
                    npc.netUpdate = true;
                }
            }
            base.AI();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 position = new Vector2(10f, (float)120);
            StringBuilder sb = new StringBuilder();
            sb.Append("pos(" + npc.position.X + ", " + npc.position.Y + ")");
            sb.Append("\noldposlen = " + npc.oldPos.Length + "\n");
            for(int i = 0; i < npc.oldPos.Length; i++)
            {
                sb.Append("oldpos" + i + "(");
                sb.Append(npc.oldPos[i].X + ", " + npc.oldPos[i].Y + ")\n");
            }
            Main.spriteBatch.DrawString(
                Main.fontMouseText,
                sb.ToString(),
                position,
                new Microsoft.Xna.Framework.Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor),
                0f,
                new Vector2(0, 0),
                1f,
                SpriteEffects.None,
                0f);
            if (npc.ai[0] == 1)
            {                
                Texture2D texture = Main.npcTexture[npc.type];
                Vector2 vector10 = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[npc.type] / 2));

                Microsoft.Xna.Framework.Color color9 = Lighting.GetColor(
                    (int)((npc.position.X + npc.width / 2) / 16),
                    (int)((npc.position.Y + npc.height / 2) / 16));

                int num116 = npc.frame.Y / (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]);

                Texture2D texture2D9 = Main.npcTexture[npc.type];
                Texture2D texture2D10 = Main.extraTexture[30];

                Microsoft.Xna.Framework.Rectangle rectangle = texture2D9.Frame(1, 1, 0, 0);
                rectangle.Height /= 2;
                if (num116 >= 4)
                {
                    rectangle.Y += rectangle.Height;
                }

                Microsoft.Xna.Framework.Color white = Microsoft.Xna.Framework.Color.White;
                float amount4 = 0f;
                Microsoft.Xna.Framework.Color color24 = color9;
                int num117 = 0;
                int num118 = 0;
                int num119 = 0;
                if (npc.ai[0] == -1f)
                {
                    if (npc.ai[1] >= 320f && npc.ai[1] < 960f)
                    {
                        white = Microsoft.Xna.Framework.Color.White;
                        amount4 = 0.5f;
                        num117 = 6;
                        num118 = 2;
                        num119 = 1;
                    }
                }
                else if (npc.ai[0] == 1f)
                {
                    white = Microsoft.Xna.Framework.Color.White;
                    amount4 = 0.7f;
                    num117 = 6;
                    num118 = 2;
                    num119 = 1;
                }
                else
                {
                    color24 = color9;
                }
                for (int num120 = num119; num120 < num117; num120 += num118)
                {
                    Vector2 arg_626F_0 = npc.oldPos[num120];
                    Microsoft.Xna.Framework.Color color25 = color24;
                    color25 = Microsoft.Xna.Framework.Color.Lerp(color25, white, amount4);
                    color25 = npc.GetAlpha(color25);
                    color25 *= (float)(num117 - num120) / (float)num117;
                    color25.A = 100;
                    Vector2 vector26 = npc.oldPos[num120] + new Vector2((float)npc.width, (float)npc.height) / 2f - Main.screenPosition;
                    vector26 -= rectangle.Size() * npc.scale / 2f;
                    vector26 += vector10 * npc.scale + new Vector2(0f, npc.gfxOffY);
                    Main.spriteBatch.Draw(texture, vector26, new Microsoft.Xna.Framework.Rectangle?(rectangle), color25, npc.rotation, vector10, npc.scale, SpriteEffects.None, 0f);


                    int index2 = Dust.NewDust(npc.Center, 0, 0, 231, 0.0f, 0.0f, 0, default(Color), 1f);
                    Main.dust[index2].scale = 1.5f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = npc.oldPos[num120];
                    Main.dust[index2].velocity = npc.velocity * 0.0f;
                    Main.dust[index2].color = Color.DarkRed;
                }
                Vector2 vector28 = npc.Center - Main.screenPosition;
                vector28 -= new Vector2((float)texture2D9.Width, (float)(texture2D9.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f;
                vector28 += vector10 * npc.scale + new Vector2(0f, npc.gfxOffY);
                Main.spriteBatch.Draw(texture2D9, vector28, new Microsoft.Xna.Framework.Rectangle?(npc.frame), npc.GetAlpha(color9), npc.rotation, vector10, npc.scale, SpriteEffects.None, 0f);
                
            }
            return base.PreDraw(spriteBatch, drawColor);
        }
        public void saveOldPos(Vector2 newPos)
        {
            if (oldCnt < 10)
            {
                oldCnt++;
            }
            for (int i = oldCnt - 1; i >= 0; i--)
            {
                if(i == 0)
                {
                    npc.oldPos[i] = newPos;
                    break;
                }
                npc.oldPos[i] = npc.oldPos[i - 1];
            }
        }
        public override Color? GetAlpha(Color drawColor)
        {
            float num1 = (float)((int)byte.MaxValue - npc.alpha) / (float)byte.MaxValue;
            int r1 = (int)((double)drawColor.R * (double)num1);
            int g1 = (int)((double)drawColor.G * (double)num1);
            int b1 = (int)((double)drawColor.B * (double)num1);
            int a = (int)drawColor.A - npc.alpha;

            if (a < 0)
                a = 0;
            if (a > (int)byte.MaxValue)
                a = (int)byte.MaxValue;
            return new Color(r1, g1, b1, a);

            return base.GetAlpha(drawColor);
        }
    }
}
