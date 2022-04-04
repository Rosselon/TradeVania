using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private float xSpeed, ySpeed;

    // Update is called once per frame
    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, rawImage.uvRect.size);
    }
}
