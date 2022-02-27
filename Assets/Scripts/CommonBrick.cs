using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBrick : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) // This needs to be separate because Bomb Area GO is Kinematic / isTrigger = true
    {
        if (other.tag == "Bomb")
        {
            Destroy(gameObject);
        }
    }

}
