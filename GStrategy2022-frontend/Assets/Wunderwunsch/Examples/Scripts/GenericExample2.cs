using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Examples
{ 
    /// <summary>
    /// Example 2: Line of Sight with sight blocking tiles. In this example we will use a map which itself is hexagonal
    /// </summary>
    public class GenericExample2 : MonoBehaviour
    {
        //you will find the prefabs and materials used here in the "NonScriptAssets folder or of the package.
        [SerializeField] private int mapRadius = 11; // the mapSize, can be set in inspector 
        [SerializeField] private GameObject tilePrefab = null; // the prefab we use for each Tile -> use TilePrefab.prefab
        [SerializeField] private GameObject tileVisionMarker = null; // the prefab we use for each Tile -> use TilePrefab.prefab
        [SerializeField] private GameObject edgeVisionBorder = null;
        [SerializeField] private List<Material> materials = null; // the materials we want to assign to the tiles for visualisation purposes -> set size to 4 in inspector and add TileMat1 to TileMat4
        private HexMap<int> hexMap; // our map. For this example we create a map where an integer represents the data of each tile 
        private HexMouse hexMouse = null; // the HexMouse component we add to keep track of the mouse position
        private GameObject[] tileObjects; // this will contain all the GameObjects for visualisation purposes, their array index corresponds with the index of our Tiles
        private List<GameObject> visionMarkers; //we will use this to display the border of the vision range

        void Start ()
        {
            hexMap = new HexMap<int>(HexMapBuilder.CreateHexagonalShapedMap(mapRadius), null); //creates a HexMap using one of the pre-defined shapes in the static MapBuilder Class            
            hexMouse = gameObject.AddComponent<HexMouse>(); //we attach the HexMouse script to the same gameObject this script is attached to, could also attach it anywhere else
            hexMouse.Init(hexMap); //initializes the HexMouse 
            
            InitMap();
            SetupCamera(); //set camera settings so that the map is captured by it
            
        }

        void Update ()
        { 
            if (!hexMouse.CursorIsOnMap) return; // if we are not on the map we won't do anything so we can return

            Vector3Int mouseTilePosition = hexMouse.TileCoord;

            if (Input.GetMouseButtonDown(0))
            {
                var visibleTiles = CalculateVisibleTiles(mouseTilePosition, 5);
                UpdateVisionMarkers(visibleTiles);
            }
        }

        private void InitMap()
        {
            tileObjects = new GameObject[hexMap.TilesByPosition.Count]; //creates an array with the size equal to the number on tiles of the map
            visionMarkers = new List<GameObject>();

            foreach (var tile in hexMap.Tiles) //loops through all the tiles, assigns them a random value and instantiates and positions a gameObject for each of them.
            {
                tile.Data = (Random.Range(0, 4));
                GameObject instance = GameObject.Instantiate(tilePrefab);
                instance.GetComponent<Renderer>().material = materials[tile.Data];
                instance.name = "MapTile_" + tile.Position;
                instance.transform.position = tile.CartesianPosition;
                tileObjects[tile.Index] = instance;
            }
        }

        private void UpdateVisionMarkers(IEnumerable<Vector3Int> visibleTiles)
        {
            foreach(GameObject g in visionMarkers)
            {
                Destroy(g);
            }
            visionMarkers.Clear();
           
            foreach(var tilePos in visibleTiles)
            {
                GameObject tileObj = Instantiate(tileVisionMarker, HexConverter.TileCoordToCartesianCoord(tilePos,0.1f), Quaternion.identity); 
                //0.1f = explicitly set y-Coord of the tile so it is slightly above the tiles of the map
                visionMarkers.Add(tileObj);                
            }

            List<Vector3Int> borderEdges = hexMap.GetEdgePositions.TileBorders(visibleTiles);

            foreach(var edgePos in borderEdges)
            {
                EdgeAlignment orientation = HexUtility.GetEdgeAlignment(edgePos);
                float angle = HexUtility.anglebyEdgeAlignment[orientation];
                GameObject edgeObj = Instantiate(edgeVisionBorder, HexConverter.EdgeCoordToCartesianCoord(edgePos), Quaternion.Euler(0, angle, 0));
                visionMarkers.Add(edgeObj);
            }
        }

        private HashSet<Vector3Int> CalculateVisibleTiles(Vector3Int origin, int range)
        {
            List<Vector3Int> ringTiles = HexGrid.GetTiles.Ring(origin, range, 1); //we use hexGrid because we use the result as first step
            HashSet<Vector3Int> reachedTiles = new HashSet<Vector3Int>();

            foreach(var ringTile in ringTiles)
            {
                List<Tile<int>> lineA = hexMap.GetTiles.Line(origin, ringTile, true, +0.001f);
                List<Tile<int>> lineB = hexMap.GetTiles.Line(origin, ringTile, true, -0.001f);
                //we draw 2 lines, one slightly nudged to the left, one slightly nudged to the right because for some origin->target lines there are 2 valid/mirrored solutions
                //and by just using 1 line you would get inconsistent results. As long as one of the lines provides vision we consider the tile visible.
                List<List<Tile<int>>> lines = new List<List<Tile<int>>> { lineA, lineB};
                foreach(var line in lines)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        reachedTiles.Add(line[i].Position);
                        if (line[i].Data == 0) break; //0 = wall
                        
                    }
                }              
            }
            return reachedTiles;
        }

        private void SetupCamera()
        {
            Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
            Camera.main.orthographic = true; //for this example we use an orthographic camera.
            Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0); //rotates the camera to it looks at the XZ-plane
            Camera.main.orthographicSize = hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.
        }

    }
}
