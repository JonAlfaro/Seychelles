using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ShopSceneUI : MonoBehaviour
{
    public Text PremiumCurrencyText;
    public Text RollCostText;
    public Text TotalUnlockedCharactersText;
    public Button RollButton;
    public NewCharacterScreenUI NewCharacterScreen;

    private void Awake()
    {
        Assert.IsNotNull(PremiumCurrencyText);
        Assert.IsNotNull(TotalUnlockedCharactersText);
        Assert.IsNotNull(RollCostText);
        Assert.IsNotNull(RollButton);
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

    private void SetUpUI()
    {
        RollButton.interactable = GameDataManager.Instance.GameData.PremiumCurrency >= Constants.GachaCost;
        PremiumCurrencyText.text =
            $"{GameDataManager.Instance.GameData.PremiumCurrency} {Constants.PremiumCurrencyName}";
        RollCostText.text = $"${Constants.GachaCost}";
        
        int unlockedCharacterCount = GameDataManager.Instance.GameData.UnlockedCharacters.Length +
                                     GameDataManager.Instance.GameData.SelectedCharacters.Count(character => character != null);
        TotalUnlockedCharactersText.text = $"Unlocked Frogs: {unlockedCharacterCount}/{CharacterList.AllCharacters.Count}";
    }
}