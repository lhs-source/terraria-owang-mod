using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class HitEffect1 : ModProjectile
    {
        Vector2 vel;
        int dir;
        Vector2 angle;
        bool set;
        public override void SetDefaults()
        {
            projectile.name = "HitEffect1";
            projectile.width = 220;
            projectile.height = 180;
            projectile.timeLeft = 20;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.light = 0.9f;
            Main.projFrames[projectile.type] = 4;
        }
        public override void AI()
        {
            if (projectile.frameCounter < 5)
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
