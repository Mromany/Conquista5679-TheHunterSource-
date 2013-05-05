using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    public class DMapsTablesss
    {
        public struct DMapInformation
        {
            public long ID;
            public long BaseID;
            public uint Status;
            public uint Weather;
            public uint Owner;
            public uint HouseLevel;
        }
        public static SafeDictionary<ulong, DMapInformation> HouseInfo = new SafeDictionary<ulong, DMapInformation>(280);
        
    }
}