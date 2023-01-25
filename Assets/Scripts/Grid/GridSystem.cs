using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem 
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;
    private GridDebugObject[,] gridDebugObjectArray;

    public GridSystem(int width,int height,float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridObjectArray = new GridObject[width, height]; // array need to initialized.
        gridDebugObjectArray = new GridDebugObject[width, height];
        CreateGridObjects();
    }

    public void CreateGridObjects()
    {
        for(int x = 0; x<width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(gridPosition, this);
                //Debug.DrawLine(GetWorldPositionFromGridPos(x, z), GetWorldPositionFromGridPos(x, z) + Vector3.right * .2f, Color.white, 1000);
            }
        }
    }

    public Vector3 GetWorldPositionFromGridPos(GridPosition gridPosition)
    {
        return new Vector3(Mathf.RoundToInt(gridPosition.x * cellSize), 0, Mathf.RoundToInt(gridPosition.z * cellSize));
    }

    public GridPosition GetGridPositionFromWorldPos(Vector3 position)
    {
        return new GridPosition(
            Mathf.RoundToInt(position.x / cellSize), 
            Mathf.RoundToInt(position.z / cellSize));
    }

    public Vector3 GetWorldPositionFromGridPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, gridPosition.z * cellSize);
    }

    public void CreateDebugObjects(Transform debugPrefabs)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridDebugTransform=GameObject.Instantiate(debugPrefabs, GetWorldPositionFromGridPos(gridPosition), Quaternion.identity);
                gridDebugObjectArray[x,z] = gridDebugTransform.GetComponent<GridDebugObject>();
                gridDebugObjectArray[x,z].ConnectGridObject(gridObjectArray[x,z]);
                //gridDebugObject.DisplayPos(GetGridObject(gridPosition));
            }
        }
    }

    public void DisplayGridDebugObjects()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridDebugObjectArray[x, z].DisplayPos(gridObjectArray[x, z]);
            }

        }
        
    }


    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public GridPosition GetGridPosition(GridObject gridObject)
    {
        return gridObject.gridPosition;

    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return
                gridPosition.x >= 0 &&
                gridPosition.z >= 0 &&
                gridPosition.x < width &&
                gridPosition.z < height;
    }

    public bool HasUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = GetGridObject(gridPosition);
        return gridObject.HasUnitAtGridPosition();
    }


}
