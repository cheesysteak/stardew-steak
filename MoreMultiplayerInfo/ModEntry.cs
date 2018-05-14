using StardewModdingAPI;

namespace MoreMultiplayerInfo
{
    public class ModEntry : Mod
    {
        private ModEntryHelper _baseHandler;

        public override void Entry(IModHelper helper)
        {
            _baseHandler = new ModEntryHelper(Monitor, helper);
        }
    }
}
