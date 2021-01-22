using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class csRat : MonoBehaviour
{
    private bool _run;

    private float _dieTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_run)
        {
            transform.position += Vector3.right*0.1f;
        }
    }

    private void FixedUpdate()
    {
        if (_run)
        {
            _dieTime += Time.fixedDeltaTime;
        }

        if (_dieTime > 3)
        {
            Destroy(gameObject);
        }
    }

    public void Run()
    {
        _run = true;
    }
}
