using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BattleSceneUI : MonoBehaviour
{
    public Text PremiumCurrencyText;
    public Character[] Characters;

    private void Awake()
    {
        Assert.IsNotNull(PremiumCurrencyText);
        Assert.IsTrue(Characters.Length != 0);
    }

    private void Start()
    {
        PremiumCurrencyText.text = $"{GameDataManager.Instance.GameData.PremiumCurrency} {Constants.PremiumCurrencyName}";
        for (int i = 0; i < GameDataManager.Instance.GameData.SelectedCharacters.Length; i++)
        {
            Characters[i].SetCharacter(GameDataManager.Instance.GameData.SelectedCharacters[i]);
        }
    }

    public void OnCharacterAttacked(CharacterData characterData)
    {
        Characters.First(character => character?.CharacterData?.Id == characterData.Id)?.OnDamageTaken();
    }
}
