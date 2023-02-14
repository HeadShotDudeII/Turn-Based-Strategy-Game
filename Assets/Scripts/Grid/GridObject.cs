using System.Collections.Generic;

public class GridObject
{
    public GridPosition gridPosition;
    public GridSystem gridSystem;
    public List<Unit> unitList;

    public GridObject(GridPosition gridPosition, GridSystem gridSystem)
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "!";
        }


        return gridPosition.ToString() + unitString;

    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasUnitAtGridPosition()
    {
        return unitList.Count > 0;
    }

    public Unit GetFirstUnit()
    {
        if (HasUnitAtGridPosition())
        {
            return unitList[0];
        }
        else
            return null;
    }







}
