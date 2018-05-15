using MoreMultiplayerInfo.Helpers;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreMultiplayerInfo
{
    public class ReadyCheckHandler
    {
        private readonly IModHelper _helper;

        private readonly IMonitor _monitor;

        private Dictionary<long, HashSet<string>> ReadyPlayers { get; set; }

        private Dictionary<string, HashSet<long>> ReadyChecks { get; set; }

        public ReadyCheckHandler(IMonitor monitor, IModHelper helper)
        {
            _helper = helper;
            _monitor = monitor;

            ReadyPlayers = new Dictionary<long, HashSet<string>>();
            ReadyChecks = new Dictionary<string, HashSet<long>>();
            
            GameEvents.OneSecondTick += UpdateReadyChecks;
        }
        
        private void UpdateReadyChecks(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady) return;

            var readyPlayersBefore = new Dictionary<long, HashSet<string>>(ReadyPlayers);

            UpdateReadyChecksAndReadyPlayers();

            WatchForReadyCheckChanges(readyPlayersBefore);
        }

        private void WarnIfIAmLastPlayerReady(string readyCheck)
        {
            var readyPlayers = ReadyChecks[readyCheck];

            if (Game1.numberOfPlayers() - 1 == readyPlayers.Count && !readyPlayers.Contains(Game1.player.UniqueMultiplayerID))
            {
                _helper.SelfInfoMessage($"You are the last player not ready {GetFriendlyReadyCheckName(readyCheck)}...");
            }

        }

        private void WatchForReadyCheckChanges(Dictionary<long, HashSet<string>> readyPlayersBefore)
        {
            foreach (var player in readyPlayersBefore.Keys)
            {
                if (player == Game1.player.UniqueMultiplayerID) continue; /* Don't care about current player */

                if (!ReadyPlayers.ContainsKey(player))
                {
                    ReadyPlayers.Add(player, new HashSet<string>());
                }

                var checksBefore = readyPlayersBefore[player];
                var checksNow = ReadyPlayers[player];

                var newCheck = checksNow.FirstOrDefault(c => !checksBefore.Contains(c));
                var removedCheck = checksBefore.FirstOrDefault(c => !checksNow.Contains(c));

                var playerName = PlayerHelpers.GetPlayerWithUniqueId(player)?.Name;

                if (newCheck != null && newCheck != "wakeup")
                {
                    _helper.SelfInfoMessage($"{playerName} is now ready {GetFriendlyReadyCheckName(newCheck)}.");

                    WarnIfIAmLastPlayerReady(newCheck);
                }

                if (removedCheck != null && removedCheck != "wakeup")
                {
                    _helper.SelfInfoMessage($"{playerName} is no longer ready {GetFriendlyReadyCheckName(removedCheck)}.");
                }
            }
        }

        private void UpdateReadyChecksAndReadyPlayers()
        {
            var readyChecksField = _helper.Reflection.GetField<object>(Game1.player.team, "readyChecks");
            var readyChecksValue = readyChecksField.GetValue();
            var readyChecksValueType = readyChecksValue.GetType();

            var readyChecksValueTypeValues = ((IEnumerable<object>)readyChecksValueType.GetProperty("Values").GetValue(readyChecksValue)).ToList();

            var readyChecksResult = new Dictionary<string, HashSet<long>>();
            var readyPlayersResult = new Dictionary<long, HashSet<string>>();

            var allPlayerIds = Game1.getAllFarmers().Select(f => f.UniqueMultiplayerID);

            foreach (var player in allPlayerIds)
            {
                readyPlayersResult.Add(player, new HashSet<string>());
            }

            foreach (var readyCheck in readyChecksValueTypeValues)
            {

                var readyCheckName = _helper.Reflection.GetProperty<string>(readyCheck, "Name").GetValue();
                var readyPlayersCollection = _helper.Reflection.GetField<NetFarmerCollection>(readyCheck, "readyPlayers").GetValue();

                var readyPlayersIds = new HashSet<long>(readyPlayersCollection.Select(p => p.UniqueMultiplayerID).Distinct());

                readyChecksResult.Add(readyCheckName, readyPlayersIds);

                foreach (var playerId in readyPlayersIds)
                {
                    if (!readyPlayersResult.ContainsKey(playerId))
                    {
                        readyPlayersResult.Add(playerId, new HashSet<string>());
                    }

                    readyPlayersResult[playerId].Add(readyCheckName);
                }
            }

            ReadyChecks = readyChecksResult;
            ReadyPlayers = readyPlayersResult;
        }

        public bool IsPlayerWaiting(long playerId)
        {
            if (!ReadyPlayers.ContainsKey(playerId))
            {
                ReadyPlayers.Add(playerId, new HashSet<string>());
            }

            return ReadyPlayers[playerId].Any(r => r != "wakeup");
        }

        public string GetReadyCheckDisplayForPlayer(long playerId)
        {
            return string.Join(", ", ReadyPlayers[playerId].Select(GetFriendlyReadyCheckName));
        }

        private string GetFriendlyReadyCheckName(string readyCheckName)
        {
            switch (readyCheckName)
            {
                case "festivalStart":
                    return "for " + Game1.CurrentEvent.FestivalName;
                case "festivalEnd":
                    return "to leave";
                case "sleep":
                    return "to sleep";
                case "wakeup":
                    return "to wake up";
                case "passOut":
                    return "to pass out";
                default:
                    return readyCheckName;
            }
        }
    }
}