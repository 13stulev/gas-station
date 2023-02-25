using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class GridBuildingSystem3D : MonoBehaviour {
    
    public static GridBuildingSystem3D Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    

<<<<<<< Updated upstream
    private GridXZ<GridObject> grid;
    //private GridXZ<GridObject> gridService;
    //private GridXZ<GridObject> gridroad;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null; // сериалайз филд
=======
    public GridXZ grid;
    public GridXZ serviceGrid;
    public GridXZ roadGrid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
>>>>>>> Stashed changes
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;

    private void Awake() {
        Instance = this;

        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new GridXZ(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0));
        serviceGrid = new GridXZ(3, gridHeight, cellSize, new Vector3(gridWidth * cellSize, 0, 0));
        roadGrid = new GridXZ(gridWidth + 3, 1, cellSize, new Vector3(0, 0, -cellSize));

        placedObjectTypeSO = placedObjectTypeSOList[0];
        for(int i = 0; i < gridWidth + 3; i++)
        {
            PlacedObject_Done placedObject = PlacedObject_Done.Create(roadGrid.GetWorldPosition(i, 0), new Vector2Int(i, 0), dir, placedObjectTypeSOList[1]);
            roadGrid.GetGridObject(i, 0).SetPlacedObject(placedObject);
        }
    }
    
    public class GridObject : IHeapItem<GridObject> {

        public GridXZ grid;
        private int x;
        private int y;
        private bool isWalkable;
        private bool isStopable;
        public PlacedObject_Done placedObject;

<<<<<<< Updated upstream
        // одна ячейка с хранением, есть ли здание и координаты
        public GridObject(GridXZ<GridObject> grid, int x, int y) {
=======
        public int gCost;
        public int hCost;
        public GridObject parent;
        int heapIndex;
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public GridObject(GridXZ grid, int x, int y) {
>>>>>>> Stashed changes
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public int getX() {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject) {
            this.placedObject = placedObject;
            isWalkable = placedObject.isWalkable();
            grid.TriggerGridObjectChanged(x, y, isWalkable);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            isWalkable = false;
            grid.TriggerGridObjectChanged(x, y, false);
        }

        public PlacedObject_Done GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }
        public bool getIsWalkable()
        {
            return isWalkable;
        }

        public int HeapIndex {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        public int CompareTo(GridObject other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }

    // проверяет ли кнопка или нет, проверяет координаты нажатой ячейки, делает их читаемыми, ставит объект на ячейку
    private void Update() {
        // добавить if на положение мышки вне сетки
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null && !IsMouseOverUI()) {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);
   
            // Test Can Build
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canBuild = false;
                    break;
                }
            }
   
            if (canBuild) {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
   
                PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
   
                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
   
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
   
                //DeselectObjectType();
            } else {
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                UtilsClass.CreateWorldTextPopup("Здесь нельзя строить", placedObjectWorldPosition);
                
            }
        }
   
       if (Input.GetKeyDown(KeyCode.R)) {
           dir = PlacedObjectTypeSO.GetNextDir(dir);
       }
   
       if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.Alpha6)) { placedObjectTypeSO = placedObjectTypeSOList[5]; RefreshSelectedObjectType(); }
       if (Input.GetKeyDown(KeyCode.R)) { var car = GameObject.Find("sedanpref"); car.transform.position = grid.GetWorldPosition(5, 1); car.GetComponent<Unit>().request();}
   
       if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }
   
        // удаление объекта правой кнопкой мыши
       if (Input.GetMouseButtonDown(1)) {
           Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
           if (grid.GetGridObject(mousePosition) != null) {
               // Valid Grid Position
               PlacedObject_Done placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
               if (placedObject != null) {
                   // Demolish
                   placedObject.DestroySelf();
   
                   List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                   foreach (Vector2Int gridPosition in gridPositionList) {
                       grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                   }
               }
           }
       }
   }

    private void DeselectObjectType() {
        placedObjectTypeSO = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z);
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

}
