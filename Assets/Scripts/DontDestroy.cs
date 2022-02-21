using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    void Awake()
    {
        if (transform.parent == null)
        {
            DontDestroyOnLoad(this.gameObject);
        }

    }

}
