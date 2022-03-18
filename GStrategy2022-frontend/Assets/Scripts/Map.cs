using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;

public class Map : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize = new Vector2Int(11, 11); // the mapSize, can be set in inspector 
    [SerializeField] private GameObject tilePrefab = null; // the prefab we use for each Tile -> use TilePrefab.prefab [TODO check if that is the correct name]
    private HexMap<int> hexMap; // our map. For this example we create a map where an integer represents the data of each tile
    // Start is called before the first frame update
    void Start()
    {
        hexMap = new HexMap<int>(HexMapBuilder.CreateRectangularShapedMap(mapSize), null); //creates a HexMap using one of the pre-defined shapes in the static MapBuilder Class                        

        foreach (var tile in hexMap.Tiles) //loops through all the tiles, assigns them a random value and instantiates and positions a gameObject for each of them.
        {
            GameObject instance = GameObject.Instantiate(tilePrefab);
            instance.transform.position = tile.CartesianPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
