using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace owang.projectile
{
    class ExamOnyxBlaster : ModProjectile
    {
        bool set = false;
        public override void SetDefaults()
        {
            projectile.name = "ExamOnyxBlaster";
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 255;

            projectile.aiStyle = 1;
            projectile.timeLeft = 40;
            projectile.extraUpdates = 1;

            projectile.friendly = true;
            projectile.ranged = true;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;

            base.SetDefaults();
        }
        public override bool PreAI()
        {
            if(set == false)
            {
                
                set = true;
            }
            return base.PreAI();
        }
        public override void AI()
        {
            if (projectile.alpha <= 0)
            {
                for (int num101 = 0; num101 < 3; num101++)
                {
                    int num102 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 240, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num102].noGravity = true;
                    Main.dust[num102].velocity *= 0.3f;
                    Main.dust[num102].noLight = true;
                }
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 55;
                projectile.scale = 1.3f;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                    float num103 = 16f;
                    int num104 = 0;
                    while ((float)num104 < num103)
                    {
                        Vector2 vector12 = Vector2.UnitX * 0f;
                        vector12 += -Vector2.UnitY.RotatedBy((double)((float)num104 * (6.28318548f / num103)), default(Vector2)) * new Vector2(1f, 4f);
                        vector12 = vector12.RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2));
                        int num105 = Dust.NewDust(projectile.Center, 0, 0, 62, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[num105].scale = 1.5f;
                        Main.dust[num105].noLight = true;
                        Main.dust[num105].noGravity = true;
                        Main.dust[num105].position = projectile.Center + vector12;
                        Main.dust[num105].velocity = Main.dust[num105].velocity * 4f + projectile.velocity * 0.3f;
                        num104++;
                    }
                }
            }
            base.AI();
        }
        public override void Kill(int timeLeft)
        {
            projectile.position = projectile.Center;
            projectile.width = (projectile.height = 160);
            projectile.Center = projectile.position;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.Damage();
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
            Vector2 position = projectile.Center + Vector2.One * -20f;
            int num34 = 40;
            int height3 = num34;
            for (int num35 = 0; num35 < 4; num35++)
            {
                int num36 = Dust.NewDust(position, num34, height3, 240, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num36].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num34 / 2f;
            }
            for (int num37 = 0; num37 < 20; num37++)
            {
                int num38 = Dust.NewDust(position, num34, height3, 62, 0f, 0f, 200, default(Color), 3.7f);
                Main.dust[num38].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num34 / 2f;
                Main.dust[num38].noGravity = true;
                Main.dust[num38].noLight = true;
                Main.dust[num38].velocity *= 3f;
                Main.dust[num38].velocity += projectile.DirectionTo(Main.dust[num38].position) * (2f + Main.rand.NextFloat() * 4f);
                num38 = Dust.NewDust(position, num34, height3, 62, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[num38].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * (float)num34 / 2f;
                Main.dust[num38].velocity *= 2f;
                Main.dust[num38].noGravity = true;
                Main.dust[num38].fadeIn = 1f;
                Main.dust[num38].color = Color.Crimson * 0.5f;
                Main.dust[num38].noLight = true;
                Main.dust[num38].velocity += projectile.DirectionTo(Main.dust[num38].position) * 8f;
            }
            for (int num39 = 0; num39 < 20; num39++)
            {
                int num40 = Dust.NewDust(position, num34, height3, 62, 0f, 0f, 0, default(Color), 2.7f);
                Main.dust[num40].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2)) * (float)num34 / 2f;
                Main.dust[num40].noGravity = true;
                Main.dust[num40].noLight = true;
                Main.dust[num40].velocity *= 3f;
                Main.dust[num40].velocity += projectile.DirectionTo(Main.dust[num40].position) * 2f;
            }
            for (int num41 = 0; num41 < 70; num41++)
            {
                int num42 = Dust.NewDust(position, num34, height3, 240, 0f, 0f, 0, default(Color), 1.5f);
                Main.dust[num42].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)projectile.velocity.ToRotation(), default(Vector2)) * (float)num34 / 2f;
                Main.dust[num42].noGravity = true;
                Main.dust[num42].velocity *= 3f;
                Main.dust[num42].velocity += projectile.DirectionTo(Main.dust[num42].position) * 3f;
            }
            base.Kill(timeLeft);
        }
    }
}
