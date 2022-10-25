using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class OnlineData
{
    public string Guid;
    public int BestScore;

    public int Language;
    public int Sound;

    public static OnlineData ReturnOnlineData()
    {
        return new OnlineData()
        {
            Guid = GameData.Guid,
            BestScore = GameData.BestScore,
            Language = (int)GameData.Language,
            Sound = GameData.GetSound()
        };
    }

    public static void SetOnlineData(OnlineData data)
    {
        Debug.Log($@"guid is equal: {GameData.Guid == data.Guid} guid: {data.Guid} best score: {data.BestScore} set sound: {data.Sound} language: {data.Language}");
        GameData.Guid = data.Guid;
        GameData.BestScore = data.BestScore;
        GameData.SetSound(data.Sound);
        switch(data.Language)
        {
            case 0: GameData.Language = Languages.Portuguese; break;
            case 1: GameData.Language = Languages.English; break;
            case 2: GameData.Language = Languages.Español; break;
        }
        SQLiteManager.RunQuery
        (
            CommonQuery.Update("DATABASE", $"GUID = '{GameData.Guid}'", "GUID = GUID")
        );
    }
}
