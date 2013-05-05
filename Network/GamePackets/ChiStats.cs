using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class ChiSystem2
    {

        public ChiSystem2()
        {
        }

        public static void Handle(byte[] Data, Client.GameState Client)
        {
           
                ChiSystem chiSystem = new ChiSystem(Data);
               /* Console.WriteLine(chiSystem.Identifier);
                Console.WriteLine(chiSystem.Unknown1);
                Console.WriteLine(chiSystem.Type);
                Console.WriteLine(chiSystem.StudyFlag);*/
               // Console.WriteLine(chiSystem.Unknown2);
               // chiSystem.Unknown1 = 1;
               // chiSystem.StudyFlag = 1;
                //chiSystem.Unknown2 = 1;
                switch (chiSystem.Type)
                {
                    case ChiSystemType.Open:
                        ChiStats chiStats = new ChiStats()
                        {
                            Identifier = Client.Entity.UID,
                            ChiPoints = Client.Entity.ChiPoints,
                            Unknown2 = 13,
                            Unknown3 = 1,
                            ChiGate = ChiGate.Dragon,
                            Val1 = 1000,
                            Val2 = 2000,
                            Val3 = 3000,
                            Val4 = 4000
                        };
                        ChiStats chiStats2 = new ChiStats()
                        {
                            Identifier = Client.Entity.UID,
                            ChiPoints = Client.Entity.ChiPoints,
                            Unknown2 = 13,
                            Unknown3 = 1,
                            ChiGate = ChiGate.Phoneix,
                            Val1 = 1000,
                            Val2 = 2000,
                            Val3 = 3000,
                            Val4 = 4000
                        };
                        ChiStats chiStats3 = new ChiStats()
                        {
                            Identifier = Client.Entity.UID,
                            ChiPoints = Client.Entity.ChiPoints,
                            Unknown2 = 13,
                            Unknown3 = 1,
                            ChiGate = ChiGate.Tiger,
                            Val1 = 1000,
                            Val2 = 2000,
                            Val3 = 3000,
                            Val4 = 4000
                        };
                        ChiStats chiStats4 = new ChiStats()
                        {
                            Identifier = Client.Entity.UID,
                            ChiPoints = Client.Entity.ChiPoints,
                            Unknown2 = 13,
                            Unknown3 = 1,
                            ChiGate = ChiGate.Turtle,
                            Val1 = 1000,
                            Val2 = 2000,
                            Val3 = 3000,
                            Val4 = 4000
                        };
                        Client.Send(chiStats);
                        Client.Send(chiStats2);
                        Client.Send(chiStats3);
                        Client.Send(chiStats4);
                        break;

                    case ChiSystemType.CPFillChi:
                        uint ui1 = 2000;
                        uint ui2 = (uint)Math.Floor((double)(1000.0F - (1000.0F * ((float)ui1 / 4000.0F))));
                       // Client.Entity.ChiPoints += 50;
                       // Client.Entity.ConquerPoints -= ui2;
                       // Console.WriteLine("qq "+ui2+"");
                       // Console.WriteLine("pp "+chiSystem.Unknown1+" ");
                        break;

                    default:
                        Console.WriteLine(String.Concat("Unhandled ChiSystem Type: ", chiSystem.Type));
                        break;
            }
        }

    } // class ChiSystem
}
