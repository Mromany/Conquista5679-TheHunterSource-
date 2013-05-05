using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    public class PkExpelTable
    {
        public static void Load(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("pk_explorer").Where("uid", client.Account.EntityID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                Game.PkExpeliate pk = new Game.PkExpeliate();
                pk.UID = r.ReadUInt32("killed_uid");
                pk.Name = r.ReadString("killed_name");
                pk.KilledAt = r.ReadString("killed_map");
                pk.LostExp = r.ReadUInt32("lost_exp");
                pk.Times = r.ReadUInt32("times");
                pk.Potency = r.ReadUInt32("battle_power");
                pk.Level = r.ReadByte("level");
                //client.Entity.PkExplorerValues.SafeAdd(pk.UID, pk);
                client.Entity.PkExplorerValues.Add(pk.UID, pk);

            }
            r.Close();
            r.Dispose();
        }
        public static void PkExploitAdd(Client.GameState client, uint UIDEnemy, Game.PkExpeliate pk)
        {
            MySqlCommand cmds = new MySqlCommand(MySqlCommandType.SELECT);
            cmds.Select("pk_explorer").Where("uid", client.Account.EntityID);
            MySqlReader rdr = new MySqlReader(cmds);
            if (rdr.Read())
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("pk_explorer")
                     .Set("killed_uid", UIDEnemy)
                     .Set("killed_name", pk.Name).Set("killed_map", pk.KilledAt)
                     .Set("lost_exp", pk.LostExp).Set("times", pk.Times++)
                     .Set("battle_power", pk.Potency).Set("level", pk.Level);
                cmd.Execute();
                if (!client.Entity.PkExplorerValues.ContainsKey(pk.UID))
                {
                    client.Entity.PkExplorerValues.Add(pk.UID, pk);
                }
                else
                {
                    client.Entity.PkExplorerValues.Remove(pk.UID);
                    client.Entity.PkExplorerValues.Add(pk.UID, pk);
                }
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("pk_explorer")
                     .Insert("uid", pk.UID).Insert("killed_uid", UIDEnemy)
                     .Insert("killed_name", pk.Name).Insert("killed_map", pk.KilledAt)
                     .Insert("lost_exp", pk.LostExp).Insert("times", pk.Times)
                     .Insert("battle_power", pk.Potency).Insert("level", pk.Level);
                cmd.Execute();
                if (!client.Entity.PkExplorerValues.ContainsKey(pk.UID))
                    client.Entity.PkExplorerValues.Add(pk.UID, pk);
            }
            rdr.Close();
            rdr.Dispose();

        }
    }
}

