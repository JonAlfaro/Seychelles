using System;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class CharacterData
{
    public int Experience { get; set; }
    public int ExperienceLevel { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Attack { get; set; }
    public int Health { get; set; }
    public int CurrentHealth { get; set; }
    public Rarity Rarity { get; set; }
    public int DuplicateLevel { get; set; }
    public SkillData SkillData { get; set; }

    public CharacterData(int id, int health, int attack, string name, string description, Rarity rarity, SkillData skillData)
    {
        Id = id;
        Name = name;
        Description = description;
        Attack = attack;
        Health = health;
        CurrentHealth = health;
        Rarity = rarity;
        SkillData = skillData;
        Experience = 0;
        ExperienceLevel = 0;
        DuplicateLevel = 0;
    }

    public void Damage(int damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, Health);
    }

    public void AddExperience(int experience)
    {
        Experience += experience;
        
        int experienceRequired = (ExperienceLevel+1) * 100;
        if (Experience >= experienceRequired)
        {
            Experience = experience % experienceRequired;
            LevelUp();
        }
    }

    public void AddDuplicate()
    {
        DuplicateLevel++;
        
        int attackGained = 10 * DuplicateLevel + Random.Range(0, 5);
        int healthGained = 10 * DuplicateLevel + Random.Range(0, 5);
        
        Attack += attackGained;
        Health += healthGained;
    }

    private void LevelUp()
    {
        ExperienceLevel++;
        int attackGained = ExperienceLevel + Random.Range(0, 3);
        int healthGained = ExperienceLevel + Random.Range(0, 3);
        
        Attack += attackGained;
        Health += healthGained;
    }
}

public enum Rarity
{
    frog,
    fish,
    knife,
    gun,
    doubleGun,
}