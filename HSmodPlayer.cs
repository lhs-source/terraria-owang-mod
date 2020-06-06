using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Hsmod
{
    class HSmodPlayer : ModPlayer
    {
        public int matterValues { get; set; }
        public override void Load(TagCompound tag)
        {
            matterValues = tag.GetInt("mv");
            //base.Load(tag);
        }
        public override TagCompound Save()
        {
            return new TagCompound {
                {"mv", matterValues}
            };
            //return base.Save();
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
        }
    }
}
