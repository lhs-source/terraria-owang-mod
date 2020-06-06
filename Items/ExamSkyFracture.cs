using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.Items
{
    class ExamSkyFracture : ModItem
    {
        public override void SetDefaults()
        {
            //item.name = "ExamStarfury";

            item.noMelee = true;
            item.autoReuse = true;

            item.useStyle = 5;
            item.useAnimation = 20;
            item.useTime = 4;
            item.reuseDelay = 8;

            item.knockBack = 6f;
            item.damage = 380;
            item.crit = 20;
            item.rare = 4;
            item.value = 300000;

            item.width = 93;
            item.height = 93;
            item.UseSound = SoundID.Item1;

            item.shoot = mod.ProjectileType("ExamSkyFracture");
            item.shootSpeed = 3f;

        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 oldposition = position;
            float oldSpeedX = speedX;
            float oldSpeedY = speedY;

            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num72 = item.shootSpeed;
            int num71 = type;
            int num73 = damage;
            float num74 = knockBack;

            float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
            float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;

            float f = Main.rand.NextFloat() * 6.28318548f;
            float value12 = 20f;
            float value13 = 60f;
            Vector2 vector13 = vector2 + f.ToRotationVector2() * MathHelper.Lerp(value12, value13, Main.rand.NextFloat());
            for (int num202 = 0; num202 < 50; num202++)
            {
                vector13 = vector2 + f.ToRotationVector2() * MathHelper.Lerp(value12, value13, Main.rand.NextFloat() * 2);
                if (Collision.CanHit(vector2, 0, 0, vector13 + (vector13 - vector2).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                {
                    break;
                }
                f = Main.rand.NextFloat() * 6.28318548f;
            }
            Vector2 mouseWorld = Main.MouseWorld;
            Vector2 vector14 = mouseWorld - vector13;
            Vector2 vector15 = new Vector2(num78, num79).SafeNormalize(Vector2.UnitY) * num72;
            vector14 = vector14.SafeNormalize(vector15) * num72;
            vector14 = Vector2.Lerp(vector14, vector15, 0.25f);

            position = vector13;
            speedX = vector14.X;
            speedY = vector14.Y;

            //Projectile.NewProjectile(vector13, vector14, num71, num73, num74);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
                

            /*
            type = mod.ProjectileType("ExamSkyFracture");
            //type = 660;
            Vector2 oldPosition = new Vector2(position.X, position.Y);
            position = new Vector2(player.position.X + Main.rand.Next(-player.width * 3 / 2, player.width * 3 / 2),
                player.position.Y + Main.rand.Next(-player.height * 3 / 2, player.height * 3 / 2));
            Main.dust[Dust.NewDust(position, 0, 0, 268, 0.5f, 0.5f)].noGravity = true;
            //speedX = speedX + (oldPosition.X - position.X);
            //speedY = speedY + (oldPosition.Y - position.Y);
        
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            */
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
