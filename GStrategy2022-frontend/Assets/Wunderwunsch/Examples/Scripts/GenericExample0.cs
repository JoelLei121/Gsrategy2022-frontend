using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

namespace Wunderwunsch.HexMapLibrary.Examples
{ 
    /// <summary>
    /// Example 0: "Hello Hex World" 
    /// this is a minimal example setting up a HexMap and visualising it.
    /// </summary>
    public class GenericExample0 : MonoBehaviour
    {
        //you will find the prefabs and materials used here in the "NonScriptAssets folder or of the package.
        [SerializeField] private Vector2Int mapSize = new Vector2Int(11, 11); // the mapSize, can be set in inspector 
        [SerializeField] private GameObject tilePrefab = null; // the prefab we use for each Tile -> use TilePrefab.prefab         
        private HexMap<int> hexMap; // our map. For this example we create a map where an integer represents the data of each tile (which we don't actually use yet in this example)


        void Start ()
        {
            hexMap = new HexMap<int>(HexMapBuilder.CreateRectangularShapedMap(mapSize), null); //creates a HexMap using one of the pre-defined shapes in the static MapBuilder Class                        

            foreach (var tile in hexMap.Tiles) //loops through all the tiles, assigns them a random value and instantiates and positions a gameObject for each of them.
            {
                GameObject instance = GameObject.Instantiate(tilePrefab);
                instance.transform.position = tile.CartesianPosition;
            }

            Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
            Camera.main.orthographic = true; //for this example we use an orthographic camera.
            Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0); //rotates the camera to it looks at the XZ-plane
            Camera.main.orthographicSize = hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.
        }    	
    }
}
