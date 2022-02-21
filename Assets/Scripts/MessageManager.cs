using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageManager : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void Clear()
    {
        textMesh.text = "";
    }

    public void ReadyToLaunch()
    {
        textMesh.text = "PRESS SPACE";
    }

    public void TimeUp()
    {
        int message = Random.Range(1, 4); // Maximally exclusive

        if (message == 1)
        {
            string line1 = "TIME'S UP!";
            //string line2 = "Did you forget what you have to do?";
            //string text = line1 + "\n" + "\n" + line2;
            //textMesh.text = text;
            textMesh.text = line1;
        }

        if (message == 2)
        {
            string line1 = "TIME'S UP!";
            //string line2 = "Yeah, there's a clock in case you didn't notice it";
            //string text = line1 + "\n" + "\n" + line2;
            //textMesh.text = text;
            textMesh.text = line1;
        }

        if (message == 3)
        {
            string line1 = "TIME'S UP!";
            //string line2 = "Seriously, I don't have the whole day for this";
            //string text = line1 + "\n" + "\n" + line2;
            //textMesh.text = text;
            textMesh.text = line1;
        }
    }

    public void LostBall()
    {
        int message = Random.Range(1, 4); // Maximally exclusive

        if (message == 1)
        {
            string line1 = "BALL LOST!";
            //string line2 = "Maybe you should play something else like Tetris";
            //string text = line1 + "\n" + "\n" + line2;
            //textMesh.text = text;
            textMesh.text = line1;
        }

        if (message == 2)
        {
            string line1 = "BALL LOST!";
            //string line2 = "I'm kind of mad. I liked that one";
            //string text = line1 + "\n" + "\n" + line2;
            //textMesh.text = text;
            textMesh.text = line1;
        }

        if (message == 3)
        {
            string line1 = "BALL LOST!";
            //string line2 = "Try harder next time, slowpoke!";
            //string text = line1 + "\n" + "\n" + line2;
            //textMesh.text = text;
            textMesh.text = line1;
        }

    }

    public void Win()
    {
        textMesh.text = "WELL DONE!";
    }

    // you don't have the balls to play this game

    // hit walls and didn't hit any bricks

    // max speed

    // min speed

    // hurryup

}
