using System;
using System.IO;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharacterData CharacterData;
    public Image CharacterImage;
    public UIShiny CharacterShiny;
    public Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();

        CharacterImage = GetComponent<Image>();
        Assert.IsNotNull(CharacterImage);
        Assert.IsNotNull(Animator);
        Assert.IsNotNull(CharacterShiny);
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
        CharacterImage.preserveAspect = true;
        string imageResourceName = $"{CharacterData.Id.ToString()}{Constants.BremiumResourceSuffix}";
        // TODO Resources.Load should be cached for performance. Ignoring it for now since it's not important on this project
        Sprite characterSprite =
            Resources.Load<Sprite>(Path.Combine(Constants.CharacterResourceFolder, imageResourceName));
        CharacterImage.sprite = characterSprite;

        if (CharacterData.DuplicateLevel >= 1)
        {
            CharacterShiny.Play();
            CharacterShiny.brightness = Mathf.Min(CharacterData.DuplicateLevel / 10f, 1);
            CharacterShiny.width = CharacterData.DuplicateLevel > 10 ? 0.6f : 0.3f;
        }
        else
        {
            CharacterShiny.Stop();
        }

        if (CharacterData.CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Damage(int damage)
    {
        if (CharacterData == null) return;

        CharacterData.Damage(damage);
        OnDamageTaken();
    }

    public void ResetHealth()
    {
        if (CharacterData == null) return;

        CharacterData.CurrentHealth = CharacterData.Health;
        Animator.Play("Character Idle");
    }

    public void OnDamageTaken()
    {
        if (CharacterData == null) return;

        if (CharacterData.CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            Animator.Play("Character Damage");
        }
    }

    public void StartSkillCooldown()
    {
        if (CharacterData == null) return;

        CharacterData.SkillData.CurrentCoolDown = CharacterData.SkillData.CoolDown;
    }

    private void Die()
    {
        if (CharacterData == null) return;

        Animator.Play("Character Death");
    }
}