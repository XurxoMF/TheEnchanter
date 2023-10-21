using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace TheEnchanter
{
    public class TheEnchanterModSystem : ModSystem
    {
        // Called on server and client
        public override void Start(ICoreAPI api)
        {
            api.Logger.Notification("Hello from The Enchanter: " + Lang.Get("theenchanter:hello"));
        }
    }
}
