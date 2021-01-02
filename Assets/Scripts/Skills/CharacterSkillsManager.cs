using UnityEngine;
using UnityEngine.Assertions;

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
        // Get targets based on target type
        
        // Get damage amount by character.ATK * skill.atkMultiplier * (skill.damageType == heal ? -1 : 1)
        
        // Loop through targets array
        
        // Modify health
        
        // TODO Find a way to animate them all
    }
    
    // TODO return array of mobs or characters, throw an interface on both of em
    public void GetTargets(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.self:
                break;
            case TargetType.allyRandom:
                break;
            case TargetType.enemyRandom:
                break;
            case TargetType.allyAOE:
                break;
            case TargetType.enemyAOE:
                break;
        }
    }
}
