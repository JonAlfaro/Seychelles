using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class TransitionLoadingScreen : MonoBehaviour
{

    public Sprite SplashIcon;

    public Canvas UICanvas;

    public Color OverlayColor;
    
    private GameObject loaderLeft;
    private GameObject loaderRight;
    private RectTransform rtRight;
    private RectTransform rtLeft;
    private Vector2 rtAlignLeft = new Vector2(1f, 0.5f); 
    private Vector2 rtAlignRight = new Vector2(0f, 0.5f);
    private float scaleStep = 0;
    private float direction = 1;


    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        Create();
    }

    // Update is called once per frame
    void Update()
    {
        ScaleRect(rtLeft, rtAlignLeft, scaleStep);
        ScaleRect(rtRight, rtAlignRight, scaleStep);
        
        if (scaleStep > 1)
        {
            direction = -1;
        }

        if (scaleStep < 0)
        {
            direction = 1;
        }
        
        scaleStep += (Time.deltaTime * Speed)*direction;


    }

    private void Create()
    {
        loaderLeft = Instantiate(new GameObject(), UICanvas.transform, true);
        loaderLeft.AddComponent<RawImage>();
        loaderLeft.GetComponent<RawImage>().color = OverlayColor;
        rtLeft = loaderLeft.GetComponent<RectTransform>();
        
        loaderRight = Instantiate(new GameObject(), UICanvas.transform, true);
        loaderRight.AddComponent<RawImage>();
        loaderRight.GetComponent<RawImage>().color = OverlayColor;
        rtRight = loaderRight.GetComponent<RectTransform>();
    }
    
    private void ScaleRect(RectTransform myRect, Vector2 rectMiddle, float hScale)
    {
        float horizontalSize = hScale;
        float verticalSize = 1f; //100%  of vertical screen used
     
        myRect.sizeDelta = Vector2.zero; //Dont want any delta sizes, because that would defeat the point of anchors
        myRect.anchoredPosition = Vector2.zero; //And the position is set by the anchors aswell so we set the offset to 0
        
        myRect.anchorMin = new Vector2(rectMiddle.x - horizontalSize / 2,
            rectMiddle.y - verticalSize / 2);
        myRect.anchorMax = new Vector2(rectMiddle.x + horizontalSize / 2,
            rectMiddle.y + verticalSize / 2);
    }
}
