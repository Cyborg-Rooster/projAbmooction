using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GameData
{
    public static float LastRange;
    public static float SpeedRange = 1;
    public static float ObstacleSpeed { get => SpeedRange * 0.5f; }

    public static float ConfusedTime = 5f;
    public static float DoubledTime = 5f;
    public static float MagneticTime = 5f;

    public static GamePhase Phase = GamePhase.OnMain;

    public static bool OnPause;
}
