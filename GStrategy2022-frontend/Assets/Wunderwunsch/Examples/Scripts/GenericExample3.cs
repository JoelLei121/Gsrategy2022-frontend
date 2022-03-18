using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using System.Linq;


namespace Wunderwunsch.HexMapLibrary.Examples
{
    /// <summary>
    /// Example 3: Pathfinding with the following rules:
    /// Different move costs depending on the base terrain type of a tile -> we keep it to 1 for all except water which we give a prohibitive expensive cost of 99
    /// Moving on a tile with a forest increases the movement cost
    /// Crossing a river also increases the movement cost
    /// To move on a tile you need to have at least as many movementpoints remaining as the move costs.
    /// </summary>
    public class GenericExample3 : MonoBehaviour
    {
        private class MyTile //contains the data of a tile
        {
            public bool hasForest;
            public int terrainType;
        }

        private class MyEdge //contains the data of an edge
        {
            public bool hasRiver; //as that is just a single bool, no need to really have it a struct or class but for demonstration purposes we do it this way.
        }

        [SerializeField] private int movementPoints = 3;
        [SerializeField] private List<int> baseMovementCostByTerrainType = null;
        [SerializeField] private int extraCostForest = 1;
        [SerializeField] private int extraCostRiver = 1;
        [SerializeField] private Vector2Int mapSize = new Vector2Int(13, 13); 
        [SerializeField] private GameObject tilePrefab = null;
        [SerializeField] private GameObject forestPrefab = null;
        [SerializeField] private GameObject riverPrefab = null;
        [SerializeField] private GameObject reachableMarker = null;
        [SerializeField] private Material[] materials = null;
        [SerializeField] private GameObject edgeReachableBorder = null;
        private HexMap<MyTile,MyEdge> hexMap; 
        private HexMouse hexMouse;
        private List<GameObject> reachableTilesMarkers; // the gameObjects we use to visualise which tiles are within movementRange

        void Start()
        {
            hexMap = new HexMap<MyTile, MyEdge>(HexMapBuilder.CreateRectangularShapedMapOddRowsOneShorter(mapSize), null);
            hexMouse = gameObject.AddComponent<HexMouse>(); 
            hexMouse.Init(hexMap); 

            InitMap();
            reachableTilesMarkers = new List<GameObject>();
            SetupCamera();
        }


        void Update()
        {
            if (!hexMouse.CursorIsOnMap) return;

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                List<Tile<MyTile>> reachableTiles = CalculateReachableTiles(hexMouse.TileCoord);
                UpdateReachableTileVisuals(reachableTiles);
            }
        }


        void InitMap()
        {
            //Please note that we omit adding the GameObjects used for visualising the map to some array for later changes/updates in this example. 
            //We can do that as we do not change the map at all after creation, in most real use cases you will need to keep track of them.
            foreach (var tile in hexMap.Tiles)
            {
                tile.Data.terrainType = Random.Range(0,4);

                int randomNumber = Random.Range(0, 100);
                if (randomNumber > 70 && tile.Data.terrainType != 0) tile.Data.hasForest = true;

                GameObject tileInstance = GameObject.Instantiate(tilePrefab);
                tileInstance.GetComponent<Renderer>().material = materials[tile.Data.terrainType];
                tileInstance.name = "MapTile_" + tile.Position;
                tileInstance.transform.position = tile.CartesianPosition;

                if(tile.Data.hasForest == true)
                { 
                    GameObject forestInstance = GameObject.Instantiate(forestPrefab);
                    forestInstance.transform.SetParent(tileInstance.transform,false);
                    forestInstance.transform.position += new Vector3(0, 0.05f, 0);
                }
            }

            foreach(var edge in hexMap.Edges)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 80) continue;
                var adjTiles = hexMap.GetTiles.AdjacentToEdge(edge);
                if (adjTiles.Any(x => x.Data.terrainType == 0)) continue; //if the edge is adjacent to a water tile we don't create a river

                edge.Data.hasRiver = true;                
                GameObject edgeInstance = GameObject.Instantiate(riverPrefab);
                edgeInstance.transform.position = edge.CartesianPosition;
                edgeInstance.transform.rotation = Quaternion.Euler(0, edge.EdgeAlignmentAngle, 0);
            }
        }


        private List<Tile<MyTile>> CalculateReachableTiles(Vector3Int startTile)
        {
            //this is a very simple and inefficient solution to the problem, but our main focus is using the hex library.
            //in a real implementation you want to use a pathfinding algorithm like A*Star and better suited data structures like a priority queue.
            Dictionary<Tile<MyTile>, int> costToTile = new Dictionary<Tile<MyTile>, int>();
            Queue<Tile<MyTile>> openTiles = new Queue<Tile<MyTile>>();
            var currentTile = hexMap.TilesByPosition[startTile];
            if (currentTile.Data.terrainType == 0) return new List<Tile<MyTile>>(); //we return an empty list if the start tile is water.
            openTiles.Enqueue(currentTile);
            costToTile.Add(currentTile, 0);
            int safeGuard = 0;
            while(openTiles.Count > 0 &&  safeGuard < 1000)
            {
                safeGuard++;
                currentTile = openTiles.Dequeue();
                int costToCurTile = costToTile[currentTile];
                var neighbours = hexMap.GetTiles.AdjacentToTile(currentTile);
                foreach(var neighbour in neighbours)
                {
                    var edgeBetween = hexMap.GetEdge.BetweenNeighbouringTiles(currentTile.Position, neighbour.Position);

                    int newCost = costToCurTile;
                    newCost += baseMovementCostByTerrainType[neighbour.Data.terrainType];
                    if (neighbour.Data.hasForest) newCost += extraCostForest;
                    if (edgeBetween.Data.hasRiver) newCost += extraCostRiver;

                    if (newCost > movementPoints) continue;

                    int oldCost = costToTile.ContainsKey(neighbour) ? costToTile[neighbour] : int.MaxValue; 
                    if (newCost >= oldCost) continue; //we have been on this tile earlier with a lower cost;

                    openTiles.Enqueue(neighbour);
                    costToTile[neighbour] = newCost; //using the index adds the key/value pair if it is not in the dictionary and updates it if it already exists 
                }
            }

            List<Tile<MyTile>> tilesInRange = costToTile.Keys.ToList();
            return tilesInRange;
        }


        private void UpdateReachableTileVisuals(List<Tile<MyTile>> reachableTiles)
        {
            foreach(GameObject g in reachableTilesMarkers)
            {
                Destroy(g);            
            }
            reachableTilesMarkers.Clear();

            foreach(Tile<MyTile> tile in reachableTiles)
            {
                GameObject tileObj = Instantiate(reachableMarker, tile.CartesianPosition, Quaternion.identity);
                tileObj.transform.position += new Vector3(0, 0.1f, 0); //0.1f = explicitly set y-Coord of the tile so it is slightly above the tiles of the map
                reachableTilesMarkers.Add(tileObj);
            }

            List<Vector3Int> borderEdges = hexMap.GetEdgePositions.TileBorders(reachableTiles);

            foreach (var edgePos in borderEdges)
            {
                EdgeAlignment orientation = HexUtility.GetEdgeAlignment(edgePos);
                float angle = HexUtility.anglebyEdgeAlignment[orientation];
                GameObject edgeObj = Instantiate(edgeReachableBorder, HexConverter.EdgeCoordToCartesianCoord(edgePos), Quaternion.Euler(0, angle, 0));
                reachableTilesMarkers.Add(edgeObj);
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
}
