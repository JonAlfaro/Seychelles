[System.Serializable]
public class CharacterData
{
    public int CurrentHealth { get; set; }
    public int Experience { get; set; }
    public int Id { get; set; }
    public CharacterStats CharacterStats { get; set; }

    public CharacterData(int id, int health, int attack)
    {
        Id = id;
        CharacterStats = new CharacterStats {Attack = attack, Health = health};
        CurrentHealth = health;
        Experience = 0;
    }
}