using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CharacterDetailsManager : MonoBehaviour
{
    public Text PlayerNameText;
    public Text SelectedCharacterNameText;
    public Text SelectedCharacterDescriptionText;
    public Button SelectCharacterButton;
    public GameObject CharacterSelectScreen;
    public GameObject CharacterGrid;
    public CharacterSelectUI[] CharacterSelectUIs;
    public GameObject CharacterSelectItemPrefab;
    public Sprite NoCharacterSelectedImage;
    
    private CharacterData selectedCharacter;
    private int selectedCharacterSlot;

    private void Awake()
    {
        Assert.IsNotNull(PlayerNameText);
        Assert.IsNotNull(CharacterSelectScreen);
        Assert.IsNotNull(CharacterGrid);
        Assert.IsNotNull(SelectedCharacterNameText);
        Assert.IsNotNull(SelectedCharacterDescriptionText);
        Assert.IsNotNull(SelectCharacterButton);
        Assert.IsNotNull(NoCharacterSelectedImage);
        Assert.IsTrue(CharacterSelectUIs != null && CharacterSelectUIs.Length != 0);
    }

    private void Start()
    {
        PlayerNameText.text = GameDataManager.Instance.GameData.Name;
        SetCharacterSelectUIs();
    }

    public void OpenSelectCharacterScreen(int characterSlot)
    {
        // Clear the selected character if there was one
        OnCharacterSelected(null);
        selectedCharacterSlot = characterSlot;
        CreateCharacterGrid(CharacterGrid, GameDataManager.Instance.GameData.UnlockedCharacters);
        
        CharacterSelectScreen.SetActive(true);
    }
    
    public void CloseSelectCharacterScreen()
    {
        GameData gameData = GameDataManager.Instance.GameData;
        
        // Put the previously selected character into the unlocked list before we remove it from the selected list
        int newlySelectedCharacterIndex = Array.FindIndex(gameData.UnlockedCharacters, character => character.Id == selectedCharacter.Id);
        CharacterData currentCharacter = gameData.SelectedCharacters[selectedCharacterSlot];
        gameData.UnlockedCharacters[newlySelectedCharacterIndex] = currentCharacter;

        // Put the selected character into the selected characters list
        gameData.SelectedCharacters[selectedCharacterSlot] = selectedCharacter;

        // Remove null characters in the case that the previously selected character was null
        gameData.UnlockedCharacters = gameData.UnlockedCharacters.Where(character => character != null).ToArray();
        
        GameDataManager.Instance.Save();

        SetCharacterSelectUIs();
        
        CharacterSelectScreen.SetActive(false);
    }

    public void CancelSelectCharacterScreen()
    {
        CharacterSelectScreen.SetActive(false);
    }

    private void CreateCharacterGrid(GameObject characterGrid, CharacterData[] characters)
    {
        // Destroy any existing character list items
        Image[] existingCharactersList = characterGrid.GetComponentsInChildren<Image>();
        foreach (Image image in existingCharactersList)
        {
            Destroy(image.gameObject);
        }
        
        // Create the list of character select items
        foreach (CharacterData character in characters)
        {
            // Instantiate a character select item and add it to the grid
            GameObject go = Instantiate(CharacterSelectItemPrefab, characterGrid.transform, true);
            
            // Assign the sprite of this character based on its id
            Image characterImage = go.GetComponent<Image>();
            string imageResourceName = $"{character.Id.ToString()}{Constants.BremiumResourceSuffix}";
            // TODO Resources.Load should be cached for performance. Ignoring it for now since it's not important on this project
            Sprite characterSprite = Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, imageResourceName));
            characterImage.sprite = characterSprite;
            
            // Add an onClick event to this character select item
            Button button = go.GetComponent<Button>();
            button.onClick.AddListener(() => OnCharacterSelected(character));
        }
    }

    private void OnCharacterSelected(CharacterData character)
    {
        selectedCharacter = character;
        SelectedCharacterNameText.text = character?.Name;
        SelectedCharacterDescriptionText.text = character?.Description;
        SelectCharacterButton.interactable = selectedCharacter != null;
    }

    private void SetCharacterSelectUIs()
    {
        for (int i = 0; i < CharacterSelectUIs.Length; i++)
        {
            CharacterData character = GameDataManager.Instance.GameData.SelectedCharacters.Length > i
                ? GameDataManager.Instance.GameData.SelectedCharacters[i]
                : null;
            
            SetSelectedCharacterUI(CharacterSelectUIs[i], character);
        }
    }
    
    private void SetSelectedCharacterUI(CharacterSelectUI characterSelectUI, CharacterData characterData)
    {
        if (characterData == null)
        {
            characterSelectUI.Image.sprite = NoCharacterSelectedImage;
            characterSelectUI.AttackText.text = "";
            characterSelectUI.HealthText.text = "";
            characterSelectUI.NameText.text = "Select A Frog";
            return;
        }
        string imageResourceName = $"{characterData.Id.ToString()}{Constants.BremiumResourceSuffix}";
        characterSelectUI.Image.sprite = Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, imageResourceName));
        characterSelectUI.AttackText.text = $"ATK: {characterData.CharacterStats.Attack.ToString()}";
        characterSelectUI.HealthText.text = $"HP: {characterData.CharacterStats.Health.ToString()}";
        characterSelectUI.NameText.text = characterData.Name;
    }
}
