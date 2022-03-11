using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Clock : MonoBehaviour
{
    private Rigidbody2D rb;

    private GameObject soundManagerObj;
    private SoundManager sound;

    private TimerManager timer;
    private Player player;

    private Collider2D collider2d;

    private bool used = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timer = GameObject.Find("Timer Text").GetComponent<TimerManager>();

        player = GameObject.Find("Player").GetComponent<Player>();

        soundManagerObj = GameObject.Find("Sound Manager");
        sound = soundManagerObj.GetComponent<SoundManager>();

        collider2d = GetComponent<Collider2D>();
        collider2d.enabled = false;
        StartCoroutine(WaitToEnableCollider());

        rb.velocity = new Vector2(0, -1f);
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
                sound.PowerUp();
                timer.ModifyTimer(30);
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
