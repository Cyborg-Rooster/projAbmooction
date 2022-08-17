using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using UnityEngine;

class FirebaseManager
{
    static FirebaseFirestore Database;

    public static void Init()
    {
        Database = FirebaseFirestore.DefaultInstance;
    }
}
