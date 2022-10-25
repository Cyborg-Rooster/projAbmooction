using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Facebook.Unity;
using Facebook.MiniJSON;

class FacebookManager
{
    public static DefaultState IsLogIn = DefaultState.Null;

    public static string UserName;
    public static bool IsInitialized { get => FB.IsInitialized; }
    public static string UserID { get => AccessToken.CurrentAccessToken.UserId; }

    public static bool IsLogged { get => FB.IsLoggedIn; }

    public static void Init()
    {
        if (!FB.IsInitialized) FB.Init(InitCallback, OnHideUnity);
        else FB.ActivateApp();
    }

    public static void Login()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }


    private static void InitCallback()
    {
        if (FB.IsInitialized) FB.ActivateApp();
        else Debug.LogError("Failed to Initialize the Facebook SDK");
    }

    private static void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    private static void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn) FB.API("/me?fields=first_name,last_name", HttpMethod.GET, GetNameCallback);
        else
        {
            IsLogIn = DefaultState.No;
            Debug.Log("User cancelled login");
        }
    }

    private static void GetNameCallback(IResult result)
    {
        if (result.Error == null)
        {
            UserName = $"{result.ResultDictionary["first_name"]} {result.ResultDictionary["last_name"]}";
            Debug.Log($"User logged successful. Username: {UserName}");
            IsLogIn = DefaultState.Yes;
        }
        else 
        { 
            Debug.LogError(result.Error);
            IsLogIn = DefaultState.No;
        }
    }
}
