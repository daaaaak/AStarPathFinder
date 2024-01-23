using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldSpawner : MonoBehaviour
{
    private Tilemap map;
    private Tile fieldTile;
    private Camera cam;
    private int mapWidth;
    private int mapHeight;
    private GameObject obj;
    private DataStorage storage;



    void Awake()
    {
        obj = GameObject.Find("DataStorage");
        storage = obj.GetComponent<DataStorage>();

        map = storage.GetTilemap();
        fieldTile = storage.GetFieldTile();
        cam = storage.GetCamera();
        mapWidth = storage.GetMapWidth();
        mapHeight = storage.GetMapHeight();

        float mapAspect = mapWidth / mapHeight;

        if (mapAspect > cam.aspect)
        {
            cam.orthographicSize = mapWidth / cam.aspect / 2;
            storage.SetOrientation(true);
        }
        else
        {
            cam.orthographicSize = mapHeight / 2;
            storage.SetOrientation(false);
        }

        for (int i = -mapWidth / 2; i < mapWidth / 2; i++) 
        {
            for (int j = -mapHeight / 2; j < mapHeight / 2; j++) 
            {
                Vector3Int pos = new Vector3Int(i, j, -20);
                map.SetTile(pos, fieldTile);
            }
        }
    }
}
