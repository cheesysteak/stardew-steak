using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreMultiplayerInfo.Helpers
{
    public static class PlayerHelpers
    {
        public static List<Farmer> GetAllCreatedFarmers()
        {
            return Game1.getAllFarmers()
                .Where(f => !string.IsNullOrEmpty(f.Name))
                .Where(f => f.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID)
                .ToList();
        }

        public static Farmer GetPlayerWithUniqueId(long id)
        {
            return Game1.getAllFarmers()
                .FirstOrDefault(f => f.UniqueMultiplayerID == id);
        }

        public static bool IsPlayerOffline(long playerId)
        {
            var onlineFarmerIds = Game1.getOnlineFarmers().Select(f => f.UniqueMultiplayerID);
            return !onlineFarmerIds.Contains(playerId);
        }
    }

    public static class MonitorExtensions
    {
        public static void BroadcastInfoMessage(this IModHelper helper, string message)
        {
            if (Context.IsMainPlayer)
            {
                helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue().globalChatInfoMessage("ChatMessageFormat", new String[] { "PlayerReady", message });
            }

        }

        public static void SelfInfoMessage(this IModHelper helper, string message)
        {
            Game1.chatBox.addInfoMessage(message);
        }

        public static void BroadcastIfHost(this IModHelper helper, string message)
        {
            if (Game1.player.IsMainPlayer)
            {
                helper.BroadcastInfoMessage(message);
            }
            else
            {
                helper.SelfInfoMessage(message);
            }

        }
    }


}
