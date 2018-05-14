using StardewModdingAPI;

namespace MoreMultiplayerInfo
{
    public class ModEntryHelper
    {
        public ModEntryHelper(IMonitor monitor, IModHelper modHelper)
        {
            var showIcon = new ShowPlayerIconHandler(monitor, modHelper);

            // var logHandler = new LogInputHandler(monitor, modHelper);
        }



    }
}