using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class LevelMobs
{
    public GameObject Boss;
    public GameObject[] Mobs;
}

public class CurrentLevelMob
{
    public GameObject Mob;
    public Mob MobInfo;
    public Image HealthBar;
    public int MaxHealth;
    public Vector3 BoundingSize;
    public GameObject MobRef;
    private Vector3 _trueStartPosition;
    private Vector3 _startPosition;
    private float _step = 1f;

    public CurrentLevelMob(GameObject mob)
    {
        Mob = mob;
        var pos = Mob.transform.position;
        _startPosition = pos;
        pos.x -= 10;
        _trueStartPosition = pos;
        MobInfo = Mob.GetComponent<Mob>();
    }

    public bool Move()
    {
        if (_step < 0f)
            return false;
        Vector3 interpolatedPosition = Vector3.Lerp(_trueStartPosition, _startPosition, _step);
        _step -= 0.01f;
        Mob.transform.position = interpolatedPosition;
        return true;
    }
}

public class MobManager : MonoBehaviour
{
    public AutoAttackManager autoAttackMgr;
    public GameObject charactersUI;
    public GameObject coin;

    public LevelMobs[] MobsPerLevel = { };
    public GameObject[] levelMobs = { };
    public GameObject levelBoss;
    public FloorManager flrManager;
    public GameObject[] mobSpawnPoints = { };
    public GameObject bossSpawnPoint;
    private int _mobSpawnIndx;
    public List<CurrentLevelMob> currentMobs = new List<CurrentLevelMob>();

    public Camera playerCam;
    public Canvas gameCanvas;
    public Image gameHealthBar;

    public UnityEvent OnMobKilled;

