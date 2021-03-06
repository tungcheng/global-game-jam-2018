﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    [SerializeField]
    DesignData m_DesignData;

    [SerializeField]
    GameObject m_FirstSpawnBlock;

    GameObject m_LastSpawnBlock;

    [SerializeField]
    GameObject m_RestartButton;

    [SerializeField]
    GameObject m_ContinueText;

    [SerializeField]
    GameObject m_Fog;

    [SerializeField]
    Text m_ScoreTxt;

    [SerializeField]
    Text m_HighScoreTxt;

    [SerializeField]
    AudioSource m_AudioSource;

    const string HIGH_SCORE_KEY = "highscore";

    int curScore;
    int curHighScore;

    Camera mainCam;

    float blockLength;

    FlappyBird m_FlappyBird;

    float cameraBirdDistanceX;
    float startCameraPosX;

    Vector3 startBirdPos;

    bool isGameOver = false;

    List<GameObject> listNewObstacle = new List<GameObject>();
    List<GameObject> listBirds = new List<GameObject>();

    float fogBirdDistanceX;
    Vector3 startFogPos;
    
	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        m_LastSpawnBlock = m_FirstSpawnBlock;
        blockLength = m_LastSpawnBlock.GetComponent<BoxCollider2D>().size.x;
        m_FlappyBird = FindObjectOfType<FlappyBird>();
        cameraBirdDistanceX = m_FlappyBird.transform.position.x - mainCam.transform.position.x;
        startCameraPosX = mainCam.transform.position.x;
        startBirdPos = m_FlappyBird.transform.position;
        startFogPos = m_Fog.transform.position;
        fogBirdDistanceX = m_FlappyBird.transform.position.x - m_Fog.transform.position.x;

        m_RestartButton.SetActive(false);
        m_ContinueText.SetActive(false);
        listBirds.Add(m_FlappyBird.gameObject);

        curScore = 0;
        curHighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        m_ScoreTxt.text = "Score: " + curScore;
        m_HighScoreTxt.text = "Best: " + curHighScore;
    }

    public void NewBird()
    {
        isGameOver = false;
        m_RestartButton.SetActive(false);
        m_ContinueText.SetActive(false);

        curScore = 0;
        m_ScoreTxt.text = "Score: " + curScore;

        var prefabs = m_DesignData.birdPrefabs;
        var newBird = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        m_FlappyBird = newBird.GetComponent<FlappyBird>();
        newBird.transform.position = startBirdPos;

        listBirds.Add(newBird);

        var camPos = mainCam.transform.position;
        camPos.x = startCameraPosX;
        mainCam.transform.position = camPos;

        var backgrounds = FindObjectsOfType<Background>();
        for(int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].Reset();
        }

        var fogPos = m_Fog.transform.position;
        fogPos.x = m_FlappyBird.transform.position.x - fogBirdDistanceX;
        if (m_Fog.transform.position.x > fogPos.x)
        {
            fogPos.x = Mathf.Lerp(fogPos.x, m_Fog.transform.position.x, 0.9f);
            m_Fog.transform.position = fogPos;
        }
    }

    public void RestartGame()
    {
        for(int i = 0; i < listNewObstacle.Count; i++)
        {
            Destroy(listNewObstacle[i]);
        }
        listNewObstacle.Clear();

        for (int i = 0; i < listBirds.Count; i++)
        {
            Destroy(listBirds[i]);
        }
        listBirds.Clear();

        m_LastSpawnBlock = m_FirstSpawnBlock;

        m_Fog.transform.position = startFogPos;

        NewBird();
    }
	
	// Update is called once per frame
	void Update () {
		if(m_LastSpawnBlock.transform.position.x < mainCam.transform.position.x)
        {
            var blockPrefabs = m_DesignData.blockPrefabs;
            var prefab = blockPrefabs[Random.Range(0, blockPrefabs.Length)];
            var nextPos = m_LastSpawnBlock.transform.position;
            nextPos.x += blockLength;
            m_LastSpawnBlock = Instantiate(prefab);
            m_LastSpawnBlock.transform.position = nextPos;

            listNewObstacle.Add(m_LastSpawnBlock);
        }
        
        var camPos = mainCam.transform.position;
        camPos.x = m_FlappyBird.transform.position.x - cameraBirdDistanceX;
        mainCam.transform.position = camPos;

        var fogPos = m_Fog.transform.position;
        fogPos.x = m_FlappyBird.transform.position.x - fogBirdDistanceX;
        if(m_Fog.transform.position.x < fogPos.x)
        {
            m_Fog.transform.position = fogPos;
        }
	}

    public void OnHitNewScore()
    {
        if (!isGameOver)
        {
            curScore += 1;
            m_ScoreTxt.text = "Score: " + curScore;

            if (curScore > curHighScore)
            {
                curHighScore = curScore;
                m_HighScoreTxt.text = "Best: " + curHighScore;
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, curHighScore);
            }

            PlaySfxPoint();
        }
    }

    public DesignData GetDesignData()
    {
        return m_DesignData;
    }

    public void OnBirdDead()
    {
        isGameOver = true;
        m_RestartButton.SetActive(true);
        m_ContinueText.SetActive(true);
        PlaySfxHit();
    }

    public void PlaySfxWing()
    {
        m_AudioSource.PlayOneShot(m_DesignData.sfxWing);
    }

    public void PlaySfxHit()
    {
        m_AudioSource.PlayOneShot(m_DesignData.sfxHit);
    }

    public void PlaySfxPoint()
    {
        m_AudioSource.PlayOneShot(m_DesignData.sfxPoint);
    }
}
