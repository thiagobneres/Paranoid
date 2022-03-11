using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject playerObj;
    private Player player;

    public GameObject ballObj;
    private Ball ball;

    public GameObject timerObj;
    private TimerManager timer;

    public GameObject lifeManagerObj;
    private LifeManager life;

    public GameObject levelManagerObj;
    private LevelManager level;

    public GameObject soundManagerObj;
    private SoundManager playSound;

    public GameObject messageManagerObj;
    private MessageManager message;

    public int thisLevel;
    public int playerLife;
    public int levelTimer;

    public bool loadingNextLevel = false;

    void Awake()
    {
        AccessObjects();
    }

    void AccessObjects()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
        }

        player = playerObj.GetComponent<Player>();

        if (ballObj == null)
        {
            ballObj = GameObject.Find("Ball");
        }

        ball = ballObj.GetComponent<Ball>();

        if (timerObj == null)
        {
            timerObj = GameObject.Find("Timer Text");
        }

        timer = timerObj.GetComponent<TimerManager>();

        if (lifeManagerObj == null)
        {
            lifeManagerObj = GameObject.Find("Lives Text");
        }

        life = lifeManagerObj.GetComponent<LifeManager>();

        if (levelManagerObj == null)
        {
            levelManagerObj = GameObject.Find("Level Text");
        }

        level = levelManagerObj.GetComponent<LevelManager>();

        if (soundManagerObj == null)
        {
            soundManagerObj = GameObject.Find("Sound Manager");
        }

        playSound = soundManagerObj.GetComponent<SoundManager>();

        if (messageManagerObj == null)
        {
            messageManagerObj = GameObject.Find("Message");
        }

        message = messageManagerObj.GetComponent<MessageManager>();
    }

    void Start()
    {
        ThisLevel = 1;
    }

    public int ThisLevel
    {
        get
        {
            return thisLevel;
        }
        set
        {
            thisLevel = value;
            GetLevelVars();
            level.UpdateLevel(ThisLevel);
            PrepareLevel();
        }
    }

    void GetLevelVars()
    {
        if (thisLevel == 1)
        {
            PlayerLife = 5;
            LevelTimer = 120;
        }

        if (thisLevel == 2)
        {
            LevelTimer = 100;
        }

        if (thisLevel == 3)
        {
            LevelTimer = 150;
        }

        if (ThisLevel == 6)
        {
            LevelTimer = 60;
        }
    }

    public int PlayerLife
    {
        get
        {
            return playerLife;
        }
        set
        {
            playerLife = value;
            life.UpdateLife(playerLife);
        }
    }

    public int LevelTimer
    {
        get
        {
            return levelTimer;
        }
        set
        {
            levelTimer = value;
            timer.SetTimer(levelTimer); // Time limit for the current level; Changes (e.g: items) to current time will be handled in the TimeManager
        }
    }

    void PrepareLevel()
    {
        ball.Attach();
    }


    public void Win()
    {
        ball.HideBall();
        playSound.Win();
        loadingNextLevel = true;
        message.Win();
        timer.StopTimer();
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        loadingNextLevel = false;
        ThisLevel++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Win();
        }
    }

}
