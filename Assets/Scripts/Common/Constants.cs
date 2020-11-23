using UnityEngine;

public static class Constants
{
    public static string SaveFileName => "bigsavefile.seychelles";
    public static string BattleSceneName = "BattleScene";
    public static int StartingPremiumCurrencyAmount = 30;
    public static string PremiumCurrencyName = "Frog Knife Coin Gem Crystals";
    public static Color DeadColor = new Color(125, 125, 125, 100);
    public static string CharacterResourceFolder = "Characters";
    public static string CharacterResourcePrefix = "psr"; // playstation virtual reality
    public static CharacterData[] StarterSelectedCharacters =
    {
        new CharacterData(0, 10, 5, "Andy", "Description for big andy."),
        null,
        null,
        null
    }; 
    
    public static CharacterData[] StarterUnlockedCharacters = {
        new CharacterData(1, 8, 4, "Andette", "Description for female andy."),
        new CharacterData(2, 6, 3, "Andy's younger brother", "Description for young andy."),
        new CharacterData(3, 3, 9, "Andon't", "Description not for andy."),
        new CharacterData(4, 15, 2, "5th Character", "Description for andy 5.")
    };
}
