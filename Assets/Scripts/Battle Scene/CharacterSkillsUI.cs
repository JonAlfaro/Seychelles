using System.IO;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CharacterSkillsUI : MonoBehaviour
{
    public BattleSceneUI BattleSceneUI;
    public Button[] SkillButtons;
    private UIDissolve[] skillButtonUIDissolves;

    private void Awake()
    {
        Assert.IsNotNull(BattleSceneUI);
    }

    void Start()
    {
        UpdateSkillIcons();

        skillButtonUIDissolves = new UIDissolve[SkillButtons.Length];
        for (int i = 0; i < SkillButtons.Length; i++)
        {
            skillButtonUIDissolves[i] = SkillButtons[i].GetComponent<UIDissolve>();
        }
    }

    private void UpdateSkillIcons()
    {
        for (int i = 0; i < SkillButtons.Length; i++)
        {
            if (BattleSceneUI.Characters[i] != null && BattleSceneUI.Characters[i].CharacterData.SkillData != null)
            {
                // TODO Resources.Load should be cached for performance. Ignoring it for now since it's not important on this project
                Sprite skillSprite = Resources.Load<Sprite>(Path.Combine(Constants.SkillResourceFolder,
                    BattleSceneUI.Characters[i].CharacterData.SkillData.IconName));
                SkillButtons[i].image.sprite = skillSprite;
            }
            
            SkillButtons[i].interactable = BattleSceneUI.Characters[i] != null
                                           && BattleSceneUI.Characters[i].CharacterData.CurrentHealth > 0;
        }
    }

    public void UpdateSkillIconsEnabled()
    {
        for (int i = 0; i < SkillButtons.Length; i++)
        {
            SkillButtons[i].interactable = BattleSceneUI.Characters[i] != null
                                           && BattleSceneUI.Characters[i].CharacterData.CurrentHealth > 0
                                           && BattleSceneUI.Characters[i].CharacterData.SkillData.CurrentCoolDown == 0;
        }
    }

    public void StartSkillCooldown(int index)
    {
        // Assumes the buttons and characters share the same index in their arrays
        skillButtonUIDissolves[index].effectPlayer.duration = BattleSceneUI.Characters[index].CharacterData.SkillData.CoolDown;
        skillButtonUIDissolves[index].Play(true);
    }
}
