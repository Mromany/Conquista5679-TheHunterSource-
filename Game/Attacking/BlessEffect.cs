using System;
using System.Linq;
using System.Text;
using PhoenixProject.ServerBase;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game.Attacking
{
    public class BlessEffect
    {
        public static void Effect(Entity client)
        {
            byte bless1 = 0;
            byte bless3 = 0;
            byte bless5 = 0;
            byte bless7 = 0;
            for (byte i = 1; i < 12; i++)
            {

                if (i != ConquerItem.Bottle)
                {
                    Interfaces.IConquerItem item = client.Owner.Equipment.TryGetItem(i);
                    if (item != null && item.ID != 0)
                    {
                        if (item.Bless == 1)
                            bless1++;
                        if (item.Bless == 3)
                            bless3++;
                        if (item.Bless == 5)
                            bless5++;
                        if (item.Bless == 7)
                            bless7++;
                    }
                }
            }
            #region Effect Bless 7
            if (bless7 >= bless5 && bless7 >= bless3 && bless7 >= bless1)
            {
                if (bless7 > 0)
                {
                    if (Kernel.Rate(5, 1000))
                    {
                        _String str = new _String(true);
                        str.UID = client.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("Aegis4");
                        client.Owner.SendScreen(str, true);

                    }
                }
            }
            #endregion
            #region Effect Bless 5
            else if (bless5 > bless7 && bless5 >= bless3 && bless5 >= bless1)
            {
                if (bless5 > 0)
                {
                    if (Kernel.Rate(5, 1000))
                    {
                        _String str = new _String(true);
                        str.UID = client.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("Aegis3");
                        client.Owner.SendScreen(str, true);

                    }
                }
                goto jump;
            }
            #endregion
            #region Effect Bless 3
            else if (bless3 >= bless1 && bless3 > bless7 && bless3 > bless5)
            {
                if (bless3 > 0)
                {
                    if (Kernel.Rate(5, 1000))
                    {
                        _String str = new _String(true);
                        str.UID = client.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("Aegis2");
                        client.Owner.SendScreen(str, true);

                    }
                }
                goto jump;
            }
            #endregion
            #region Effect Bless 1
            else if (bless1 > bless7 && bless1 > bless5 && bless1 > bless3)
            {
                if (bless1 > 0)
                {
                    if (Kernel.Rate(5, 1000))
                    {
                        _String str = new _String(true);
                        str.UID = client.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("Aegis1");
                        client.Owner.SendScreen(str, true);

                    }
                }
                goto jump;
            }
            #endregion
        jump:
            return;
        }
    }
}