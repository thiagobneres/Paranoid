using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBrick : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ball" || col.tag == "Bomb")
        {
            Destroy(gameObject);
        }
    }

}
