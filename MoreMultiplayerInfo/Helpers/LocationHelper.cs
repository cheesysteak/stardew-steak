using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StardewValley;

namespace MoreMultiplayerInfo.Helpers
{
    public class LocationHelper
    {

        public static string GetFriendlyLocationName(string locationName)
        {
            Regex regex = new Regex(@"\d+$");
            if (regex.IsMatch(locationName))
            {
                if (locationName.Contains("UndergroundMine"))
                    return "Floor " + regex.Match(locationName) + " of " + "Mountain Mines";
                else
                    return "Floor " + regex.Match(locationName) + " of " + "Skull Cavern";
            }

            switch (locationName)
            {
                case "BusStop":
                    return "Bus Stop";
                case "ArchaeologyHouse":
                    return "Archaeology Office";
                case "SeedShop":
                    return "General Store";
                case "JoshHouse":
                    return "1 River Road";
                case "ManorHouse":
                    return "Mayor's Manor";
                case "HaleyHouse":
                    return "2 Willow Lane";
                case "SamHouse":
                    return "1 Willow Lane";
                case "Town":
                    return "Pelican Town";
                case "FarmHouse":
                    return "Farm House";
                case "Farm":
                    return Game1.player.farmName.Value + " Farm";
                case "Cabin":
                    return "Farm Cabin";
                case "Tent":
                    return "Mountain Tent";
                case "CommunityCenter":
                    return "Community Center";
                case "WizardHouseBasement":
                    return "Wizard Tower Basement";
                case "WitchHut":
                    return "Witch Hut";
                case "WitchSwamp":
                    return "Witch Swamp";
                case "WitchWarpCave":
                    return "Witch Warp Cave";
                case "Greenhouse":
                    return "Farm Greenhouse";
                case "FarmCave":
                    return "Farm Cave";
                case "Coop":
                    return "Farm Coop";
                case "Barn":
                    return "Farm Barn";
                case "SkullCave":
                    return "Skull Cavern Entrance";
                case "UndergroundMine":
                    return "Mountain Mines Entrance";
                case "SandyHouse":
                    return "Oasis";
                case "BathHouse_Pool":
                    return "Bath House Pool";
                case "BathHouse_WomensLocker":
                    return "Bath House";
                case "BathHouse_MensLocker":
                    return "Bath House";
                case "BathHouse_Entry":
                    return "Bath House Entrance";
                case "WizardHouse":
                    return "Wizard's Tower";
                case "AdventureGuild":
                    return "Adventurer's Guild";
                case "ScienceHouse":
                    return "Carpenter's Shop";
                case "SebastianRoom":
                    return "Sebastian's Room";
                case "LeahHouse":
                    return "Leah's Cottage";
                case "FishShop":
                    return "Fishing Shop";
                case "Hospital":
                    return "Harvey's Clinic";
                case "Saloon":
                    return "Stardrop Saloon";
                case "JojaMart":
                    return "Joja Mart";
                case "AnimalShop":
                    return "Marnie's Ranch";
                case "Woods":
                    return "Secret Woods";
                case "Forest":
                    return "Cindersap Forest";
                default:
                    return locationName;
            }
        }
    }
}
