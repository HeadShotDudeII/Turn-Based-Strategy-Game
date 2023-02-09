using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform gridVisualTransformPrefab;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    private int height;
    private int width;

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
        CreateGridVisual();
    }



    // Update is called once per frame
    void Update()
    {
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

    public void ShowGridVisual(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].ShowVisual();
        }

    }

    public void UpdateGridVisual()
    {
        HideGridVisual();
        Unit unitSelected = UnitActionSystem.Instance.GetSelectedUnit();
        ShowGridVisual(unitSelected.GetSelectedBaseAction().GetValidGridPositionsList());

    }



}
