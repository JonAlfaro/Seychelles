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
            mobManager.AttackRandomAlive(1);
        }
    }
}