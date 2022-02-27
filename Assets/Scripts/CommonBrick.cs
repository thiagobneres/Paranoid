using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBrick : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball" || col.gameObject.tag == "Bomb")
        {
            Debug.Log("Brick destroyed");
            Destroy(gameObject);
        }
    }

}
