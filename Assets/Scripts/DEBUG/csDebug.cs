using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class csDebug : MonoBehaviour
{
    public MobManager mobManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            var mobIndex = mobManager.currentMobs.Count;
            mobManager.AttackMob(mobIndex-1, 1);
        }
    }
    
    
}
