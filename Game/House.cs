using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;

namespace PhoenixProject.Game
{
    class House//Coded By Kimo
    {
        public static void createhouse(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("house").Insert("id", client.Entity.UID).Insert("mapdoc", "3024").Insert("type", "7").Insert("weather", "0").Insert("owner", client.Entity.UID).Insert("HouseLevel", "1");

            cmd.Execute();

            //PhoenixProject.Database.MapsTable.MapInformation info = new PhoenixProject.Database.MapsTable.MapInformation();
           // info.ID = (ushort)client.Entity.UID;
           // info.BaseID = 601;
            //info.Status = 7;
           // info.Weather = 0;
            //info.Owner = client.Entity.UID;
            //info.HouseLevel = 1;
            //PhoenixProject.Database.MapsTable.MapInformations.Add(info.ID, info);
            bool Success = DMaps.CreateDynamicMap2(client.Entity.UID, 1765, (uint)client.Entity.UID, 1);
            return;
        }
        public static void UpgradeHouse(Client.GameState client)
        {

            //ServerBase.Kernel.Maps.Remove((ushort)client.Entity.UID);
            ServerBase.Kernel.Maps.Remove((ushort)client.Entity.UID);
            bool Success = DMaps.CreateDynamicMap2(client.Entity.UID, 3024, (uint)client.Entity.UID, 1);
            new MySqlCommand(MySqlCommandType.UPDATE).Update("house").Set("HouseLevel", "2").Set("mapdoc", "3024").Where("id", client.Entity.UID).Execute();
            return;
        }
        public static void AddBox(Client.GameState client)
        {

            PhoenixProject.Database.MapsTable.MapInformations.Remove((ushort)client.Entity.UID);
            ServerBase.Kernel.Maps.Remove((ushort)client.Entity.UID);
            PhoenixProject.Database.MapsTable.MapInformation info = new PhoenixProject.Database.MapsTable.MapInformation();
            info.ID = (ushort)client.Entity.UID;
            info.BaseID = 1099;
            info.Status = 7;
            info.Weather = 0;
            info.Owner = client.Entity.UID;
            info.HouseLevel = 2;
            info.Box = 1;
            info.BoxX = client.Entity.X;
            info.BoxY = client.Entity.Y;
            PhoenixProject.Database.MapsTable.MapInformations.Add(info.ID, info);

           
            new MySqlCommand(MySqlCommandType.UPDATE).Update("house").Set("Box", "1").Set("BoxX", client.Entity.X).Set("BoxY", client.Entity.Y).Where("id", client.Entity.UID).Execute();
            return;
        }

    }
}
