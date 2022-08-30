using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Mechanics
{
    public static float SlowMotionLastRange = 1;
    public static float PauseLastRange = 1;
    public static float SpeedRange = 1;
    public static float ObstacleSpeed { get => SpeedRange * 0.5f; }

    public static float ConfusedTime = 5f;

    public static GamePhase Phase = GamePhase.OnMain;

    public static bool OnPause;
    public static bool CanSpeedUp = true;

    public static float Meters = 0;

    public static Box BoxCatched;

    public static bool CanSpawnBox()
    {
        if (BoxCatched != null || !GameData.IsOnline || GameData.GetFirstBoxesEmptySpace() == -1) return false;
        else return true;
    }

    public static void RestartAttributes(bool rewarded)
    {
        Phase = GamePhase.OnMain;
        SlowMotionLastRange = 1;
        PauseLastRange = 1;
        SpeedRange = 1;
        BoxCatched = null;
        if (rewarded) Meters = 0;
    }
}
