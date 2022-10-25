using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine;

class FirebaseManager
{
    static DatabaseReference Database;

    public static DefaultState UserAreRegistered = DefaultState.Null;
    public static DefaultState LocalScoreIsGreaterThanCloudData = DefaultState.Null;
    public static DefaultState DataLoaded = DefaultState.Null;

    public static bool BoxLoaded = false;
    public static bool DataLoadedOrSaved = false;
    public static void Init()
    {
        Database = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Firebase initialization complete.");
    }

    public static void SaveBox(Box box)
    {
        string json = JsonUtility.ToJson(box);
        Database.Child("Users_Boxes").Child(GameData.Guid).Child($"{box.ID}").SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                Debug.Log("Added data successful.");
            });
        /*string json = JsonUtility.ToJson(box);
        await Database.Child("Users_Boxes").Child(GameData.Guid).Child($"{box.ID}").SetRawJsonValueAsync(json);
        Debug.Log("Added data successful.");*/
    }

    public static void LoadBox()
    {
        BoxLoaded = false;
       // FirebaseDatabase.DefaultInstance.GetReference("Users_Boxes").Child(GameData.Guid)
       Database.Child("Users_Boxes").Child(GameData.Guid)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                DataSnapshot snapshot = task.Result;
                foreach (var s in snapshot.Children)
                {
                    try
                    {
                        Debug.Log(s.GetRawJsonValue());

                        GameData.Boxes[int.Parse(s.Key)] = JsonUtility.FromJson<Box>(s.GetRawJsonValue());
                        GameData.Boxes[int.Parse(s.Key)].EndTime = DateTime.Parse(GameData.Boxes[int.Parse(s.Key)].EndTimeStringFormat);
                    } catch(Exception e) { Debug.LogError(e); }
                }
            });
        BoxLoaded = true;
        /*var load = await FirebaseDatabase.DefaultInstance.GetReference("Users_Boxes").Child(GameData.Guid).GetValueAsync();

        if(load.Exists)
        {
            foreach (var s in load.Children)
            {
                try
                {
                    GameData.Boxes[int.Parse(s.Key)] = JsonUtility.FromJson<Box>(s.GetRawJsonValue());
                    Debug.Log(s.GetRawJsonValue());
                    GameData.Boxes[int.Parse(s.Key)].EndTime = DateTime.Parse(GameData.Boxes[int.Parse(s.Key)].EndTimeStringFormat);
                }
                catch (Exception e) { Debug.LogError(e); }
            }
        }*/
    }

    public static void RemoveBox(Box box)
    {
        FirebaseDatabase.DefaultInstance.GetReference("Users_Boxes").Child(GameData.Guid).Child($"{box.ID}")
            .RemoveValueAsync().ContinueWithOnMainThread(task => 
            {
                if (task.Exception != null) Debug.LogError(task.Exception);
                else Debug.Log($"Box {box.ID} deleted successful.");
            });
        /*await FirebaseDatabase.DefaultInstance.GetReference("Users_Boxes").Child(GameData.Guid).Child($"{box.ID}")
            .RemoveValueAsync();*/
        
    }

    public static void LoadData()
    {
        DataLoadedOrSaved = false;
        DataLoaded = DefaultState.Null;
        Database.Child("Users").Child(FacebookManager.UserID).Child("Options").GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
                DataLoaded = DefaultState.No;
            }
            else
            {
                OnlineData.SetOnlineData(JsonUtility.FromJson<OnlineData>(task.Result.GetRawJsonValue()));
                DataLoaded = DefaultState.Yes;
            }
        });
        /*var task = await Database.Child("Users").Child(FacebookManager.UserID).Child("Options").GetValueAsync();

        if(task.Exists) JsonUtility.FromJson<OnlineData>(task.GetRawJsonValue());
        return null;*/
        DataLoadedOrSaved = true;
    }

    public static void CheckIfUserAreRegistered()
    {
        Database.Child("Users").Child(FacebookManager.UserID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) Debug.LogError(task.Exception);
            else
            {
                if (task.Result.Exists) UserAreRegistered = DefaultState.Yes;
                else UserAreRegistered = DefaultState.No;
            }
        });
        
        //var task = await Database.Child("Users").Child(FacebookManager.UserID).GetValueAsync();
        //return task.Exists;
    }

    public static void CheckIfLocalScoreIsGreaterThanCloudData()
    {
        Database.Child("Users").Child(FacebookManager.UserID).Child("Options")
            .Child("BestScore").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null) Debug.LogError(task.Exception);
                else if (task.IsCompleted)
                {
                    int onlineScore = Convert.ToInt32(task.Result.Value.ToString());
                    LocalScoreIsGreaterThanCloudData = GameData.BestScore > onlineScore ? DefaultState.Yes : DefaultState.No;
                }
            });
        /*var task = await Database.Child("Users").Child(FacebookManager.UserID).Child("Options")
            .Child("BestScore").GetValueAsync();

        if (task.Exists)
        {
            int onlineScore = (int)task.Value;
            return GameData.BestScore > onlineScore;
        }
        return false;*/
    }

    public static void SaveData(OnlineData data)
    {
        DataLoadedOrSaved = false;
        string json = JsonUtility.ToJson(data);
        Database.Child("Users").Child(FacebookManager.UserID).Child("Options").SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                Debug.Log("Added data successful.");
            });
        /*string json = JsonUtility.ToJson(data);
        await Database.Child("Users").Child(FacebookManager.UserID).Child("Options").SetRawJsonValueAsync(json);
        Debug.Log("Added data successful.");*/
        DataLoadedOrSaved = true;
    }

    public static async void UpdateGUID()
    {
        /*DataSnapshot exist = await FirebaseDatabase.DefaultInstance.GetReference("Users_Boxes").Child(oldGuid).GetValueAsync();
        
        if (exist.Exists)
        {
            await FirebaseDatabase.DefaultInstance.GetReference("Users_Boxes").Child(oldGuid).RemoveValueAsync();
            Debug.Log($"Old GUID deleted successful.");
        }*/
        DataSnapshot guid = await Database.Child("Users").Child(FacebookManager.UserID).Child("Options").
            Child("Guid").GetValueAsync();

        if(guid.Exists)
        {
            if ((string)guid.Value != GameData.Guid)
            {
                await FirebaseDatabase.DefaultInstance.GetReference("Users_Boxes").Child(GameData.Guid).RemoveValueAsync();
                Debug.Log("Old GUID removed.");
            }
        }
    }
}
