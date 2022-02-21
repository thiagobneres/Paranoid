using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Selector : MonoBehaviour
{

    private int pos = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(-1.25f, 0.45f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("down") && pos == 1)
        {
            pos = 2;
            transform.position = new Vector2(-1f, -0.46f);
        }

        if (Input.GetKey("up") && pos == 2)
        {
            pos = 1;
            transform.position = new Vector2(-1.25f, 0.45f);
        }

        if (Input.GetKeyDown("space") || Input.GetKeyDown("return"))
        {
            if (pos == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (pos == 2)
            {
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }

                else
                {
                    Application.Quit();
                }
            }
        }
    }
}
