using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public string Name = "defaultMob";
    public int Health = 1;
    public int Damage = 1;
    public bool IsBoss = false;
    public GameObject MobBody;
    public int[] test;
    
    // Extra
    private Vector3 _originPosition;
    private Quaternion _originRotation;
    public float shakeDecay = 0.002f;
    public float shakeIntensity = .3f;
    private float _tempShakeIntensity = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_tempShakeIntensity > 0){
            transform.position = _originPosition + Random.insideUnitSphere * _tempShakeIntensity;
            transform.rotation = new Quaternion(
                _originRotation.x + Random.Range (-_tempShakeIntensity,_tempShakeIntensity) * .2f,
                _originRotation.y + Random.Range (-_tempShakeIntensity,_tempShakeIntensity) * .2f,
                _originRotation.z + Random.Range (-_tempShakeIntensity,_tempShakeIntensity) * .2f,
                _originRotation.w + Random.Range (-_tempShakeIntensity,_tempShakeIntensity) * .2f);
            _tempShakeIntensity -= shakeDecay;
        }
        
        // Drift towards original position after shake
    }
    
    public void Shake(){
        _originPosition = transform.position;
        _originRotation = transform.rotation;
        _tempShakeIntensity = shakeIntensity;

    }
    
    
}