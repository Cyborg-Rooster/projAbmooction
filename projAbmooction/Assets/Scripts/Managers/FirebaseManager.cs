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

    public static bool BoxLoaded = false;
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
        BoxLoaded = false;
        Query allBoxes = Database.Collection("Users_Boxes").Document(GameData.Guid).
            Collection("Boxes");

        allBoxes.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot query = task.Result;
            foreach (DocumentSnapshot documentSnapshot in query.Documents)
            {
                Dictionary<string, object> box = documentSnapshot.ToDictionary();

                Debug.Log($"ID:{documentSnapshot.Id} Type:{box["Type"]} Active:{box["Active"]} EndTime:{box["EndTime"]}");
                try
                {
                    GameData.Boxes[int.Parse(documentSnapshot.Id)] = new Box()
                    {
                        ID = int.Parse(documentSnapshot.Id),
                        Type = Convert.ToInt32(box["Type"]),
                        Active = (bool)box["Active"],
                        EndTime = (Timestamp)box["EndTime"]
                    };

                    int i = int.Parse(documentSnapshot.Id);

                    if (GameData.Boxes[i] != null)
                        Debug.Log($"ID:{GameData.Boxes[i].ID} Type:{GameData.Boxes[i].Type} " +
                            $"Active:{GameData.Boxes[i].Active} EndTime:{GameData.Boxes[i].EndTime}");
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }
            BoxLoaded = true;
        });
    }

    public static void RemoveBox(Box box)
    {
        DocumentReference docRef = Database.Collection("Users_Boxes").Document(GameData.Guid).
            Collection("Boxes").Document(box.ID.ToString());
        docRef.DeleteAsync();
    }
}
