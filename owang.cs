using Terraria.ModLoader;
using owang.Items;
using owang.UI;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria;
using System.Collections.Generic;

namespace owang
{
	class owang : Mod
    {
        UserInterface exchangeUIF;
        public ExchangeUI exchangeUI;
        internal static owang instance;

        public owang()
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
            instance = this as owang;

            
            exchangeUIF = new UserInterface();
            exchangeUI = new ExchangeUI();
            exchangeUI.Activate();
            exchangeUIF.SetState(exchangeUI);
            exchangeUI.visible = true;            
        }
        public override void ModifyInterfaceLayers(List<MethodSequenceListItem> layers)
        {
            int insertLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (insertLayer != -1)
            {
                layers.Insert(insertLayer, new MethodSequenceListItem("owang: UI",
                    delegate
                    {
                        if (exchangeUI.visible)
                        {
                            exchangeUIF.Update(Main._drawInterfaceGameTime);
                            exchangeUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    null));
            }

            insertLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Interact Item Icon"));
            layers[insertLayer].Skip = insertLayer != -1 && exchangeUI.IsMouseHovering;
        }
    }
}
