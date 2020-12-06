[System.Serializable]
public class CharacterData
{
    public int Experience { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Attack { get; set; }
    public int Health { get; set; }
    public int CurrentHealth { get; set; }
    public Rarity Rarity { get; set; }
    public int DuplicateLevel { get; set; }

    public CharacterData(int id, int health, int attack, string name, string description, Rarity rarity)
    {
        Id = id;
        Name = name;
        Description = description;
        Attack = attack;
        Health = health;
        CurrentHealth = health;
        Rarity = rarity;
        Experience = 0;
        DuplicateLevel = 0;
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