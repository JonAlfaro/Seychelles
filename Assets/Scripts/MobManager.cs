using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


[System.Serializable]
public class LevelMobs
{
    public Mob Boss;
    public Mob[] Mobs;
}
public class MobManager : MonoBehaviour
{
    public LevelMobs[] MobsPerLevel = new LevelMobs[] {};
    public Mob[] levelMobs = new Mob[] {};
    public Mob levelBoss;
    public FloorManager flrManager;
    public GameObject[] mobSpawnPoints = new GameObject[] { };
    public GameObject bossSpawnPoint;
    private int _mobSpawnIndx = 0;
    public List<GameObject> currentMobs = new List<GameObject>();

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
            Destroy(mob.gameObject);
        }

        // Clear list
        currentMobs = new List<GameObject>();

        // Not Boss
        if (flrManager._floor < 6)
        {
            for (int i = 0; i < flrManager._ePerFloor[flrManager._floor-1]; i++)
            {
                currentMobs.Add(Instantiate(levelMobs[Random.Range(0, levelMobs.Length)].MobBody, mobSpawnPoints[_mobSpawnIndx].transform.position, Quaternion.identity));
                currentMobs[i].GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
                _mobSpawnIndx = _mobSpawnIndx == mobSpawnPoints.Length - 1 ? _mobSpawnIndx = 0 : _mobSpawnIndx + 1;
            }
        }
        else
        {
            currentMobs.Add(Instantiate(levelBoss.MobBody, bossSpawnPoint.transform.position, Quaternion.identity));
            currentMobs[0].GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
        }
        
    }
}
