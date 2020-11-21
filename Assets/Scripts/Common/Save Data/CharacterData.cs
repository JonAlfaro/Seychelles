[System.Serializable]
public class CharacterData
{
    public int CurrentHealth { get; set; }
    public int Experience { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CharacterStats CharacterStats { get; set; }

    public CharacterData(int id, int health, int attack, string name, string description)
    {
        Id = id;
        CharacterStats = new CharacterStats {Attack = attack, Health = health};
        CurrentHealth = health;
        Name = name;
        Description = description;
        Experience = 0;
    }
}