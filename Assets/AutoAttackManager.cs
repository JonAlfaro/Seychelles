using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class AutoAttackManager : MonoBehaviour
{
    public MobManager mobManager;
    private CharacterData[] Characters;
    private float AutoAttackInterval = 0.5f;

    private int AttackingCharacterIndex = 0;
    private int AttackingEnemyIndex = 0;

    private void Awake()
    {
        Assert.IsNotNull(mobManager);
    }

    void Start()
    {
        Characters = GameDataManager.Instance.GameData.SelectedCharacters;
        StartCoroutine(AutoAttack());
    }

    public void StartAutoAttacking()
    {
        StopCoroutine(AutoAttack());
        StartCoroutine(AutoAttack());
    }
    
    public void StopAutoAttacking()
    {
        StopCoroutine(AutoAttack());
    }

    IEnumerator AutoAttack()
    {
        while (Characters.Any(character => character != null && character.CurrentHealth > 0))
        {
            CharacterData attackingCharacter = Characters[AttackingCharacterIndex];
            if (attackingCharacter != null)
            {
                mobManager.AttackMob(AttackingEnemyIndex, attackingCharacter.Attack);
                attackingCharacter.CurrentHealth -= mobManager.currentMobs[AttackingEnemyIndex].MobInfo.Damage;
            }

            AttackingCharacterIndex++;
            AttackingEnemyIndex++;

            if (AttackingCharacterIndex >= Characters.Length)
            {
                AttackingCharacterIndex = 0;
            }
            
            if (AttackingEnemyIndex >= mobManager.currentMobs.Count)
            {
                AttackingEnemyIndex = 0;
            }

            // TODO Emit an event that the attack has happened so that we can update if characters have died etc.
            
            yield return new WaitForSeconds(AutoAttackInterval);
        }
    }
}
