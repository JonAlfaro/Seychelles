using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterData CharacterData;
    public Image CharacterImage;

    private void Awake()
    {
        CharacterImage = GetComponent<Image>();
        Assert.IsNotNull(CharacterImage);
    }

    public void SetCharacter(CharacterData data)
    {
        CharacterData = data;

        if (CharacterData == null)
        {
            CharacterImage.enabled = false;
            return;
        }
        
        CharacterImage.enabled = true;
        string imageResourceName = $"{CharacterData.Id.ToString()}{Constants.BremiumResourceSuffix}";
        // TODO Resources.Load should be cached for performance. Ignoring it for now since it's not important on this project
        Sprite characterSprite = Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, imageResourceName));
        CharacterImage.sprite = characterSprite;

        if (CharacterData.CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        CharacterImage.color = Constants.DeadColor;
    }
}
