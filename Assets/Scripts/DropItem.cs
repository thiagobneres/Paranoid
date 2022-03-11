using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{

    public string item;
    private bool droppedItem = false; 
    private GameObject itemGO;
    // Variable is set via Inspector and controlled by DropItemInspector.cs in /Editor

    // "Life +" 
    // Add 1 life

    // "Life -"
    // Remove 1 life

    // "Bar Speed +"
    // Increase bar movement speed

    // "Bar Speed -" 
    // Decrease bar movement speed

    // "Ball Speed +" 
    // Speed up ball

    // "Ball Speed -" 
    // Slow down ball

    // "Expand Bar" 
    // Bar gets wider

    // "Shrink Bar" 
    // Bar gets narrower

    // "Invert Bar" 
    // Player inputs are inverted (right and left)

    // "Bomb" 
    // Instantiates a circle area transparent prefab on the first collision with a brick. All bricks that collide with the circle get destroyed

    // "Ghost" 
    // Collision with bricks doesn't invert ball movement until it hits 2 other fixed objects (player, bound, or gray brick)

    // "Bot"
    // Bar Y axis follows ball for 10s

    // "Clock"
    // Time + 30s

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Ball" && !droppedItem)
        {
            droppedItem = true; // Prevent items dropping twice when ball hits switching blocks

            if (item == "Life +")
            {
                itemGO = Instantiate(Resources.Load("Items/Life", typeof(GameObject))) as GameObject;
            }

            if (item == "Life -")
            {
                itemGO = Instantiate(Resources.Load("Items/Death", typeof(GameObject))) as GameObject;
            }

            if (item == "Bar Speed +")
            {
                itemGO = Instantiate(Resources.Load("Items/Bar Speed Up", typeof(GameObject))) as GameObject;
            }

            if (item == "Bar Speed -")
            {
                itemGO = Instantiate(Resources.Load("Items/Bar Speed Down", typeof(GameObject))) as GameObject;
            }

            if (item == "Ball Speed +")
            {
                itemGO = Instantiate(Resources.Load("Items/Ball Speed Up", typeof(GameObject))) as GameObject;
            }

            if (item == "Ball Speed -")
            {
                itemGO = Instantiate(Resources.Load("Items/Ball Speed Down", typeof(GameObject))) as GameObject;
            }

            if (item == "Expand Bar")
            {
                itemGO = Instantiate(Resources.Load("Items/Extend Bar", typeof(GameObject))) as GameObject;
            }

            if (item == "Shrink Bar")
            {
                itemGO = Instantiate(Resources.Load("Items/Shrink Bar", typeof(GameObject))) as GameObject;
            }

            if (item == "Invert Bar")
            {
                itemGO = Instantiate(Resources.Load("Items/Invert Bar", typeof(GameObject))) as GameObject;
            }

            if (item == "Bomb")
            {
                itemGO = Instantiate(Resources.Load("Items/Bomb", typeof(GameObject))) as GameObject;
            }

            if (item == "Ghost")
            {
                itemGO = Instantiate(Resources.Load("Items/Ghost", typeof(GameObject))) as GameObject;
            }

            if (item == "Bot")
            {
                itemGO = Instantiate(Resources.Load("Items/Bot", typeof(GameObject))) as GameObject;
            }

            if (item == "Clock")
            {
                itemGO = Instantiate(Resources.Load("Items/Clock", typeof(GameObject))) as GameObject;
            }




            itemGO.transform.position = new Vector2(transform.position.x, transform.position.y);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
