using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class AutoAttackManager : MonoBehaviour
{
    public MobManager mobManager;
    public UnityEvent OnAutoAttack = new UnityEvent();
    private CharacterData[] Characters;
    private float AutoAttackInterval = 0.5f;

    public int AttackingCharacterIndex { get; private set; } = 0;
    public int AttackingEnemyIndex { get; private set; } = 0;

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
                // Character attacks random enemy
                mobManager.AttackRandomAlive(attackingCharacter.Attack);
                // Enemy attacks character
                AttackingEnemyIndex = GetNextEnemyToAttack(AttackingEnemyIndex);
                attackingCharacter.CurrentHealth -= mobManager.currentMobs[AttackingEnemyIndex].MobInfo.Damage;
            }

            AttackingCharacterIndex = GetNextCharacterToAttack(AttackingCharacterIndex);

            OnAutoAttack.Invoke();
            
            yield return new WaitForSeconds(AutoAttackInterval);
        }
    }

    private int GetNextCharacterToAttack(int currentAttackingCharacterIndex)
    {
        while (true)
        {
            currentAttackingCharacterIndex++;
            
            if (currentAttackingCharacterIndex >= Characters.Length)
            {
                return Array.FindIndex(Characters, character => character != null && character.CurrentHealth > 0);
            }

            if (Characters[currentAttackingCharacterIndex] != null)
            {
                return currentAttackingCharacterIndex;
            }
        }
    }

    private int GetNextEnemyToAttack(int currentAttackingEnemyIndex)
    {
        while (true)
        {
            currentAttackingEnemyIndex++;
            
            if (currentAttackingEnemyIndex >= mobManager.currentMobs.Count)
            {
                return mobManager.GetAliveMobIndexes().First();
            }

            if (mobManager.currentMobs[currentAttackingEnemyIndex] != null 
                && mobManager.currentMobs[currentAttackingEnemyIndex].MobInfo.Health > 0)
            {
                return currentAttackingEnemyIndex;
            }
        }
    }
}
