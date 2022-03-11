using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    [SerializeField]
    public bool isAttached = true;

    private Rigidbody2D rb;

    private Vector2 direction;

    private float startSpeed = 2f;
    private float maxSpeed = 4.5f;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float speedX;
    [SerializeField]
    private float speedY;

    private float minX = -5.865f;
    private float maxX = 5.865f;
    private float maxY = 4.565f;

    private float bounceSpeedMultiplier = 1.01f;

    public GameObject soundManagerObj;
    private SoundManager playSound;

    public GameObject playerObj;
    private Player player;

    public GameObject timerObj;
    private TimerManager timer;

    public GameObject gameManagerObj;
    private GameManager game;

    public GameObject messageManagerObj;
    private MessageManager message;

    public bool hasBomb = false;
    public bool hasGhost = false;

    private int ghostCount = 0;

    private int brickCollisions = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Player>();

        soundManagerObj = GameObject.Find("Sound Manager");
        playSound = soundManagerObj.GetComponent<SoundManager>();

        timerObj = GameObject.Find("Timer Text");
        timer = timerObj.GetComponent<TimerManager>();

        gameManagerObj = GameObject.Find("GameManager");
        game = gameManagerObj.GetComponent<GameManager>();

        messageManagerObj = GameObject.Find("Message");
        message = messageManagerObj.GetComponent<MessageManager>();
    }

    public void Attach()
    {
        message.ReadyToLaunch(); // Tells the MessageManager to update the left panel message
        player.ResetPosition(); // Center the player obj
        rb.velocity = Vector2.zero;
        float playerX = playerObj.transform.position.x;
        transform.position = new Vector2(playerX, -4.65f); // -4.65f puts the ball on the bar (Y axis)
        transform.parent = playerObj.transform; // Makes the ball obj a child of the player obj, so they move together before launching the ball
        isAttached = true;
        player.canMove = true;
        timer.Reset(); // Resets timer when ball is attached (before launching)
    }

    void Detach()
    {
        transform.parent = null; // Ball obj is no longer a child of the player obj
        isAttached = false;
    }

    // Update is called once per frame
    void Update()
    {
        Launch();
        GetDirection();
        HitBounds();
        EnforceLimits();
        BallDrop();
        StopBall(); // Prevents the ball from moving after life is lost
        Reset();

        if (brickCollisions > 0)
        {
            brickCollisions = 0;
        }

    }

    void GetDirection()
    {

        if (direction.x > 0f && rb.velocity.x < 0f)
        {
            direction = new Vector2(-direction.x, direction.y);
        }

        if (direction.x < 0f && rb.velocity.x > 0f)
        {
            direction = new Vector2(-direction.x, direction.y);
        }

        if (direction.y > 0f && rb.velocity.y < 0f)
        {
            direction = new Vector2(direction.x, -direction.y);
        }

        if (direction.y < 0f && rb.velocity.y > 0f)
        {
            direction = new Vector2(direction.x, -direction.y);
        }
    }

    void Launch()
    {
        if (isAttached && transform.parent != null && Input.GetKeyDown("space"))
        {
            Detach();
            timer.StartTimer(); // Starts timer when ball is launched

            if (Input.GetKey("left") && !Input.GetKey("right")) 
            {
                direction = new Vector2(-1, 1); // Ball is launched to the left if key is pressed
                startSpeed = 2.5f; // Faster speed to balance player able to pick direction
                Debug.Log("Player was moving. Ball launched to the left with increased speed");
            }

            else if (Input.GetKey("right") && !Input.GetKey("left"))
            {
                direction = new Vector2(1, 1); // Ball is launched to the left if key is pressed
                startSpeed = 2.5f; // Faster speed to balance player able to pick direction
                Debug.Log("Player was moving. Ball launched to the right with increased speed");
            }

            else
            {
                direction = new Vector2(1, 1); // Default behavior - ball is launched to the right even if no key is pressed - regular speed
                startSpeed = 2f;
                Debug.Log("Player wasn't moving. Ball launched to the right with regular speed");
            }

            Debug.Log("Direction: " + direction);

            speed = startSpeed;
            rb.velocity = direction * speed;

            message.Clear();
            playSound.Launch();

        }
    }

    void HitBounds()
    {
        if (transform.position.x < minX || transform.position.x > maxX || transform.position.y > maxY)
        {
            MultiplySpeed(bounceSpeedMultiplier);
            rb.velocity = rb.velocity * bounceSpeedMultiplier;
            playSound.Beep();

            if (transform.position.x < minX)
            {
                transform.position = new Vector2(minX, transform.position.y);
                InvertSpeedX();
            }

            if (transform.position.x > maxX)
            {
                transform.position = new Vector2(maxX, transform.position.y);
                InvertSpeedX();
            }

            if (transform.position.y > maxY)
            {
                transform.position = new Vector2(transform.position.x, maxY);
                InvertSpeedY();
            }

            Ghost();

        }
    }

    public void MultiplySpeed(float multiplier)
    {
        speed *= multiplier;

    }

    void InvertSpeedX()
    {
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    void InvertSpeedY()
    {
        rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Common Brick")
        {
            if (hasBomb)
            {
                ExplodeBomb();
            }

            else
            {
                MultiplySpeed(bounceSpeedMultiplier);
                rb.velocity = rb.velocity * bounceSpeedMultiplier;

                brickCollisions++;

                if (brickCollisions == 1) // Prevents playing sound twice when ball collides with 2 bricks at the same time
                {
                    playSound.Beep();
                }
            }

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && rb.velocity.y != 0)
        {
            HitPlayer();
            Ghost();
            playSound.Beep();
        }
    }

    void HitPlayer()
    {

        if (transform.position.y < -4.75f) // Ball can't be reached by the player, but collided horizontally
        {
            Kill();

            return;
        }

        else
        {

            float playerX = playerObj.transform.position.x;
            float offset = Mathf.Abs(playerX - transform.position.x);

            // min offset 0 = 0.5 direction x
            // max offset 0.6 = 1.5 direction x

            float newDirX = ((offset * 1.5f) / 0.6f) + 0.5f;

            if (Input.GetKey("left"))
            {
                // Kick ball to the left
                direction = new Vector2(-newDirX, direction.y);
            }

            else if (Input.GetKey("right"))
            {
                // Kick ball to the right
                direction = new Vector2(newDirX, direction.y);
            }

            else
            {
                // Not pressing either, ball will continue same direction and invert Y only
                if (direction.x > 0)
                {
                    direction = new Vector2(newDirX, direction.y);
                }

                if (direction.x < 0)
                {
                    direction = new Vector2(-newDirX, direction.y);
                }
            }

            MultiplySpeed(bounceSpeedMultiplier);
            rb.velocity = direction * speed;
            InvertSpeedY();

        }
    }

    void EnforceLimits()
    {

        if (!isAttached)
        {
            // Limit speed to max speed

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }

            // Fix direction X

            if (direction.x > 0)
            {
                if (direction.x > 1.5f)
                    direction = new Vector2(1.5f, direction.y);

                if (direction.x < 0.5f)
                    direction = new Vector2(0.5f, direction.y);
            }

            if (direction.x < 0)
            {
                if (direction.x < -1.5f)
                    direction = new Vector2(-1.5f, direction.y);

                if (direction.x > -0.5f)
                    direction = new Vector2(-0.5f, direction.y);
            }

        }
    }

    public void BallDrop()
    {
        if (transform.position.y <= -5.12f && rb.velocity.y != 0)
        {
            Kill();
        }
    }

    public void HideBall()
    {
        transform.position = new Vector2(0, -6); // Hide the ball
        rb.velocity = Vector2.zero;
    }

    public void Kill()
    {
        HideBall();
        message.LostBall();
        playSound.DropBall();
        Death();
        timer.StopTimer(); // This will stop the timer
    }

    public void Death()
    {
        game.PlayerLife--; // Tells the GameManager that the player lost 1 life
        player.canMove = false;
    }

    void StopBall() // Holds the ball in place in case of time out
    {
        if (timer.Timer == 0)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void Reset()
    {
        if (Input.GetKeyDown("space") && !isAttached && !player.canMove && !game.loadingNextLevel)
        {
            player.ResetSpeed();
            player.ResetSize();
            player.ResetAxis();
            player.StopBot();
            hasBomb = false;
            hasGhost = false;
            Attach();
        }
    }

    void ExplodeBomb()
    {
        GameObject bombObj = Instantiate(Resources.Load("Items/Bomb Area", typeof(GameObject))) as GameObject;
        bombObj.transform.position = new Vector2(transform.position.x, transform.position.y);
        bombObj.transform.parent = this.transform;
        hasBomb = false;
        playSound.Explosion();

    }

    public void Warp(Vector3 targetPos)
    {
        transform.position = targetPos;
    }

    void Ghost()
    {
        if (hasGhost)
        {
            ghostCount++;

            if (ghostCount == 2)
            {
                hasGhost = false;
            }
        }
    }
}