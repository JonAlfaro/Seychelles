using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    public void ChangeSpriteTo(Texture2D sprite)
    {
        RawImage rawImage = GetComponent<RawImage>();
        if (rawImage)
        {
            rawImage.texture = sprite;
        }
    }
}
