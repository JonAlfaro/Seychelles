using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[System.Serializable]
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

    public CurrentLevelMob(GameObject mob)
    {
        Mob = mob;
        MobInfo = Mob.GetComponent<Mob>();
    }
}

public class MobManager : MonoBehaviour
{
    public LevelMobs[] MobsPerLevel = new LevelMobs[] {};
    public GameObject[] levelMobs = new GameObject[] {};
    public GameObject levelBoss;
    public FloorManager flrManager;
    public GameObject[] mobSpawnPoints = new GameObject[] { };
    public GameObject bossSpawnPoint;
    private int _mobSpawnIndx = 0;
    public List<CurrentLevelMob> currentMobs = new List<CurrentLevelMob>();

    public Camera playerCam;
    public Canvas gameCanvas;
    public Image gameHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        SetFloorMobs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFloorMobs()
    {
        Debug.Log("flrManager._level-1 % MobsPerLevel.Length = "+ (flrManager._level-1) % MobsPerLevel.Length);
        levelMobs = MobsPerLevel[(flrManager._level-1) % MobsPerLevel.Length].Mobs;
        levelBoss = MobsPerLevel[(flrManager._level-1) % MobsPerLevel.Length].Boss;
        SpawnMobs();
    }

    public void SpawnMobs()
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
            for (int i = 0; i < flrManager._ePerFloor[flrManager._floor-1]; i++)
            {
                var mob = levelMobs[Random.Range(0, levelMobs.Length)];
                currentMobs.Add(new CurrentLevelMob(Instantiate(mob, mobSpawnPoints[_mobSpawnIndx].transform.position, Quaternion.identity)));
                var mobBody = Instantiate(currentMobs[i].MobInfo.MobBody, currentMobs[i].Mob.transform.position,
                    Quaternion.identity);
                mobBody.transform.parent = currentMobs[i].Mob.transform;
                mobBody.GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
                currentMobs[i].HealthBar = Instantiate(gameHealthBar, gameCanvas.transform);
                
                // Move health bar to correct location on Canvas
                // Only Fired Once, so it'll probably break if resolution changes mid-game
                var mobPos = mobBody.transform.position;
                currentMobs[i].HealthBar.transform.position = playerCam.WorldToScreenPoint(
                    new Vector3(
                        mobPos.x,
                        mobPos.y + (mobBody.GetComponent<BoxCollider>().bounds.size.y/2) +0.5f,
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
            currentMobs.Add(new CurrentLevelMob(Instantiate(levelBoss, bossSpawnPoint.transform.position, Quaternion.identity)));
            var mobBody = Instantiate(levelBoss.GetComponent<Mob>().MobBody, levelBoss.transform.position,
                Quaternion.identity);
            mobBody.transform.parent = currentMobs.Last().Mob.transform;
            currentMobs.Last().HealthBar = Instantiate(gameHealthBar, gameCanvas.transform);
                
            // Move health bar to correct location on Canvas
            // Only Fired Once, so it'll probably break if resolution changes mid-game
            var mobPos = mobBody.transform.position;
            currentMobs.Last().HealthBar.transform.position = playerCam.WorldToScreenPoint(
                new Vector3(
                    mobPos.x,
                    mobPos.y + (mobBody.GetComponent<BoxCollider>().bounds.size.y/2) +0.5f,
                    mobPos.z
                )
            );

            currentMobs.Last().HealthBar.enabled = true;

            currentMobs.Last().HealthBar.gameObject.SetActive(true);
            
            // Save record of max health
            currentMobs.Last().MaxHealth = currentMobs.Last().MobInfo.Health;
            
            mobBody.GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
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

    public void AttackMob(int mobIndex, int dmg)
    {
        currentMobs[mobIndex].MobInfo.Health -= dmg;
        if (currentMobs[mobIndex].MobInfo.Health <= 0)
        {
            currentMobs[mobIndex].MobInfo.Health = 0;
            currentMobs[mobIndex].MobInfo.fadeAway = true;
        }
        currentMobs[mobIndex].MobInfo.Shake();
        float healthMissing = currentMobs[mobIndex].MaxHealth - currentMobs[mobIndex].MobInfo.Health;
        currentMobs[mobIndex].HealthBar.fillAmount = 1f - (healthMissing / currentMobs[mobIndex].MaxHealth);
    }

}
