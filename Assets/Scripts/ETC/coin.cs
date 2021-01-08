using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.velocity = new Vector3(Random.Range(-3,-6), Random.Range(6,12));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var srColor = sr.color;
        srColor.a -= Time.fixedDeltaTime;
        if (srColor.a <= 0)
        {
        }
        else
        {
            sr.color = srColor;
 
        }
    }
}
