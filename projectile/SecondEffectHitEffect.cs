using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class SecondEffectHitEffect : ModProjectile
    {
        Vector2 vel;
        int dir;
        Vector2 angle;
        bool set;
        public override void SetDefaults()
        {
            projectile.name = "SecondEffectHitEffect";
            projectile.width = 156;
            projectile.height = 157;
            projectile.timeLeft = 28;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            Main.projFrames[projectile.type] = 7;
        }
        public override void AI()
        {
            if (projectile.frameCounter < 4)
            {
                projectile.frameCounter++;
            }
            else
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
        }
    }
}
