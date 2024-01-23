using UnityEngine;
using UnityEngine.Tilemaps;

public class PaintHandler : MonoBehaviour
{
    private Tilemap map;
    private Tile fieldTile;
    private Tile wallTile;
    private Tile startTile;
    private Tile endTile;

    private Camera cam;
    private int mapWidth;
    private int mapHeight;
    private GameObject obj;
    private DataStorage storage;

    private Vector3 mousePosition;
    private Vector3 relativeMousePosition;

    private float xScaler = 1f;
    private float yScaler = 1f;
    private float mapAspect;

    void Start()
    {
        obj = GameObject.Find("DataStorage");
        storage = obj.GetComponent<DataStorage>();

        map = storage.GetTilemap();
        fieldTile = storage.GetFieldTile();
        wallTile = storage.GetWallTile();
        startTile = storage.GetStartTile();
        endTile = storage.GetEndTile();
        cam = storage.GetCamera();
        mapWidth = storage.GetMapWidth();
        mapHeight = storage.GetMapHeight();

        mapAspect = (float)mapWidth / (float)mapHeight;

        if (storage.GetOrientation() == true)
        {
            yScaler = cam.aspect / mapAspect;
        }
        else
        {
            xScaler = cam.aspect / mapAspect;
        }
    }

    void Update()
    {
        mousePosition = Input.mousePosition;

        relativeMousePosition.x = ((mousePosition.x - cam.pixelWidth / 2) * mapWidth / cam.pixelWidth) * xScaler;
        relativeMousePosition.y = ((mousePosition.y - cam.pixelHeight / 2) * mapHeight / cam.pixelHeight) / yScaler;

        if ((Mathf.Abs(relativeMousePosition.x) <= mapWidth / 2) && (Mathf.Abs(relativeMousePosition.y) <= mapHeight / 2)) 
        {
            Vector3Int pos = Vector3Int.FloorToInt(relativeMousePosition);
            TileBase activeTile = map.GetTile(pos);

            if (activeTile != startTile && activeTile != endTile)
            {
                
                if (Input.GetMouseButton(0))
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        map.SetTile(pos, fieldTile);
                    }
                    else
                    {
                        map.SetTile(pos, wallTile);
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (storage.GetIsStartChosen() == false)
                    {
                        map.SetTile(pos, startTile);
                        storage.SetIsStartChosen(true);
                        storage.SetStartPosition(pos);
                        goto end;
                    }

                    if (storage.GetIsStartChosen() && storage.GetIsEndChosen() == false)
                    {
                        map.SetTile(pos, endTile);
                        storage.SetIsEndChosen(true);
                        storage.SetEndPosition(pos);
                        goto end;
                    }

                    if (storage.GetIsStartChosen() && storage.GetIsEndChosen())
                    {
                        map.SetTile(storage.GetEndPosition(), fieldTile);
                        map.SetTile(storage.GetStartPosition(), fieldTile);
                        storage.SetIsStartChosen(false);
                        storage.SetIsEndChosen(false);
                        storage.SetStartPosition(new Vector3Int(mapWidth + 1, mapHeight + 1, 0));
                        storage.SetEndPosition(new Vector3Int(mapWidth + 1, mapHeight + 1, 0));
                    }
                end:;
                }
            }

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                storage.SetMapUpdated(true);
                storage.SetTilemap(map);
            }
        }
    }
}
