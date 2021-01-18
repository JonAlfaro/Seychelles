using System.Collections.Generic;

public static class Constants
{
    public static string SaveFileName => "bigsavefile.seychelles";
    public static string BattleSceneName = "BattleScene";
    public static int StartingPremiumCurrencyAmount = 30;
    public static string PremiumCurrencyName = "Frog Knife Coins";
    public static string CharacterResourceFolder = "Characters";
    public static string SkillResourceFolder = "Skills";
    public static string BremiumResourceSuffix = ".psvr"; // playstation virtual reality

    public static SortedDictionary<Rarity, float> GachaOdds = new SortedDictionary<Rarity, float>
    {
        {Rarity.frog, 50},
        {Rarity.fish, 30},
        {Rarity.knife, 12},
        {Rarity.gun, 7},
        {Rarity.doubleGun, 1}
    };

    public static int GachaCost = 100;
}