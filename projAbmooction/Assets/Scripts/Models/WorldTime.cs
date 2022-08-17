using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class WorldTime
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int seconds;
    public int milliSeconds;
    public string dateTime;
    public string date;
    public string time;
    public string timeZone;
    public string dayOfWeek;
    public bool dstActive;
}
