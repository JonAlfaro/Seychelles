using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public Text InputNameText;
    public Text ErrorText;
    public string PleaseEnterANameErrorMessage = "Please enter a valid frog name!";
    public string NextSceneName = Constants.BattleSceneName;
    public GameObject NameInputMenu;
    private ChangeScene ChangeScene;

    private void Awake()
    {
        ChangeScene = GetComponent<ChangeScene>();
        Assert.IsNotNull(ChangeScene);
        Assert.IsNotNull(NameInputMenu);
    }

    void Start()
    {
        string existingPlayerName = GameDataManager.Instance.GameData.Name;

        if (existingPlayerName != null)
        {
            ChangeScene.ChangeSceneTo(NextSceneName);
        }
        else
        {
            NameInputMenu.SetActive(true);
        }
    }

    public void SubmitName()
    {
        if (InputNameText.text.Length == 0)
        {
            ErrorText.text = PleaseEnterANameErrorMessage;
            return;
        }

        GameDataManager.Instance.GameData.Name = InputNameText.text;
        GameDataManager.Instance.GameData.SelectedCharacters = Constants.StarterSelectedCharacters;
        GameDataManager.Instance.GameData.UnlockedCharacters = Constants.StarterUnlockedCharacters;
        GameDataManager.Instance.GameData.PremiumCurrency = Constants.StartingPremiumCurrencyAmount;
        GameDataManager.Instance.Save();

        ChangeScene.ChangeSceneTo(NextSceneName);
    }

    public void ClearErrorMessage()
    {
        ErrorText.text = "";
    }
}