using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField]
    public bool isAttached = true;

    private Rigidbody2D rb;
    private float startSpeedX;
    private float startSpeedY = 3f;

    [SerializeField]
    private float speedX;
    [SerializeField]
    private float speedY;

    private float minX = -5.865f;
    private float maxX = 5.865f;
    private float maxY = 4.565f;
    private float offsetSpeed1 = 0.05f;
    private float offsetSpeed2 = 0.3f;
    private float offsetSpeed3 = 0.45f;
    private float offsetSpeed4 = 0.598f;
    private float bounceSpeedMultiplier = 1.01f;
    private float speed1 = 0.95f;
    private float speed2 = 1f;
    private float speed3 = 1.02f;
    private float speed4 = 1.05f;
    private float minSpeedY = 2f;
    private float maxSpeedY = 6f;

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

    int collisionsInFrame; // Adjusts the Y axis inversion in case of 2 collisions at the same time

    void Awake() // Need to use Awake here, otherwise other scripts try to access the rb before it is initialized
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
        }

        player = playerObj.GetComponent<Player>();

        if (soundManagerObj == null)
        {
            soundManagerObj = GameObject.Find("Sound Manager");
        }

        playSound = soundManagerObj.GetComponent<SoundManager>();

        if (timerObj == null)
        {
            timerObj = GameObject.Find("Timer Text");
        }

        timer = timerObj.GetComponent<TimerManager>();

        if (gameManagerObj == null)
        {
            gameManagerObj = GameObject.Find("GameManager");
        }

        game = gameManagerObj.GetComponent<GameManager>();

        if (messageManagerObj == null)
        {
            messageManagerObj = GameObject.Find("Message");
        }

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
        LimitSpeed();
        BallDrop();
        StopBall(); // Prevents the ball from moving after life is lost
        Reset();


        collisionsInFrame = 0;
    }

    void GetSpeed()
    {
        speedX = rb.velocity.x;
        speedY = rb.velocity.y;
    }

    void Launch()
    {
        if (isAttached && transform.parent != null && Input.GetKeyDown("space"))
        {
            Detach();
            timer.StartTimer(); // Starts timer when ball is launched

            int i = Random.Range(0, 2); // Necessary to avoid randomizer picking a super low X speed
            // Random Range with integer is not maximally inclusive - The above can return only 0 or 1

            if (i == 0)
            {
                startSpeedX = Random.Range(-2f, -1f);
            }

            if (i == 1)
            {
                startSpeedX = Random.Range(2f, 1f);
            }

            rb.velocity = new Vector2(startSpeedX, startSpeedY);
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
        }
    }

    public void MultiplySpeed(float multiplier)
    {

        rb.velocity *= multiplier;
        GetSpeed();
    }

    void InvertSpeedX()
    {
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    void InvertSpeedY()
    {
        rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && rb.velocity.y != 0)
        {
            HitPlayer();
            playSound.Beep();
        }

        if (col.tag == "Common Brick")
        {

            if (hasBomb)
            {
                InvertSpeedX();
                InvertSpeedY();
                ExplodeBomb();
                Debug.Log("Exploded bomb");
                return;
            }

            collisionsInFrame++;

            float brickPosX = col.transform.position.x;
            float ballPosX = transform.position.x;
            float offsetX = Mathf.Abs(brickPosX - ballPosX);

            if (offsetX >= 0.47f) // Hit sides // 0.42 default
            {

                InvertSpeedX();

                if (collisionsInFrame == 2)
                {
                    InvertSpeedX();
                }

                if (collisionsInFrame == 3)
                {
                    InvertSpeedY();
                }
            }

            if (offsetX < 0.47f) // Hit top or bottom // 0.42 default
            {

                InvertSpeedY();

                if (collisionsInFrame == 2)
                {
                    InvertSpeedY();
                }

                if (collisionsInFrame == 3)
                {
                    InvertSpeedX();
                }
            }
            
            if (collisionsInFrame == 1)
            {
                MultiplySpeed(1.02f);
                playSound.Beep();
            }

        }
    }

    void HitPlayer()
    {
        float playerX = playerObj.transform.position.x;
        float offset = Mathf.Abs(playerX - transform.position.x);

        float ballX = transform.position.x;
        float dist = ballX - playerX;

        if (offset <= offsetSpeed1)
        {
            
            MultiplySpeed(speed1);
            float newSpeedX = Random.Range(((rb.velocity.x - 0) / 2), rb.velocity.x); // Widen angle
            rb.velocity = new Vector2(newSpeedX, rb.velocity.y);

        }

        if (offset > offsetSpeed1 && offset <= offsetSpeed2)
        {
            MultiplySpeed(speed2);
        }

        if (offset > offsetSpeed2 && offset <= offsetSpeed3)
        {
            MultiplySpeed(speed3);

            if (dist > 0) // Ball ahead of bar
            {
                float newSpeedX = Random.Range((Mathf.Abs(rb.velocity.x) + 1f), rb.velocity.x); // Sharpen angle
                rb.velocity = new Vector2(newSpeedX, rb.velocity.y);
            }

            if (dist < 0) // Ball behind bar
            {
                float newSpeedX = Random.Range((-Mathf.Abs(rb.velocity.x) - 1f), rb.velocity.x); // Sharpen angle
                rb.velocity = new Vector2(newSpeedX, rb.velocity.y);
            }
        }

        if (offset > offsetSpeed3 && offset <= offsetSpeed4)
        {
            MultiplySpeed(speed4);

            if (dist > 0) // Ball ahead of bar
            {
                float newSpeedX = Random.Range((Mathf.Abs(rb.velocity.x) + 1.5f), rb.velocity.x); // Sharpen angle
                rb.velocity = new Vector2(newSpeedX, rb.velocity.y);
            }

            if (dist < 0) // Ball behind bar
            {
                float newSpeedX = Random.Range((-Mathf.Abs(rb.velocity.x) - 1.5f), rb.velocity.x); // Sharpen angle
                rb.velocity = new Vector2(newSpeedX, rb.velocity.y);
            }

        }

        if (transform.position.y <= -4.75f) // Hit the bar, but it's TOO LATE
        {
            InvertSpeedX();
            player.canMove = false;
            return;
        }

        InvertSpeedY();
    }

    void LimitSpeed()
    {
        if (Mathf.Abs(rb.velocity.y) > maxSpeedY)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxSpeedY);
            }
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -maxSpeedY);
            }
        }

        if (Mathf.Abs(rb.velocity.y) < minSpeedY && !isAttached)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, minSpeedY);
            }
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -minSpeedY);
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
            hasBomb = false;
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
}
