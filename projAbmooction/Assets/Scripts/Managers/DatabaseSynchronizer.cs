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
        SQLiteManager.RunQuery(CommonQuery.Create("VERSION", "DATABASE_VERSION INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("GAME_DATA", "COINS INTEGER, SKIN INTEGER, SCENARIO INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("STATISTIC", "BEST_SCORE INTEGER, DEATHS INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("OPTIONS", "SOUND INTEGER, LANGUAGE INTEGER"));
        SQLiteManager.RunQuery(CommonQuery.Create("SKINS", "SKIN_ID VARCHAR(55)"));

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
        SQLiteManager.RunQuery(CommonQuery.Add("SKINS", "SKIN_ID", "'0 1'"));
    }
}
