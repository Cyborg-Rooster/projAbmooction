﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Strings
{
    public static string[] items = new string[4]
    {
        "2x Coins",
        "Magnet",
        "Shield",
        "Slow Motion"
    };

    public static string lblStore = "Store";
    public static string lblOptions = "Options";
    public static string lblSkins = "Skins";
    public static string lblScenarios = "Scenarios";
    public static string lblImprovements = "Improvements";
    public static string lblSlots = "Special Boxes";
    public static string lblCoins = "Get Coins";

    public static string lblBought = "-Bought-";
    public static string coins = "coins";

    public static string[] skins = new string[22]
    {
        "Default",
        "Angus",
        "Ayrshire",
        "Punk",
        "Diver",
        "Modern",
        "Cool",
        "Cowboy",
        "Nerd",
        "Monk",
        "Japanese",
        "Soccer Player",
        "Cow Aerodynamics",
        "Alien",
        "Medieval",
        "Monster",
        "Astronaut",
        "Puppy",
        "Cat",
        "Turtle",
        "Man",
        "Women"
    };

    public static string[] scenarios = new string[6]
    {
        "Farm",
        "City",
        "Mountain",
        "Western",
        "Medieval",
        "Alien"
    };

    public static string yes = "Yes";
    public static string no = "No";
    public static string ok = "Ok";
    public static string confirm = "Confirm";
    public static string cancel = "Cancel";

    public static string BestScore = "Best score:";
    public static string New = "NEW";

    public static string restart = "Restart";
    public static string seeAnAd = "See an ad and restore score";
    public static string backToMain = "Back to main menu";

    public static string skin = "skin";
    public static string titleError = "Error";

    public static string ContentBuySkin(string content, int price)
    {
        return $"Do you want to buy this {content} for {price} {coins}?";
    }

    public static string contentError = "This operation could not be continued. :(";

    public static string selectScenarioError = "You don't have all the cards to use this scenario.";

    public static string noMoneyEnough = "You don't have enough coins.";
    //public static string selectScenarioError = "Você ainda não possui todos os cartões para usar esse cenário.";

    public static string SelectScenario(string scenario)
    {
        return $"Do you want to select the {scenario} scenario?";
        //Deseja selecionar o cenário ***?
    }

    public static string AddChanges(string item, float time, int coins)
    {
        return $"The {item} item lasts for {time} seconds. Would you like to add 2 seconds for {coins} coins?";
        //O item *** dura por *** segundos. Gostaria de acrescentar 2 segundos por *** moedas?
    }
    public static string ChangesFinish(string item, float time)
    {
        return $"You have already upgraded item {item} by 100%, lasting for {time} seconds.";
        //Você já atualizou o item {} em 100%, com duração de {} segundos.
    }

    public static string Empty = "Empty";
    public static string Open = "Open";
    public static string GetReward = "Get Reward";

    public static string Congratulations = "Congratulations!";
    public static string YouAcquired = "You acquired:";
    public static string Collect = "Collect";

    public static string CollectCards(int quantity, int scenarioId)
    {
        return $"{quantity}x {scenarios[scenarioId]} cards!";
    }
    public static string CollectCoins(int quantity)
    {
        return $"{quantity}x coins!";
    }

    public static string BuyRegularBox = "Would you like to buy a regular box for 5000 coins?";
    public static string OpenBox = "Do you want to open the box?";
    public static string SeeAnADAndDecreaseTime = "Do you want to see an ad and decrease 1 hour?";
    public static string SeeAnADAndGetCoins = "Do you want to see an ad and earn 500 coins?";

    public static string Reloading = "Reconnecting";
    public static string Waiting = "Reconnecting, wait a moment...";

    public static string[] getCoins = new string[4]
    {
        "See an Ad",
        "Connect on Facebook",
        "Like Our Page",
        "Follow Our Page"
    };

    public static string LikePage = "Do you want to like the Cyborg Rooster page on Facebook and earn 1000 coins?";
    public static string FollowPage = "Do you want to follow the Cyborg Rooster page on Instagram and earn 1000 coins?";

    public static string AlreadyLiked = "You've already liked the Cyborg Rooster page.";
    public static string AlreadyFollow = "You've already followed the Cyborg Rooster page.";

    public static string Day = "Day ";
    public static string Adquired = "Adquired";
    public static string DailyReward = "Daily Reward";

    public static string lblConnect = "Log on Facebook";
    public static string ConnectOnFacebook = "Do you want to link the game to your Facebook account? In addition to " +
        "winning a prize of 1000 coins, you will be able to compete your score with other players!";

    //Você quer vincular o jogo à sua conta do Facebook?   Além de ganhar um prêmio de 1000 moedas,
    //você poderá competir sua pontuação com outros jogadores!

    public static string AlreadyLogged = "You are logged on Facebook.";
    public static string NotPossibleToLogin = "Unable to log in to Facebook.";

    public static string UpdateScore = "Your local score is higher than the one saved on the server. Would you like " +
        "to update your score or continue with the score saved on our server?";
    //Sua pontuação local é maior que a está salva no servidor.Gostaria de atualizar sua pontuação
    //ou continuar com a pontuação salva no nosso servidor? 

    /*public static string[] skins = new string[22]
    {
        "Padrão",
        "Angus",
        "Malhada",
        "Roqueira",
        "Mergulhadora",
        "Moderna",
        "Descolada",
        "Cowboy",
        "Nerd",
        "Monge",
        "Japonesa",
        "Jogadora de futebol",
        "Aerodinâmica da Vaca",
        "Alien",
        "Medieval",
        "Monstro",
        "Astronauta",
        "Cachorro",
        "Gato",
        "Tartaruga",
        "Homem",
        "Mulher"
    };*/
}
