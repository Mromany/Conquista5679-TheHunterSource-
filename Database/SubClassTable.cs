using System;
using PhoenixProject.Game;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Database
{
    public class SubClassTable
    {

        public static void Load(Entity Entity)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.SELECT);
            Command.Select("subclasses").Where("id", Entity.UID);
            MySqlReader Reader = new MySqlReader(Command);
            while (Reader.Read())
            {
                Statement.SubClass Sub = new Statement.SubClass();
                Sub.ID = Reader.ReadByte("uid");
                Sub.Level = Reader.ReadByte("level");
                Sub.Phase = Reader.ReadByte("phase");
                Entity.SubClasses.Classes.Add(Sub.ID, Sub);

                Game_SubClass packet = new Game_SubClass();
                packet.ClassId = (Game_SubClass.ID)Sub.ID;
                packet.Phase = Sub.Phase;
                packet.Type = Game_SubClass.Types.Learn;
                Entity.Owner.Send(packet);
                packet.Type = Game_SubClass.Types.MartialPromoted;
                Entity.Owner.Send(packet);
            }
            Reader.Close();
            Reader.Dispose();
           
        }
       /* public static void save(Entity Entity, Statement.SubClass SubClass)
        {

             MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("subclasses")
                .Set("phase", SubClass.Phase)
                .Set("level", SubClass.Level)
                .Where("id", Entity.UID)
                .And("uid", SubClass.ID)
                .Execute();

          
        }*/

        public static bool Contains(Entity Entity, byte id)
        {
            bool Return = false;
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.SELECT);
            Command.Select("subclasses").Where("id", Entity.UID).And("uid", id);
            PhoenixProject.Database.MySqlReader Reader = new PhoenixProject.Database.MySqlReader(Command);
            if (Reader.Read())
            {
                if (Reader.ReadByte("uid") == id)
                    Return = true;
            }

            return Return;
        }

        public static void Insert(Entity Entity, byte id)
        {
            Statement.SubClass Sub = new Statement.SubClass();
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.INSERT);
            Command.Insert("subclasses")
                .Insert("uid", id)
                .Insert("id", Entity.UID)
                .Execute();
        }
        
        public static void Update(Game.Entity Entity, Statement.SubClass SubClass)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("subclasses")
                .Set("phase", SubClass.Phase)
                .Set("level", SubClass.Level)
                .Where("id", Entity.UID)
                .And("uid", SubClass.ID)
                .Execute();
        }

        public static void Update(Client.GameState client)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("entities")
                .Set("StudyPoints", client.Entity.SubClasses.StudyPoints)
                .Where("UID", client.Entity.UID)
                .Execute();
        }
    }
}
