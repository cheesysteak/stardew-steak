using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace LogDebugStuffEtc
{
    public class Class1 : Mod
    {
        private IModHelper _modHelper;

        public override void Entry(IModHelper helper)
        {
            _modHelper = helper;
            InputEvents.ButtonPressed += LogStuff;
        }

        private void LogStuff(object sender, EventArgsInput e)
        {
            if (e.Button == SButton.NumPad0)
            {
                Monitor.Log($"{Game1.timeOfDay / 100}:{Game1.timeOfDay % 100}");
            }
        }
    }
}
