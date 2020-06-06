using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Hsmod.UI;

namespace Hsmod.Items
{
    class HsmodGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            var info = item.GetModInfo<OwangItemInfo>(Hsmod.instance);
            if (info.addValueTooltip)
            {
                ItemValue value = new ItemValue().SetFromCopperValue(item.value * item.stack).ToSellValue();
                tooltips[0].text += $"{value.ToTagString()}";
            }
        }
    }
    public class HsmodItemInfo : ItemInfo
    {
        public bool addValueTooltip = false;

        public override ItemInfo Clone()
        {
            var clone = new OwangItemInfo();
            clone.addValueTooltip = this.addValueTooltip;
            return clone;
        }
    }
}
