[System.Serializable]
public class GameData
{
    public CharacterData[] SelectedCharacters { get; set; } = new CharacterData[4];
    public CharacterData[] UnlockedCharacters { get; set; }
    public string Name { get; set; }
    public int PremiumCurrency { get; set; }
}