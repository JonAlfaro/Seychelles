using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CharacterDetailsManager : MonoBehaviour
{
    public Text PlayerNameText;

    private void Awake()
    {
        Assert.IsNotNull(PlayerNameText);
    }

    private void Start()
    {
        GameData gameData = GameDataManager.Instance.GameData;
        
        PlayerNameText.text = gameData.Name;
    }
}
