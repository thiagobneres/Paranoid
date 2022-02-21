using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountBricksLeft : MonoBehaviour
{

    public GameObject gameManagerObj;
    private GameManager game;
    public int bricksLeft;

    void Awake()
    {
        gameManagerObj = GameObject.Find("GameManager");
        game = gameManagerObj.GetComponent<GameManager>();
    }

    void Update()
    {
        Count();
    }

    void Count()
    {
        bricksLeft = transform.childCount;

        if (bricksLeft == 0 && !game.loadingNextLevel)
        {
            game.Win();
        }
    }
}
