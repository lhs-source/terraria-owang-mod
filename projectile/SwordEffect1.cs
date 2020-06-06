using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class SwordEffect1 : ModProjectile
    {
        Vector2 vel;
        int dir;
        Vector2 angle;
        bool set;
        public override void SetDefaults()
        {
            //projectile.name = "SwordEffect1";
            projectile.width = 300;
            projectile.height = 300;
            projectile.timeLeft = 40;
            projectile.penetrate = -1;
            //projectile.hostile = true;
            projectile.melee = true;
            projectile.friendly = true;
            //projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 7;
            //projectile.Center = new Vector2(300 * projectile.scale, 300 * projectile.scale);
            //projectile.scale *= 1.5f;
            projectile.alpha = 32;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (set == false)
            {
                Player player = Main.player[projectile.owner];
                //  캐릭터가 보는 방향에 맞춰 이미지 뒤집기
                dir = player.direction;
                projectile.spriteDirection = dir;
                //
                vel = player.velocity;
                if(vel.Length() < 1)
                {
                    vel = new Vector2(1, 1) * dir;
                }
                //  마우스의 위치
                Vector2 pos = GetLightPosition(player);

                //  플레이어로부터의 마우스 상대 위치
                float tempx = pos.X - player.Center.X;
                float tempy = pos.Y - player.Center.Y;
                Vector2 realpos = new Vector2(tempx, tempy);

                //  벡터의 크기를 1로 만들기
                //realpos = realpos / Vector2.Distance(new Vector2(0, 0), realpos);
                realpos = realpos / realpos.Length() * 2;

                //  벡터로부터 float 앵글값 뽑기
                float angleFromVector =
                  (float)Math.Atan2(realpos.X, -realpos.Y);

                //  float 앵글로부터 벡터 구하기
                Vector2 angleVector = new Vector2(
                  (float)Math.Cos(angleFromVector),
                    -(float)Math.Sin(angleFromVector));

                //  검기 방향
                if(realpos.X < 0)
                {
                    projectile.rotation = angleFromVector + 90;
                }
                else
                {
                    projectile.rotation = angleFromVector - 90;
                }
                //  시작 지점을 좀 나아가서
                projectile.position += realpos * 35;

                //  플레이어의 속도에 따라 검기 속도도 좀 빠르게
                projectile.velocity = realpos * 3 + player.velocity * 2;
				
				//	빛나라!
				projectile.light = 0.9f;

                set = true;
            }
            return base.PreDraw(spriteBatch, lightColor);
        }
        
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.velocity *= 0.90f;

            if(25 < projectile.timeLeft)
            {
                projectile.alpha += 6;
            }
            if (1 <= projectile.frame || projectile.frame <= 4)
            {
                if (projectile.frameCounter < 7)
                {
                    projectile.frameCounter++;
                }
                else
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
            }
            else if(projectile.frame < 7)
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            damage = damage * 2;
            Projectile.NewProjectile(new Vector2(
                (projectile.position.X + (float)(projectile.width / 2) + target.position.X) / 2 + new Random().Next(-20, 20), (projectile.position.Y + (float)(projectile.height / 2) + target.position.Y) / 2 + new Random().Next(-20, 20)), 
                new Vector2(0, 0),
                mod.ProjectileType("HitEffect1"), 
                0,
                1f, 
                projectile.owner);
            projectile.vampireHeal(100, target.Center);
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                //Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("Sparkle"), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
                
            }
            //Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 67);
        }
        private Vector2 GetLightPosition(Player player)
        {
            Vector2 position = Main.screenPosition;
            position.X += Main.mouseX;
            position.Y += player.gravDir == 1 ? Main.mouseY : Main.screenHeight - Main.mouseY;
            return position;
        }
    }
}
