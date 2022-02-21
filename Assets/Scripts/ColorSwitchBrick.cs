using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitchBrick : MonoBehaviour
{

    private int hits = 0;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ball")

        {
            hits++;

            if (hits == 1)
            {
                ColorSwitch();
                return;
            }

            if (hits == 2)
            {
                Destroy(gameObject);
            }

        }

        if (col.tag == "Bomb")
        {
            Destroy(gameObject);
        }

    }

    void ColorSwitch()
    {
        if (transform.name.Contains("Lime")) // Lime
        {
            sr.color = new Color(1f, 0f, 0.5f); // Turns pink
        }

        if (transform.name.Contains("Pink")) // Pink
        {
            sr.color = new Color(0f, 1f, 0f); // Turns lime
        }

    }
}
