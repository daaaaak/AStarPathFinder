using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataStorage : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private Tile fieldTile;
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile startTile;
    [SerializeField] private Tile endTile;
    [SerializeField] private Tile pathTile;

    [SerializeField] private Tilemap tileMap;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    private bool isEndChosen = false;
    private bool isStartChosen = false;
    private Vector3Int startPosition = new Vector3Int();
    private Vector3Int endPosition = new Vector3Int();

    private bool orientation = false;

    private bool mapUpdated = false;

    public void OnValidate()
    {
        if(mapWidth < 2)
        { 
            mapWidth = 2; 
        }

         if(mapHeight < 2)
        { 
            mapHeight = 2;
        }
    }

    public Camera GetCamera() 
    {
        return cam;
    }

    public Tile GetFieldTile()
    {
        return fieldTile;
    }

    public Tile GetWallTile()
    {
        return wallTile;
    }

    public Tile GetStartTile()
    {
        return startTile;
    }

    public Tile GetPathTile()
    {
        return pathTile;
    }

    public Tile GetEndTile()
    {
        return endTile;
    }

    public int GetMapWidth()
    {
        return mapWidth;
    }

    public int GetMapHeight()
    {
        return mapHeight;
    }

    public Tilemap GetTilemap() 
    { 
        return tileMap;
    }

    public void SetTilemap(Tilemap temp)
    {
        tileMap = temp;
    }

    public void SetOrientation(bool temp)
    {
        orientation = temp;
    }

    public bool GetOrientation()
    {
        return orientation;
    }

    public Vector3Int GetStartPosition()
    {
        return startPosition;
    }

    public Vector3Int GetEndPosition()
    {
        return endPosition;
    }

    public void SetStartPosition(Vector3Int temp)
    {
        startPosition = temp;
    }

    public void SetEndPosition(Vector3Int temp)
    {
        endPosition = temp;
    }

    public bool GetIsStartChosen()
    {
        return isStartChosen;
    }

    public bool GetIsEndChosen()
    {
        return isEndChosen;
    }

    public void SetIsStartChosen(bool temp)
    {  
        isStartChosen = temp;
    }
    public void SetIsEndChosen(bool temp)
    {
        isEndChosen = temp;
    }

    public bool GetMapUpdated()
    { 
        return mapUpdated; 
    }

    public void SetMapUpdated(bool temp)
    {  
        mapUpdated = temp; 
    }
}

