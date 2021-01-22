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
        CharacterDatabase.CharFive,
        CharacterDatabase.Gun,
        CharacterDatabase.Knight,
        CharacterDatabase.Milton,
        CharacterDatabase.Beanist,
        CharacterDatabase.AmyBeaner,
        CharacterDatabase.DonXu,
        CharacterDatabase.HamburgerCountry,
        CharacterDatabase.MikeBike,
        CharacterDatabase.MonkeyneticClown,
        CharacterDatabase.OrangeMan,
        CharacterDatabase.RigworkWizard,
        CharacterDatabase.TheRats,
        CharacterDatabase.SerialBeanKiller,
        CharacterDatabase.TheGiantRat,
        CharacterDatabase.ElectricUsingBeanRat,
        CharacterDatabase.Samurai,
        CharacterDatabase.Monkey,
        CharacterDatabase.Chad,
        CharacterDatabase.LooooongBoi,
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
        CharacterDatabase.TheGiantRat
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
        
        public static CharacterData Chad = new CharacterData(4, 15, 2, "Chad Froggünd", "An animal of a man; Chad Froggünd has a collection of over twelve knives which he chooses not to use in favour of attacking with his bear fists.",
            Rarity.frog, SkillDatabase.Punch);
        
        public static CharacterData DonXu = new CharacterData(5, 99999999, 99999999, "Don Xu", "Strongest Mother-loving Bean.",
            Rarity.doubleGun, SkillDatabase.Offer);
        
        public static CharacterData AmyBeaner = new CharacterData(6, 100, 100, "Amy Beaner", "Extremely hilarious, beaninist.",
            Rarity.doubleGun, SkillDatabase.Joke);
        
        public static CharacterData Gun = new CharacterData(7, 5, 400, "Gun", "Contains bullets.",
            Rarity.fish, SkillDatabase.Shoot);
        
        public static CharacterData MikeBike = new CharacterData(8, 123, 123, "Mike Bike", "Angry cyclist.",
            Rarity.gun, SkillDatabase.Monologue);
        
        public static CharacterData Beanist = new CharacterData(9, 100, 250, "Beanist", "Just wants to preserve beans.",
            Rarity.knife, SkillDatabase.Monologue);
        
        public static CharacterData OrangeMan = new CharacterData(10, 103, 25, "Orange Man", "Bad.",
            Rarity.knife, SkillDatabase.Punch);
        
        public static CharacterData Milton = new CharacterData(11, 1, -5, "Milton", "Bort's friend.",
            Rarity.fish, SkillDatabase.PoisonBean);
        
        public static CharacterData Monkey = new CharacterData(12, 420, 420, "Monkey", "A primate.",
            Rarity.fish, SkillDatabase.BeanToss);
        
        public static CharacterData HamburgerCountry = new CharacterData(13, 12, 12, "Hamburger Country", "A human man. Evolved from primate.",
            Rarity.frog, SkillDatabase.BeanToss);
        
        public static CharacterData Samurai = new CharacterData(14, 150, 13, "Samurai", "Can also create sushi.",
            Rarity.gun, SkillDatabase.Apologise);
        
        public static CharacterData SerialBeanKiller = new CharacterData(15, 25, 6000000, "Serial Bean Killer", "Took thousands of beans' lives.",
            Rarity.knife, SkillDatabase.Monologue);
        
        public static CharacterData Knight = new CharacterData(16, 50, 50, "Knight", "Defends men, woman and women.",
            Rarity.fish, SkillDatabase.BeanPunch);
        
        public static CharacterData ElectricUsingBeanRat = new CharacterData(17, 35, 100, "Electric Using Bean Rat", "Original character.",
            Rarity.gun, SkillDatabase.Electrocute);
        
        public static CharacterData RigworkWizard = new CharacterData(18, 35, 100, "Rigwork Wizard", "Celestial being that controls gravity.",
            Rarity.doubleGun, SkillDatabase.RealityShift);
        
        public static CharacterData MonkeyneticClown = new CharacterData(19, 45, 60, "Monkeynetic Clown", "I dunno. Chief executive shadow councillor. NO Vice president of the shadow council.",
            Rarity.gun, SkillDatabase.Heal);
        
        public static CharacterData TheGiantRat = new CharacterData(20, 70, 70, "The Giant Rat", "i am the rat that makes all the rules",
            Rarity.doubleGun, SkillDatabase.RatNuke);
        
        public static CharacterData TheRats = new CharacterData(21, 10, 10, "The Rats", "Rats, we're rats; we're the rats. we prey at night, we stalk at night; we're the rats.",
            Rarity.frog, SkillDatabase.Nibble);
        
        public static CharacterData LooooongBoi = new CharacterData(22, 100, 100, "Loooong Boi", "His bark is worse than his bite.",
            Rarity.doubleGun, SkillDatabase.LogDogFireballBarrage);
    }

    private static class SkillDatabase
    {
        public static SkillData Flame = new SkillData(0, "Flame",
            "Flame on's the enemies which causes low fire damage.",
            "fire-spell-cast", EffectType.damage, TargetType.enemyAOE, 1.25f, 25f);

        public static SkillData LightningBolt = new SkillData(1, "Lightning Bolt",
            "zzzzzzzZAP's the enemy which causes lightning damage.",
            "bolt-spell-cast", EffectType.damage, TargetType.enemyRandom, 2.75f, 15f);

        public static SkillData Colding = new SkillData(2, "Colding",
            "Colding's the enemy which causes high cold damage.",
            "ice-spell-cast", EffectType.damage, TargetType.enemyRandom, 4.5f, 30f);

        public static SkillData Heal = new SkillData(3, "Heal", "Heals all allies which causes healing damage.",
            "healing", EffectType.heal, TargetType.allyAOE, 1f, 20f);

        public static SkillData Peck = new SkillData(4, "Peck",
            "Unleashes \"Sealed Peck (7th Gate)\" which deals massive peck-based damage to all enemies.",
            "shoebill-stork", EffectType.damage, TargetType.enemyAOE, 7.5f, 90f);
        
        public static SkillData Punch = new SkillData(5, "Punch",
            "Attacks using fists or fist-like limbs.",
            "punch-blast", EffectType.damage, TargetType.enemyRandom, 1.5f, 5f);
        
        public static SkillData Offer = new SkillData(6, "Offer",
            "Gives the target an offer they can't refuse and they follow you and be personal body guard (Can target anything and everyone).",
            "crown-coin", EffectType.charm, TargetType.enemyRandom, 0f, 10f);
        
        public static SkillData Joke = new SkillData(7, "Joke",
            "Bean hahahahaha.",
            "jester-hat", EffectType.damage, TargetType.enemyAOE, 2f, 10f);
        
        public static SkillData Shoot = new SkillData(8, "Shoot",
            "Shot to the head.",
            "gunshot", EffectType.damage, TargetType.enemyRandom, 1f, 2f);
        
        public static SkillData PoisonBean = new SkillData(9, "Eat Poison Bean",
            "Eats a poisonous bean.",
            "backstab", EffectType.damage, TargetType.self, 1f, 0f);
        
        public static SkillData Monologue = new SkillData(10, "Monologue",
            "Talking forever.",
            "lightning-shout", EffectType.heal, TargetType.enemyAOE, -2f, 6f);
        
        public static SkillData BeanToss = new SkillData(11, "Bean Toss",
            "You just got beaned.",
            "throwing-ball", EffectType.damage, TargetType.enemyAOE, 2f, 3f);
        
        public static SkillData Apologise = new SkillData(12, "Apologise",
            "He didn't even do anything wrong.",
            "tear-tracks", EffectType.damage, TargetType.enemyAOE, 2f, 1f);
        
        public static SkillData BeanPunch = new SkillData(13, "Bean Punch",
            "Very weak punch.",
            "high-punch", EffectType.damage, TargetType.enemyRandom, 0.1f, 0f);
        
        public static SkillData Electrocute = new SkillData(14, "Electrocute",
            "Puts enemy into an electric chair.",
            "lightning-arc", EffectType.damage, TargetType.enemyAOE, 1f, 1f);
        
        public static SkillData RealityShift = new SkillData(15, "Reality Shift",
            "Even the Rigwork Wizard cannot predict this manifest of unholy energy. (long cooldown) (SCOPE INCREASE).",
            "atomic-slashes", EffectType.disableGravity, TargetType.enemyAOE, 0f, 120f);
        
        public static SkillData MakesRules = new SkillData(16, "Makes Rules",
            "Makes all the rules.",
            "atomic-slashes", EffectType.disableGravity, TargetType.enemyAOE, 0f, 120f);
        
        public static SkillData RatNuke = new SkillData(17, "RatNuke",
            "Send in the rats",
            "rat-nuke", EffectType.ratNuke, TargetType.enemyAOE, 0.1f, 10f);
        
        public static SkillData Nibble = new SkillData(17, "Nibble",
            "Attacks using fists or fist-like limbs.",
            "shark-bite", EffectType.damage, TargetType.enemyRandom, 0.5f, 10f);
        
        public static SkillData LogDogFireballBarrage = new SkillData(18, "Log Dog Fireball Barrage",
            "Shoots fireballs from his mouth (ranged magical).",
            "basset-hound-head", EffectType.damage, TargetType.enemyAOE, 10f, 0f);
    }
}