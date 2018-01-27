using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour 
{
	public static GameControl instance;			//A reference to our game control script so we can access it statically.
	public Text scoreText;						//A reference to the UI text component that displays the player's score.
	public GameObject gameOvertext;				//A reference to the object that displays the text which appears when the player dies.

	private int score = 0;						//The player's score.
	public bool gameOver = false;				//Is the game over?
	public float scrollSpeed = -1.5f;

    public GameObject rootBird;
    GameObject currentBird;

    public float deadShowRange = 10f;
    float timeStart = 0f;
    float timeDead = 0f;
    Dictionary<int, List<Bird>> deadBirds = new Dictionary<int, List<Bird>>();

    List<Bird> showingDeadBirds = new List<Bird>();

	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);

        StartNewGame();
	}

	void Update()
	{
		//If the game is over and the player has pressed some input...
		if (gameOver && Input.GetMouseButtonDown(0)) 
		{
            //...reload the current scene.
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            StartNewGame();
		}

        if(!gameOver)
        {
            float playTime = GetCurrentTime() - timeStart;
            int timeBlock = Mathf.FloorToInt(playTime);
            if(deadBirds.ContainsKey(timeBlock))
            {
                var listBirds = deadBirds[timeBlock];
                int count = listBirds.Count;
                for(int i = 0; i < count; i++)
                {
                    bool isInTimeShowDead = IsTimeInShowDeadRange(listBirds[i].deadTime, playTime, deadShowRange);
                    if (isInTimeShowDead && !listBirds[i].isDeadAndShow)
                    {
                        listBirds[i].gameObject.SetActive(true);
                        listBirds[i].isDeadAndShow = true;
                        showingDeadBirds.Add(listBirds[i]);
                    }
                }
            }
            HideOutRangeDeadBirds(playTime);
        }
	}

    void HideOutRangeDeadBirds(float playTime)
    {
        int count = showingDeadBirds.Count;
        for (int i = 0; i < count; i++)
        {
            bool isInTimeShowDead = IsTimeInShowDeadRange(showingDeadBirds[i].deadTime, playTime, deadShowRange);

            if(!isInTimeShowDead && showingDeadBirds[i].isDeadAndShow)
            {
                showingDeadBirds[i].gameObject.SetActive(false);
                showingDeadBirds[i].isDeadAndShow = false;
                i -= 1;
                count -= 1;
                showingDeadBirds.RemoveAt(i);
            }
        }
    }

    void HideAllDeadBirds()
    {
        int count = showingDeadBirds.Count;
        for(int i = 0; i < count; i++)
        {
            showingDeadBirds[i].gameObject.SetActive(false);
            showingDeadBirds[i].isDeadAndShow = false;
        }
    }

    bool IsTimeInShowDeadRange(float deadTime, float currentPlayTime, float deadShowRange)
    {
        if (deadTime < currentPlayTime - deadShowRange) return false;
        if (deadTime > currentPlayTime + deadShowRange) return false;
        return true;
    }

    void StartNewGame()
    {
        if(currentBird != null)
        {
            float deadTime = GetCurrentTime() - timeStart;
            if(deadTime > deadShowRange)
            {
                var bird = currentBird.GetComponent<Bird>();
                bird.deadTime = deadTime;
                bird.isDeadAndShow = false;
                currentBird.SetActive(false);
                int deadTimeBlock = Mathf.FloorToInt(deadTime);
                if (!deadBirds.ContainsKey(deadTimeBlock))
                {
                    deadBirds[deadTimeBlock] = new List<Bird>();
                }
                deadBirds[deadTimeBlock].Add(bird);
            }
            else
            {
                Destroy(currentBird);
            }
        }
        HideAllDeadBirds();
        showingDeadBirds.Clear();
        timeStart = GetCurrentTime();
        rootBird.SetActive(false);
        currentBird = Instantiate(rootBird);
        currentBird.SetActive(true);
        FindObjectOfType<ColumnPool>().NewGame();
        gameOver = false;
        gameOvertext.SetActive(false);
    }

    float GetCurrentTime()
    {
        if(gameOver)
        {
            return timeDead;
        }
        return Time.realtimeSinceStartup;
    }

	public void BirdScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)	
			return;
		//If the game is not over, increase the score...
		score++;
		//...and adjust the score text.
		scoreText.text = "Score: " + score.ToString();
	}

	public void BirdDied()
	{
        timeDead = GetCurrentTime();
        //Activate the game over text.
        gameOvertext.SetActive (true);
		//Set the game to be over.
		gameOver = true;
    }
}
