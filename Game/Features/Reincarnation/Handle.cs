using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game.Features.Reincarnation
{
    public class Handle
    {
        public static void Hash(Client.GameState client)
        {
            if (ServerBase.Kernel.ReincarnatedCharacters.ContainsKey(client.Entity.UID))
            {
                if (client.Entity.Level >= 110 && client.Entity.Reborn >= 2)
                {
                    ushort stats = 0;
                    uint lev1 = client.Entity.Level;
                    ReincarnateInfo info = ServerBase.Kernel.ReincarnatedCharacters[client.Entity.UID];
                    client.Entity.Level = info.Level;
                    client.Entity.Experience = info.Experience;
                    ServerBase.Kernel.ReincarnatedCharacters.Remove(info.UID);
                    Database.ReincarnationTable.RemoveReincarnated(client.Entity);
                    stats = (ushort)(((client.Entity.Level - lev1) * 3) - 3);
                    client.Entity.Atributes += stats;
                }
            }
        }
    }
    #region Reincarnation
    public class Reincarnation
    {
        private Client.GameState _client;
        private SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill> RemoveSkill = null;
        private SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill> Addskill = null;
        public Reincarnation(Client.GameState client, byte new_class)
        {
            if (client.Entity.Level < 130)
                return;
            _client = client;
            RemoveSkill = new SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill>(500);
            Addskill = new SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill>(500);
            #region Low level items
            for (byte i = 1; i < 9; i++)
            {
                if (i != 7)
                {
                    Interfaces.IConquerItem item = client.Equipment.TryGetItem(i);
                    if (item != null && item.ID != 0)
                    {
                        try

                        {
                            //client.UnloadItemStats(item, false);
                            Database.ConquerItemInformation cii = new PhoenixProject.Database.ConquerItemInformation(item.ID, item.Plus);
                            item.ID = cii.LowestID(Network.PacketHandler.ItemMinLevel(Network.PacketHandler.ItemPosition(item.ID)));
                            item.Mode = PhoenixProject.Game.Enums.ItemMode.Update;
                            item.Send(client);
                            client.LoadItemStats(client.Entity);
                            Database.ConquerItemTable.UpdateItemID(item, client);
                        }
                        catch
                        {
                            Console.WriteLine("Reborn item problem: " + item.ID);
                        }
                    }
                }
            }
            Interfaces.IConquerItem hand = client.Equipment.TryGetItem(5);
            if (hand != null)
            {
                client.Equipment.Remove(5);
                client.CalculateStatBonus();
                client.CalculateHPBonus();
                client.SendStatMessage();
            }
            else
                //client.Screen.send(client.Entity.SpawnPacket, false);
            #endregion

            #region Remove Extra Skill
            if (client.Entity.FirstRebornClass == 15 && client.Entity.SecondRebornClass == 15 && client.Entity.Class == 15)
            {
                WontAdd(PhoenixProject.Game.Enums.SkillIDs.DragonWhirl);
            }
            if (client.Entity.FirstRebornClass == 25 && client.Entity.SecondRebornClass == 25 && client.Entity.Class == 25)
            {
                WontAdd(PhoenixProject.Game.Enums.SkillIDs.Perseverance);
            }
            if (client.Entity.FirstRebornClass == 45 && client.Entity.SecondRebornClass == 45 && client.Entity.Class == 45)
            {
                WontAdd(PhoenixProject.Game.Enums.SkillIDs.StarArrow);
            }
            if (client.Entity.FirstRebornClass == 55 && client.Entity.SecondRebornClass == 55 && client.Entity.Class == 55)
            {
                WontAdd(PhoenixProject.Game.Enums.SkillIDs.PoisonStar);
            }
            if (client.Entity.FirstRebornClass == 65 && client.Entity.SecondRebornClass == 65 && client.Entity.Class == 65)
            {
                WontAdd(PhoenixProject.Game.Enums.SkillIDs.SoulShackle);
            }
            if (client.Entity.FirstRebornClass == 135 && client.Entity.SecondRebornClass == 135 && client.Entity.Class == 135)
            {
                WontAdd(PhoenixProject.Game.Enums.SkillIDs.AzureShield);
            }
            if (client.Entity.FirstRebornClass == 145 && client.Entity.SecondRebornClass == 145 && client.Entity.Class == 145)
            {
                WontAdd(PhoenixProject.Game.Enums.SkillIDs.HeavenBlade);
            }
            #endregion
            Database.ReincarnationTable.NewReincarnated(client.Entity);
            Game.Features.Reincarnation.ReincarnateInfo info = new Game.Features.Reincarnation.ReincarnateInfo();
            info.UID = client.Entity.UID;
            info.Level = client.Entity.Level;
            info.Experience = client.Entity.Experience;
            ServerBase.Kernel.ReincarnatedCharacters.Add(info.UID, info);
            client.Entity.FirstRebornClass = client.Entity.SecondRebornClass;
            client.Entity.SecondRebornClass = client.Entity.Class;
            client.Entity.Class = new_class;
            client.Entity.SecondRebornLevel = client.Entity.Level;
            client.Entity.Level = 15;
            client.Entity.Experience = 0;
            client.Entity.Atributes =
 (ushort)(client.ExtraAtributePoints(client.Entity.FirstRebornClass, client.Entity.FirstRebornLevel) +
  client.ExtraAtributePoints(client.Entity.SecondRebornClass, client.Entity.SecondRebornLevel) + 62);



            client.Spells.Clear();
            client.Spells = new SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill>(100);
            switch (client.Entity.FirstRebornClass)
            {
                case 15:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Cyclone);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Hercules);
                        Add(PhoenixProject.Game.Enums.SkillIDs.SpiritHealing);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Robot);
                        break;
                    }
                case 25:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.SuperMan);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Dash);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Shield);
                        break;
                    }
                case 45:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Intensify);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Scatter);
                        Add(PhoenixProject.Game.Enums.SkillIDs.RapidFire);
                        Add(PhoenixProject.Game.Enums.SkillIDs.XPFly);
                        Add(PhoenixProject.Game.Enums.SkillIDs.AdvancedFly);
                        break;
                    }
                case 55:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.FatalStrike);
                        Add(PhoenixProject.Game.Enums.SkillIDs.ShurikenVortex);
                        Add(PhoenixProject.Game.Enums.SkillIDs.ToxicFog);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TwofoldBlades);
                        Add(PhoenixProject.Game.Enums.SkillIDs.PoisonStar);

                        break;
                    }
                case 65:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.RadiantPalm);
                        Add(PhoenixProject.Game.Enums.SkillIDs.WhirlWindKick);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TripleAttack);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Oblivion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Serenity);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Compassion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.DeflectionAura);
                        break;
                    }
                case 75:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.RadiantPalm);
                        Add(PhoenixProject.Game.Enums.SkillIDs.WhirlWindKick);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TripleAttack);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Oblivion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Serenity);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Compassion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.DeflectionAura);
                        break;
                    }
                case 135:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Thunder);
                        Add(PhoenixProject.Game.Enums.SkillIDs.WaterElf);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Cure);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Lightning);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Volcano);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Pray);
                        Add(PhoenixProject.Game.Enums.SkillIDs.AdvancedCure);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Meditation);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Stigma);
                        break;
                    }
                case 140:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Thunder);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Cure);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Lightning);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Tornado);
                        Add(PhoenixProject.Game.Enums.SkillIDs.FireCircle);
                        Add(PhoenixProject.Game.Enums.SkillIDs.FireMeteor);
                        Add(PhoenixProject.Game.Enums.SkillIDs.FireRing);
                        break;
                    }

            }

            byte PreviousClass = client.Entity.FirstRebornClass;
            byte toClass = (byte)(client.Entity.SecondRebornClass - 4);

            Interfaces.ISkill[] ADD_spells = this.Addskill.Values.ToArray();
            foreach (Interfaces.ISkill skill in ADD_spells)
            {
                skill.Available = true;
                if (!client.Spells.ContainsKey(skill.ID))
                    client.Spells.Add(skill.ID, skill);
            }
            #region Spells
            Interfaces.ISkill[] spells = client.Spells.Values.ToArray();
            foreach (Interfaces.ISkill spell in spells)
            {
                spell.PreviousLevel = spell.Level;
                spell.Level = 0;
                spell.Experience = 0;
                #region Pirate
                if (PreviousClass == 75)
                {
                    if (client.Entity.Class != 71)
                    {
                        switch (spell.ID)
                        {
                            case 10490:
                            case 10415:
                            case 10381:
                                client.RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                #region Monk
                if (PreviousClass == 65)
                {
                    if (client.Entity.Class != 61)
                    {
                        switch (spell.ID)
                        {
                            case 10490:
                            case 10415:
                            case 10381:
                                client.RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                #region Warrior
                if (PreviousClass == 25)
                {
                    if (client.Entity.Class != 21)
                    {
                        switch (spell.ID)
                        {
                            case 1025:
                                if (client.Entity.Class != 21 && client.Entity.Class != 132)
                                    client.RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                #region Ninja
                if (toClass != 51)
                {
                    switch (spell.ID)
                    {
                        case 6010:
                        case 6000:
                        case 6011:
                            client.RemoveSpell(spell);
                            break;
                    }
                }
                #endregion
                #region Trojan
                if (toClass != 11)
                {
                    switch (spell.ID)
                    {
                        case 1115:
                            client.RemoveSpell(spell);
                            break;
                    }
                }
                #endregion
                #region Archer
                if (toClass != 41)
                {
                    switch (spell.ID)
                    {
                        case 8001:
                        case 8000:
                        case 8003:
                        case 9000:
                        case 8002:
                        case 8030:
                            client.RemoveSpell(spell);
                            break;
                    }
                }
                #endregion
                #region WaterTaoist
                if (PreviousClass == 135)
                {
                    if (toClass != 132)
                    {
                        switch (spell.ID)
                        {
                            case 1000:
                            case 1001:
                            case 1010:
                            case 1125:
                            case 1100:
                            case 8030:
                                client.RemoveSpell(spell);
                                break;
                            case 1050:
                            case 1175:
                            case 1170:
                                if (toClass != 142)
                                    client.RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                #region FireTaoist
                if (PreviousClass == 145)
                {
                    if (toClass != 142)
                    {
                        switch (spell.ID)
                        {
                            case 1000:
                            case 1001:
                            case 1150:
                            case 1180:
                            case 1120:
                            case 1002:
                            case 1160:
                            case 1165:
                                client.RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                if (client.Spells.ContainsKey(spell.ID))
                    if (spell.ID != (ushort)Game.Enums.SkillIDs.Reflect)
                        spell.Send(client);
            }
            #endregion
            Add(PhoenixProject.Game.Enums.SkillIDs.Bless);

            Addskill.Clear();
            Addskill = new SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill>(100);

            PreviousClass = client.Entity.SecondRebornClass;
            toClass = client.Entity.Class;
            switch (client.Entity.SecondRebornClass)
            {
                case 15:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Robot);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Cyclone);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Hercules);
                        Add(PhoenixProject.Game.Enums.SkillIDs.SpiritHealing);

                        break;
                    }
                case 25:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.SuperMan);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Dash);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Shield);
                        break;
                    }
                case 45:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Intensify);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Scatter);
                        Add(PhoenixProject.Game.Enums.SkillIDs.RapidFire);
                        Add(PhoenixProject.Game.Enums.SkillIDs.XPFly);
                        Add(PhoenixProject.Game.Enums.SkillIDs.AdvancedFly);
                        break;
                    }
                case 55:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.FatalStrike);
                        Add(PhoenixProject.Game.Enums.SkillIDs.ShurikenVortex);
                        Add(PhoenixProject.Game.Enums.SkillIDs.ToxicFog);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TwofoldBlades);
                        break;
                    }
                case 65:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.RadiantPalm);
                        Add(PhoenixProject.Game.Enums.SkillIDs.WhirlWindKick);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TripleAttack);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Oblivion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Serenity);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Compassion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.DeflectionAura);
                        break;
                    }
                case 75:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.RadiantPalm);
                        Add(PhoenixProject.Game.Enums.SkillIDs.WhirlWindKick);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TripleAttack);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Oblivion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Serenity);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Compassion);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.TyrantAura);
                        Add(PhoenixProject.Game.Enums.SkillIDs.DeflectionAura);
                        break;
                    }
                case 135:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Thunder);
                        Add(PhoenixProject.Game.Enums.SkillIDs.WaterElf);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Cure);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Lightning);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Volcano);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Pray);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Stigma);
                        Add(PhoenixProject.Game.Enums.SkillIDs.AdvancedCure);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Meditation);
                        break;
                    }
                case 140:
                    {
                        Add(PhoenixProject.Game.Enums.SkillIDs.Thunder);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Cure);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Lightning);
                        Add(PhoenixProject.Game.Enums.SkillIDs.Tornado);
                        Add(PhoenixProject.Game.Enums.SkillIDs.FireCircle);
                        Add(PhoenixProject.Game.Enums.SkillIDs.FireMeteor);
                        Add(PhoenixProject.Game.Enums.SkillIDs.FireRing);
                        break;
                    }

            }

            //PreviousClass = client.Entity.FirstRebornClass;
            //toClass = client.Entity.SecondRebornClass;
            Add(PhoenixProject.Game.Enums.SkillIDs.Bless);

            Interfaces.ISkill[] aADD_spells = this.Addskill.Values.ToArray();
            foreach (Interfaces.ISkill skill in aADD_spells)
            {
                skill.Available = true;
                if (!client.Spells.ContainsKey(skill.ID))
                    client.Spells.Add(skill.ID, skill);
            }
            #region Spells
            Interfaces.ISkill[] aspells = client.Spells.Values.ToArray();
            foreach (Interfaces.ISkill aspell in spells)
            {
                aspell.PreviousLevel = aspell.Level;
                aspell.Level = 0;
                aspell.Experience = 0;
                #region Pirate
                if (PreviousClass == 75)
                {
                    if (client.Entity.Class != 71)
                    {
                        switch (aspell.ID)
                        {
                            case 10490:
                            case 10415:
                            case 10381:
                                client.RemoveSpell(aspell);
                                break;
                        }
                    }
                }
                #endregion
                #region Monk
                if (PreviousClass == 65)
                {
                    if (client.Entity.Class != 61)
                    {
                        switch (aspell.ID)
                        {
                            case 10490:
                            case 10415:
                            case 10381:
                                client.RemoveSpell(aspell);
                                break;
                        }
                    }
                }
                #endregion
                #region Warrior
                if (PreviousClass == 25)
                {
                    if (client.Entity.Class != 21)
                    {
                        switch (aspell.ID)
                        {
                            case 1025:
                                if (client.Entity.Class != 21 && client.Entity.Class != 132)
                                    client.RemoveSpell(aspell);
                                break;
                        }
                    }
                }
                #endregion
                #region Ninja
                if (toClass != 51)
                {
                    switch (aspell.ID)
                    {
                        case 6010:
                        case 6000:
                        case 6011:
                            client.RemoveSpell(aspell);
                            break;
                    }
                }
                #endregion
                #region Trojan
                if (toClass != 11)
                {
                    switch (aspell.ID)
                    {
                        case 1115:
                            client.RemoveSpell(aspell);
                            break;
                    }
                }
                #endregion
                #region Archer
                if (toClass != 41)
                {
                    switch (aspell.ID)
                    {
                        case 8001:
                        case 8000:
                        case 8003:
                        case 9000:
                        case 8002:
                        case 8030:
                            client.RemoveSpell(aspell);
                            break;
                    }
                }
                #endregion
                #region WaterTaoist
                if (PreviousClass == 135)
                {
                    if (toClass != 132)
                    {
                        switch (aspell.ID)
                        {
                            case 1000:
                            case 1001:
                            case 1010:
                            case 1125:
                            case 1100:
                            case 8030:
                                client.RemoveSpell(aspell);
                                break;
                            case 1050:
                            case 1175:
                            case 1170:
                                if (toClass != 142)
                                    client.RemoveSpell(aspell);
                                break;
                        }
                    }
                }
                #endregion
                #region FireTaoist
                if (PreviousClass == 145)
                {
                    if (toClass != 142)
                    {
                        switch (aspell.ID)
                        {
                            case 1000:
                            case 1001:
                            case 1150:
                            case 1180:
                            case 1120:
                            case 1002:
                            case 1160:
                            case 1165:
                                client.RemoveSpell(aspell);
                                break;
                        }
                    }
                }
                #endregion
                if (client.Spells.ContainsKey(aspell.ID))
                    if (aspell.ID != (ushort)Game.Enums.SkillIDs.Reflect)
                        aspell.Send(client);
            }
            #endregion
            Addskill.Clear();
            Addskill = new SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill>(20);
            #region Add Extra Skill
            if (client.Entity.FirstRebornClass == 15 && client.Entity.SecondRebornClass == 15 && client.Entity.Class == 11)
            {
                Add(PhoenixProject.Game.Enums.SkillIDs.DragonWhirl);
            }
            if (client.Entity.FirstRebornClass == 25 && client.Entity.SecondRebornClass == 25 && client.Entity.Class == 21)
            {
                Add(PhoenixProject.Game.Enums.SkillIDs.Perseverance);
            }
            if (client.Entity.FirstRebornClass == 45 && client.Entity.SecondRebornClass == 45 && client.Entity.Class == 41)
            {
                Add(PhoenixProject.Game.Enums.SkillIDs.StarArrow);
            }
            if (client.Entity.FirstRebornClass == 55 && client.Entity.SecondRebornClass == 55 && client.Entity.Class == 55)
            {
                Add(PhoenixProject.Game.Enums.SkillIDs.PoisonStar);
                Add(PhoenixProject.Game.Enums.SkillIDs.CounterKill);
            }
            if (client.Entity.FirstRebornClass == 65 && client.Entity.SecondRebornClass == 65 && client.Entity.Class == 61)
            {
                Add(PhoenixProject.Game.Enums.SkillIDs.SoulShackle);
            }
            if (client.Entity.FirstRebornClass == 135 && client.Entity.SecondRebornClass == 135 && client.Entity.Class == 132)
            {
                Add(PhoenixProject.Game.Enums.SkillIDs.AzureShield);
            }
            if (client.Entity.FirstRebornClass == 145 && client.Entity.SecondRebornClass == 145 && client.Entity.Class == 142)
            {
                Add(PhoenixProject.Game.Enums.SkillIDs.HeavenBlade);
            }
            #endregion
            Interfaces.ISkill[] aaADD_spells = this.Addskill.Values.ToArray();
            foreach (Interfaces.ISkill skill in aaADD_spells)
            {
                skill.Available = true;
                if (!client.Spells.ContainsKey(skill.ID))
                    client.Spells.Add(skill.ID, skill);
            }

            #region Proficiencies
            foreach (Interfaces.ISkill proficiency in client.Proficiencies.Values)
            {
                proficiency.PreviousLevel = proficiency.Level;
                proficiency.Level = 0;
                proficiency.Experience = 0;
                proficiency.Send(client);
            }
            #endregion
            Database.DataHolder.GetStats(client.Entity.Class, client.Entity.Level, client);
            client.CalculateStatBonus();
            client.CalculateHPBonus();
            client.GemAlgorithm();
            client.SendStatMessage();
            Network.PacketHandler.WorldMessage(client.Entity.Name + " has got Reincarnation! Congratulations!");

        }
        void Add(PhoenixProject.Game.Enums.SkillIDs S)
        {
            Interfaces.ISkill New = new Network.GamePackets.Spell(true);
            New.ID = (ushort)S;
            New.Level = 0;
            New.Experience = 0;
            New.PreviousLevel = 0;
            New.Send(_client);
            Addskill.Add(New.ID, New);
        }

        void WontAdd(PhoenixProject.Game.Enums.SkillIDs S)
        {
            Network.GamePackets.Data data = new Data(true);
            data.UID = _client.Entity.UID;
            data.dwParam = (byte)S;
            data.ID = 109;
            data.Send(_client);

            Interfaces.ISkill New = new Network.GamePackets.Spell(true);
            New.ID = (ushort)S;
            New.Level = 0;
            New.Experience = 0;
            New.PreviousLevel = 0;
            RemoveSkill.Add(New.ID, New);
        }
    }
    #endregion
    public class Reincarnate
    {
        public Game.ConquerStructures.Inventory Inventory;
        public Game.ConquerStructures.Equipment Equipment;
        public Entity Entity;
        public byte Class;
        public byte First;
        public byte Second;
        Interfaces.ISkill[] Skills;
        Interfaces.IProf[] Profs; 
        Dictionary<ushort, Interfaces.ISkill> Learn = null;
        Dictionary<ushort, Interfaces.ISkill> WontLearn = null;
        //Client.GameState[] States;

        public Reincarnate(Entity _Entity, byte _class)
        {
            Entity = _Entity;
            Class = _class;
            First = Entity.SecondRebornClass;
            Second = Entity.Class;
            Learn = new Dictionary<ushort, Interfaces.ISkill>();
            WontLearn = new Dictionary<ushort, Interfaces.ISkill>();
            Skills = new Interfaces.ISkill[Entity.Owner.Spells.Values.Count];
            Entity.Owner.Spells.Values.CopyTo(Skills, 0);
            Profs = new Interfaces.IProf[Entity.Owner.Proficiencies.Values.Count];
            Entity.Owner.Proficiencies.Values.CopyTo(Profs, 0);
            //States = new Client.GameState[Program.SafeReturn().Count];
            //Program.SafeReturn().Values.CopyTo(States, 0);
            doIt();
        }

        void doIt()
        {
            #region Reincarnate
            Database.ReincarnationTable.NewReincarnated(Entity);
            Game.Features.Reincarnation.ReincarnateInfo info = new Game.Features.Reincarnation.ReincarnateInfo();
            info.UID = Entity.UID;
            info.Level = Entity.Level;
            info.Experience = Entity.Experience;
            ServerBase.Kernel.ReincarnatedCharacters.Add(info.UID, info);
            Entity.FirstRebornClass = First;
            Entity.SecondRebornClass = Second;
            Entity.Class = Class;
            Entity.Atributes = 182;
            Entity.Level = 15;
            Entity.Experience = 0;
            #endregion
            #region Low level items
            for (byte i = 1; i < 9; i++)
            {
                if (i != 7)
                {
                    Interfaces.IConquerItem item = Entity.Owner.Equipment.TryGetItem(i);
                    if (item != null && item.ID != 0)
                    {
                        try
                        {
                            UnloadItemStats(item, false);
                            Database.ConquerItemInformation cii = new PhoenixProject.Database.ConquerItemInformation(item.ID, item.Plus);
                            item.ID = cii.LowestID(Network.PacketHandler.ItemMinLevel(Network.PacketHandler.ItemPosition(item.ID)));
                            item.Mode = PhoenixProject.Game.Enums.ItemMode.Update;
                            item.Send(Entity.Owner);
                            LoadItemStats(item);
                            Database.ConquerItemTable.UpdateItemID(item, Entity.Owner);
                        }
                        catch
                        {
                            Console.WriteLine("Reborn item problem: " + item.ID);
                        }
                    }
                }
            }
            Interfaces.IConquerItem hand = Entity.Owner.Equipment.TryGetItem(5);
            if (hand != null)
            {
                Entity.Owner.Equipment.Remove(5);
                CalculateStatBonus();
                CalculateHPBonus();
                Entity.Owner.Screen.Reload(null);
            }
            else
                Entity.Owner.SendScreen(Entity.Owner.Entity.SpawnPacket, false);
            #endregion

            foreach (Interfaces.ISkill s in Skills)
                Entity.Owner.Spells.Remove(s.ID);

            switch (First)
            {
                #region Trojan
                case 15:
                    {
                        switch (Second)
                        {
                            case 11:
                                Add(Enums.SkillIDs.CruelShade);
                                break;
                            default: WontAdd(Enums.SkillIDs.Accuracy); break;
                        }
                        break;
                    }
                #endregion
                #region Warrior
                case 25:
                    {
                        switch (Second)
                        {
                            case 11:
                                Add(Enums.SkillIDs.IronShirt);
                                WontAdd(Enums.SkillIDs.Shield);
                                WontAdd(Enums.SkillIDs.SuperMan);
                                break;
                            case 21:
                                Add(Enums.SkillIDs.Reflect);
                                break;
                            case 132:
                                WontAdd(Enums.SkillIDs.FlyingMoon);
                                WontAdd(Enums.SkillIDs.Shield);
                                break;
                            case 142:
                                WontAdd(Enums.SkillIDs.Accuracy);
                                WontAdd(Enums.SkillIDs.FlyingMoon);
                                WontAdd(Enums.SkillIDs.SuperMan);
                                break;
                            default:
                                WontAdd(Enums.SkillIDs.Accuracy);
                                WontAdd(Enums.SkillIDs.FlyingMoon);
                                WontAdd(Enums.SkillIDs.SuperMan);
                                WontAdd(Enums.SkillIDs.Shield);
                                break;
                        }
                        break;
                    }
                #endregion
                #region Archer
                case 45:
                    {
                        switch (Second)
                        {
                            case 41: break;
                            default:
                                WontAdd(Enums.SkillIDs.Scatter);
                                WontAdd(Enums.SkillIDs.XPFly);
                                WontAdd(Enums.SkillIDs.AdvancedFly);
                                WontAdd(Enums.SkillIDs.ArrowRain);
                                WontAdd(Enums.SkillIDs.Intensify);
                                WontAdd(Enums.SkillIDs.RapidFire);
                                break;
                        }
                        break;
                    }
                #endregion
                #region Ninja
                case 55:
                    {
                        switch (Second)
                        {
                            case 51: break;
                            default:
                                WontAdd(Enums.SkillIDs.PoisonStar);
                                WontAdd(Enums.SkillIDs.ShurikenVortex);
                                WontAdd(Enums.SkillIDs.FatalStrike);
                                WontAdd(Enums.SkillIDs.TwofoldBlades);
                                WontAdd(Enums.SkillIDs.ArcherBane);
                                break;
                        }
                        break;
                    }
                #endregion
                #region Monk
                case 65:
                    {
                        switch (Second)
                        {
                            case 61: break;
                            default:
                                WontAdd(Enums.SkillIDs.Oblivion);
                                WontAdd(Enums.SkillIDs.RadiantPalm);
                                WontAdd(Enums.SkillIDs.TyrantAura);
                                WontAdd(Enums.SkillIDs.DeathBlow);
                                WontAdd(Enums.SkillIDs.DeflectionAura);
                                WontAdd(Enums.SkillIDs.TripleAttack);
                                break;
                        }
                        break;
                    }
                #endregion
                #region Water
                case 135:
                    {
                        switch (Second)
                        {
                            case 132: Add(Enums.SkillIDs.Pervade); break;
                            case 142:
                                WontAdd(Enums.SkillIDs.Nectar);
                                WontAdd(Enums.SkillIDs.HealingRain);
                                break;
                            default:
                                WontAdd(Enums.SkillIDs.Nectar);
                                WontAdd(Enums.SkillIDs.Lightning);
                                WontAdd(Enums.SkillIDs.Volcano);
                                WontAdd(Enums.SkillIDs.AdvancedCure);
                                WontAdd(Enums.SkillIDs.SpeedLightning);
                                WontAdd(Enums.SkillIDs.HealingRain);
                                break;
                        }
                        break;
                    }
                #endregion
                #region Fire
                case 145:
                    {
                        switch (Second)
                        {
                            case 142: Add(Enums.SkillIDs.Dodge); break;
                            default:
                                if (Second != 132)
                                    WontAdd(Enums.SkillIDs.FireCircle);

                                WontAdd(Enums.SkillIDs.Tornado);
                                WontAdd(Enums.SkillIDs.FireMeteor);
                                WontAdd(Enums.SkillIDs.FireOfHell);
                                WontAdd(Enums.SkillIDs.FireRing);
                                WontAdd(Enums.SkillIDs.Volcano);
                                WontAdd(Enums.SkillIDs.Lightning);
                                WontAdd(Enums.SkillIDs.SpeedLightning);
                                break;
                        }
                        break;
                    }
                #endregion
            }


            Add(Enums.SkillIDs.Bless);

            #region Re-Learn Profs
            foreach (Interfaces.IProf Prof in Profs)
            {
                if (Prof == null)
                    continue;

                Prof.Available = false;
                Prof.PreviousLevel = Prof.Level;
                Prof.Level = 0;
                Prof.Experience = 0;
                Entity.Owner.Proficiencies.Add(Prof.ID, Prof);
                Prof.Send(Entity.Owner);
            }
            #endregion
            #region Re-Learn Skills
            foreach (Interfaces.ISkill Skill in Skills)
            {
                if (Skill == null)
                    continue;

                Skill.Available = false;
                Skill.PreviousLevel = Skill.Level;
                Skill.Level = 0;
                Skill.Experience = 0;

                if (!WontLearn.ContainsKey(Skill.ID))
                {
                    Entity.Owner.Spells.Add(Skill.ID, Skill);
                    Skill.Send(Entity.Owner);
                }
            }
            #endregion
            #region Learn Skills
            foreach (Interfaces.ISkill L in Learn.Values)
            {
                if (L == null)
                    continue;

                L.Available = false;
                Entity.Owner.Spells.Add(L.ID, L);
                L.Send(Entity.Owner);
            }
            #endregion
            #region Remove Skills
            foreach (Interfaces.ISkill L in WontLearn.Values)
            {
                if (L == null)
                    continue;

                L.Available = false;
                Entity.Owner.Spells.Remove(L.ID);
                Database.SkillTable.DeleteSpell(Entity.Owner, L.ID);
                L.Send(Entity.Owner);
            }
            #endregion


            Database.DataHolder.GetStats(Entity.Class, Entity.Level, Entity.Owner);
            Entity.Owner.CalculateStatBonus();
            Entity.Owner.CalculateHPBonus();
          
                Database.SkillTable.SaveProficiencies(Entity.Owner);
                Database.SkillTable.SaveSpells(Entity.Owner);
           
            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulations, " + Entity.Name + " reincarnated!", System.Drawing.Color.White, Network.GamePackets.Message.TopLeft), ServerBase.Kernel.GamePool.Values);
        }

        void Add(Enums.SkillIDs S)
        {
            Interfaces.ISkill New = new Network.GamePackets.Spell(true);
            New.ID = (ushort)S;
            New.Level = 0;
            New.Experience = 0;
            New.PreviousLevel = 0;
            Learn.Add(New.ID, New);
        }

        void WontAdd(Enums.SkillIDs S)
        {
            Interfaces.ISkill New = new Network.GamePackets.Spell(true);
            New.ID = (ushort)S;
            New.Level = 0;
            New.Experience = 0;
            New.PreviousLevel = 0;
            WontLearn.Add(New.ID, New);
        }
        void WontAdd2(Enums.SkillIDs S)
        {
            Interfaces.ISkill New = new Network.GamePackets.Spell(true);
            New.ID = (ushort)S;
            New.Level = 0;
            New.Experience = 0;
            New.PreviousLevel = 0;
            WontLearn.Add(New.ID, New);
        }
        private int StatHP;
        public void CalculateHPBonus()
        {
            switch (Entity.Class)
            {
                case 11: Entity.MaxHitpoints = (uint)(StatHP * 1.05F); break;
                case 12: Entity.MaxHitpoints = (uint)(StatHP * 1.08F); break;
                case 13: Entity.MaxHitpoints = (uint)(StatHP * 1.10F); break;
                case 14: Entity.MaxHitpoints = (uint)(StatHP * 1.12F); break;
                case 15: Entity.MaxHitpoints = (uint)(StatHP * 1.15F); break;
                default: Entity.MaxHitpoints = (uint)StatHP; break;
            }
            Entity.MaxHitpoints += Entity.ItemHP;
            Entity.Hitpoints = Math.Min(Entity.Hitpoints, Entity.MaxHitpoints);
        }
        public void CalculateStatBonus()
        {
            byte ManaBoost = 5;
            const byte HitpointBoost = 24;
            sbyte Class = (sbyte)(Entity.Class / 10);
            if (Class == 13 || Class == 14)
                ManaBoost += (byte)(5 * (Entity.Class - (Class * 10)));
            StatHP = (ushort)((Entity.Strength * 3) +
                                     (Entity.Agility * 3) +
                                     (Entity.Spirit * 3) +
                                     (Entity.Vitality * HitpointBoost));
            Entity.MaxMana = (ushort)((Entity.Spirit * ManaBoost) + Entity.ItemMP);
            Entity.Mana = Math.Min(Entity.Mana, Entity.MaxMana);
        }
        public void LoadItemStats(Interfaces.IConquerItem item)
        {
            if (item == null)
                return;
            if (item.Durability == 0)
                return;
            if (item.Position == ConquerItem.Garment)
                return;
            Database.ConquerItemInformation Infos = new PhoenixProject.Database.ConquerItemInformation(item.ID, item.Plus);

            if (Infos.BaseInformation == null)
                return;

            if (item.Position == ConquerItem.Tower)
            {
                Entity.PhysicalDamageDecrease += Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDamageDecrease += Infos.BaseInformation.MagicDefence;
            }
            else
            {
                Entity.Defence += Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDefencePercent += Infos.BaseInformation.MagicDefence;
                Entity.Dodge += (byte)Infos.BaseInformation.Dodge;
                if (item.Position != ConquerItem.Fan)
                    Entity.BaseMagicAttack += Infos.BaseInformation.MagicAttack;
            }
            Entity.ItemHP += Infos.BaseInformation.ItemHP;
            Entity.ItemMP += Infos.BaseInformation.ItemMP;
            Entity.ItemBless += item.Bless;
            if (item.Position == ConquerItem.RightWeapon)
            {
                Entity.AttackRange += Infos.BaseInformation.AttackRange;
                if (Network.PacketHandler.IsTwoHand(Infos.BaseInformation.ID))
                    Entity.AttackRange += 3;
                else
                {
                    Entity.AttackRange += 2;
                }
            }
            if (item.Position == ConquerItem.LeftWeapon)
            {
                Entity.BaseMinAttack += (uint)(Infos.BaseInformation.MinAttack * 0.5F);
                Entity.BaseMaxAttack += (uint)(Infos.BaseInformation.MaxAttack * 0.5F);
            }
            else if (item.Position == ConquerItem.Fan)
            {
                Entity.PhysicalDamageIncrease += Infos.BaseInformation.MinAttack;
                Entity.MagicDamageIncrease += Infos.BaseInformation.MagicAttack;
            }
            else
            {
                Entity.BaseMinAttack += Infos.BaseInformation.MinAttack;
                Entity.BaseMaxAttack += Infos.BaseInformation.MaxAttack;
            }
            if (item.Plus != 0)
            {
                if (item.Position == ConquerItem.Tower)
                {
                    Entity.PhysicalDamageDecrease += Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDamageDecrease += (ushort)Infos.PlusInformation.MagicDefence;
                }
                else if (item.Position == ConquerItem.Fan)
                {
                    Entity.PhysicalDamageIncrease += (ushort)Infos.PlusInformation.MinAttack;
                    Entity.MagicDamageIncrease += (ushort)Infos.PlusInformation.MagicAttack;
                }
                else
                {
                    if (item.Position == ConquerItem.Steed)
                        Entity.ExtraVigor += Infos.PlusInformation.Agility;
                    Entity.BaseMinAttack += Infos.PlusInformation.MinAttack;
                    Entity.BaseMaxAttack += Infos.PlusInformation.MaxAttack;
                    Entity.BaseMagicAttack += Infos.PlusInformation.MagicAttack;
                    Entity.Defence += Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDefence += Infos.PlusInformation.MagicDefence;
                    Entity.ItemHP += Infos.PlusInformation.ItemHP;
                    if (item.Position == ConquerItem.Boots)
                        Entity.Dodge += (byte)Infos.PlusInformation.Dodge;
                }
            }
            byte socketone = (byte)item.SocketOne;
            byte sockettwo = (byte)item.SocketTwo;
            ushort madd = 0, dadd = 0, aatk = 0, matk = 0;
            if (item.Position != ConquerItem.Garment &&
                item.Position != ConquerItem.Bottle &&
                item.Position != ConquerItem.Steed)
                switch (socketone)
                {
                    case 1: Entity.PhoenixGem += 5; break;
                    case 2: Entity.PhoenixGem += 10; break;
                    case 3: Entity.PhoenixGem += 15; break;

                    case 11: Entity.DragonGem += 5; break;
                    case 12: Entity.DragonGem += 10; break;
                    case 13: Entity.DragonGem += 15; break;

                    case 71: Entity.TortisGem += 15; break;
                    case 72: Entity.TortisGem += 30; break;
                    case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            if (item.Position != ConquerItem.Garment &&
                 item.Position != ConquerItem.Bottle &&
                 item.Position != ConquerItem.Steed)
                switch (sockettwo)
                {
                    case 1: Entity.PhoenixGem += 5; break;
                    case 2: Entity.PhoenixGem += 10; break;
                    case 3: Entity.PhoenixGem += 15; break;

                    case 11: Entity.DragonGem += 5; break;
                    case 12: Entity.DragonGem += 10; break;
                    case 13: Entity.DragonGem += 15; break;

                    case 71: Entity.TortisGem += 15; break;
                    case 72: Entity.TortisGem += 30; break;
                    case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            Entity.PhysicalDamageDecrease += dadd;
            Entity.MagicDamageDecrease += madd;
            Entity.PhysicalDamageIncrease += aatk;
            Entity.MagicDamageIncrease += matk;
            if (item.Position != ConquerItem.Garment &&
               item.Position != ConquerItem.Bottle)
            {
                Entity.ItemHP += item.Enchant;
                GemAlgorithm();
            }
        }
        public void UnloadItemStats(Interfaces.IConquerItem item, bool onPurpose)
        {
            if (item == null) return;

            if (item.Durability == 0 && !onPurpose)
                return;
            if (item.Position == ConquerItem.Garment)
                return;
            Database.ConquerItemInformation Infos = new PhoenixProject.Database.ConquerItemInformation(item.ID, item.Plus);
            if (Infos.BaseInformation == null)
                return;

            if (item.Position == ConquerItem.Tower)
            {
                Entity.PhysicalDamageDecrease -= Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDamageDecrease -= Infos.BaseInformation.MagicDefence;
            }
            else
            {
                Entity.Defence -= Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDefencePercent -= Infos.BaseInformation.MagicDefence;
                Entity.Dodge -= (byte)Infos.BaseInformation.Dodge;
                if (item.Position != ConquerItem.Fan)
                    Entity.BaseMagicAttack -= Infos.BaseInformation.MagicAttack;
            }

            Entity.ItemHP -= Infos.BaseInformation.ItemHP;
            Entity.ItemMP -= Infos.BaseInformation.ItemMP;
            Entity.ItemBless -= item.Bless;
            if (item.Position == ConquerItem.RightWeapon)
            {
                Entity.AttackRange -= Infos.BaseInformation.AttackRange;
                if (Network.PacketHandler.IsTwoHand(Infos.BaseInformation.ID))
                    Entity.AttackRange -= 2;
            }
            if (item.Position == ConquerItem.LeftWeapon)
            {
                Entity.BaseMinAttack -= (uint)(Infos.BaseInformation.MinAttack * 0.5F);
                Entity.BaseMaxAttack -= (uint)(Infos.BaseInformation.MaxAttack * 0.5F);
            }
            else if (item.Position == ConquerItem.Fan)
            {
                Entity.PhysicalDamageIncrease -= Infos.BaseInformation.MinAttack;
                Entity.MagicDamageIncrease -= Infos.BaseInformation.MagicAttack;
            }
            else
            {
                Entity.BaseMinAttack -= Infos.BaseInformation.MinAttack;
                Entity.BaseMaxAttack -= Infos.BaseInformation.MaxAttack;
            }
            if (item.Plus != 0)
            {
                if (item.Position == ConquerItem.Tower)
                {
                    Entity.PhysicalDamageDecrease -= Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDamageDecrease -= (ushort)Infos.PlusInformation.MagicDefence;
                }
                else if (item.Position == ConquerItem.Fan)
                {
                    Entity.PhysicalDamageIncrease -= (ushort)Infos.PlusInformation.MinAttack;
                    Entity.MagicDamageIncrease -= (ushort)Infos.PlusInformation.MagicAttack;
                }
                else
                {
                    if (item.Position == ConquerItem.Steed)
                        Entity.ExtraVigor -= Infos.PlusInformation.Agility;
                    Entity.BaseMinAttack -= Infos.PlusInformation.MinAttack;
                    Entity.BaseMaxAttack -= Infos.PlusInformation.MaxAttack;
                    Entity.BaseMagicAttack -= Infos.PlusInformation.MagicAttack;
                    Entity.Defence -= Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDefence -= Infos.PlusInformation.MagicDefence;
                    Entity.ItemHP -= Infos.PlusInformation.ItemHP;
                    if (item.Position == ConquerItem.Boots)
                        Entity.Dodge -= (byte)Infos.PlusInformation.Dodge;
                }
            }
            byte socketone = (byte)item.SocketOne;
            byte sockettwo = (byte)item.SocketTwo;
            ushort madd = 0, dadd = 0, aatk = 0, matk = 0;
            if (item.Position != ConquerItem.Garment &&
                item.Position != ConquerItem.Bottle &&
                item.Position != ConquerItem.Steed)
                switch (socketone)
                {
                    case 1: Entity.PhoenixGem += 5; break;
                    case 2: Entity.PhoenixGem += 10; break;
                    case 3: Entity.PhoenixGem += 15; break;

                    case 11: Entity.DragonGem += 5; break;
                    case 12: Entity.DragonGem += 10; break;
                    case 13: Entity.DragonGem += 15; break;

                    case 71: Entity.TortisGem += 15; break;
                    case 72: Entity.TortisGem += 30; break;
                    case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            if (item.Position != ConquerItem.Garment &&
                 item.Position != ConquerItem.Bottle &&
                 item.Position != ConquerItem.Steed)
                switch (sockettwo)
                {
                    case 1: Entity.PhoenixGem += 5; break;
                    case 2: Entity.PhoenixGem += 10; break;
                    case 3: Entity.PhoenixGem += 15; break;

                    case 11: Entity.DragonGem += 5; break;
                    case 12: Entity.DragonGem += 10; break;
                    case 13: Entity.DragonGem += 15; break;

                    case 71: Entity.TortisGem += 15; break;
                    case 72: Entity.TortisGem += 30; break;
                    case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            Entity.PhysicalDamageDecrease -= dadd;
            Entity.MagicDamageDecrease -= madd;
            Entity.PhysicalDamageIncrease -= aatk;
            Entity.MagicDamageIncrease -= matk;
            if (item.Position != ConquerItem.Garment &&
                item.Position != ConquerItem.Bottle)
            {
                Entity.ItemHP -= item.Enchant;
                GemAlgorithm();
            }
        }
        public void LoadSoulStats(uint ID)
        {
            var Infos = PhoenixProject.Database.ConquerItemInformation.BaseInformations[ID];

            Entity.Defence += Infos.PhysicalDefence;
            Entity.MagicDefence += Infos.MagicDefence;
            Entity.Dodge += (byte)Infos.Dodge;
            Entity.BaseMagicAttack += Infos.MagicAttack;
            Entity.BaseMinAttack += Infos.MinAttack;
            Entity.BaseMaxAttack += Infos.MaxAttack;
        }
        public void UnloadSoulStats(uint ID)
        {
            var Infos = PhoenixProject.Database.ConquerItemInformation.BaseInformations[ID];

            Entity.Defence -= Infos.PhysicalDefence;
            Entity.MagicDefence -= Infos.MagicDefence;
            Entity.Dodge -= (byte)Infos.Dodge;
            Entity.BaseMagicAttack -= Infos.MagicAttack;
            Entity.BaseMinAttack -= Infos.MinAttack;
            Entity.BaseMaxAttack -= Infos.MaxAttack;
        }
        public void GemAlgorithm()
        {
            Entity.MaxAttack = Entity.Strength + Entity.BaseMaxAttack;
            Entity.MinAttack = Entity.Strength + Entity.BaseMinAttack;
            Entity.MagicAttack = Entity.BaseMagicAttack;
            if (Entity.PhoenixGem != 0)
            {
                Entity.MagicAttack += (uint)Math.Floor(Entity.MagicAttack * (double)(Entity.PhoenixGem * 0.01));
            }
            if (Entity.DragonGem != 0)
            {
                Entity.MaxAttack += (uint)Math.Floor(Entity.MaxAttack * (double)(Entity.DragonGem * 0.01));
                Entity.MinAttack += (uint)Math.Floor(Entity.MinAttack * (double)(Entity.DragonGem * 0.01));
            }
            /*if (Entity.TortisGem != 0)
            {
                Entity.Defence += (ushort)Math.Floor(Entity.Defence * (double)(Entity.TortisGem * 0.01));
                Entity.MagicDefence += (ushort)Math.Floor(Entity.MagicDefence * (double)(Entity.TortisGem * 0.01));
            }*/
        }
    }
}
