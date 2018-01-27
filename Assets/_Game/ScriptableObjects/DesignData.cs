using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DesignData", menuName = "GameConfig/DesignData", order = 1)]

public class DesignData : ScriptableObject {

    [Header("Obstacle Settings")]
    public float obsScaleMinX;
    public float obsScaleMaxX;
    public float obsCenterMinY;
    public float obsCenterMaxY;
    public float obsHoleHeightMin;
    public float obsHoleHeightMax;

    [Header("Blocks Prefabs")]
    public GameObject[] blockPrefabs;

    [Header("Bird Settings")]
    public float flyUpForce;
    public float horizontalSpeed;

    [Header("Bird Prefabs")]
    public GameObject[] birdPrefabs;
}