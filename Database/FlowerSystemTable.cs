using PhoenixProject.Client;
using PhoenixProject.Game.Features.Flowers;
using System;
namespace PhoenixProject.Database
{
    public class FlowerSystemTable
    {
        public static uint[] TopLilies = new uint[2];
        public static uint[] TopOrchids = new uint[2];
        public static uint[] TopRedRoses = new uint[2];
        public static uint[] TopTulips = new uint[2];
        private static bool Exists(uint id)
        {
            bool result;
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("flowers").Where("id", (long)((ulong)id));
                MySqlReader r = new MySqlReader(cmd);
                if (r.Read())
                {
                    r.Close();
                    r.Dispose();
                    result = true;
                    return result;
                }
            }
            catch
            {
            }
            result = false;
            return result;
        }
        public static void Flowers(GameState client)
        {
            client.Entity.Flowers = new Flowers();
            if (!FlowerSystemTable.Exists(client.Entity.UID))
            {
                FlowerSystemTable.Insert(client);
            }
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("flowers").Where("id", (long)((ulong)client.Entity.UID));
            MySqlReader r = new MySqlReader(cmd);
            if (r.Read())
            {
                client.Entity.Flowers.id = client.Entity.UID;
                client.Entity.Flowers.RedRoses2day = 0u;
                client.Entity.Flowers.Lilies2day = 0u;
                client.Entity.Flowers.Tulips2day = 0u;
                client.Entity.Flowers.Orchads2day = 0u;
                client.Entity.Flowers.RedRoses = r.ReadUInt32("redroses");
                client.Entity.Flowers.Lilies = r.ReadUInt32("lilies");
                client.Entity.Flowers.Tulips = r.ReadUInt32("tulips");
                client.Entity.Flowers.Orchads = r.ReadUInt32("orchads");
            }
            r.Close();
            r.Dispose();
        }
        public static void Insert(GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("flowers").Insert("id", (long)((ulong)client.Entity.UID)).Insert("redroses", (long)((ulong)client.Entity.Flowers.RedRoses)).Insert("redrosestoday", (long)((ulong)client.Entity.Flowers.RedRoses2day)).Insert("lilies", (long)((ulong)client.Entity.Flowers.Lilies)).Insert("liliestoday", (long)((ulong)client.Entity.Flowers.Lilies2day)).Insert("tulips", (long)((ulong)client.Entity.Flowers.Tulips)).Insert("tulipstoday", (long)((ulong)client.Entity.Flowers.Tulips2day)).Insert("orchads", (long)((ulong)client.Entity.Flowers.Orchads)).Insert("orchadstoday", (long)((ulong)client.Entity.Flowers.Orchads2day)).Insert("last_flower_sent", System.DateTime.Now.Subtract(System.TimeSpan.FromDays(1.0)).ToString()).Execute();
        }
        public static void SaveFlowerTable(GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("flowers").Set("id", (long)((ulong)client.Entity.UID)).Set("redroses", (long)((ulong)client.Entity.Flowers.RedRoses)).Set("redrosestoday", (long)((ulong)client.Entity.Flowers.RedRoses2day)).Set("lilies", (long)((ulong)client.Entity.Flowers.Lilies)).Set("liliestoday", (long)((ulong)client.Entity.Flowers.Lilies2day)).Set("tulips", (long)((ulong)client.Entity.Flowers.Tulips)).Set("tulipstoday", (long)((ulong)client.Entity.Flowers.Tulips2day)).Set("orchads", (long)((ulong)client.Entity.Flowers.Orchads)).Set("orchadstoday", (long)((ulong)client.Entity.Flowers.Orchads2day)).Set("last_flower_sent", client.Entity.Flowers.LastFlowerSent.ToString()).Where("id", (long)((ulong)client.Entity.UID)).Execute();
        }
    }
}