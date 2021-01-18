using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public GameObject[] flrLayers;
    public float[] flrLayerSpeeds;
    public float defaultLayerSpeed = 1;
    public bool panning;
    private List<Tuple<List<GameObject>, Vector2>> _flrLayerSegments = new List<Tuple<List<GameObject>, Vector2>>();

    private void Start()
    {
        for (int i = 0; i < flrLayers.Length; i++)
        {
            var layerSpeed = defaultLayerSpeed;
            // Set layer Speed if its set in the array
            if (i < flrLayerSpeeds.Length)
            {
                layerSpeed = flrLayerSpeeds[i];
            }

            var layerSegments = new List<GameObject>();
            bool first = true;
            foreach (Transform childT in flrLayers[i].GetComponentsInChildren<Transform>())
            {
                if (first)
                {
                    first = false;
                    continue;
                }

                layerSegments.Add(childT.gameObject);
            }


            if (layerSegments.Count >= 3)
            {
                float startPos = layerSegments.First().transform.localPosition.x +
                                 (layerSegments[0].transform.localPosition.x -
                                  layerSegments[1].transform.localPosition.x);
                _flrLayerSegments.Add(Tuple.Create(layerSegments,
                    new Vector2(startPos, layerSegments.Last().transform.localPosition.x)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!panning)
            return;

        for (var i = 0; i < _flrLayerSegments.Count; i++)
        {
            foreach (var gbSegment in _flrLayerSegments[i].Item1)
            {
                var layerSpeed = defaultLayerSpeed;
                // Set layer Speed if its set in the array
                if (i < flrLayerSpeeds.Length)
                {
                    layerSpeed = flrLayerSpeeds[i];
                }

                // Note:
                // _flrLayerSegments[i].Item2.x = First x
                // _flrLayerSegments[i].Item2.y = Last x
                if (gbSegment.transform.localPosition.x < _flrLayerSegments[i].Item2.x)
                {
                    gbSegment.transform.localPosition = new Vector3(_flrLayerSegments[i].Item2.y,
                        gbSegment.transform.localPosition.y, gbSegment.transform.localPosition.z);
                }

                gbSegment.transform.Translate(Vector3.left * layerSpeed * Time.deltaTime);
            }
        }
    }
}