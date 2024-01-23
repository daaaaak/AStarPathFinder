using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    private Tilemap map;
    private Tile wallTile;
    private Tile pathTile;
    private Tile fieldTile;

    private int mapWidth;
    private int mapHeight;
    private GameObject obj;
    private DataStorage storage;

    private Vector3Int startPosition;
    private Vector3Int endPosition;

    private List<Vector3Int> oldPath = new List<Vector3Int>();

    void Start()
    {
        obj = GameObject.Find("DataStorage");
        storage = obj.GetComponent<DataStorage>();

        wallTile = storage.GetWallTile();
        pathTile = storage.GetPathTile();
        fieldTile = storage.GetFieldTile();
        mapWidth = storage.GetMapWidth();
        mapHeight = storage.GetMapHeight();
    }


    void Update()
    {
        if (storage.GetMapUpdated() && storage.GetIsEndChosen() == true && storage.GetIsStartChosen() == true)
        {
            startPosition = storage.GetStartPosition();
            endPosition = storage.GetEndPosition();
            map = storage.GetTilemap();

            startPosition.x = startPosition.x + mapWidth / 2;
            startPosition.y = startPosition.y + mapHeight / 2;
            endPosition.x = endPosition.x + mapWidth / 2;
            endPosition.y = endPosition.y + mapHeight / 2;

            bool destinationReached = false;

            storage.SetMapUpdated(false);
            Dictionary<Vector2Int,float> costArray = new Dictionary<Vector2Int, float>();
            float[,] lengthArray = new float[mapWidth, mapHeight];
            Dictionary<Vector2Int, int> reachedBlocks = new Dictionary<Vector2Int, int>();
            int[,] parentsArray = new int[mapWidth, mapHeight];


            for (int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapWidth; j++)
                {
                    lengthArray[i, j] = Mathf.Infinity;
                }
            }


            Vector2Int currentPosition = new Vector2Int(startPosition.x, startPosition.y);
            Vector2Int destinationPosition = new Vector2Int(endPosition.x, endPosition.y);

            costArray.Add(currentPosition, ManhattanLength(currentPosition, destinationPosition));

            lengthArray[startPosition.x, startPosition.y] = 0;
            reachedBlocks.Add(currentPosition, 1);

            while (true)
            {
                if (currentPosition == destinationPosition || costArray.Count == 0)
                {
                    if(currentPosition == destinationPosition)
                    {
                        destinationReached = true;
                    }
                    break;
                }

                float minimumCost = Mathf.Infinity;
                foreach (KeyValuePair<Vector2Int, float> pair in costArray)
                {
                    if (pair.Value < minimumCost)
                    {
                        minimumCost = pair.Value;
                        currentPosition = pair.Key;
                    }
                }
                costArray.Remove(currentPosition);

                reachedBlocks[currentPosition] =  1;

                for (int i = -1; i <=1; i += 2)
                {
                    if (currentPosition.x + i >=0 && currentPosition.x + i < mapWidth)
                    {
                        Vector3Int neighbourPosition3D = new Vector3Int(currentPosition.x + i - mapWidth / 2, currentPosition.y - mapHeight / 2, 0);
                        Vector2Int neighbourPosition2D = new Vector2Int(currentPosition.x + i, currentPosition.y);
                        int t = 0;
                        reachedBlocks.TryGetValue(neighbourPosition2D, out t);
                        if (t != 1 && map.GetTile(neighbourPosition3D) != wallTile)
                        {
                            if (lengthArray[neighbourPosition2D.x, neighbourPosition2D.y] > lengthArray[currentPosition.x, currentPosition.y] + 1)
                            {   
                                float g = lengthArray[currentPosition.x, currentPosition.y] + 1;
                                lengthArray[neighbourPosition2D.x, neighbourPosition2D.y] = g;
                                float m = ManhattanLength(neighbourPosition2D, destinationPosition);
                                costArray[neighbourPosition2D] = g + m;
                                parentsArray[neighbourPosition2D.x, neighbourPosition2D.y] = Ind2Sub(currentPosition.x, currentPosition.y);
                            }
                        }
                    }

                    if (currentPosition.y + i >= 0 && currentPosition.y + i < mapHeight)
                    {
                        Vector3Int neighbourPosition3D = new Vector3Int(currentPosition.x - mapWidth / 2, currentPosition.y + i - mapHeight / 2, 0);
                        Vector2Int neighbourPosition2D = new Vector2Int(currentPosition.x, currentPosition.y + i);
                        int t = 0;
                        reachedBlocks.TryGetValue(neighbourPosition2D, out t);
                        if (t != 1 && map.GetTile(neighbourPosition3D) != wallTile)
                        {
                            
                            if (lengthArray[neighbourPosition2D.x, neighbourPosition2D.y] > lengthArray[currentPosition.x, currentPosition.y] + 1)
                            {
                                float g = lengthArray[currentPosition.x, currentPosition.y] + 1;
                                lengthArray[neighbourPosition2D.x, neighbourPosition2D.y] = g;
                                float m = ManhattanLength(neighbourPosition2D, destinationPosition);
                                costArray[neighbourPosition2D] = g + m;
                                parentsArray[neighbourPosition2D.x, neighbourPosition2D.y] = Ind2Sub(currentPosition.x, currentPosition.y);
                            }
                        }
                    }
                }
            }

            int currentSub = parentsArray[endPosition.x, endPosition.y];
            int startSub = Ind2Sub(startPosition.x, startPosition.y);

            while (oldPath.Count != 0)
            {
                Vector3Int pos = oldPath[0];
                if (map.GetTile(pos) != wallTile) 
                {                   
                    map.SetTile(pos, fieldTile);                   
                }
                oldPath.Remove(pos);
            }
            
            while(currentSub != startSub && destinationReached)
            {
                Vector2Int pos = Sub2Int(currentSub);
                Vector3Int tilePos = new Vector3Int(pos.x - mapWidth / 2, pos.y - mapHeight / 2, 0);
                oldPath.Add(tilePos);
                map.SetTile(tilePos, pathTile);
                currentSub = parentsArray[pos.x, pos.y];
            }
        }
    }

    private int Ind2Sub(int i, int j)
    {
        int sub = i * mapHeight + j;
        return sub;
    }

    private Vector2Int Sub2Int(int sub)
    {
        Vector2Int index = new Vector2Int();
        index.x = sub / mapHeight;
        index.y = sub % mapHeight;
        return index;
    }

    private int ManhattanLength(Vector2Int start, Vector2Int end)
    {
        return Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
    }
}
