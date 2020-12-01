using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ShopSceneUI : MonoBehaviour
{
    public Text PremiumCurrencyText;

    private void Awake()
    {
        Assert.IsNotNull(PremiumCurrencyText);
    }

    private void Start()
    {
        PremiumCurrencyText.text = $"{Constants.PremiumCurrencyName}: {GameDataManager.Instance.GameData.PremiumCurrency}";
    }
}
