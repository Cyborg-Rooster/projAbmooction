using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Firestore;

class Box
{
    public int ID;
    public int Type;
    public Timestamp EndTime;
    public TimeSpan ActualTime;
    public bool Active;
}
