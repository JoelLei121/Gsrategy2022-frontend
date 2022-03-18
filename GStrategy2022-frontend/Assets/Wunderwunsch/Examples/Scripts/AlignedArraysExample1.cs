using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;

/// <summary>
/// In this example we use the non-generic version of HexMap and store the tile data in a simple array which we keep aligned to the indices of the map tiles.
/// We randomly assign a terrain value to each tile and set each tile of the map boundary to terrain type 0 (water). We also can cycle through the terrain types of a tile by clicking on it
/// </summary>
public class AlignedArraysExample1 : MonoBehaviour
{
    private struct TileData
    {    
        public readonly int terrainType;
        public readonly bool forest;

        public TileData(int terrainType, bool forest)
        {
            this.terrainType = terrainType;
            this.forest = forest;
        }
    }

    [SerializeField] Vector2Int mapSize = new Vector2Int(13, 13);
    [SerializeField] GameObject tilePrefab = null;
    [SerializeField] GameObject forestPrefab = null;
    [SerializeField] Material[] materials = null;    
    private HexMap hexMap; //note that we use the non-generic version here.
    private TileData[] tiles;
    private GameObject[] tileObjects; //stores the gameObjects used for visualisation
    private HexMouse hexMouse;

    void Start()
    {
        hexMap = new HexMap(HexMapBuilder.CreateRectangularShapedMap(mapSize), null);
        tiles= new TileData[hexMap.TileCount];
        tileObjects = new GameObject[hexMap.TileCount];

        hexMouse = gameObject.AddComponent<HexMouse>();
        hexMouse.Init(hexMap);
        SetupCamera();
        InitMapData();
        InitMapVisualisation();
    }

    void Update()
    {
        if (!hexMouse.CursorIsOnMap) return;
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) //we cycle through the terrain values when clicking on a tile
        {
            Vector3Int curTile = hexMouse.TileCoord;
            int idx = hexMap.TileIndexByPosition[curTile];
            TileData tile = tiles[idx];
            int newValue = (tile.terrainType + 1) % 4;
            TileData newTile = new TileData(newValue, tile.forest); //our struct is immutable so we create a new one with the updated values.
            tiles[idx] = newTile;
            tileObjects[idx].GetComponent<Renderer>().material = materials[newTile.terrainType];

        }
    }

    private void InitMapData()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            int terrainType = Random.Range(0, 4);
            //let's change it a bit and make sure the tiles at the map bounds are always water:
            //a tile which is at the map boundary will have less than 6 neighbours, 
            //there are obviously more efficient ways to do it for specific map layouts than going that route but this works always            

            Vector3Int pos = hexMap.TilePositions[i];
            int neighbourCount = hexMap.GetTilePositions.AdjacentToTile(pos).Count;
            if (neighbourCount < 6) terrainType = 0;
            bool forest = (Random.value > 0.7f && terrainType != 0);
            tiles[i] = new TileData(terrainType, forest);                        
        }
    }

    private void InitMapVisualisation()
    {
        for(int i = 0; i < hexMap.TileCount; i++)
        {
            Vector3Int pos = hexMap.TilePositions[i];
            int terrainType = tiles[i].terrainType;
            bool forest = tiles[i].forest;
            var tileObject = GameObject.Instantiate(tilePrefab, HexConverter.TileCoordToCartesianCoord(pos), Quaternion.identity);
            tileObject.GetComponent<Renderer>().material = materials[terrainType]; 
            if(forest)
            {
                var forestObject = GameObject.Instantiate(forestPrefab);
                forestObject.transform.SetParent(tileObject.transform, false);
                forestObject.transform.position += new Vector3(0, 0.05f, 0); //ensures the forest quad is slightly about the tile and visible.
            }
            tileObjects[i] = tileObject;
        }
    }



    private void SetupCamera()
    {
        Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
        Camera.main.orthographic = true; //for this example we use an orthographic camera.
        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0); //rotates the camera to it looks at the XZ-plane
        Camera.main.orthographicSize = hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.
    }
}
