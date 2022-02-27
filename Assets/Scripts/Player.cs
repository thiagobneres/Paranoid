using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int lifes = 5;
    public float minX = -5.4f;
    public float maxX = 5.4f;
    public float playerY = -4.8f;
    public float playerSpeed = 1;
    public bool canMove = true;

    public GameObject gameManagerObj;
    private GameManager game;

    private GameObject ballObj;
    private Ball ball;

    private bool speedCoroutineRunning = false;
    private bool sizeCoroutineRunning = false;
    private bool invertedAxisCoroutineRunning = false;
    private bool botCoroutineRunning = false;

    private bool invertedAxis = false;

    public bool hasBot = false;

    // Start is called before the first frame update
    void Awake()
    {

        gameManagerObj = GameObject.Find("GameManager");
        game = gameManagerObj.GetComponent<GameManager>();

        ballObj = GameObject.Find("Ball");
        ball = ballObj.GetComponent<Ball>();
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(0, playerY);
    }


    // Update is called once per frame
    void Update()
    {
        DetectMovement();
        CheckBounds();
    }

    void DetectMovement()
    { 
        if (canMove && !game.loadingNextLevel)
        {

            if (hasBot)
            {
                transform.position = new Vector2(ballObj.transform.position.x, transform.position.y);
                return;
            }

            if (Input.GetKey("left") && Input.GetKey("right"))
            {
                return;
            }

            if (Input.GetKey("left") && transform.position.x >= minX)
            {
                if (!invertedAxis)
                {
                    transform.position += Vector3.left * (playerSpeed + 5) * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.right * (playerSpeed + 5) * Time.deltaTime;
                }
            }

            if (Input.GetKey("right") && transform.position.x <= maxX)
            {
                if (!invertedAxis)
                {
                    transform.position += Vector3.right * (playerSpeed + 5) * Time.deltaTime;
                }

                else
                {
                    transform.position += Vector3.left * (playerSpeed + 5) * Time.deltaTime;
                }
            }
        }
    }

    void CheckBounds()
    {
        if (transform.position.x < minX)
        {
            transform.position = new Vector2(minX, playerY);
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector2(maxX, playerY);
        }
    }

    public void AddLife(int lives)
    {
        lives += lives;
    }

    public void ResetSpeed()
    {
        playerSpeed = 1f;

        if (speedCoroutineRunning)
        {
            speedCoroutineRunning = false;
            StopCoroutine(WaitToResetSpeed());
        }
    }

    public void ModifySpeed(float modifier)
    {
        float newSpeed = playerSpeed += modifier;

        if (newSpeed < 0.5f || newSpeed > 3)
        {
            return; // Hit min or max speed already
        }

        else
        {
            playerSpeed = newSpeed;
            StartCoroutine(WaitToResetSpeed());
        }
    }

    IEnumerator WaitToResetSpeed()
    {
        speedCoroutineRunning = true;
        yield return new WaitForSeconds(10f);
        speedCoroutineRunning = false;
        ResetSpeed();
    }

    public void ResetSize()
    {
        minX = -5.4f;
        maxX = 5.4f;
        transform.localScale = new Vector2(1f, 0.1f);

        if (sizeCoroutineRunning)
        {
            sizeCoroutineRunning = false;
            StopCoroutine(WaitToResetSize());
        }
    }

    public void Extend()
    {
        transform.localScale = new Vector2(2f, 0.1f);
        minX = -4.9f;
        maxX = 4.9f;
        StartCoroutine(WaitToResetSize());
    }

    public void Shrink()
    {
        transform.localScale = new Vector2(0.5f, 0.1f);
        minX = -5.63f;
        maxX = 5.63f;
        StartCoroutine(WaitToResetSize());
    }

    IEnumerator WaitToResetSize()
    {
        sizeCoroutineRunning = true;
        yield return new WaitForSeconds(15f);
        sizeCoroutineRunning = false;
        ResetSize();
    }

    public void Invert()
    {
        invertedAxis = true;
        StartCoroutine(WaitToResetAxis());
    }

    public void ResetAxis()
    {
        invertedAxis = false;

        if (invertedAxisCoroutineRunning)
        {
            invertedAxisCoroutineRunning = false;
            StopCoroutine(WaitToResetAxis());
        }
    }

    IEnumerator WaitToResetAxis()
    {
        invertedAxisCoroutineRunning = true;
        yield return new WaitForSeconds(15f);
        invertedAxisCoroutineRunning = false;
        ResetAxis();
    }

    public void StartBot()
    {
        if (!hasBot)
        {
            hasBot = true;
            StartCoroutine(WaitToStopBot());
        }
    }

    IEnumerator WaitToStopBot()
    {
        botCoroutineRunning = true;
        yield return new WaitForSeconds(10f);
        botCoroutineRunning = false;
        StopBot();
    }

    public void StopBot()
    {
        hasBot = false;

        if (botCoroutineRunning)
        {
            botCoroutineRunning = false;
            StopCoroutine(WaitToStopBot());
        }
    }

}
