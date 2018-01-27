using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    Transform column1;
    Transform column2;

	// Use this for initialization
	void Start () {
        column1 = transform.GetChild(0);
        column2 = transform.GetChild(1);

        var columnRender = column1.gameObject.GetComponent<SpriteRenderer>();
        var columnHeight = Mathf.Abs(columnRender.sprite.bounds.size.y * column1.lossyScale.y);

        var data = GameManager.Instance.GetDesignData();

        float scaleX = Random.Range(data.obsScaleMinX, data.obsScaleMaxX);

        var scale = column1.transform.localScale;
        scale.x = scaleX;
        column1.transform.localScale = scale;

        scale = column2.transform.localScale;
        scale.x = scaleX;
        column2.transform.localScale = scale;

        float centerY = Random.Range(data.obsCenterMinY, data.obsCenterMaxY);
        float holeHeight = Random.Range(data.obsHoleHeightMin, data.obsHoleHeightMax);

        var pos = column1.transform.position;
        pos.y = centerY - holeHeight * 0.5f - columnHeight * 0.5f;
        column1.transform.position = pos;

        pos = column2.transform.position;
        pos.y = centerY + holeHeight * 0.5f + columnHeight * 0.5f;
        column2.transform.position = pos;
    }
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
