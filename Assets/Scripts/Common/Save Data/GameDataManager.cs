using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager instance;
    public static GameDataManager Instance => instance;
    public GameData GameData { get; private set; }

    public void Save()
    {
        SaveSystem.SaveGame(GameData);
    }

    public void AddUnlockedCharacter(CharacterData character)
    {
        if (GameData.UnlockedCharacters.Any(c => c?.Id == character.Id))
        {
            GameData.UnlockedCharacters.First(c => c?.Id == character.Id).AddDuplicate();
        }
        else if (GameData.SelectedCharacters.Any(c => c?.Id == character.Id))
        {
            GameData.SelectedCharacters.First(c => c?.Id == character.Id).AddDuplicate();
        }
        else
        {
            List<CharacterData> charactersList = GameData.UnlockedCharacters.ToList();
            charactersList.Add(character);
            GameData.UnlockedCharacters = charactersList.ToArray();
        }
    }

    public void AddPremiumCurrency(int amount)
    {
        GameData.PremiumCurrency += amount;
    }

    void Awake()
    {
        // Handle singleton instantiation
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        GameData = SaveSystem.LoadGame();
    }
}