using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

class FirebaseManager
{
    static FirebaseFirestore Database;

    public static void Init()
    {
        Database = FirebaseFirestore.DefaultInstance;
    }

    public static void SaveBox(Box box)
    {
        DocumentReference docRef = Database.Collection("Users_Boxes").Document(GameData.Guid).
            Collection("Boxes").Document(box.ID.ToString());

        Dictionary<string, object> dictBox = new Dictionary<string, object>
        {
            { "Type", box.Type },
            { "EndTime", box.EndTime },
            { "Active", box.Active }
        };

        docRef.SetAsync(dictBox).ContinueWithOnMainThread(task => {
            Debug.Log("Added data successful.");
        });
    }

    public static void LoadBox()
    {
        Query allBoxes = Database.Collection("Users_Boxes").Document(GameData.Guid).
            Collection("Boxes");

        allBoxes.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot query = task.Result;
            foreach (DocumentSnapshot documentSnapshot in query.Documents)
            {
                Dictionary<string, object> box = documentSnapshot.ToDictionary();

                Debug.Log($"ID:{documentSnapshot.Id} Type:{box["Type"]}");
                GameData.Boxes[int.Parse(documentSnapshot.Id)] = new Box()
                {
                    ID = int.Parse(documentSnapshot.Id),
                    Type = (int)box["Type"],
                    Active = (bool)box["Active"],
                    EndTime = (Timestamp)box["EndTime"]
                };
            }
        });
    }
}
