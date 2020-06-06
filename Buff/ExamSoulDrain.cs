using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.Buff
{
    class ExamSoulDrain : ModBuff
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.soulDrain > 0 && player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(151, 2, true);
                

            }

            if (player.soulDrain > 0)
            {
                player.lifeRegenTime += 2;
                int num = (5 + player.soulDrain) / 2;
                player.lifeRegenTime += num;
                player.lifeRegen += num;
            }
            base.Update(player, ref buffIndex);
        }
    }
}
