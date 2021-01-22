using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ShopSceneUI : MonoBehaviour
{
    public Text PremiumCurrencyText;
    public Text RollCostText;
    public Text TenRollCostText;
    public Text TotalUnlockedCharactersText;
    public Button RollButton;
    public Button TenRollButton;
    public NewCharacterScreenUI NewCharacterScreen;

    private void Awake()
    {
        Assert.IsNotNull(PremiumCurrencyText);
        Assert.IsNotNull(TotalUnlockedCharactersText);
        Assert.IsNotNull(RollCostText);
        Assert.IsNotNull(TenRollCostText);
        Assert.IsNotNull(RollButton);
        Assert.IsNotNull(TenRollButton);
        Assert.IsNotNull(NewCharacterScreen);
    }

    private void Start()
    {
        SetUpUI();
        AudioManager.Instance.PlayShopEnterClip();
    }

    private void OnDestroy()
    {
        AudioManager.Instance.PlayShopLeaveClip();
    }

    public void Roll()
    {
        if (GameDataManager.Instance.GameData.PremiumCurrency >= Constants.GachaCost)
        {
            GameDataManager.Instance.GameData.PremiumCurrency -= Constants.GachaCost;
            CharacterData unlockedCharacter = CharacterList.GetRandomCharacter();

            GameDataManager.Instance.AddUnlockedCharacter(unlockedCharacter);
            GameDataManager.Instance.Save();
            SetUpUI();
            ShowNewCharacterScreen(unlockedCharacter);
        }
    }
    
    public void Roll10()
    {
        if (GameDataManager.Instance.GameData.PremiumCurrency >= Constants.GachaCost * 10)
        {
            GameDataManager.Instance.GameData.PremiumCurrency -= Constants.GachaCost * 10;
            CharacterData[] unlockedCharacters =
            {
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
                CharacterList.GetRandomCharacter(),
            };
            
            foreach (CharacterData character in unlockedCharacters)
            {
                GameDataManager.Instance.AddUnlockedCharacter(character);
            }
            GameDataManager.Instance.Save();
            SetUpUI();
            ShowNewCharactersScreen(unlockedCharacters);
        }
    }

    public void FreeMoneyForTesting()
    {
        GameDataManager.Instance.GameData.PremiumCurrency += 10;
        GameDataManager.Instance.Save();
        SetUpUI();
    }

    public void HideNewCharacterScreen()
    {
        NewCharacterScreen.gameObject.SetActive(false);
    }

    private void ShowNewCharacterScreen(CharacterData character)
    {
        NewCharacterScreen.SetCharacter(character);
        NewCharacterScreen.gameObject.SetActive(true);
    }
    
    private void ShowNewCharactersScreen(CharacterData[] characters)
    {
        NewCharacterScreen.SetCharacters(characters);
        NewCharacterScreen.gameObject.SetActive(true);
    }

    private void SetUpUI()
    {
        RollButton.interactable = GameDataManager.Instance.GameData.PremiumCurrency >= Constants.GachaCost;
        TenRollButton.interactable = GameDataManager.Instance.GameData.PremiumCurrency >= Constants.GachaCost * 10;
        PremiumCurrencyText.text =
            $"{GameDataManager.Instance.GameData.PremiumCurrency} {Constants.PremiumCurrencyName}";
        RollCostText.text = $"${Constants.GachaCost}";
        TenRollCostText.text = $"${Constants.GachaCost * 10}";
        
        int unlockedCharacterCount = GameDataManager.Instance.GameData.UnlockedCharacters.Length +
                                     GameDataManager.Instance.GameData.SelectedCharacters.Count(character => character != null);
        TotalUnlockedCharactersText.text = $"Unlocked Frogs: {unlockedCharacterCount}/{CharacterList.AllCharacters.Count}";
    }
}