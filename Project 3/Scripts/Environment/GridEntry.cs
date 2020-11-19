using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridEntry
{
    [HideInInspector]
    public enum Value
    {
        Empty,
        Mine,
        Num_1,
        Num_2,
        Num_3,
        Num_4,
        Num_5,
        Num_6,
        Num_7,
        Num_8,
    }

    public GameObject go;

    [HideInInspector]
    public Vector2 position;

    [HideInInspector]
    public Value value;

    [HideInInspector]
    public bool isRevealed;

    [HideInInspector]
    public bool isMarked;

    [HideInInspector]
    public List<int> neighborList;
}
