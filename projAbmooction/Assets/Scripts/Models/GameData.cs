using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GameData
{
    public static int Coins = 0;
    public static int Skin = 0;
    public static int Scenario = 0;

    public static int BestScore = 0;
    public static int Deaths = 0;

    public static Item Doubled;
    public static Item Magnetic;
    public static Item Shield;
    public static Item SlowMotion;

    public static Languages Language = Languages.Portuguese;
    private static bool Sound = true;

    public static bool IsOnline = false;
    public static DateTime DateTimeNow;

    public static int GetSound()
    {
        return Sound ? 1 : 0;
    }

    public static void SetSound(int sound)
    {
        Sound = sound == 1 ? true : false;
    }

    public static void Load()
    {
        int language = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("LANGUAGE", "OPTIONS"));

        Coins = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("COINS", "GAME_DATA"));
        Skin = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("SKIN", "GAME_DATA"));
        Scenario = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("SCENARIO", "GAME_DATA"));

        BestScore = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("BEST_SCORE", "STATISTIC"));
        Deaths = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("DEATHS", "STATISTIC"));

        if(language == 0) Language = Languages.Portuguese;
        else if (language == 1) Language = Languages.English;
        else Language = Languages.Español;

        SetSound(SQLiteManager.ReturnValueAsInt(CommonQuery.Select("SOUND", "OPTIONS")));

        Doubled = new Item
        (
            0,
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("LEVEL", "ITEMS", "ITEM_ID = 0")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("PRICE", "ITEMS", "ITEM_ID = 0")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("TIME", "ITEMS", "ITEM_ID = 0"))
        );

        SetItems();
    }

    public static void Save()
    {
        SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {Coins}", "COINS = COINS"));
        SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"SKIN = {Skin}", "SKIN = SKIN"));
        SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"SCENARIO = {Scenario}", "SCENARIO = SCENARIO"));

        SQLiteManager.RunQuery(CommonQuery.Update("STATISTIC", $"BEST_SCORE = {BestScore}", "BEST_SCORE = BEST_SCORE"));
        SQLiteManager.RunQuery(CommonQuery.Update("STATISTIC", $"DEATHS = {Deaths}", "DEATHS = DEATHS"));

        SQLiteManager.RunQuery(CommonQuery.Update("OPTIONS", $"SOUND = {GetSound()}", "SOUND = SOUND"));
        SQLiteManager.RunQuery(CommonQuery.Update("OPTIONS", $"LANGUAGE = {(int)Language}", "LANGUAGE = LANGUAGE"));
    }

    public static void SetItems()
    {
        Doubled = new Item
        (
            0,
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("LEVEL", "ITEMS", "ITEM_ID = 0")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("PRICE", "ITEMS", "ITEM_ID = 0")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("TIME", "ITEMS", "ITEM_ID = 0"))
        );

        Magnetic = new Item
        (
            1,
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("LEVEL", "ITEMS", "ITEM_ID = 1")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("PRICE", "ITEMS", "ITEM_ID = 1")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("TIME", "ITEMS", "ITEM_ID = 1"))
        );

        Shield = new Item
        (
            2,
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("LEVEL", "ITEMS", "ITEM_ID = 2")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("PRICE", "ITEMS", "ITEM_ID = 2")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("TIME", "ITEMS", "ITEM_ID = 2"))
        );

        SlowMotion = new Item
        (
            3,
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("LEVEL", "ITEMS", "ITEM_ID = 3")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("PRICE", "ITEMS", "ITEM_ID = 3")),
            SQLiteManager.ReturnValueAsInt(CommonQuery.Select("TIME", "ITEMS", "ITEM_ID = 3"))
        );
    }
}
