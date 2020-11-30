using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public List<Sprite> flrSprites = new List<Sprite>();
    public List<GameObject> flrLayers = new List<GameObject>();
    public Camera camera;
    private float length, startPostion;
    public float parallaxEffect;
    
    void Start()
    {
        startPostion = transform.position.x;
        Object[] loadedSprites = Resources.LoadAll("Floors/floor0", typeof(Sprite));
        Debug.Log(loadedSprites.Length);
        GameObject go = new GameObject();
        for (int x = 0; x < loadedSprites.Length; x++)
        {
            flrSprites.Add((Sprite) loadedSprites[x]);
            for (int y = 0; y < 3; y++)
            {
                flrLayers.Add(Instantiate(go, this.transform, true));
                flrLayers[(x*3) + y].name = "flrLayer"+x+"-"+y;
                SpriteRenderer sprRenderer = flrLayers[(x * 3) + y].AddComponent<SpriteRenderer>();
                sprRenderer.sprite = (Sprite) loadedSprites[x];
                
                // TODO: Make this a function I guess
                float spriteHeight = sprRenderer.sprite.bounds.size.y;
                float spriteWidth = sprRenderer.sprite.bounds.size.x;
                float distance = flrLayers[(x*3) + y].transform.position.z - camera.transform.position.z;
                float screenHeight = 2 * Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * distance;
                float screenWidth = screenHeight * camera.aspect;
                flrLayers[(x*3) + y].transform.localScale = new Vector3(screenWidth / spriteWidth, screenHeight / spriteHeight, 1);

                sprRenderer.sortingOrder = (loadedSprites.Length-x);
                length = sprRenderer.bounds.size.x;

                var pos = flrLayers[(x * 3) + y].transform.position;
                if (y == 1)
                {
                    pos.x += sprRenderer.bounds.size.x;
                    flrLayers[(x * 3) + y].transform.position = pos;
                } else if (y == 2)
                {
                    pos.x -= sprRenderer.bounds.size.x;
                    flrLayers[(x * 3) + y].transform.position = pos;
                }
                
            }
        }
        
        Destroy(go);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (camera.transform.position.x * parallaxEffect);
        for (int y = 0; y < 3; y++)
        {
            flrLayers[y].transform.position = new Vector3(startPostion + dist, flrLayers[y].transform.position.y, flrLayers[y].transform.position.z);
        }
    }
}