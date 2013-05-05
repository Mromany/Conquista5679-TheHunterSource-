using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Game.Features.Reincarnation;


namespace PhoenixProject.Database
{
    public class ReincarnationTable
    {
        public static void Load(Client.GameState client)
        {
             MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
             cmd.Select("reincarnation").Where("uid", client.Entity.UID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                ReincarnateInfo info = new ReincarnateInfo();
                info.UID = r.ReadUInt32("uid");
                info.Level = r.ReadByte("level");
                info.Experience = r.ReadUInt64("experience");
                if (!ServerBase.Kernel.ReincarnatedCharacters.ContainsKey(info.UID))
                    ServerBase.Kernel.ReincarnatedCharacters.Add(info.UID, info);
            }
            r.Close();
            r.Dispose();
        }

        public static void NewReincarnated(Game.Entity entity)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("reincarnation")
                .Insert("uid", entity.UID)
                .Insert("level", entity.Level)
                .Insert("experience", 0);
            cmd.Execute();
           // Conquer.Database.Reincarnation.Insert((int)entity.UID, (int)entity.Level, 0);
        }

        public static void RemoveReincarnated(Game.Entity entity)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("reincarnation", "uid", entity.UID).Execute();
        }
    }
}
