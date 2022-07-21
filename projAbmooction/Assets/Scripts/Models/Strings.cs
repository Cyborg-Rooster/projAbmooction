using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Strings
{
    public static string itemDouble = "2x Coins";
    public static string itemShield = "Shield";
    public static string itemMagnetic = "Magnet";
    public static string itemSlowMotion = "Slow Motion";

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
        "Shabby",
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

    public static string yes = "Yes";
    public static string no = "No";
    public static string ok = "Ok";

    public static string skin = "skin";
    public static string titleError = "Error";

    public static string ContentBuySkin(string content, int price)
    {
        return $"Do you want to buy this {content} for {price} {coins}?";
    }

    public static string contentError = "This operation could not be continued. :(";

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
