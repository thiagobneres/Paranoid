using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Transform))]
//[CanEditMultipleObjects]
public class PongMaker : Editor
{

    private int clones;
    private string cloneDir;
    private GameObject obj;
    private GameObject cloneObj;
  
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        obj = Selection.activeGameObject;

        GUILayout.Space(20);
        EditorGUILayout.LabelField("Move GameObject", EditorStyles.boldLabel);
        if (GUILayout.Button("Right")) // Moves brick GO 0.9f to the right
        {
            obj.transform.position = new Vector2(obj.transform.position.x + 0.9f, obj.transform.position.y);
        }

        if (GUILayout.Button("Left")) // Moves brick GO 0.9f to the left
        {
            obj.transform.position = new Vector2(obj.transform.position.x - 0.9f, obj.transform.position.y);
        }

        if (GUILayout.Button("Up")) // Moves brick GO 0.4f to the top
        {
            obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + 0.4f);
        }

        if (GUILayout.Button("Down")) // Moves brick GO 0.4f to the bottom
        {
            obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y - 0.4f);
        }

        GUILayout.Space(20);
        EditorGUILayout.LabelField("Clone GameObject", EditorStyles.boldLabel);

        clones = EditorGUILayout.IntSlider(clones, 1, 10);

        if (GUILayout.Button("Right")) 
        {
            CloneGO("right");
        }

        if (GUILayout.Button("Left")) 
        {
            CloneGO("left");
        }

        if (GUILayout.Button("Up")) 
        {
            CloneGO("up");
        }

        if (GUILayout.Button("Down")) 
        {
            CloneGO("down");
        }
    }

    public void CloneGO(string dir)
    {
        int i = 0;
        float originalPosX = obj.transform.position.x;
        float originalPosY = obj.transform.position.y;

        while (i < clones)
        {
            i++;

            if (dir == "up")
            {
                Vector2 newPos = new Vector2(originalPosX, originalPosY + (i * 0.4f));
                cloneObj = Instantiate(obj, newPos, Quaternion.identity);
            }

            if (dir == "down")
            {
                Vector2 newPos = new Vector2(originalPosX, originalPosY - (i * 0.4f));
                cloneObj = Instantiate(obj, newPos, Quaternion.identity);
            }

            if (dir == "right")
            {
                Vector2 newPos = new Vector2(originalPosX + (i * 0.9f), originalPosY);
                cloneObj = Instantiate(obj, newPos, Quaternion.identity);
            }

            if (dir == "left")
            {
                Vector2 newPos = new Vector2(originalPosX - (i * 0.9f), originalPosY);
                cloneObj = Instantiate(obj, newPos, Quaternion.identity);
            }

            cloneObj.transform.parent = obj.transform.parent;

        }
    }
}
