using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


[System.Serializable]
public class LevelMobs
{
    public GameObject Boss;
    public GameObject[] Mobs;
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
                var mob = levelMobs[Random.Range(0, levelMobs.Length)];
                currentMobs.Add(Instantiate(mob, mobSpawnPoints[_mobSpawnIndx].transform.position, Quaternion.identity));
                var mobBody = Instantiate(currentMobs[i].GetComponent<Mob>().MobBody, currentMobs[i].transform.position,
                    Quaternion.identity);
                mobBody.transform.parent = currentMobs[i].transform;
                mobBody.GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
                _mobSpawnIndx = _mobSpawnIndx == mobSpawnPoints.Length - 1 ? _mobSpawnIndx = 0 : _mobSpawnIndx + 1;
            }
        }
        else
        {
            currentMobs.Add(Instantiate(levelBoss, bossSpawnPoint.transform.position, Quaternion.identity));
            var mobBody = Instantiate(levelBoss.GetComponent<Mob>().MobBody, levelBoss.transform.position,
                Quaternion.identity);
            mobBody.transform.parent = currentMobs.Last().transform;
            mobBody.GetComponentInChildren<SpriteRenderer>().color = flrManager.hueShift;
        }
        
    }
}
