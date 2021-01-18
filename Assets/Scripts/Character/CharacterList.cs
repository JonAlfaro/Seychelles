using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterList
{
    public static List<CharacterData> AllCharacters = new List<CharacterData>
    {
        CharacterDatabase.Andy,
        CharacterDatabase.Andette,
        CharacterDatabase.LilAndy,
        CharacterDatabase.Andont,
        CharacterDatabase.CharFive
    };

    public static List<CharacterData> StarterSelectedCharacters = new List<CharacterData>
    {
        CharacterDatabase.Andy,
        null,
        null,
        null
    };

    public static List<CharacterData> StarterUnlockedCharacters = new List<CharacterData>
    {
        CharacterDatabase.Andette,
        CharacterDatabase.LilAndy,
        CharacterDatabase.Andont,
        CharacterDatabase.CharFive
    };

    public static CharacterData GetRandomCharacter()
    {
        float rolledNumber = Random.Range(0, 100);
        float numberToCheck = 0;
        Rarity rarity = Rarity.fish;

        // Loop through each rarity odds (ascending order)
        foreach (KeyValuePair<Rarity, float> keyValuePair in Constants.GachaOdds)
        {
            // Increase the number that would need to be rolled to get this character
            numberToCheck += keyValuePair.Value;
            if (rolledNumber < numberToCheck)
            {
                rarity = keyValuePair.Key;
                break;
            }
        }

        return AllCharacters.Shuffle().First(character => character.Rarity == rarity);
    }

    private static class CharacterDatabase
    {
        public static CharacterData Andy = new CharacterData(0, 10, 5, "Andy", "Description for big andy.", Rarity.gun,
            SkillDatabase.Flame);

        public static CharacterData Andette = new CharacterData(1, 8, 4, "Andette", "Description for female andy.",
            Rarity.frog, SkillDatabase.Heal);

        public static CharacterData LilAndy = new CharacterData(2, 6, 3, "Andy's younger brother",
            "Description for young andy.", Rarity.fish, SkillDatabase.Peck);

        public static CharacterData Andont = new CharacterData(3, 3, 9, "Andon't", "Description not for andy.",
            Rarity.frog, SkillDatabase.LightningBolt);

        public static CharacterData CharFive = new CharacterData(4, 15, 2, "5th Character", "Description for andy 5.",
            Rarity.frog, SkillDatabase.Colding);
    }

    private static class SkillDatabase
    {
        public static SkillData Flame = new SkillData(0, "Flame",
            "\"Flame on\"'s the enemies which causes low fire damage.",
            "fire-spell-cast", EffectType.damage, TargetType.enemyAOE, 1.25f, 25f);

        public static SkillData LightningBolt = new SkillData(0, "Lightning Bolt",
            "\"zzzzzzzZAP\"'s the enemy which causes lightning damage.",
            "bolt-spell-cast", EffectType.damage, TargetType.enemyRandom, 2.75f, 15f);

        public static SkillData Colding = new SkillData(0, "Colding",
            "\"Colding\"'s the enemy which causes high cold damage.",
            "ice-spell-cast", EffectType.damage, TargetType.enemyRandom, 4.5f, 30f);

        public static SkillData Heal = new SkillData(0, "Heal", "Heals all allies which causes healing damage.",
            "healing", EffectType.heal, TargetType.allyAOE, 1f, 20f);

        public static SkillData Peck = new SkillData(0, "Peck",
            "Unleashes \"Sealed Peck (7th Gate)\" which deals massive peck-based damage to all enemies.",
            "shoebill-stork", EffectType.damage, TargetType.enemyAOE, 7.5f, 90f);
    }
}