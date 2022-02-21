using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{

    private bool skip = false;
    private Rigidbody2D rb;
    public GameObject playerObj;
    public GameObject leftboundObj;
    public GameObject topboundObj;
    public GameObject rightboundObj;
    public GameObject brickObj;
    public GameObject canvaObj;
    private AudioSource audioSource;
    public AudioClip beep;
    private int collisions = 0;
    private bool hitBrick = false;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(3.5f, 0.75f);
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        IntroScene();
    }

    // Update is called once per frame
    void Update()
    {
       if (collisions > 0)
        {
            collisions = 0;
        }

       if (hitBrick && transform.position.x > 1.5f)
        {
            canvaObj.SetActive(true);
            Destroy(gameObject);
        }
    }

    void IntroScene()
    {
        rb.velocity = new Vector2(-1.9f, -3f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player" && !skip)
        {
            audioSource.PlayOneShot(beep, 0.7F);
            playerObj.GetComponent<SpriteRenderer>().enabled = true;
            rb.velocity = new Vector2(-2.5f, 3f);
        }

        if (col.tag == "Bound" && transform.position.y < 4.5f && !skip) // Hit side bounds
        {
            audioSource.PlayOneShot(beep, 0.7F);
            leftboundObj.GetComponent<SpriteRenderer>().enabled = true;
            rightboundObj.GetComponent<SpriteRenderer>().enabled = true;
            topboundObj.GetComponent<SpriteRenderer>().enabled = true;
            rb.velocity = new Vector2(1.9f, 3f);
        }

        if (col.tag == "Bound" && transform.position.y >= 4.5f && !skip) // Hit top bound
        {
            audioSource.PlayOneShot(beep, 0.7F);
            rb.velocity = new Vector2(1.9f, -3f);
            StartCoroutine(ShowBricks());
        }


        if (col.tag == "Common Brick" && !skip && collisions == 0)
        {

            audioSource.PlayOneShot(beep, 0.7F);
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);

            if (!hitBrick)
            {
                hitBrick = true;
            }
        }

        collisions++;
    }

    IEnumerator ShowBricks()
    {
        yield return new WaitForSeconds(1.4f);
        brickObj.SetActive(true);
    }

}
