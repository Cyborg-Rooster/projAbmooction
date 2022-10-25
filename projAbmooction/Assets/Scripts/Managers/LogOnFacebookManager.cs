using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class LogOnFacebookManager
{
    public static IEnumerator LogOnFacebook(DialogBoxBuilderController Builder)
    {
        if(!FacebookManager.IsLogged)
        {
            yield return Builder.ShowTyped(Strings.lblConnect, Strings.ConnectOnFacebook, true);

            if (Builder.LastButtonState == ButtonPressed.Yes)
            {
                FacebookManager.Login();
                yield return new WaitUntil(() => FacebookManager.IsLogIn != DefaultState.Null);

                if (FacebookManager.IsLogIn == DefaultState.Yes) 
                {
                    FirebaseManager.UpdateGUID();

                    yield return CheckIfUserAreRegistered(Builder);
                    yield return new WaitUntil(() => FirebaseManager.DataLoadedOrSaved);

                    GetReward();
                    GameData.Save();

                    FirebaseManager.LoadBox();
                    yield return new WaitUntil(() => FirebaseManager.BoxLoaded);

                    yield return Builder.ShowTyped(Strings.lblConnect, Strings.AlreadyLogged, false);
                }
                else yield return Builder.ShowTyped(Strings.lblConnect, Strings.NotPossibleToLogin, false);
            }
        }
        else yield return Builder.ShowTyped(Strings.lblConnect, Strings.AlreadyLogged, false);
    }

    static IEnumerator CheckIfUserAreRegistered(DialogBoxBuilderController Builder)
    {
        //check if user are registered, if not, create data on server
        GameObject waiting = Builder.ShowWaiting();

        FirebaseManager.CheckIfUserAreRegistered();
        yield return new WaitUntil(() => FirebaseManager.UserAreRegistered != DefaultState.Null);

        if (FirebaseManager.UserAreRegistered == DefaultState.Yes) 
        {
            yield return CheckIfLocalScoreIsGreaterThanCloudData(Builder, waiting); 
        }
        else FirebaseManager.SaveData(OnlineData.ReturnOnlineData());
    }

    static IEnumerator CheckIfLocalScoreIsGreaterThanCloudData(DialogBoxBuilderController Builder, GameObject Waiting)
    {
        //determinate if local score is greater than saved on cloud data, if yes, ask to user if he wants
        //to overwrite. if not, load the data.

        FirebaseManager.CheckIfLocalScoreIsGreaterThanCloudData();
        yield return new WaitUntil(() => FirebaseManager.LocalScoreIsGreaterThanCloudData != DefaultState.Null);

        Builder.CloseWaiting(Waiting);
        if (FirebaseManager.LocalScoreIsGreaterThanCloudData == DefaultState.Yes)
        {
            yield return Builder.ShowTyped(Strings.lblConnect, Strings.UpdateScore, true);
            if (Builder.LastButtonState == ButtonPressed.Yes) FirebaseManager.SaveData(OnlineData.ReturnOnlineData());
            else FirebaseManager.LoadData();
        }
        else FirebaseManager.LoadData();
    }

    static void GetReward()
    {
        if (SQLiteManager.ReturnValueAsInt(CommonQuery.Select("ALREADY_LOGGED", "DATABASE")) == 0)
        {
            GameData.Coins += 1000;
            SQLiteManager.RunQuery(CommonQuery.Update("DATABASE", $"ALREADY_LOGGED = 1", "ALREADY_LOGGED = ALREADY_LOGGED"));
            //SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {GameData.Coins}", "COINS = COINS"));
        }
    }
    /* 
    IEnumerator LogOnFacebook()
    {
        if (!FacebookManager.IsLogged)
        {
            yield return Builder.ShowTyped(Strings.lblConnect, Strings.ConnectOnFacebook, true);

            if (Builder.LastButtonState == ButtonPressed.Yes)
            {
                FacebookManager.Login();

                yield return new WaitUntil(() => FacebookManager.IsLogIn != DefaultState.Null);

                if (FacebookManager.IsLogIn == DefaultState.Yes) yield return LogOnFacebookManager.CheckIfUserAreRegistered(Builder);
                else yield return Builder.ShowTyped(Strings.lblConnect, Strings.NotPossibleToLogin, false);
            }
        }
        else yield return Builder.ShowTyped(Strings.lblConnect, Strings.AlreadyLogged, false);
    }

    public static IEnumerator CheckIfUserAreRegistered(DialogBoxBuilderController Builder)
    {
        GameObject waiting = Builder.ShowWaiting();
        //check if user are registered, if not, create data on server
        //FirebaseManager.CheckIfUserAreRegistered();
        /*yield return new WaitUntil(() => FirebaseManager.UserAreRegistered != DefaultState.Null);

        if (FirebaseManager.UserAreRegistered == DefaultState.Yes)
        {
            yield return CheckIfLocalScoreIsGreaterThanCloudData(Builder, waiting);
        }
        else FirebaseManager.SaveData(OnlineData.ReturnOnlineData());
        //if(await FirebaseManager.CheckIfUserAreRegistered().Result) yield return CheckIfLocalScoreIsGreaterThanCloudData(Builder, waiting);
        //else FirebaseManager.SaveData(OnlineData.ReturnOnlineData());
        if (FirebaseManager.CheckIfUserAreRegistered().Result) Debug.Log("AAAAAAAAAAAAAAAAAAAA");
        FirebaseManager.LoadBox();
        //yield return new WaitUntil(() => FirebaseManager.BoxLoaded);

        GetReward();
        yield return Builder.ShowTyped(Strings.lblConnect, Strings.AlreadyLogged, false);
    }

    public static IEnumerator CheckIfLocalScoreIsGreaterThanCloudData(DialogBoxBuilderController Builder, GameObject Waiting)
    {
        Debug.Log("The user are registered already.");

        //check if local score is greater than cloud score, if not, load the server data
        /*FirebaseManager.CheckIfLocalScoreIsGreaterThanCloudData();
        yield return new WaitUntil(() => FirebaseManager.LocalScoreIsGreaterThanCloudData != DefaultState.Null);

        Builder.CloseWaiting(Waiting);
        if (FirebaseManager.LocalScoreIsGreaterThanCloudData == DefaultState.Yes)
        {
            Debug.Log("The local best score is greater than cloud best score.");

            yield return Builder.ShowTyped(Strings.lblConnect, Strings.UpdateScore, true);

            //ask to user if he want to upload your data, if not, load the server data
            if (Builder.LastButtonState == ButtonPressed.Yes) FirebaseManager.SaveData(OnlineData.ReturnOnlineData());
            else yield return WaitToLoadData(Builder);
        }
        else yield return WaitToLoadData(Builder);

        if(FirebaseManager.CheckIfLocalScoreIsGreaterThanCloudData().Result)
        {
            Builder.CloseWaiting(Waiting);
            Debug.Log("The local best score is greater than cloud best score.");
            yield return Builder.ShowTyped(Strings.lblConnect, Strings.UpdateScore, true);
            if (Builder.LastButtonState == ButtonPressed.Yes) FirebaseManager.SaveData(OnlineData.ReturnOnlineData());
            else WaitToLoadData(Builder);
        }
        else
        {
            Builder.CloseWaiting(Waiting);
            WaitToLoadData(Builder);
        }
    }

    public static void WaitToLoadData(DialogBoxBuilderController Builder)
    {
        /*FirebaseManager.LoadData();
        yield return new WaitUntil(() => FirebaseManager.DataLoaded != DefaultState.Null);

        if (FirebaseManager.DataLoaded == DefaultState.Yes) 
        {
            string oldGuid;
            OnlineData.SetOnlineData(FirebaseManager.data, out oldGuid);
            FirebaseManager.UpdateGUID(oldGuid);
        }
        else yield return Builder.ShowTyped(Strings.lblConnect, Strings.NotPossibleToLogin, false);
        string oldGuid;
        OnlineData.SetOnlineData(FirebaseManager.LoadData().Result, out oldGuid);
        FirebaseManager.UpdateGUID(oldGuid);
    }

    public static void GetReward()
    {
        if (SQLiteManager.ReturnValueAsInt(CommonQuery.Select("ALREADY_LOGGED", "DATABASE")) == 0)
        {
            GameData.Coins += 1000;
            SQLiteManager.RunQuery(CommonQuery.Update("DATABASE", $"ALREADY_LOGGED = 1", "ALREADY_LOGGED = ALREADY_LOGGED"));
            SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {GameData.Coins}", "COINS = COINS"));
        }
    }*/
}
