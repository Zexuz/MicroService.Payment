using System.Collections.Generic;
using MicroService.Payment.Service.Steam.Services;

namespace MicroService.Payment.Service.Steam.Helpers
{
    public static class SteamPaymentMethodToAppId
    {
        private static readonly Dictionary<SteamGames, int> MethodAsKey = new Dictionary<SteamGames, int>
        {
            {SteamGames.Csgo, 730},
            {SteamGames.Pubg, 578080},
        };

        private static readonly Dictionary<int, SteamGames> AppIdAsKey = new Dictionary<int, SteamGames>
        {
            {730, SteamGames.Csgo},
            {578080, SteamGames.Pubg},
        };

        public static int ToInt(SteamGames methods)
        {
            return MethodAsKey[methods];
        }

        public static SteamGames ToEnum(int i)
        {
            return AppIdAsKey[i];
        }
    }
}