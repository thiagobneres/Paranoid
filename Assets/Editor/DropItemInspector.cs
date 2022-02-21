using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DropItem))]
[CanEditMultipleObjects]
public class DropItemInspector : Editor
{

    SerializedProperty listOfItems;
    string[] _items = new[] { "Life +", "Life -", "Bar Speed +", "Bar Speed -", "Ball Speed +", "Ball Speed -", "Expand Bar", "Shrink Bar", "Invert Bar", "Bomb", "Ghost", "Laser" };
    int _index;

    public override void OnInspectorGUI()
    {
        var DropItem = target as DropItem;

        // Set the choice index to the previously selected index
        _index = Array.IndexOf(_items, DropItem.item);

        // If the choice is not in the array then the _choiceIndex will be -1 so set back to 0
        if (_index < 0)
            _index = 0;

        _index = EditorGUILayout.Popup(_index, _items);

        // Update the selected choice in the underlying object
        DropItem.item = _items[_index];
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }

}