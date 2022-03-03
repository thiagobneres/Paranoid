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
    private float speed;
    private float maxSpeed = 4.5f;

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

    private int brickCollisions = 0; // Adjusts the Y axis inversion in case of 2 collisions at the same time

    private float brickCollisionOffsetY;

    private float angle;
    private Vector3 lastPos;

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
        GetSpeed();
        Launch();
        HitBounds();
        EnforceLimits();
        BallDrop();
        StopBall(); // Prevents the ball from moving after life is lost
        Reset();
        GetLastPosition();

    }

    void GetSpeed()
    {
        if (rb.velocity.x != 0 && rb.velocity.y != 0)
        {
            speedX = rb.velocity.x;
            speedY = rb.velocity.y;
        }
    }

    void GetLastPosition()
    {
        lastPos = transform.position;
    }

    void Launch()
    {
        if (isAttached && transform.parent != null && Input.GetKeyDown("space"))
        {
            Detach();
            timer.StartTimer(); // Starts timer when ball is launched

            direction = new Vector2(1, 1);

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
        Debug.Log("MultiplySpeed(): Speed is now " + speed);
        Debug.Log("Direction is " + direction);
        rb.velocity = direction * speed;
        GetSpeed();
    }

    void InvertSpeedX()
    {
        transform.position = lastPos;
        direction = new Vector2(-direction.x, direction.y);
        rb.velocity = direction * speed;
    }

    void InvertSpeedY()
    {
        transform.position = lastPos;
        direction = new Vector2(direction.x, -direction.y);
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && rb.velocity.y != 0)
        {
            HitPlayer();
            Ghost();
            playSound.Beep();
            Debug.Log("Hit player");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Common Brick")
        {
            //Debug.Log("Hit a brick");

            rb.velocity = new Vector2(speedX, speedY); // Restore rb velocity after collision

            if (hasBomb)
            {
                InvertSpeedX();
                InvertSpeedY();
                ExplodeBomb();
                Debug.Log("Exploded bomb");
                return;
            }

            else
            {
                var direction = (Vector2)col.contacts[0].point - (Vector2)transform.position;
                angle = Mathf.Abs(Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);

                brickCollisions++;

            }

        }
    }

    void FixedUpdate()
    {
        CheckBrickCollision();
    }

    void CheckBrickCollision()
    {

        if (brickCollisions > 0)
        {

            if (brickCollisions == 3)
            {
                if (!hasGhost)
                {
                    InvertSpeedX();
                    InvertSpeedY();
                    MultiplySpeed(1.02f);
                }

                playSound.Beep();
                Debug.Log("Hit 3+ bricks at once");
                return;
            }

            else if (brickCollisions > 3)
            {
                Reset();
                Debug.Log("Something wrong happened, resetting ball");
            }

            else
            {
                Debug.Log("Brick collisions: " + brickCollisions);

                if (!hasGhost)
                {
                        if (angle > 45f && angle < 135f) // 2nd option: 34f and 146f
                        {
                            InvertSpeedX();
                            Debug.Log("Hit side");
                            Debug.Log("### Registered Angle: " + angle);
                        }

                        else
                        {
                            InvertSpeedY();
                            Debug.Log("Hit top/bottom");
                            Debug.Log("### Registered Angle: " + angle);
                        }                        
                }

                MultiplySpeed(1.02f);
                playSound.Beep();
            }
        }

        brickCollisions = 0;
    }

    void HitPlayer()
    {

        if (transform.position.y < -4.75f) // Ball can't be reached by the player, but collided horizontally
        {
            InvertSpeedX();
            MultiplySpeed(2f);
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
                Debug.Log("Maximum speed reached");
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
            //Kill();
            rb.position = new Vector2(playerObj.transform.position.x, playerObj.transform.position.y + 0.2f);
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