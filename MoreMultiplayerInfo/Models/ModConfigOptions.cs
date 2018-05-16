namespace MoreMultiplayerInfo
{
    public class ModConfigOptions
    {

        [OptionDisplay("Show Inventory")]
        public bool ShowInventory { get; set; }


        [OptionDisplay("Show Info in Text Box")]
        public bool ShowReadyInfoInChatBox { get; set; }


        [OptionDisplay("Last Player Alert")]
        public bool ShowLastPlayerReadyInfoInChatBox { get; set; }

        [OptionDisplay("Hide in Single Player")]
        public bool HideInSinglePlayer { get; set; }
    }
}