    // Start is called before the first frame update
    void Awake()
    {
        levelMobs = MobsPerLevel[(flrManager._level - 1) % MobsPerLevel.Length].Mobs;
        levelBoss = MobsPerLevel[(flrManager._level - 1) % MobsPerLevel.Length].Boss;
        SpawnMobs(10);
        flrManager.currentFloor.panning = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Start()
    {
    }

    void FixedUpdate()
    {
        UpdateHealthBarPos();
        if (flrManager.currentFloor.panning)
        {
            flrManager.currentFloor.panning = false;
            foreach (var cMob in currentMobs)
            {
                if (cMob.Move())
                    flrManager.currentFloor.panning = true;
            }

            // Resume Auto Attack, after pan
            if (!flrManager.currentFloor.panning)
            {
                autoAttackMgr.StartAutoAttacking();
            }
        }
    }

    public void SetFloorMobs(float adjustX)
    {
        // Debug.Log("flrManager._level-1 % MobsPerLevel.Length = "+ (flrManager._level-1) % MobsPerLevel.Length);
        SetFloorMobsAndLevel(adjustX, flrManager._level);
    }

    public void SetFloorMobsAndLevel(float adjustX, int level)
    {
        levelMobs = MobsPerLevel[(level) % MobsPerLevel.Length].Mobs;
        levelBoss = MobsPerLevel[(level) % MobsPerLevel.Length].Boss;
        SpawnMobs(adjustX);
    }

    public void HandleFloorChange()
    {
        SpawnMobs(10);
        flrManager.currentFloor.panning = true;
    }

    public void HandleLevelChange()
    {
        // -1 because I'm dumb and levels start at 1
        levelMobs = MobsPerLevel[(flrManager._level - 1) % MobsPerLevel.Length].Mobs;
        levelBoss = MobsPerLevel[(flrManager._level - 1) % MobsPerLevel.Length].Boss;
        SpawnMobs(10);
        flrManager.currentFloor.panning = true;
    }

    public void SpawnMobs(float adjustX)
    {
        // Destroy Exisiting gb
        foreach (var mob in currentMobs)
        {
            Destroy(mob.HealthBar.gameObject);
            Destroy(mob.Mob.gameObject);
        }

        // Clear list
        currentMobs = new List<CurrentLevelMob>();

        // Not Boss
        if (flrManager._floor < 6)
        {
            for (int i = 0; i < flrManager._ePerFloor[flrManager._floor - 1]; i++)
            {
                var mob = levelMobs[Random.Range(0, levelMobs.Length)];
                var spawnPos = mobSpawnPoints[_mobSpawnIndx].transform.position;
                spawnPos.x += adjustX;
                currentMobs.Add(new CurrentLevelMob(Instantiate(mob, spawnPos, Quaternion.identity)));
                currentMobs[i].MobRef = Instantiate(currentMobs[i].MobInfo.MobBody,
                    currentMobs[i].Mob.transform.position,
                    Quaternion.identity);
                currentMobs[i].MobRef.transform.parent = currentMobs[i].Mob.transform;
                currentMobs[i].MobRef.GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
                currentMobs[i].HealthBar = Instantiate(gameHealthBar, gameCanvas.transform);
                currentMobs[i].BoundingSize = currentMobs[i].MobRef.GetComponent<BoxCollider>().bounds.size;


                // Move health bar to correct location on Canvas
                // Only Fired Once, so it'll probably break if resolution changes mid-game
                var mobPos = currentMobs[i].MobRef.transform.position;
                currentMobs[i].HealthBar.transform.position = playerCam.WorldToScreenPoint(
                    new Vector3(
                        mobPos.x,
                        mobPos.y + (currentMobs[i].BoundingSize.y / 2) + 0.5f,
                        mobPos.z
                    )
                );


                currentMobs[i].HealthBar.enabled = true;

                currentMobs[i].HealthBar.gameObject.SetActive(true);

                // Save record of max health
                currentMobs[i].MaxHealth = currentMobs[i].MobInfo.Health;

                _mobSpawnIndx = _mobSpawnIndx == mobSpawnPoints.Length - 1 ? _mobSpawnIndx = 0 : _mobSpawnIndx + 1;
            }
        }
        else
        {
            var spawnPos = bossSpawnPoint.transform.position;
            spawnPos.x += adjustX;
            currentMobs.Add(new CurrentLevelMob(Instantiate(levelBoss, spawnPos, Quaternion.identity)));
            currentMobs.Last().MobRef = Instantiate(currentMobs.Last().MobInfo.MobBody,
                currentMobs.Last().Mob.transform.position,
                Quaternion.identity);
            currentMobs.Last().MobRef.transform.parent = currentMobs.Last().Mob.transform;
            currentMobs.Last().HealthBar = Instantiate(gameHealthBar, gameCanvas.transform);
            currentMobs.Last().BoundingSize = currentMobs.Last().MobRef.GetComponent<BoxCollider>().bounds.size;


            // Move health bar to correct location on Canvas
            // Only Fired Once, so it'll probably break if resolution changes mid-game
            var mobPos = currentMobs.Last().MobRef.transform.position;
            currentMobs.Last().HealthBar.transform.position = playerCam.WorldToScreenPoint(
                new Vector3(
                    mobPos.x,
                    mobPos.y + (currentMobs.Last().BoundingSize.y / 2) + 0.5f,
                    mobPos.z
                )
            );

            currentMobs.Last().HealthBar.enabled = true;

            currentMobs.Last().HealthBar.gameObject.SetActive(true);

            // Save record of max health
            currentMobs.Last().MaxHealth = currentMobs.Last().MobInfo.Health;

            // currentMobs.Last().MobRef.GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
        }
    }

    public void UpdateHealthBarPos()
    {
        foreach (var cMob in currentMobs)
        {
            var mobPos = cMob.MobRef.transform.position;
            cMob.HealthBar.transform.position = playerCam.WorldToScreenPoint(
                new Vector3(
                    mobPos.x,
                    mobPos.y + (cMob.BoundingSize.y / 2) + 0.5f,
                    mobPos.z
                )
            );
        }
    }

    public void AttackRandomAlive(int dmg)
    {
        var aliveIndex = GetAliveMobIndexes();
        if (aliveIndex.Count > 0)
            AttackMob(aliveIndex[Random.Range(0, aliveIndex.Count)], dmg);
    }

    public List<int> GetAliveMobIndexes()
    {
        List<int> aliveIndex = new List<int>();
        for (int i = 0; i < currentMobs.Count; i++)
        {
            if (currentMobs[i].MobInfo.Health > 0)
                aliveIndex.Add(i);
        }

        return aliveIndex;
    }
    
    public void DisableGravity(int mobIndex)
    {
        
    }

    public void AttackMob(int mobIndex, int dmg)
    {
        currentMobs[mobIndex].MobInfo.Health -= dmg;
        if (currentMobs[mobIndex].MobInfo.Health <= 0)
        {
            currentMobs[mobIndex].MobInfo.Health = 0;
            currentMobs[mobIndex].MobInfo.fadeAway = true;

            // Mob is dead, handle gold and exp
            var goldMultiplier = Random.Range(0.5f, 1.5f);
            var gold = currentMobs[mobIndex].MaxHealth * goldMultiplier;

            var expMultiplier = Random.Range(0.5f, 1.5f);
            var exp = currentMobs[mobIndex].MaxHealth * expMultiplier;

            foreach (var character in charactersUI.GetComponentsInChildren<Character>())
            {
                character.CharacterData?.AddExperience((int) exp + 50);
            }

            GameDataManager.Instance.AddPremiumCurrency((int) gold + 1);

            for (int i = 0; i < (int) (expMultiplier / 0.2); i++)
            {
                Instantiate(coin, currentMobs[mobIndex].MobRef.transform.position, Quaternion.identity)
                    .GetComponent<coin>().SetSize(gold / 100);
            }

            OnMobKilled.Invoke();
        }

        currentMobs[mobIndex].MobInfo.Shake();
        float healthMissing = currentMobs[mobIndex].MaxHealth - currentMobs[mobIndex].MobInfo.Health;
        currentMobs[mobIndex].HealthBar.fillAmount = 1f - (healthMissing / currentMobs[mobIndex].MaxHealth);

        CheckNewFloor();
    }

    public void CheckNewFloor()
    {
        if (GetAliveMobIndexes().Count > 0)
            return;

        autoAttackMgr.StopAutoAttacking();
        flrManager.NextFloor();
    }
}