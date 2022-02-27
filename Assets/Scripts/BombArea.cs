using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArea : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col) // Note that this requires IsTrigger in Bomb Area GO
    {

        if (col.tag == "Common Brick") // Need to make sure that gray brick is considered here
        {
            Destroy(gameObject);
            Debug.Log("Bomb area GO destroyed properly");
        }
    }
}

