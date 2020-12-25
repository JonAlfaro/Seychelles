using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = System.Random;
using URandom = UnityEngine.Random;


public class FloorManager : MonoBehaviour
{
    public MobManager mobManager;
    public UnityEvent floorChange;

    // 6 floors per level
    public int _floor = 1;
    // 2,147,483,647 levels per game
    public int _level = 1;

    private double[] _halfLifeDifficultyIncrease = new double[] {0.1, 0.2, 0.3, 0.4, 0.5, 0.5};

    public Floor[] floors = new Floor[] { };

    public Floor currentFloor;
    // Enemies Per Floor
    public List<int> _ePerFloor = new List<int> {2, 3, 4, 2, 3,};
    public Color hueShift = Color.white;
    
   

    void Start()
    {
        currentFloor = Instantiate(floors[0]);
        if (floorChange == null)
            floorChange = new UnityEvent();
    }

    void Update()
    {
        // Shake
        if (Input.GetKeyDown("z"))
        {
            mobManager.currentMobs[1].MobInfo.Shake();   
        }

        // if (Input.GetKeyDown("space"))
        // {
        //     NextFloor();
        //     print("d = " + GetDifficulty(false));
        //     string z = "";
        //     foreach( var x in _ePerFloor)
        //     {
        //         z += " " + x.ToString();
        //     }
        //     Debug.Log("_ePerFloor = " + z);
        // }
    }


    public Tuple<int, int> NextFloor()
    {
        // Adjust floor
        _floor = _floor == 6 ? _floor = 1 : _floor + 1;
        
        // New level
        if (_floor == 1)
        {
            _level++;
            Shuffle(_ePerFloor);
            // Change Background
            Destroy(currentFloor.gameObject);
            currentFloor = Instantiate(floors[_level%floors.Length]);
            
            // hueShift = new Color(
            //     URandom.Range(0f, 1f), 
            //     URandom.Range(0f, 1f), 
            //     URandom.Range(0f, 1f)
            // );
        }
        
        floorChange.Invoke();

        return GetFloorLevel();
    }

    public Tuple<int, int> GetFloorLevel()
    {
        return new Tuple<int,int>(_floor, _level);
    }
    
    public double GetDifficulty(bool surge)
    {
        return CalcDifficulty(_floor, _level, _halfLifeDifficultyIncrease[_floor-1], surge);
    }

    private double CalcDifficulty(int f, int l, double mod, bool surge)
    {
        double d = (l * 6) + f;
        d += (mod * d)/2;
        if (surge)
        {
            d += (mod * d)/2;
        }
        return d;
    }
    
    
    // Utils
    private static Random rng = new Random();  

    public static void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
