using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Linq;

namespace MoreMultiplayerInfo
{
    public class LogInputHandler
    {
        private readonly IMonitor _monitor;

        private Vector2 _lastMousePos;

        public LogInputHandler(IMonitor monitor, IModHelper modHelper)
        {
            _monitor = monitor;
            InputEvents.ButtonPressed += LogInfo;

            _lastMousePos = new Vector2(0, 0);
        }

        private void LogInfo(object sender, EventArgsInput e)
        {
            if (e.Button == SButton.NumPad2)
            {
                foreach (var player in Game1.getAllFarmers().Where(f => f != null))
                {
                    _monitor.Log($"{player.Name}: {player.stamina}");
                }
            }
            
        }

        private void ExpLog()
        {
            var player = Game1.player;

            player.foragingLevel = 1;
            player.MiningLevel = 2;
            player.FarmingLevel = 3;
            player.CombatLevel = 4;
            player.FishingLevel = 5;

            var exps = player.experiencePoints;

            var farmingIdx = 0;
            var fishingIdx = 1;
            var forageIdx = 2;
            var miningIdx = 3;
            var combatIdx = 4;
            

        }

        private void MousePosLog(EventArgsInput e)
        {
            var diffX = e.Cursor.ScreenPixels.X - _lastMousePos.X;
            var diffY = e.Cursor.ScreenPixels.Y - _lastMousePos.Y;

            var diffXDisplay = ((diffX > 0) ? "+" : "-") + Math.Abs(diffX);
            var diffYDisplay = ((diffY > 0) ? "+" : "-") + Math.Abs(diffY);

            // _monitor.Log($"Mouse Position: ({e.Cursor.ScreenPixels.X}, {e.Cursor.ScreenPixels.Y}) ({diffXDisplay}, {diffYDisplay})");
            _lastMousePos = new Vector2(e.Cursor.ScreenPixels.X, e.Cursor.ScreenPixels.Y);
        }
    }
}