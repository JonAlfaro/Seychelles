using UnityEngine;
using UnityEngine.Assertions;

public class Character : MonoBehaviour
{
    public CharacterData CharacterData;
    public SpriteRenderer CharacterSprite;

    private void Awake()
    {
        CharacterSprite = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(CharacterSprite);
    }

    public void SetCharacter(CharacterData data)
    {
        // TODO get character image from data.Id. This might need to be reworked a little

        CharacterData = data;
        
        if (CharacterData.CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        CharacterSprite.color = Constants.DeadColor;
    }
}
