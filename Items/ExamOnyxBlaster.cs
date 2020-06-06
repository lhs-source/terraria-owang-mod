using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.Items
{
    class ExamOnyxBlaster : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.OnyxBlaster);
            //item.name = "ExamOnyxBlaster";
            item.shoot = mod.ProjectileType("ExamOnyxBlaster");

            item.width = 33;
            item.height = 78;

            base.SetDefaults();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("ExamOnyxBlaster");
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}
