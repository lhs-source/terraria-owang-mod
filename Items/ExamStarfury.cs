using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.Items
{
    class ExamStarfury : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.SkyFracture);
            //item.name = "ExamStarfury";
            item.shoot = 660;
            item.shootSpeed = 10f;
            /*
            item.name = "ExamStarfury";
            item.damage = 5;
            item.melee = true;
            item.width = 14;
            item.height = 28;
            //item.Bottom = new Vector2(5, 5);
            item.Center = new Vector2(9, 82);
            item.toolTip = "imitate starfury";
            item.useTime = 40;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = 50000;
            item.rare = 2;
            item.useSound = 1;
            //item.autoReuse = true;
            item.shoot = mod.ProjectileType("ExamStarfury");
            //item.shoot = 9;
            item.shootSpeed = 20f;

            item.color = new Color(150, 150, 150, 0);
            item.scale = 1.25f;
            item.alpha = 100;
            */
        }
        /*  발사 함수 */
        public override bool Shoot(
            Player player, /*쏜 주체 플레이어*/
            ref Vector2 position, /*발사한 projectile의 처음 위치*/
            ref float speedX, /*projectile의 속도*/
            ref float speedY, /*projectile의 속도*/
            ref int type, /*projectile의 ai type인듯*/
            ref int damage, /*projectile의 데미지*/
            ref float knockBack /*projectile의 넉백*/
            )
        {
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
				vector13 = vector2 + f.ToRotationVector2() * MathHelper.Lerp(value12, value13, Main.rand.NextFloat());
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
            
            Projectile.NewProjectile(vector13, vector14, num71, num73, num74);

            return base.Shoot(player, ref vector13, ref vector14.X, ref vector14.Y, 
                ref type, ref damage, ref knockBack);
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
