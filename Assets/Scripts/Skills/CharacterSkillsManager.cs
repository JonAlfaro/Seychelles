using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class CharacterSkillsManager : MonoBehaviour
{
    public MobManager MobManager;
    public BattleSceneUI BattleSceneUI;
    
    private void Awake()
    {
        Assert.IsNotNull(MobManager);
        Assert.IsNotNull(BattleSceneUI);
    }

    public void UseSkill(int index)
    {
        CharacterData activeCharacter = BattleSceneUI.Characters[index].CharacterData;
        
        float damage = activeCharacter.Attack 
                       * activeCharacter.SkillData.AttackMultiplier
                       * (activeCharacter.SkillData.EffectType == EffectType.heal ? -1 : 1);
        
        FindTargetsAndUseSkill(BattleSceneUI.Characters[index], Mathf.RoundToInt(damage));
    }
    
    private void FindTargetsAndUseSkill(Character activeCharacter, int damage)
    {
        switch (activeCharacter.CharacterData.SkillData.TargetType)
        {
            case TargetType.self:
                activeCharacter.Damage(damage);
                break;
            case TargetType.allyRandom:
                BattleSceneUI.Characters[Random.Range(0, BattleSceneUI.Characters.Length)].Damage(damage);
                break;
            case TargetType.enemyRandom:
                MobManager.AttackRandomAlive(damage);
                break;
            case TargetType.allyAOE:
                foreach (Character character in BattleSceneUI.Characters)
                {
                    character.Damage(damage);
                }
                break;
            case TargetType.enemyAOE:
                List<int> mobIndexes = MobManager.GetAliveMobIndexes();
                foreach (int mobIndex in mobIndexes)
                {
                    MobManager.AttackMob(mobIndex, damage);
                }
                break;
        }
    }
}
