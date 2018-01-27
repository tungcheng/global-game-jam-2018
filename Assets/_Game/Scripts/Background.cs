using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    
    float horizontalLength;

    Transform cameraT;

    float cameraHalfWidth;

    Vector3 startPosition;

	// Use this for initialization
	void Start () {
        var mainCam = Camera.main;
        cameraT = mainCam.transform;
        var renderer = GetComponent<SpriteRenderer>();
        horizontalLength = renderer.sprite.bounds.size.x * transform.lossyScale.x;
        horizontalLength *= 0.99f;
        cameraHalfWidth = mainCam.orthographicSize * mainCam.aspect;
        startPosition = transform.position;
    }

    public void Reset()
    {
        transform.position = startPosition;
    }
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x + horizontalLength * 0.5f < cameraT.position.x - cameraHalfWidth)
        {
            var temp = transform.position;
            temp.x += horizontalLength * 2;
            transform.position = temp;
        }
	}
}
