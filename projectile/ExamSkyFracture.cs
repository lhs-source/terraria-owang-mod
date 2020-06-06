using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile 
{
    class ExamSkyFracture : ModProjectile
    {
        bool set = false;
        public override void SetDefaults()
        {
            projectile.name = "ExamSkyFracture";
            projectile.width = 64;
            projectile.height = 64;
            projectile.aiStyle = 0;
        

            projectile.friendly = true;
            projectile.alpha = (int)byte.MaxValue;

            projectile.timeLeft = 90;
            //projectile.melee = true;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;

            projectile.light = 1f;

            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 180;

            Main.projFrames[projectile.type] = 14;
        }
        public override bool PreAI()
        {
            
            if (set == false)
            {
                //
                

                projectile.frame = Main.rand.Next(14);

                
                //
                
                set = true;
            }
            return base.PreAI();
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            /*
            //
            //  ver1 몰라 이거 안먹혀
            //
            
            Texture2D texture2D3 = Main.projectileTexture[projectile.type];
            int num155 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int y3 = num155 * projectile.frame;
            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(0, y3, texture2D3.Width, num155);
            Vector2 origin2 = rectangle.Size() / 2f;

            Microsoft.Xna.Framework.Color color25 = Lighting.GetColor(
                (int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16, 
                (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
            SpriteEffects spriteEffects = SpriteEffects.None;

            int num156 = 8;
            int num157 = 2;
            float value4 = 1f;
            float num158 = 0f;

            for (int num159 = 1; num159 < num156; num159 += num157)
            {
                Microsoft.Xna.Framework.Color color26 = color25;
                color26 = Microsoft.Xna.Framework.Color.Lerp(color26, Microsoft.Xna.Framework.Color.Red, 0.5f);
                color26 = projectile.GetAlpha(color26);
                color26 *= (float)(num156 - num159) / ((float)ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f);

                Vector2 value5 = projectile.oldPos[num159];
                float num160 = projectile.rotation;
                SpriteEffects effects = spriteEffects;
                if (ProjectileID.Sets.TrailingMode[projectile.type] == 2)
                {
                    num160 = projectile.oldRot[num159];
                    effects = ((projectile.oldSpriteDirection[num159] == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
                }
                Main.spriteBatch.Draw(
                    texture2D3,
                    value5 + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), 
                    new Microsoft.Xna.Framework.Rectangle?(rectangle), 
                    color26, 
                    num160 + projectile.rotation * num158 * (float)(num159 - 1) * (float)(-(float)spriteEffects.HasFlag(SpriteEffects.FlipHorizontally).ToDirectionInt()), 
                    origin2, 
                    MathHelper.Lerp(projectile.scale, value4, (float)num159 / 15f), 
                    effects, 
                    0f);
            }
            Microsoft.Xna.Framework.Color color28 = projectile.GetAlpha(color25);
            Main.spriteBatch.Draw(
                texture2D3, 
                projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), 
                new Microsoft.Xna.Framework.Rectangle?(rectangle), 
                color28, 
                projectile.rotation, 
                origin2, 
                projectile.scale, 
                spriteEffects, 
                0f);
            base.PostDraw(spriteBatch, lightColor);
            */

            //
            //  ver 2
            //
            
            int num147 = 0;
            int num148 = 0;
            float num149 = (float)(Main.projectileTexture[projectile.type].Width - projectile.width) * 0.5f + (float)projectile.width * 0.5f;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Microsoft.Xna.Framework.Color color25 = Lighting.GetColor(
                (int)((double)projectile.position.X + (double)projectile.width * 0.5) / 16,
                (int)(((double)projectile.position.Y + (double)projectile.height * 0.5) / 16.0));
            Texture2D texture2D6 = Main.projectileTexture[projectile.type];
            int num185 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
            int y4 = num185 * projectile.frame;
            int num186 = 14;
            int num187 = 2;
            float value13 = 0.5f;
            for (int num188 = 1; num188 < num186; num188 += num187)
            {
                //Vector2 arg_7BB5_0 = Main.npc[i].oldPos[num188];
                Microsoft.Xna.Framework.Color color32 = color25;
                color32 = projectile.GetAlpha(color32);
                color32 *= (float)(num186 - num188) / 15f;
                Vector2 _ = projectile.oldPos[num188] - Main.screenPosition + new Vector2(num149 + (float)num148, (float)(projectile.height / 2) + projectile.gfxOffY);
                Main.spriteBatch.Draw(texture2D6, projectile.oldPos[num188] + new Vector2((float)projectile.width, (float)projectile.height) / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y4, texture2D6.Width, num185)), color32, projectile.rotation, new Vector2((float)texture2D6.Width / 2f, (float)num185 / 2f), MathHelper.Lerp(projectile.scale, value13, (float)num188 / 15f), spriteEffects, 0f);
            }
            Main.spriteBatch.Draw(texture2D6, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y4, texture2D6.Width, num185)), projectile.GetAlpha(color25), projectile.rotation, new Vector2((float)texture2D6.Width / 2f, (float)num185 / 2f), projectile.scale, spriteEffects, 0f);
            base.PostDraw(spriteBatch, lightColor);
            
        }
        public override void AI()
        {
            if (projectile.velocity.Length() < 20f)
            {
                projectile.velocity = projectile.velocity * 1.06f;
            }
           
            DelegateMethods.v3_1 = new Vector3(0.6f, 1f, 1f) * 0.2f;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 10f, 8f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
            if (projectile.alpha > 0)
            {
                //Main.PlaySound(2, projectile.Center, 79);
                projectile.alpha = 0;
                projectile.scale = 1.1f;
                projectile.frame = Main.rand.Next(14);
                float num = 16f * 2;
                for (int index1 = 0; (double)index1 < (double)num; ++index1)
                {
                    if (index1 == num / 4)
                    {
                        continue;
                    }
                    Vector2 v = (Vector2.UnitX * 0.0f + -Vector2.UnitY.RotatedBy((double)index1 * (6.28318548202515 / (double)num), new Vector2()) * new Vector2(10f, 40f)).RotatedBy((double)projectile.velocity.ToRotation(), new Vector2());
                    int index2 = Dust.NewDust(projectile.Center, 0, 0, 231, 0.0f, 0.0f, 0, default(Color), 1f);
                    Main.dust[index2].scale = 1.5f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].position = projectile.Center + v;
                    Main.dust[index2].velocity = projectile.velocity * 0.0f + v.SafeNormalize(Vector2.UnitY) * 0.2f;
                    Main.dust[index2].color = Color.DarkRed;
                    //Main.dust[index2].GetAlpha(new Color(200, 0, 0, 0));
                }
            }
            float num103 = 1f;
            int num104 = 0;
            while ((float)num104 < num103)
            {
                Vector2 vector12 = Vector2.UnitX * 0f;
                vector12 += -Vector2.UnitY.RotatedBy((double)((float)num104 * (6.28318548f / num103)), default(Vector2)) * new Vector2(1f, 4f);
                vector12 = vector12.RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2));
                int num105 = Dust.NewDust(projectile.Center, 0, 0, 219, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num105].scale = 1.1f;
                Main.dust[num105].noLight = true;
                Main.dust[num105].noGravity = true;
                Main.dust[num105].position = projectile.Center + vector12;
                Main.dust[num105].velocity = Main.dust[num105].velocity * 0.1f + projectile.velocity * 0.2f;
                num104++;
            }
            projectile.ai[0] += 1f;
            projectile.rotation = projectile.velocity.ToRotation() + 0.7853982f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //projectile.ghostHeal(damage / 5, target.position);
            //Player player = Main.player[projectile.owner];
            //player.AddBuff(151, 30, false);
            float num1 = 0.075f - (float)projectile.numHits * 0.05f;
            if ((double)num1 <= 0.0)
                return;
            float ai1 = (float)damage * num1;
            if ((int)ai1 == 0 || (double)Main.player[Main.myPlayer].lifeSteal <= 0.0)
                return;
            Main.player[Main.myPlayer].lifeSteal -= ai1;
            
            Projectile.NewProjectile(target.Center.X, target.Center.Y, 0.0f, 0.0f, 305, 0, 0.0f, projectile.owner, projectile.owner, ai1 / 3);
            Projectile.NewProjectile(target.Center.X, target.Center.Y, 0.0f, 0.0f, 305, 0, 0.0f, projectile.owner, projectile.owner, ai1 / 3);
            Projectile.NewProjectile(target.Center.X, target.Center.Y, 0.0f, 0.0f, 305, 0, 0.0f, projectile.owner, projectile.owner, ai1 / 3);

            

            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void Kill(int timeLeft)
        {
            projectile.position = projectile.Center;
            projectile.width = (projectile.height = 100);
            projectile.Center = projectile.position;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.Damage();
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 71);
            int num2 = Main.rand.Next(4, 10);
            for (int index1 = 0; index1 < num2; ++index1)
            {
                int index2 = Dust.NewDust(projectile.Center, 0, 0, 183, 0.0f, 0.0f, 100, new Color(), 1f);
                Main.dust[index2].velocity *= 1.6f;
                --Main.dust[index2].velocity.Y;
                Main.dust[index2].velocity += -projectile.velocity * (float)((double)Main.rand.NextFloat() * 2.0 - 1.0) * 0.5f;
                Main.dust[index2].scale = 2f;
                Main.dust[index2].fadeIn = 0.5f;
                Main.dust[index2].color = Color.IndianRed;
                Main.dust[index2].noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            //return new Color(150, (int)byte.MaxValue, (int)byte.MaxValue, 0) * projectile.Opacity;
            //return new Color(255, 50, 50, 0);
            /*
            Color color = Color.OrangeRed;
            color.A = 180;
            return color;
            */
            return new Color(255, 150, 150, 0) * projectile.Opacity;
            //return base.GetAlpha(lightColor);
        }
    }
}
