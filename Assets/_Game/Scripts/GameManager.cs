using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField]
    DesignData m_DesignData;

    [SerializeField]
    GameObject m_FirstSpawnBlock;

    GameObject m_LastSpawnBlock;

    [SerializeField]
    GameObject m_RestartButton;

    Camera mainCam;

    float blockLength;

    FlappyBird m_FlappyBird;

    float cameraBirdDistanceX;
    float startCameraPosX;

    Vector3 startBirdPos;

    bool isGameOver = false;

    List<GameObject> listNewObstacle = new List<GameObject>();
    List<GameObject> listBirds = new List<GameObject>();
    
	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        m_LastSpawnBlock = m_FirstSpawnBlock;
        blockLength = m_LastSpawnBlock.GetComponent<BoxCollider2D>().size.x;
        m_FlappyBird = FindObjectOfType<FlappyBird>();
        cameraBirdDistanceX = m_FlappyBird.transform.position.x - mainCam.transform.position.x;
        startCameraPosX = mainCam.transform.position.x;
        startBirdPos = m_FlappyBird.transform.position;
        m_RestartButton.SetActive(false);
        listBirds.Add(m_FlappyBird.gameObject);
    }

    void NewBird()
    {
        isGameOver = false;
        m_RestartButton.SetActive(false);
        
        var prefabs = m_DesignData.birdPrefabs;
        var newBird = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        m_FlappyBird = newBird.GetComponent<FlappyBird>();
        newBird.transform.position = startBirdPos;

        listBirds.Add(newBird);

        var camPos = mainCam.transform.position;
        camPos.x = startCameraPosX;
        mainCam.transform.position = camPos;

        FindObjectOfType<Background>().Reset();
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

        if(isGameOver && Input.GetMouseButtonUp(0))
        {
            NewBird();
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
    }
}
