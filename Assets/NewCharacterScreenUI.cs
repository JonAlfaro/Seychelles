using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class NewCharacterScreenUI : MonoBehaviour
{
    public Image CharacterImage;
    public Text CharacterNameText;
    private CharacterData NewCharacter;

    private void Awake()
    {
        Assert.IsNotNull(CharacterImage);
        Assert.IsNotNull(CharacterNameText);
    }

    public void SetCharacter(CharacterData character)
    {
        string imageResourceName = $"{character.Id.ToString()}{Constants.BremiumResourceSuffix}";
        // TODO Resources.Load should be cached for performance. Ignoring it for now since it's not important on this project
        Sprite characterSprite = Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, imageResourceName));
        CharacterImage.sprite = characterSprite;
        CharacterNameText.text = $"{character.Name} unlocked! Poggies!";
    }
}
