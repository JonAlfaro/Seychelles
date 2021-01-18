using UnityEngine;

public class Mob : MonoBehaviour
{
    public string Name = "defaultMob";
    public int Health = 1;
    public int Damage = 1;
    public bool IsBoss;
    public GameObject MobBody;
    public int[] test;
    public Sprite healthBar;

    // Extra
    private Vector3 _originPosition;
    private Quaternion _originRotation;
    public float shakeDecay = 0.002f;
    public float shakeIntensity = .3f;
    private float _tempShakeIntensity;

    // Extra Dying
    public bool fadeAway;
    private SpriteRenderer[] _childrenSprites;


    // Start is called before the first frame update
    void Start()
    {
        _childrenSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_tempShakeIntensity > 0)
        {
            transform.position = _originPosition + Random.insideUnitSphere * _tempShakeIntensity;
            transform.rotation = new Quaternion(
                _originRotation.x + Random.Range(-_tempShakeIntensity, _tempShakeIntensity) * .2f,
                _originRotation.y + Random.Range(-_tempShakeIntensity, _tempShakeIntensity) * .2f,
                _originRotation.z + Random.Range(-_tempShakeIntensity, _tempShakeIntensity) * .2f,
                _originRotation.w + Random.Range(-_tempShakeIntensity, _tempShakeIntensity) * .2f);
            _tempShakeIntensity -= shakeDecay;
        }

        if (fadeAway)
        {
        }

        // Drift towards original position after shake
    }

    private void FixedUpdate()
    {
        if (fadeAway)
        {
            foreach (var childrenSprite in _childrenSprites)
            {
                var cColor = childrenSprite.color;
                cColor.a -= Time.fixedDeltaTime;
                if (cColor.a < 0)
                    cColor.a = 0;
                childrenSprite.color = cColor;
            }
        }
    }

    public void Shake()
    {
        _originPosition = transform.position;
        _originRotation = transform.rotation;
        _tempShakeIntensity = shakeIntensity;
    }
}