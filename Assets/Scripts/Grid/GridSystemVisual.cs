using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform gridVisualTransformPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    private int height;
    private int width;

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Yellow,
        RedSoft
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

    }

    void Start()
    {
        width = LevelGrid.Instance.GetWidth();
        height = LevelGrid.Instance.GetHeight();

        gridSystemVisualSingleArray = new GridSystemVisualSingle[width, height];
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedPosition += LevelGrid_OnAnyUnitMovedPosition;
        CreateGridVisual();
        UpdateGridVisual();
    }

    private void CreateGridVisual()
    {


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingle =
                       Instantiate(gridVisualTransformPrefab, LevelGrid.Instance.GetWorldPositionFromGridPos(gridPosition), Quaternion.identity);
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingle.GetComponent<GridSystemVisualSingle>();
            }
        }

    }

    public void HideGridVisual()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridSystemVisualSingleArray[x, z].HideVisual();
            }
        }
    }



    public void UpdateGridVisual()
    {
        HideGridVisual();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        GridVisualType gridVisualType = GridVisualType.White;
        switch (selectedAction)
        {
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowShootActionRange(UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition(), shootAction.GetShootRange(), GridVisualType.RedSoft);
                break;

        }
        ShowGridVisualGridPositionList(selectedAction.GetValidGridPositionsList(), gridVisualType);

    }


    public void ShowShootActionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + gridPosition;
                if (testGridPosition == gridPosition) continue;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range) continue;
                validGridPositions.Add(testGridPosition);

            }
        }

        ShowGridVisualGridPositionList(validGridPositions, gridVisualType);
    }



    public void ShowGridVisualGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].ShowVisual(GetGridVisualTypeMaterial(gridVisualType));
        }

    }


    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualType == gridVisualTypeMaterial.gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }


        }
        Debug.Log("Could not Find the given type" + gridVisualType);
        return null;
    }

}
