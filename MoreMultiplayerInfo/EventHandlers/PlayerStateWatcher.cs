using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreMultiplayerInfo.Helpers;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace MoreMultiplayerInfo.EventHandlers
{
    public class PlayerStateWatcher
    {
        public class PlayerLastActivity
        {
            private static Dictionary<string, string> ActivityDisplayNames => new Dictionary<string, string>
            {
                { "hoe", "Dug in dirt" },
                { "pickaxe", "Smashed a rock" },
                { "axe", "Chopped a tree" },
                { "wateringcan", "Watered a crop" },
                { "warped", "Switched areas" }
            };


            public string Activity { get; set; }

            public int When { get; set; }

            public string LocationName { get; set; }

            private int OneHourSpan => 6;

            private int HalfHour => OneHourSpan / 2;

            private int TwoHours => OneHourSpan * 2;

            private int TimeSinceWhen => Game1.timeOfDay - When;

            public string GetDisplayText()
            {

                return $"Last Activity: {GetActivityDisplay()} {GetWhenDisplay()}";
            }


            private string GetActivityDisplay()
            {
                if (TimeSinceWhen >= TwoHours)
                {
                    return "Standing Around";
                }

                if (ActivityDisplayNames.ContainsKey(Activity))
                {
                    return ActivityDisplayNames[Activity];
                }

                return Activity;
            }

            private string GetWhenDisplay()
            {
                if (TimeSinceWhen < HalfHour)
                {
                    return "just now";
                }

                if (TimeSinceWhen < TwoHours)
                {
                    return "one hour ago";
                }

                return "since " + Game1.getTimeOfDayString(When);
            }

        }

        public static Dictionary<long, PlayerLastActivity> LastActions { get; set; }

        public PlayerStateWatcher()
        {
            GameEvents.EighthUpdateTick += WatchPlayerActions;
        }

        private void WatchPlayerActions(object sender, EventArgs e)
        {
            var players = PlayerHelpers.GetAllCreatedFarmers();

            foreach (var player in players)
            {
                var playerId = player.uniqueMultiplayerID;

                LastActions.GetOrCreateDefault(playerId);
                
                var currentLocation = player.currentLocation.name;

                if (currentLocation != LastActions[playerId].LocationName)
                {
                    LastActions[playerId] = new PlayerLastActivity
                    {
                        Activity = "warped",
                        LocationName = currentLocation,
                        When = Game1.timeOfDay
                    };
                }

                if (player.UsingTool)
                {
                    LastActions[playerId] = new PlayerLastActivity
                    {
                        Activity = player.CurrentTool?.Name.ToLower() ?? "N/A",
                        When = Game1.timeOfDay,
                        LocationName = currentLocation
                    };
                }

                
            }
        }

        public static PlayerLastActivity GetLastActionForPlayer(long playerId)
        {
            return LastActions.GetOrCreateDefault(playerId);
        }
    }
}
