using System;
using System.Collections.Generic;
using System.Text;
using PhoenixProject.Game.ConquerStructures;
using PhoenixProject.Game.Features;

namespace PhoenixProject.Database
{
    class ArsenalTable
    {
        public static void UpdateArsenal(ulong Donation, Arsenal_ID id)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("pt_arsenal")
                .Set("arsenal_donation", Donation.ToString())
                .Where("arsenal_id", (byte)id)
                .Execute();
        }

        public static void CreateArsenalItem(uint uid, string name, uint i_uid, ushort sid, Arsenal_ID aid)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.INSERT);
            Command.Insert("pt_arsenal_inscribed")
                .Insert("uid", uid)
                .Insert("name", name)
                .Insert("iten_id", i_uid)
                .Insert("iten_atype", (byte)aid)
                .Insert("syn_id", sid)
                .Execute();
        }

        public static void DeleteArsenalItem(uint uid, uint i_uid, Arsenal_ID aid)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.DELETE);
            Command.Delete("pt_arsenal_inscribed", "uid", uid).And("iten_id", i_uid).Execute();
        }
        public static void CreateArsenal(uint s_id, Arsenal_ID id)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.INSERT);
            Command.Insert("pt_arsenal")
                .Insert("syn_id", s_id)
                .Insert("arsenal_id", (byte)id)
                .Insert("arsenal_unlocked", 1)
                .Execute();
        }

        public static void DeleteArsenal(uint s_id, Arsenal_ID id)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.DELETE);
            Command.Delete("pt_arsenal", "syn_id", s_id).And("arsenal_id", (byte)id).Execute();
        }

        public static void LoadArsenal(PhoenixProject.Game.ConquerStructures.Society.Guild Syn)
        {
            Syn.Arsenal = new Arsenal((ushort)Syn.ID);
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.SELECT);
            Command.Select("pt_arsenal").Where("syn_id", Syn.ID).Execute();
            MySqlReader Reader = new MySqlReader(Command);
            while (Reader.Read())
            {
                PhoenixProject.Game.Features.Arsenal_State Arsenal = new PhoenixProject.Game.Features.Arsenal_State();
                Arsenal.ID = (PhoenixProject.Game.Features.Arsenal_ID)Reader.ReadByte("arsenal_id");
                Arsenal.Unlocked = (Reader.ReadByte("arsenal_unlocked") == 1);
                Arsenal.Donation = Reader.ReadUInt32("arsenal_donation");

                if (!Syn.Arsenal.Arsenals.ContainsKey(Arsenal.ID))
                    Syn.Arsenal.Arsenals.Add(Arsenal.ID, Arsenal);
            }
            if (Syn.Arsenal.Arsenals.Count > 0)
            {
                Command = new MySqlCommand(MySqlCommandType.SELECT);
                Command.Select("pt_arsenal_inscribed").Where("syn_id", Syn.ID).Execute();
                Reader = new MySqlReader(Command);
                while (Reader.Read())
                {
                    uint OWNER = Reader.ReadUInt32("uid");
                    string Name = Reader.ReadString("name");
                    uint UID = Reader.ReadUInt32("iten_id");
                    PhoenixProject.Game.Features.Arsenal_ID ID = (PhoenixProject.Game.Features.Arsenal_ID)Reader.ReadByte("iten_atype");
                    if (Syn.Arsenal.Arsenals.ContainsKey(ID))
                    {
                        if (!Syn.Arsenal.Arsenals[ID].Inscribed.ContainsKey(UID))
                        {
                            Syn.Arsenal.Arsenals[ID].Inscribed.Add(UID, new PhoenixProject.Game.Features.Arsenal_Client() { UID = OWNER, Name = Name, iUID = UID });
                        }
                    }
                }
            }
            foreach (PhoenixProject.Game.Features.Arsenal_State astate in Syn.Arsenal.Arsenals.Values)
            {
                foreach (PhoenixProject.Game.Features.Arsenal_Client ac in astate.Inscribed.Values)
                {
                    if (ac.Item == null)
                    {
                        ac.Item = ConquerItemTable.LoadItem(ac.iUID);
                    }
                }
            }
            Reader.Close();
        }
    }
}
