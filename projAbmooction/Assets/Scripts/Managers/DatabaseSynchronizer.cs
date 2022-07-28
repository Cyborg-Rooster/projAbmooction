using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DatabaseSynchronizer
{
    private static readonly int Version = 1;
    public static void Synch()
    {
        #region "Crete"
        SQLiteManager.RunQuery(CommonQuery.Create("VERSION", "DATABASE_VERSION INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("GAME_DATA", "COINS INTEGER, SKIN INTEGER, SCENARIO INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("STATISTIC", "BEST_SCORE INTEGER, DEATHS INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("OPTIONS", "SOUND INTEGER, LANGUAGE INTEGER"));

        SQLiteManager.RunQuery(CommonQuery.Create("SKINS", "SKIN_ID VARCHAR(55)"));
        SQLiteManager.RunQuery(CommonQuery.Create("SCENARIOS", "SCENARIO_ID INTEGER, QUANTITY INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("ITEMS", "ITEM_ID INTEGER, LEVEL INTEGER, PRICE INTEGER, TIME INTEGER"));
        #endregion

        #region "Add"
        SQLiteManager.RunQuery(CommonQuery.Add("VERSION", "DATABASE_VERSION", Version.ToString()));
        SQLiteManager.RunQuery
        (
            CommonQuery.Add("GAME_DATA", "COINS, SKIN, SCENARIO", 
            $"{GameData.Coins}, {GameData.Skin}, {GameData.Scenario}"
        ));
        SQLiteManager.RunQuery
        (
            CommonQuery.Add("STATISTIC", "BEST_SCORE, DEATHS", 
            $"{GameData.BestScore}, {GameData.Deaths}"
        ));
        SQLiteManager.RunQuery
        (
            CommonQuery.Add("OPTIONS", "SOUND, LANGUAGE", 
            $"{GameData.GetSound()}, {(int)GameData.Language}"
        ));
        SQLiteManager.RunQuery(CommonQuery.Add("SKINS", "SKIN_ID", "'0'"));

        SQLiteManager.RunQuery(CommonQuery.Add("SCENARIOS", "SCENARIO_ID, QUANTITY", "0, 0"));
        SQLiteManager.RunQuery(CommonQuery.Add("SCENARIOS", "SCENARIO_ID, QUANTITY", "1, 0"));
        SQLiteManager.RunQuery(CommonQuery.Add("SCENARIOS", "SCENARIO_ID, QUANTITY", "2, 0"));
        SQLiteManager.RunQuery(CommonQuery.Add("SCENARIOS", "SCENARIO_ID, QUANTITY", "3, 0"));
        SQLiteManager.RunQuery(CommonQuery.Add("SCENARIOS", "SCENARIO_ID, QUANTITY", "4, 0"));
        SQLiteManager.RunQuery(CommonQuery.Add("SCENARIOS", "SCENARIO_ID, QUANTITY", "5, 0"));

        SQLiteManager.RunQuery(CommonQuery.Add("ITEMS", "ITEM_ID, LEVEL, PRICE, TIME", "0, 0, 500, 5"));
        SQLiteManager.RunQuery(CommonQuery.Add("ITEMS", "ITEM_ID, LEVEL, PRICE, TIME", "1, 0, 500, 5"));
        SQLiteManager.RunQuery(CommonQuery.Add("ITEMS", "ITEM_ID, LEVEL, PRICE, TIME", "2, 0, 500, 5"));
        SQLiteManager.RunQuery(CommonQuery.Add("ITEMS", "ITEM_ID, LEVEL, PRICE, TIME", "3, 0, 500, 5"));

        #endregion
    }
}
