using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_BallSpeedUp : MonoBehaviour
{
    private Rigidbody2D rb;

    private GameObject ballObj;
    private Ball ball;

    private GameObject soundManagerObj;
    private SoundManager sound;

    private bool used = false;

    private Collider2D collider2d;

    private GameObject playerObj;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ballObj = GameObject.Find("Ball");
        ball = ballObj.GetComponent<Ball>();

        soundManagerObj = GameObject.Find("Sound Manager");
        sound = soundManagerObj.GetComponent<SoundManager>();

        collider2d = GetComponent<Collider2D>();
        collider2d.enabled = false;
        StartCoroutine(WaitToEnableCollider());

        rb.velocity = new Vector2(0, -1f);

        playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Player>();
    }

    IEnumerator WaitToEnableCollider() // Prevent the ball colliding with item GO immediately after it's spawned
    {
        yield return new WaitForSeconds(0.2f);
        collider2d.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player" || col.tag == "Ball")
        {
            if (!used)
            {
                ball.MultiplySpeed(1.5f);
                sound.PowerUp();
            }
            used = true;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -6.5f || !player.canMove)
        {
            Destroy(gameObject);
        }
    }
}
