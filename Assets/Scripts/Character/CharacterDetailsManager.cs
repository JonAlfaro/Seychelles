using System.IO;
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
    public CharacterSelectUI[] characterSelectUIs;
    public GameObject CharacterSelectItemPrefab;
    
    private CharacterData selectedCharacter;

    private void Awake()
    {
        Assert.IsNotNull(PlayerNameText);
        Assert.IsNotNull(CharacterSelectScreen);
        Assert.IsNotNull(CharacterGrid);
        Assert.IsNotNull(SelectedCharacterNameText);
        Assert.IsNotNull(SelectedCharacterDescriptionText);
        Assert.IsNotNull(SelectCharacterButton);
    }

    private void Start()
    {
        GameData gameData = GameDataManager.Instance.GameData;
        
        PlayerNameText.text = gameData.Name;
        CreateCharactersList(CharacterGrid, gameData.Characters);
    }

    public void OpenSelectCharacterScreen(int characterSlot)
    {
        CharacterSelectScreen.SetActive(true);
    }
    
    public void CloseSelectCharacterScreen()
    {
        CharacterSelectScreen.SetActive(false);
    }

    private void CreateCharactersList(GameObject characterGrid, CharacterData[] characters)
    {
        foreach (CharacterData character in characters)
        {
            // Instantiate a character select item and add it to the grid
            GameObject go = Instantiate(CharacterSelectItemPrefab, characterGrid.transform, true);
            
            // Assign the sprite of this character based on its id
            Image characterImage = go.GetComponent<Image>();
            Sprite characterSprite = Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, character.Id.ToString()));
            characterImage.sprite = characterSprite;
            
            // Add an onclick event to this character select item
            Button button = go.GetComponent<Button>();
            button.onClick.AddListener(() => OnCharacterSelected(character));
        }
    }

    private void OnCharacterSelected(CharacterData character)
    {
        // TODO use this characters data
        // TODO also, make sure when I save I only save data that can change and use a mapping of id -> rest of the info
        selectedCharacter = character;
        SelectedCharacterNameText.text = "Test";
        SelectedCharacterDescriptionText.text = "Test desc";
        SelectCharacterButton.interactable = selectedCharacter != null;
    }
}
