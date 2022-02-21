using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarpBrick : MonoBehaviour
{

    private GameObject otherWarpBrick;
    public bool canWarpBall = true;


    private GameObject soundManagerObj;
    private SoundManager sound;

    private Ball ball;

    // Start is called before the first frame update
    void Start()
    {
        // Find the other warp brick obj in the scene

        foreach (GameObject otherGO in GameObject.FindObjectsOfType<GameObject>())
        {
            string objName = otherGO.name;
            bool isWarpBrick = objName.Contains("Warp"); // PS: String.Contains is case sensitive

            // There will be always 2 warp bricks in the scene. We want the other
            if (isWarpBrick && otherGO != this.gameObject) 
            {
                otherWarpBrick = otherGO;
            }
        }

        // Get SoundManager component

        soundManagerObj = GameObject.Find("Sound Manager");
        sound = soundManagerObj.GetComponent<SoundManager>();

        // Get Ball component

        ball = GameObject.Find("Ball").GetComponent<Ball>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Warp Ball
        // canWarpBall is important to avoid warp loop when ball enters the other obj collider area
        if (col.tag == "Ball" && canWarpBall)
        {
            otherWarpBrick.GetComponent<WarpBrick>().canWarpBall = false;
            Vector3 targetPos = otherWarpBrick.transform.position;
            ball.Warp(targetPos);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // Once ball leaves the brick, make sure it can warp the ball again
        if (col.tag == "Ball" && !canWarpBall)
        {
            canWarpBall = true;
        }
    }

}
