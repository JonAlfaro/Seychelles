using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class AutoAttackManager : MonoBehaviour
{
    public MobManager mobManager;
    public UnityEvent<CharacterData> OnAutoAttack = new UnityEvent<CharacterData>();
    private CharacterData[] Characters;
    private float AutoAttackInterval = 0.8f;
    private Coroutine autoAttackCoroutine;

    public int AttackingCharacterIndex { get; private set; } = 0;
    public int AttackingEnemyIndex { get; private set; } = 0;

    private void Awake()
    {
        Assert.IsNotNull(mobManager);
    }

    void Start()
    {
        Characters = GameDataManager.Instance.GameData.SelectedCharacters;
        autoAttackCoroutine = StartCoroutine(AutoAttack());
    }

    public void StartAutoAttacking()
    {
        if (autoAttackCoroutine != null)
        {
            StopCoroutine(autoAttackCoroutine);
        }
        
        autoAttackCoroutine = StartCoroutine(AutoAttack());
    }
    
    public void StopAutoAttacking()
    {
        StopCoroutine(autoAttackCoroutine);
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

            // Get the index for the next character that will attack
            AttackingCharacterIndex = GetNextCharacterToAttack(AttackingCharacterIndex);

            // Emit the OnAutoAttack event with the character that attacked
            OnAutoAttack.Invoke(attackingCharacter);
            
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
