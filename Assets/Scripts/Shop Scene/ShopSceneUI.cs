using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ShopSceneUI : MonoBehaviour
{
    public Text PremiumCurrencyText;
    public Text RollCostText;
    public Button RollButton;

    private void Awake()
    {
        Assert.IsNotNull(PremiumCurrencyText);
    }

    private void Start()
    {
        SetUpUI();
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
        }
    }

    public void FreeMoneyForTesting()
    {
        GameDataManager.Instance.GameData.PremiumCurrency += 10;
        GameDataManager.Instance.Save();
        SetUpUI();
    }

    private void SetUpUI()
    {
        RollButton.interactable = GameDataManager.Instance.GameData.PremiumCurrency >= Constants.GachaCost;
        PremiumCurrencyText.text = $"{Constants.PremiumCurrencyName}: {GameDataManager.Instance.GameData.PremiumCurrency}";
        RollCostText.text = $"-${Constants.GachaCost}";
    }
}
