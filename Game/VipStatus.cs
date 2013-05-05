using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network;

namespace PhoenixProject.Game
{
    public unsafe class VipStatus
    {
        public enum VIPExtras : uint
        {
            Portal = 0x1,
            Avatars = 0x2,
            MoreFlowers = 0x4,
            FrozenGrotto = 0x8,
            TeleportTeam = 0x10,
            CityTeleport = 0x20,
            CityTeleportTeam = 0x40,
            MoreBlessingTime = 0x80,
            MoreOfflineTraining = 0x100,
            LongerRefine = 0x200,
            MoreFriends = 0x400,
            Hairstyles = 0x800,
            LabyrinthFree = 0x1000,
            StudyPointDailyQuests = 0x2000,
            Furniture = 0x4000,
            BonusLottery = 0x8000,
            All = 0xffff
        }
    }
}
