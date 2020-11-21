using UnityEngine;

public static class Constants
{
    public static string SaveFileName => "bigsavefile.seychelles";
    public static string BattleSceneName = "BattleScene";
    public static CharacterData StarterCharacter = new CharacterData(0, 10, 5);
    public static int StartingPremiumCurrencyAmount = 30;
    public static string PremiumCurrencyName = "Frog Knife Coin Gem Crystals";
    public static Color DeadColor = new Color(125, 125, 125, 100);
    public static string CharacterResourceFolder = "Characters";
}
