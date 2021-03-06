﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    private int _absLevel=2;

    private bool isBoss; 
    // rat stuff
    private float ratsAttacking = 0;
    private bool playRat;
    public GameObject[] ratGO;

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

    private void SpawnRat()
    {
        var index = Random.Range(0, ratGO.Length - 1);
        var r = GameObject.Instantiate(ratGO[index]);
        r.transform.position = ratGO[index].transform.position;
        r.GetComponent<csRat>().Run();
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

        if (ratsAttacking >  0)
        {
            if (!playRat)
            {
                AudioManager.Instance.PlayRat();
            }

            playRat = true;
            
            ratsAttacking += Time.fixedDeltaTime;
            if (ratsAttacking > 0.8)
            {
                SpawnRat();
            }

            if (ratsAttacking > 1)
            {
                AttackRandomAlive((int)(_absLevel/2));
            }

            if (ratsAttacking > 10)
            {
                ratsAttacking = 0;
                playRat = false;
                AudioManager.Instance.StopRat();
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
        isBoss = false;
        _absLevel = (flrManager._level * 6) + (flrManager._floor);
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

                if (_absLevel > 18)
                {
                    currentMobs[i].MobInfo.Health = (int) ((currentMobs[i].MobInfo.Health) * (1 + ((_absLevel) * 1.7f)));
                    currentMobs[i].MobInfo.Damage =
                        (int) ((currentMobs[i].MobInfo.Damage) * (1 + ((_absLevel) * 0.01f)));
                }

                // Save record of max health
                currentMobs[i].MaxHealth = currentMobs[i].MobInfo.Health;

                _mobSpawnIndx = _mobSpawnIndx == mobSpawnPoints.Length - 1 ? _mobSpawnIndx = 0 : _mobSpawnIndx + 1;
            }
        }
        else
        {
            isBoss = true;
            var spawnPos = bossSpawnPoint.transform.position;
            spawnPos.x += adjustX;
            currentMobs.Add(new CurrentLevelMob(Instantiate(levelBoss, spawnPos, Quaternion.identity)));
            currentMobs[0].MobRef = Instantiate(currentMobs[0].MobInfo.MobBody,
                currentMobs[0].Mob.transform.position,
                Quaternion.identity);
            currentMobs[0].MobRef.transform.parent = currentMobs[0].Mob.transform;
            currentMobs[0].HealthBar = Instantiate(gameHealthBar, gameCanvas.transform);
            currentMobs[0].BoundingSize = currentMobs[0].MobRef.GetComponent<BoxCollider>().bounds.size;


            // Move health bar to correct location on Canvas
            // Only Fired Once, so it'll probably break if resolution changes mid-game
            var mobPos = currentMobs[0].MobRef.transform.position;
            currentMobs[0].HealthBar.transform.position = playerCam.WorldToScreenPoint(
                new Vector3(
                    mobPos.x,
                    mobPos.y + (currentMobs[0].BoundingSize.y / 2) + 0.5f,
                    mobPos.z
                )
            );

            currentMobs[0].HealthBar.enabled = true;

            currentMobs[0].HealthBar.gameObject.SetActive(true);

            if (_absLevel > 18)
            {
                currentMobs[0].MobInfo.Health = (int)((currentMobs[0].MobInfo.Health) * (1 + ((_absLevel)*3.1)));
                currentMobs[0].MobInfo.Damage = (int)((currentMobs[0].MobInfo.Damage) * (1 + ((_absLevel)*0.015f)));
            } 


            // Save record of max health
            currentMobs[0].MaxHealth = currentMobs[0].MobInfo.Health;

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
    
    public CurrentLevelMob GetRandomAlive()
    {
        var aliveIndex = GetAliveMobIndexes();
        if (aliveIndex.Count > 0)
            return currentMobs[aliveIndex[Random.Range(0, aliveIndex.Count)]];

        return null;
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
    
    public void RatNuke()
    {
        Debug.Log("Rat nuke");
        // currentMobs[mobIndex].MobInfo.gravity = true;
        autoAttackMgr.StopAutoAttacking();
        ratsAttacking = 0.1f;
    }
    
    public void DisableGravity(int mobIndex)
    {
        currentMobs[mobIndex].MobInfo.gravity = true;
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

            if (!isBoss && gold > 75)
            {
                gold = 75;
            }

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