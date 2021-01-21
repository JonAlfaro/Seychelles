using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BattleSceneUI : MonoBehaviour
{
    public Text PremiumCurrencyText;
    public GameObject[] GameOverUI;
    public Character[] Characters;
    public Image FollowerImage;

    private void Awake()
    {
        Assert.IsNotNull(PremiumCurrencyText);
        Assert.IsTrue(Characters.Length != 0);
    }

    private void Start()
    {
        SetFollower(null);
        UpdatePremiumCurrencyUIAmount();
        for (int i = 0; i < GameDataManager.Instance.GameData.SelectedCharacters.Length; i++)
        {
            Characters[i].SetCharacter(GameDataManager.Instance.GameData.SelectedCharacters[i]);
        }
    }

    public void OnCharacterAttacked(CharacterData characterData)
    {
        Characters.First(character => character?.CharacterData?.Id == characterData.Id)?.OnDamageTaken();
        SetGameOverUIActive();
    }

    public void SetGameOverUIActive()
    {
        bool allCharactersDead = Characters.All(character =>
            character.CharacterData == null || character.CharacterData.CurrentHealth <= 0);
        foreach (GameObject go in GameOverUI)
        {
            go.SetActive(allCharactersDead);
        }
    }

    public void ResetCharacterHealth()
    {
        foreach (Character character in Characters)
        {
            character.ResetHealth();
        }

        GameDataManager.Instance.Save();
    }

    public void UpdatePremiumCurrencyUIAmount()
    {
        PremiumCurrencyText.text =
            $"{GameDataManager.Instance.GameData.PremiumCurrency} {Constants.PremiumCurrencyName}";
    }

    public void SetFollower(Sprite sprite)
    {
        if (sprite == null)
        {
            FollowerImage.enabled = false;
        }
        else
        {
            FollowerImage.enabled = true;
            FollowerImage.preserveAspect = true;
            FollowerImage.sprite = sprite;
        }
    }
}