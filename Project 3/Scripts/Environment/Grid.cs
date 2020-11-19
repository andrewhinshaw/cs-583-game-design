using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int width;
    public int height;
    public float cellSize;
    public int[,] gridArray;

    public Grid()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y< gridArray.GetLength(1); y++)
            {
                Debug.Log(x + " " + y);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + cellSize), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + cellSize, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void GetXY(Vector3 worldPosition)
    {

    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {

    }
}
