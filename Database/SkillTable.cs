using System;
using System.IO;
using System.Linq;

using SubSonic;

namespace PhoenixProject.Database
{
    public class SkillTable
    {
        public static void LoadProficiencies(Client.GameState client)
        {
            if (client.Entity == null)
                return;


            client.Proficiencies = new System.SafeDictionary<ushort, Interfaces.IProf>(100);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("profs").Where("EntityID", client.Entity.UID);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                Interfaces.IProf proficiency = new Network.GamePackets.Proficiency(true);
                proficiency.ID = r.ReadUInt16("ID");
                proficiency.Level = r.ReadByte("Level");
                proficiency.PreviousLevel = r.ReadByte("PreviousLevel");
                proficiency.Experience = r.ReadUInt32("Experience");
                proficiency.TempLevel = r.ReadByte("TempLevel");
                proficiency.Available = true;
                if (!client.Proficiencies.ContainsKey(proficiency.ID))
                    client.Proficiencies.Add(proficiency.ID, proficiency);
            }
            r.Close();
            r.Dispose();
        }
        public static void LoadProficiencies(Client.GameState client, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (client.Entity == null)
                return;


            client.Proficiencies = new System.SafeDictionary<ushort, Interfaces.IProf>(100);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("profs").Where("EntityID", client.Entity.UID);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                Interfaces.IProf proficiency = new Network.GamePackets.Proficiency(true);
                proficiency.ID = r.ReadUInt16("ID");
                proficiency.Level = r.ReadByte("Level");
                proficiency.PreviousLevel = r.ReadByte("PreviousLevel");
                proficiency.Experience = r.ReadUInt32("Experience");
                proficiency.TempLevel = r.ReadByte("TempLevel");
                proficiency.Available = true;
                if (!client.Proficiencies.ContainsKey(proficiency.ID))
                    client.Proficiencies.Add(proficiency.ID, proficiency);
            }
            r.Close();
            r.Dispose();
        }
        public static void SaveProficiencies(Client.GameState client)
        {
            if (client.Entity == null)
                return;
            if (client.Proficiencies == null)
                return;
            if (client.Proficiencies.Count == 0)
                return;
            foreach (Interfaces.IProf proficiency in client.Proficiencies.Values)
            {
                if (proficiency.Available)
                {
                    if (proficiency.TempLevel != proficiency.Level)
                    {
                        MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                        cmd.Update("profs").Set("Level", proficiency.Level).Set("TempLevel", proficiency.Level).Set("PreviousLevel", proficiency.PreviousLevel)
                            .Set("Experience", proficiency.Experience).Where("EntityID", client.Entity.UID).And("ID", proficiency.ID).Execute();
                    }
                }
                else
                {
                    proficiency.Available = true;
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("profs").Insert("Level", proficiency.Level).Insert("TempLevel", proficiency.Level).Insert("Experience", proficiency.Experience).Insert("EntityID", client.Entity.UID)
                        .Insert("ID", proficiency.ID).Execute();
                }
            }
        }
        public static void SaveProficiencies(Client.GameState client, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (client.Entity == null)
                return;
            if (client.Proficiencies == null)
                return;
            if (client.Proficiencies.Count == 0)
                return;
            foreach (Interfaces.IProf proficiency in client.Proficiencies.Values)
            {
                if (proficiency.Available)
                {
                    if (proficiency.TempLevel != proficiency.Level)
                    {
                        MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                        cmd.Update("profs").Set("Level", proficiency.Level).Set("TempLevel", proficiency.Level).Set("PreviousLevel", proficiency.PreviousLevel)
                            .Set("Experience", proficiency.Experience).Where("EntityID", client.Entity.UID).And("ID", proficiency.ID).Execute();
                    }
                }
                else
                {
                    proficiency.Available = true;
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("profs").Insert("Level", proficiency.Level).Insert("TempLevel", proficiency.Level).Insert("Experience", proficiency.Experience).Insert("EntityID", client.Entity.UID)
                        .Insert("ID", proficiency.ID).Execute();
                }
            }
        }
        public static void LoadSpells(Client.GameState client, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (client.Entity == null)
                return;

            client.Spells = new System.SafeDictionary<ushort, Interfaces.ISkill>(100);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("skills").Where("EntityID", client.Entity.UID);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                Interfaces.ISkill spell = new Network.GamePackets.Spell(true);
                spell.ID = r.ReadUInt16("ID");
                spell.Level = r.ReadByte("Level");
                spell.PreviousLevel = r.ReadByte("PreviousLevel");
                spell.Experience = r.ReadUInt32("Experience");
                spell.TempLevel = r.ReadByte("TempLevel");
                spell.Available = true;
                if (!client.Spells.ContainsKey(spell.ID))
                    client.Spells.Add(spell.ID, spell);
            }
            r.Close();
            r.Dispose();
        }
        public static void LoadSpells(Client.GameState client)
        {
            if (client.Entity == null)
                return;

            client.Spells = new System.SafeDictionary<ushort, Interfaces.ISkill>(100);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("skills").Where("EntityID", client.Entity.UID);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                Interfaces.ISkill spell = new Network.GamePackets.Spell(true);
                spell.ID = r.ReadUInt16("ID");
                spell.Level = r.ReadByte("Level");
                spell.PreviousLevel = r.ReadByte("PreviousLevel");
                spell.Experience = r.ReadUInt32("Experience");
                spell.Available = true;
                if (!client.Spells.ContainsKey(spell.ID))
                    client.Spells.Add(spell.ID, spell);
            }
            r.Close();
            r.Dispose();
            
        }
        public static void SaveSpells(Client.GameState client)
        {
            if (client.Entity == null)
                return;
            if (client.Spells == null)
                return;
            if (client.Spells.Count == 0)
                return;
            foreach (Interfaces.ISkill spell in client.Spells.Values)
            {
                if (spell.Available)
                {
                    if (spell.TempLevel != spell.Level)
                    {
                        MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                        cmd.Update("skills").Set("Level", spell.Level).Set("TempLevel", spell.Level).Set("PreviousLevel", spell.PreviousLevel)
                            .Set("Experience", spell.Experience).Where("EntityID", client.Entity.UID).And("ID", spell.ID).Execute();
                    }
                }
                else
                {

                    spell.Available = true;
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("skills").Insert("Level", spell.Level).Insert("TempLevel", spell.Level).Insert("Experience", spell.Experience).Insert("EntityID", client.Entity.UID)
                        .Insert("ID", spell.ID).Execute();
                }
            }
        }
        public static void SaveSpells(Client.GameState client, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (client.Entity == null)
                return;
            if (client.Spells == null)
                return;
            if (client.Spells.Count == 0)
                return;
            foreach (Interfaces.ISkill spell in client.Spells.Values)
            {
                if (spell.Available)
                {
                    if (spell.TempLevel != spell.Level)
                    {
                        MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                        cmd.Update("skills").Set("Level", spell.Level).Set("TempLevel", spell.Level).Set("PreviousLevel", spell.PreviousLevel)
                            .Set("Experience", spell.Experience).Where("EntityID", client.Entity.UID).And("ID", spell.ID).Execute();

                    }
                    
                }
                else
                {
                    spell.Available = true;
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("skills").Insert("Level", spell.Level).Insert("TempLevel", spell.Level).Insert("Experience", spell.Experience).Insert("EntityID", client.Entity.UID)
                        .Insert("ID", spell.ID).Execute();
                }
            }
        }
        public static void DeleteSpell(Client.GameState client, ushort ID)
        {
            MySqlCommand command = new MySqlCommand(MySqlCommandType.DELETE);
            command.Delete("skills", "ID", ID).And("EntityID", client.Entity.UID).Execute();
            
        }
    }
}
