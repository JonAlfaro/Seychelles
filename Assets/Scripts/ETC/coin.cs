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
        rb.velocity = new Vector3(Random.Range(-3, -6), Random.Range(6, 12));
    }

    public void SetSize(float coinSize)
    {
        gameObject.transform.localScale += new Vector3(1 + coinSize, 1 + coinSize, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var srColor = sr.color;
        srColor.a -= Time.fixedDeltaTime;
        if (srColor.a <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            sr.color = srColor;
        }
    }
}