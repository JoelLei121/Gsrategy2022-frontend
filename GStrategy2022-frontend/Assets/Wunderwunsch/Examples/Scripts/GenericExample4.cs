using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Examples
{
    /// <summary>
    ///Example 4 : Building roads on edges and cities on corners with the following rules:
    ///One City is placed randomly at the beginning on a corner which is not entirely surrounded by water tiles
    ///Roads can be build next to cities and adjacent to other roads
    ///Roads can not be build on edges between 2 water tiles
    ///Cities can only be build next to a road
    ///Cities must be placed at a distance of at least 3 (= you need to move at least 3 edges from city to city)
    /// </summary>
    public class GenericExample4 : MonoBehaviour
    {
        [SerializeField] private Vector2Int mapSize = new Vector2Int(13, 13);
        [SerializeField] private GameObject tilePrefab = null;
        [SerializeField] private GameObject cityPrefab = null;
        [SerializeField] private GameObject roadPrefab = null;
        [SerializeField] private Material[] materials = null;
        private HexMap<int, bool, bool> hexMap; //tile = terrain type, edge = road yes/no, corner = city yes/no
        private HexMouse hexMouse;

        void Start()
        {
            hexMap = new HexMap<int, bool, bool>(HexMapBuilder.CreateRectangularShapedMapOddRowsOneShorter(mapSize), null);
            hexMouse = gameObject.AddComponent<HexMouse>();
            hexMouse.Init(hexMap);

            InitMap();            
            SetupCamera();
        }


        private void InitMap()
        {
            foreach (var tile in hexMap.Tiles) //assigning random terrain value to eachtile (we treat 0 as water)
            {
                tile.Data = Random.Range(0, 4); 

                GameObject tileInstance = GameObject.Instantiate(tilePrefab);
                tileInstance.GetComponent<Renderer>().material = materials[tile.Data];
                tileInstance.name = "MapTile_" + tile.Position;
                tileInstance.transform.position = tile.CartesianPosition;
            }

            //placing initial city on a random corner. Valid is any corner which is not completely surrounded by water.
            bool foundValidCorner = false; 
            int randomCornerIdx = 0;
            while (foundValidCorner == false)
            {
                randomCornerIdx = Random.Range(0, hexMap.Corners.Length);
                var corner = hexMap.Corners[randomCornerIdx];
                var adjacentTiles = hexMap.GetTiles.AdjacentToCorner(corner);
                if (adjacentTiles.Any(x => x.Data != 0)) foundValidCorner = true;
            }
            
            hexMap.Corners[randomCornerIdx].Data = true;
            GameObject.Instantiate(cityPrefab, hexMap.Corners[randomCornerIdx].CartesianPosition, Quaternion.identity);
        }


        private void Update()
        {
            if (!hexMouse.CursorIsOnMap) return;

            //leftclick to place road
            //we are allowed to place an edge if there is 1) a city adjacent or 2) a road adjacent 3) the edge is not surrounded by water tiles
            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int edgePos = hexMouse.ClosestEdgeCoord;
                Edge<bool> edge = hexMap.EdgesByPosition[edgePos];
                if (edge.Data == true) return; //there already is a road on that edge
               
                bool validPlacement = false;

                List<Corner<bool>> adjacentCorners = hexMap.GetCorners.OfEdge(edge);
                if (adjacentCorners.Any(x => x.Data == true)) validPlacement = true; //if there is a city next to the edge it is potentially a valid road target

                List<Edge<bool>> adjacentEdges = hexMap.GetEdges.AdjacentToEdge(edge);
                if (adjacentEdges.Any(x => x.Data == true)) validPlacement = true; //if there is a road on any edge neighbouring the input edge it is potentially a valid road target

                List<Tile<int>> adjacentTiles = hexMap.GetTiles.AdjacentToEdge(edge);
                if (adjacentTiles.All(x => x.Data == 0)) validPlacement = false; //if edge is completely surrounded by water we don't allow to build a road there

                if(validPlacement)
                {
                    edge.Data = true;
                    GameObject.Instantiate(roadPrefab, edge.CartesianPosition, Quaternion.Euler(0, edge.EdgeAlignmentAngle, 0));                    
                }
            }

            //rightclick to place city
            //we are allowed to place a city if 1) there is a road adjacent 2) there is no other city within a distance of 2
            //we can skip checking if the corner is completely surrounded by water because we already ensure that there will never be a road leading there.
            else if(Input.GetMouseButtonDown(1))
            {
                Vector3Int cornerPos = hexMouse.ClosestCornerCoord;
                Corner<bool> corner = hexMap.CornersByPosition[cornerPos];
                if (corner.Data == true) return; //there already is a city on that corner.
                bool nextToRoad = false;
                bool farAwayFromOtherCity = false;

                var adjacentEdges = hexMap.GetEdges.AdjacentToCorner(corner);
                if (adjacentEdges.Any(x => x.Data == true)) nextToRoad = true;

                var surroundingCorners = hexMap.GetCorners.WithinDistance(corner, 2, false);
                if (surroundingCorners.All(x => x.Data == false)) farAwayFromOtherCity = true;
                
                if(nextToRoad && farAwayFromOtherCity)
                {
                    corner.Data = true;
                    GameObject.Instantiate(cityPrefab, corner.CartesianPosition, Quaternion.identity);
                }
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
