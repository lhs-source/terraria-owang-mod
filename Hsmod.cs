using Terraria.ModLoader;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria;
using System.Collections.Generic;
using Hsmod.UI;
using Hsmod.Graphics;

namespace Hsmod
{
    class Hsmod : Mod
    {
        UserInterface exchangeUIF;
        public ExchangeUI exchangeUI;
        internal static Hsmod instance;

        public static float GlobalTimeWrappedHourly;
        public static SpriteViewMatrix GameViewMatrix;

        public Hsmod()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }
        public override void Load()
        {
            instance = this as Hsmod;


            exchangeUIF = new UserInterface();
            exchangeUI = new ExchangeUI();
            exchangeUI.Activate();
            exchangeUIF.SetState(exchangeUI);
            exchangeUI.visible = true;
        }
        //public override void ModifyInterfaceLayers(List<MethodSequenceListItem> layers)
        //{
        //    int insertLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        //    if (insertLayer != -1)
        //    {
        //        layers.Insert(insertLayer, new MethodSequenceListItem("owang: UI",
        //            delegate
        //            {
        //                if (exchangeUI.visible)
        //                {
        //                    exchangeUIF.Update(Main._drawInterfaceGameTime);
        //                    exchangeUI.Draw(Main.spriteBatch);
        //                }
        //                return true;
        //            },
        //            null));
        //    }

        //    insertLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interact Item Icon"));
        //    layers[insertLayer].Skip = insertLayer != -1 && exchangeUI.IsMouseHovering;
        //}
    }
}