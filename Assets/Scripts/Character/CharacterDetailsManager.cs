using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CharacterDetailsManager : MonoBehaviour
{
    public Text PlayerNameText;
    public GameObject CharacterSelectScreen;
    public CharacterSelectUI[] characterSelectUIs;

    private void Awake()
    {
        Assert.IsNotNull(PlayerNameText);
        Assert.IsNotNull(CharacterSelectScreen);
    }

    private void Start()
    {
        GameData gameData = GameDataManager.Instance.GameData;
        
        PlayerNameText.text = gameData.Name;
    }

    public void OpenSelectCharacterScreen(int characterSlot)
    {
        CharacterSelectScreen.SetActive(true);
    }
    
    public void CloseSelectCharacterScreen()
    {
        CharacterSelectScreen.SetActive(false);
    }
}
