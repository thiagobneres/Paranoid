using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBrick : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Ball")
        {
            RemoveHoudiniBricks();
            Destroy(gameObject);
        }
    }

    void RemoveHoudiniBricks()
    {
        foreach (GameObject otherGO in GameObject.FindObjectsOfType<GameObject>())
        {
            string objName = otherGO.name;
            bool isHoudiniBrick = objName.Contains("Houdini"); // PS: String.Contains is case sensitive

            if (isHoudiniBrick)
            {
                otherGO.GetComponent<HoudiniBrick>().Remove();
            }
        }
    }


}
