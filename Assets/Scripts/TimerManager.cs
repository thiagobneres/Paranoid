using System.Collections;
using System.Collections.Generic;
using System; // That's where TimeSpan exists
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private int levelTimer; // Timer set for the level
    private int timer; // Current timer
    public bool isRunning = false;

    public GameObject ballObj;
    private Ball ball;

    public GameObject messageManagerObj;
    private MessageManager message;

    public GameObject soundManagerObj;
    private SoundManager playSound;


    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        ball = ballObj.GetComponent<Ball>();
        message = messageManagerObj.GetComponent<MessageManager>();
        playSound = soundManagerObj.GetComponent<SoundManager>();
    }

    public void SetTimer(int seconds) // Called by GameManager
    {
        levelTimer = seconds;
        Reset();
    }

    public void Reset() // Note: It won't stop timer -- That method should be called separately
    {
        Timer = levelTimer;
    }

    public int Timer
    {
        get
        {
            return timer;
        }
        set
        {
            timer = value;
            int minutes = TimeSpan.FromSeconds(timer).Minutes;
            int seconds = TimeSpan.FromSeconds(timer).Seconds;
            textMesh.text = minutes.ToString("d2") + ":" + seconds.ToString("d2"); // Stringfy and uses D2 to force 2 digits
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        StartCoroutine(RunTimer());
    }

    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(1f);

        while (Timer > 0 && isRunning)
        {
            Timer--;
            yield return new WaitForSeconds(0.5f);
            if (Timer <= 30 && isRunning)
            {
                textMesh.text = "";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {

        if (Timer == 0 && isRunning)
        {
            StopTimer();
            message.TimeUp();
            playSound.TimeUp();
            ball.Death();
        }
    }

    public void StopTimer()
    {
        isRunning = false;
        int minutes = TimeSpan.FromSeconds(timer).Minutes;
        int seconds = TimeSpan.FromSeconds(timer).Seconds;
        textMesh.text = minutes.ToString("d2") + ":" + seconds.ToString("d2");
        StopCoroutine(RunTimer());
    }

    public void ModifyTimer(int seconds)
    {
        Timer += seconds;
    }

}
