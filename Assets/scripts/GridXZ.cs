

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static GridBuildingSystem3D;

public class GridXZ {

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int z;
        public bool isWalkable;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private GridObject[,] gridArray;

    public GridXZ(int width, int height, float cellSize, Vector3 originPosition) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new GridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int z = 0; z < gridArray.GetLength(1); z++) {
                gridArray[x, z] = new GridObject(this, x, z);
            }
        }

        bool showDebug = true;
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int z = 0; z < gridArray.GetLength(1); z++) {
                    debugTextArray[x, z] = UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z), 15, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, z) - new Vector3(cellSize / 2, 0, cellSize / 2), GetWorldPosition(x, z + 1) - new Vector3(cellSize / 2, 0, cellSize / 2), Color.white, 10000f, true);
                    Debug.DrawLine(GetWorldPosition(x, z) - new Vector3(cellSize / 2, 0, cellSize / 2), GetWorldPosition(x + 1, z) - new Vector3(cellSize / 2, 0, cellSize / 2), Color.white, 10000f, true);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height) - new Vector3(cellSize / 2, 0, cellSize / 2), GetWorldPosition(width, height) - new Vector3(cellSize / 2, 0, cellSize / 2), Color.white, 10000f, true);
            Debug.DrawLine(GetWorldPosition(width, 0) - new Vector3(cellSize / 2, 0, cellSize / 2), GetWorldPosition(width, height) - new Vector3(cellSize / 2, 0, cellSize / 2), Color.white, 10000f, true);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }
    public void increaseHeight()
    {

    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z) {
        return new Vector3(x, 0, z) * cellSize + originPosition - new Vector3(cellSize, 0, cellSize);
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z) {
        x = Mathf.FloorToInt(((worldPosition - originPosition ).x + (cellSize / 2)) / cellSize) + 1;
        z = Mathf.FloorToInt(((worldPosition - originPosition).z + (cellSize / 2)) / cellSize) + 1;
    }
    public List<GridObject> GetNeighbours(GridObject node)
    {
        List<GridObject> neighbours = new List<GridObject>();
            for (int y = -1; y <= 1; y+=2)
            {
                int x = 0;
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.getX() + x;
                int checkY = node.getY() + y;

                if (checkX >= 0 && checkX < height && checkY >= 0 && checkY < height)
                {
                    neighbours.Add(gridArray[checkX, checkY]);
                }
            }


        for (int x = -1; x <= 1; x+=2)
        {
            int y = 0;
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.getX() + x;
                int checkY = node.getY() + y;
                if (checkX >= 0 && checkX < height && checkY >= 0 && checkY < height)
                {
                    neighbours.Add(gridArray[checkX, checkY]);
                }
        }

        return neighbours;
    }

    public void TriggerGridObjectChanged(int x, int z, bool isWalkable) {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z, isWalkable = isWalkable });
    }


    public GridObject GetGridObject(int x, int z) {
        if (x >= 0 && z >= 0 && x < width && z < height) {
            return gridArray[x, z];
        } else {
            return default(GridObject);
        }
    }

    public GridObject GetGridObject(Vector3 worldPosition) {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition) {
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, width - 1),
            Mathf.Clamp(gridPosition.y, 0, height - 1)
        );
    }

}
