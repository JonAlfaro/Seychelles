using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class CharacterList
{
    public static List<CharacterData> AllCharacters = new List<CharacterData>
    {
        CharacterDatabase.Andy,
        CharacterDatabase.Andette,
        CharacterDatabase.LilAndy,
        CharacterDatabase.Andont,
        CharacterDatabase.CharFive,
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
            CharacterDatabase.CharFive,
    };

    public static CharacterData GetRandomCharacter()
    {
        float rolledNumber = Random.Range(0, 100);
        float numberToCheck = 0;
        Rarity rarity = Rarity.fish;
        
        // Loop through each rarity odds (ascending order)
        foreach (KeyValuePair<Rarity,float> keyValuePair in Constants.GachaOdds)
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
        public static CharacterData Andy = new CharacterData(0, 10, 5, "Andy", "Description for big andy.", Rarity.gun);
        public static CharacterData Andette = new CharacterData(1, 8, 4, "Andette", "Description for female andy.", Rarity.frog);
        public static CharacterData LilAndy = new CharacterData(2, 6, 3, "Andy's younger brother", "Description for young andy.", Rarity.fish);
        public static CharacterData Andont = new CharacterData(3, 3, 9, "Andon't", "Description not for andy.", Rarity.frog);
        public static CharacterData CharFive = new CharacterData(4, 15, 2, "5th Character", "Description for andy 5.", Rarity.frog);
    }
}
