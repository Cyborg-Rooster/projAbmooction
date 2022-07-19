using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GameData
{
    public static float SlowMotionLastRange = 1;
    public static float PauseLastRange = 1;
    public static float SpeedRange = 1;
    public static float ObstacleSpeed { get => SpeedRange * 0.5f; }

    public static float ConfusedTime = 5f;
    public static float DoubledTime = 5f;
    public static float MagneticTime = 5f;
    public static float ShieldTime = 5f;
    public static float SlowMotionTime = 5f;

    public static int Coins = 0;

    public static GamePhase Phase = GamePhase.OnMain;

    public static bool OnPause;
    public static bool CanSpeedUp = true;

    public static void RestartAttributes()
    {
        Phase = GamePhase.OnMain;
        SlowMotionLastRange = 1;
        PauseLastRange = 1;
        SpeedRange = 1;
    }
}
