using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BattleSceneUI : MonoBehaviour
{
    public Text PremiumCurrencyText;

    private void Awake()
    {
        Assert.IsNotNull(PremiumCurrencyText);
    }

    private void Start()
    {
        PremiumCurrencyText.text = $"{GameDataManager.Instance.GameData.PremiumCurrency} {Constants.PremiumCurrencyName}";
    }
}
