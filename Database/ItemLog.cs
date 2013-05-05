using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PhoenixProject.Database
{
    public class ItemLog
    {
        public enum ItemLogAction
        {
            Add = 1,
            Remove = 2
        }
       /* public static void CleanUp()
        {
            #region Items
            ItemCollection items = new ItemCollection();
            items.LoadAndCloseReader(Item.FetchAll());

            using (var conn = DataHolder.MySqlConnection)
            {
                conn.Open();
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].EntityID == 0)
                    {
                        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("DELETE FROM `items` WHERE `EntityID` = 0 AND `Id` = " + items[i].Id, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            //for (int i = 0; i < items.Count; i++)
            //{
            //    if (items[i].EntityID == 0)
            //        new MySql.Data.MySqlClient.MySqlCommand("DELETE * FROM `items` WHERE `Id` = " + items[i].Id, DataHolder.MySqlConnection).ExecuteNonQuery() ;

            //}
            #endregion
        }*/
        public static void LogItem(uint uid, uint param1, ItemLogAction action)
        {
            //DateTime Date = DateTime.Now;
            //MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            //cmd.Insert("itemlog").Insert("itemuid", uid).Insert("param1", param1).Insert("action", (byte)action)
            //    .Insert("datestring", Date.ToString()).Insert("datebinary", Date.ToBinary()).Execute();
        }
    }
}
