using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class NewCharacterScreenUI : MonoBehaviour
{
    public Image CharacterImage;
    public Image[] MultiCharacterImages;
    public Text CharacterNameText;
    private CharacterData NewCharacter;

    private void Awake()
    {
        Assert.IsNotNull(CharacterImage);
        Assert.IsTrue(MultiCharacterImages.Length == 10);
        Assert.IsNotNull(CharacterNameText);
    }

    public void SetCharacter(CharacterData character)
    {
        CharacterImage.gameObject.SetActive(true);
        foreach (Image image in MultiCharacterImages)
        {
            image.gameObject.SetActive(false);
        }
        
        string imageResourceName = $"{character.Id.ToString()}{Constants.BremiumResourceSuffix}";
        // TODO Resources.Load should be cached for performance. Ignoring it for now since it's not important on this project
        Sprite characterSprite =
            Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, imageResourceName));
        CharacterImage.preserveAspect = true;
        CharacterImage.sprite = characterSprite;
        CharacterNameText.text = $"{character.Name} unlocked! Poggies!";
    }

    public void SetCharacters(CharacterData[] characters)
    {
        CharacterImage.gameObject.SetActive(false);
        for (int i = 0; i < MultiCharacterImages.Length; i++)
        {
            MultiCharacterImages[i].gameObject.SetActive(true);
            
            string imageResourceName = $"{characters[i].Id.ToString()}{Constants.BremiumResourceSuffix}";
            // TODO Resources.Load should be cached for performance. Ignoring it for now since it's not important on this project
            Sprite characterSprite =
                Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, imageResourceName));
            MultiCharacterImages[i].preserveAspect = true;
            MultiCharacterImages[i].sprite = characterSprite;
        }
        
    }
}