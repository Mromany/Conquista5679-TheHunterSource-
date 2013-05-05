using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public class QdAction
    {
        public bool Bool;
        public byte[] Data;
        public List<object> Obj;
        public QdActionType Type;

        public QdAction()
        {
            this.Obj = new List<object>();
        }

        public QdAction(QdActionType t)
        {
            this.Obj = new List<object>();
            this.Type = t;
        }

        public QdAction(QdActionType t, params object[] objs)
        {
            this.Obj = new List<object>();
            this.Type = t;
            foreach (object obj2 in objs)
            {
                this.Obj.Add(obj2);
            }
        }

        public QdAction(QdActionType t, byte[] d, params object[] objs)
        {
            this.Obj = new List<object>();
            this.Type = t;
            this.Data = new byte[d.Length];
            d.CopyTo(this.Data, 0);
            foreach (object obj2 in objs)
            {
                this.Obj.Add(obj2);
            }
        }

        public QdAction(QdActionType t, byte[] d, bool bo = false)
        {
            this.Obj = new List<object>();
            this.Type = t;
            this.Data = new byte[d.Length];
            d.CopyTo(this.Data, 0);
            this.Bool = bo;
        }
    }
}
