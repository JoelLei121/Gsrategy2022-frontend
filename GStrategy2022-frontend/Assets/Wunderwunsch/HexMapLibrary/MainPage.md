\mainpage HexMap Library Documentation

![](BannerCombined.png)
###Introduction

This library makes working with hexagonal grids and maps an intuitive, easy and maybe even fun process.  
All the details of hexagonal-grid-coordinate systems and the mathematics behind it are abstracted away from you so you can focus on building your game logic using declarative statements.  
We <b>include multiple example scenes</b> displaying some approaches how the library can be used and showing how easily common features like line-of-sight or turn-based movement with movement costs based on map features can be implemented.  
<b>At the bottom of this page you will find step-by-step walkthroughs for every example.</b>

The visual representation of the hexagonal map is not part of the library as that is highly implementation specific. The sample scenes however come with some very basic visualisations.

###Key Features
- support for Tiles, Edges and Corners
- supports workflows for finite maps as well as an infinite grid
- maps can optionally be periodic (wrap around). Currently only "cylindrical" (repeating on x-axis) wrapping logic is included directly with the library but you can easily write your own wrap around logic
- support for generics: a tile,edge or corner can have any class/struct as data type
- alternatively derive from the non generic base class or keep the data of the game logic completely separate in arrays aligned with the tile/edge/corner indices of the HexMap
- extensive amount of easy to use methods which can be used as building blocks to solve complex problems
- build with ease of use as primary design
- uses primarily "cube" coordinates. An explanation about the different coordinate systems used in this library can be found [HERE LINK]
- fully documented API
- source code available
- MIT license 

### Downloads

- from itch.io : https://aurelwu.itch.io/hexmaplibrary
- from github (library code only without examples) : https://github.com/AurelWu/HexMapLibrary

### General Usage Principle

The Library has a consistent method naming scheme based on their return value allowing for intuitive usage:

\code{.cs}
result1 = map.GetTiles.Line(origin,target,true); //returns all tiles forming a line from origin to target (3rd param set to true specifies that the origin is included)
result2 = map.GetEdges.AdjacentToCorner(corner); //return all edges which are neighbouring the corner (usually 3 but at map border it can be just 2)
result3 = map.GetCorners.WithinDistance(centerCorner,distance,false); // returns all corners which are no further than distance away from the center (3rd param set to false specifies that origin is not included)
result4 = map.GetTileDistance.Grid(TileA,TileB); // returns the grid distance between those 2 tiles (there is also GetTileDistance.Euclidean if you want the euclidean distance)
result5 = map.GetEdge.BetweenTiles(TileA,TileB); // returns the edge which is between the 2 input tiles (throws an argument exception if the input tiles are not neighbouring)

\endcode 

###Overview over the classes you will use mostly:

- HexGrid
 + This is a static class representing an infinite grid on the xz-plane.
 + The methods of this class form the foundation of HexMap (which then adds bound checks and wrapping logic on top).
 + often useful for intermediate steps even when the final results are on a finite-sized HexMap

- HexMap
 + an instance of HexMap represents a specific map instance with a defined set of Tiles ,Edges and Corners.
 + optionally it can be periodic (i.e. wraps around at some or all the map borders)
 + takes care of all bound checks and wrapping logic

- HexMap<T>, HexMap<T,E>, HexMap<T,E,C>
 + please note that they are called HexMapT<T> , HexMapTE<T,E> , HexMapTEC<T,E,C> in this documentation, this is just because the documentation generation software we use does not properly work with generics classes with different overloads and same class name. In the actual library they are all called HexMap<...> . 
 + generic variants of HexMap in which every Tile (and Edge / Corner) is an object of arbitrary type.

- HexConverter
 + static class to convert coordinates between different coordinate systems (cartesian, cube, offset)

- HexMapBuilder
 + static class to easily generate rectangular ,hexagonal or triangular shaped maps.

- HexMouse
 + Monobehaviour which updates information about the mouse position every frame (current tile, nearest edge, nearest corner, is cursor on map)

###About "Pointy-Top" and "Flat-Top"-Hexagons

Everything in this library is build around pointy-top hexagons, however flat-top hexagons are just rotated pointy-top hexagons so you can just rotate the input and output coordinates, or even just your camera. Generally we recommend using pointy-top hexagons because you can fit more hexagons vertically this way which partly balances out the fact that computer screens are wider than tall. If you are not convinced by that, take a look at honeybees and how they build their combs.
![left: pointy-top hexagon, right: flat-top hexagon](PointyAndFlatTopHexagon.png)  

###Performance:

While we made sure that the library does have reasonable performance we always prioritised useability and flexibility over raw performance.
We made that decision because in most use cases it won't matter at all, and in those where it does, the solution will most likely be to just cache results.
For example instead of always recalculating the result of HexMap.GetTiles.OfRing() which might become costly if the radius is big you can just calculate the relative position of the resulting tiles to the center once for each radius you need and store them.
If you ever encounter a performance issue which is not solvable this way and you also don't find another solution, please message us and we will try to find a solution (no promises that we can solve your particular problem but we will try our best as long as it is within reasonable scope)

###Examples

These examples are kept as simple and as possible so you can focus on the usage of the library, this code is by no means intended to be used in production code and is not suitable structured for that but for ease of understanding.
You will also notice some duplicate code between them, that is consciously done to keep each example self-contained. 

All these examples use a very simple way to visualise the map which is far from perfect in many ways, especially when it comes to performance as we use one GameObject per tile. We use custom shader which draws the hex grid lines on top of the mesh based on the world position. As this is not fundamental for the library itself we omit an explanation what this shader does in detail.

Please keep in mind that this library is not tied to a certain visualisation and you can easily use procedurally generated meshes, Environments created in 3D modelling software or even handpainted backgrounds instead.


You can either follow the tutorials step by step or take a look at the finished scene & script for each example.

-\subpage Ex0

-\subpage Ex1

-\subpage Ex2

There are two more scenes in the example folder, one covering path finding while the other is about building and road placement. The script files are documented so by now they are hopefully easily understood.


###Reading Recommendation
If you want to understand hexagonal grids in depth we recommend reading Amit Patel's excellent page about hexagons: https://www.redblobgames.com/grids/hexagons/ 

###License
The MIT License (MIT)

Copyright (c) 2018 Aurel "AurelWu" Wünsch

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

###Impressum
Electronic Sport Events UG (haftungsbeschränkt)    
Geschäftsführer: Aurel Wünsch    
Biedermannstraße 42    
04277 Leipzig    
Telefon: +49 (0)175 / 780 43 40    
E-Mail: aurelwuensch@gmail.com    
Registergericht: Amtsgericht Leipzig     
Registernummer: HRB 27087    
Umsatzsteuer-Identifikationsnummer gem. § 27 a Umsatzsteuergesetz: DE294361901  

\page Ex0 Example 0: Hello hex world
The first step of almost every project will be to create the map or board of the game. To do that we first need to import our library into unity:
[TODO IMAGE]
For now we recommend importing the example folder as they include a hexagon mesh and some other sample prefabs which we use in the following tutorials.

The next step is creating a new script file and reference the needed namespaces of our library:
\code{.cs}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wunderwunsch.HexMapLibrary;
using Wunderwunsch.HexMapLibrary.Generic;
\endcode

Now we create some variables which we can set in the inspector: the map size and the gameObject we use to represent each tile. [TODO :Explanation of the gameObject and it's material/shader]
\code{.cs}
[SerializeField] private Vector2Int mapSize = new Vector2Int(11, 11); // the mapSize, can be set in inspector 
[SerializeField] private GameObject tilePrefab = null; // the prefab we use for each Tile -> use TilePrefab.prefab [TODO check if that is the correct name]
private HexMap<int> hexMap; // our map. For this example we create a map where an integer represents the data of each tile
\endcode

Then we can create our map. For this simple example we want each tile to represent an integer and we want the map to be rectangular and do not want it be wrapping around.  

\code{.cs}
void Start ()
{
	HexMap = new HexMap<int>(HexMapBuilder.CreateRectangularShapedMap(mapSize), null); //creates a HexMap using one of the pre-defined shapes in the static MapBuilder Class                        
	
	foreach (var tile in hexMap.Tiles) //loops through all the tiles, assigns them a random value and instantiates and positions a gameObject for each of them.
	{
		GameObject instance = GameObject.Instantiate(tilePrefab);
		instance.transform.position = tile.CartesianPosition;
	}
}
\endcode

Now let's set change the settings of our camera so we can see the whole map and center it:
\code{.cs}
//put the following at the end of the start method (or in its own method called after map creation)
Camera.main.transform.position = new Vector3(hexMap.MapSizeData.center.x, 4, hexMap.MapSizeData.center.z); // centers the camera and moves it 5 units above the XZ-plane
Camera.main.orthographic = true; //for this example we use an orthographic camera.
Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0); //rotates the camera to it looks at the XZ-plane
Camera.main.orthographicSize = hexMap.MapSizeData.extents.z * 2 * 0.8f; // sets orthographic size of the camera.]
//this does not account for aspect ratio but for our purposes it works good enough.
\endcode
You should end up with something looking like this:
![](Ex0.png) 
This concludes this example, now let's continue with the next [ADD LINK]


\page Ex1 Example 1: Mouse markers and simple map manipulation

In this example we are going to create a HexMap with a rectangular shape and will assign a random value to each of those tiles which we will visualise by using different colors.
We then will add the HexMouse component to get the position of the mouse and display the tile the cursor is on as well as the closest tile and edge.
When clicking a tile it will change its value.

As in the previous example we will add the using statements for our library and position the camera correctly so these things are not covered again here.

This time we will need a few more prefabs & variables :

\code{.cs}
[SerializeField] private Vector2Int mapSize = new Vector2Int(13, 13);
[SerializeField] private GameObject tilePrefab = null; // the prefab we use for each Tile -> use TilePrefab.prefab 
[SerializeField] private GameObject edgePrefab = null; // the prefab we use for each Edge -> use EdgePrefab.prefab 
[SerializeField] private GameObject tileMarker = null; // a GameObject we use to show the current mouse position -> put an instance of TileMarker.prefab in the scene and reference it in the inspector
[SerializeField] private GameObject edgeMarker = null; // a GameObject we use to we show the closest edge to the current mouse position  -> put an instance of EdgeMarker.prefab in the scene and reference it in the inspector
[SerializeField] private GameObject cornerMarker = null; // a GameObject we use to show the closest corner to the current mouse position  -> put an instance of CornerMarker.prefab in the scene and reference it in the inspector
[SerializeField] private List<Material> materials = null; // the materials we want to assign to the tiles for visualisation purposes -> set size to 4 in inspector and add TileMat1 to TileMat4
private HexMap<int,bool> hexMap; // our map. For this example we create a map where an integer represents the data of each tile and a bool the data of each edge
private HexMouse hexMouse = null; // the HexMouse component we add to keep track of the mouse position
private GameObject[] tileObjects; // this will contain all the GameObjects for visualisation purposes, their array index corresponds with the index of our Tiles
\endcode

Now it is time to create our map:

\code{.cs}
void Start ()
{
	hexMap = new HexMap<int, bool>(HexMapBuilder.CreateRectangularShapedMap(mapSize), null); //creates a HexMap using one of the pre-defined shapes in the static MapBuilder Class            
	hexMouse = gameObject.AddComponent<HexMouse>(); //we attach the HexMouse script to the same gameObject this script is attached to, could also attach it anywhere else
	hexMouse.Init(hexMap); //initializes the HexMouse 
	tileObjects = new GameObject[hexMap.TilesByPosition.Count]; //creates an array with the size equal to the number on tiles of the map

	foreach (var tile in hexMap.Tiles) //loops through all the tiles, assigns them a random value and instantiates and positions a gameObject for each of them.
	{
		tile.Data = (Random.Range(0, 4));
		GameObject instance = GameObject.Instantiate(tilePrefab);
		instance.GetComponent<Renderer>().material = materials[tile.Data];
		instance.name = "MapTile_" + tile.Position;
		instance.transform.position = tile.CartesianPosition;
		tileObjects[tile.Index] = instance;
	}

	foreach (var edge in hexMap.Edges) //we randomly spawn a GameObject on some corners (could be representing walls or rivers or anything else)
	{
		int randomNumber = Random.Range(0, 100);
		if (randomNumber > 89)
		{
			edge.Data = true;
			GameObject instance = GameObject.Instantiate(edgePrefab);
			instance.name = "MapEdge_" + edge.Position;
			instance.transform.position = edge.CartesianPosition;
			instance.transform.rotation = Quaternion.Euler(0, edge.EdgeAlignmentAngle, 0);
			//as we don't change the edges during runtime we don't need to add the gameObjects of the edges to an array as we don't need to manipulate them later on
		}
	}

	SetupCamera(); //put the camera related lines from previous script in that method
}
\endcode

If you run the scene now, it should look similar to this:
![](Example1_1.png)

Finally we add our update method which will highlight the current tile and closest edge of our mouse position and when we leftclick we will change the value of the current tile
\code{.cs}
void Update ()
{ 
	if (!hexMouse.CursorIsOnMap) return; // if we are not on the map we won't do anything so we can return

	Vector3Int mouseTilePosition = hexMouse.TileCoord;
	Vector3Int mouseEdgePosition = hexMouse.ClosestEdgeCoord;
	Vector3Int mouseCornerPosition = hexMouse.ClosestCornerCoord;

	//update the marker positions
	tileMarker.transform.position = HexConverter.TileCoordToCartesianCoord(mouseTilePosition, 0.1f); //we put our tile marker on the tile our mouse is on
	edgeMarker.transform.position = HexConverter.EdgeCoordToCartesianCoord(mouseEdgePosition); // we put our edge marker on the closest edge of our mouse position            
	edgeMarker.transform.rotation = Quaternion.Euler(0, hexMap.EdgesByPosition[mouseEdgePosition].EdgeAlignmentAngle, 0); //we set the rotation of the edge marker
	cornerMarker.transform.position = HexConverter.CornerCoordToCartesianCoord(mouseCornerPosition);

	if (Input.GetMouseButtonDown(0)) // change a tile when clicked on it
	{
		Tile<int> t = hexMap.TilesByPosition[mouseTilePosition]; //we select the tile our mouse is on
		int curValue = t.Data; //we grab the current value of the tile
		t.Data = ((curValue + 1) % 4); //we increment it and use modulo to keep it between 0 and 3
		tileObjects[t.Index].GetComponent<Renderer>().material = materials[t.Data]; // we update the material of the GameObject representing the tile based on the new value
	}
}
\endcode

This concludes this example, now let's continue with the next [ADD LINK]

\page Ex2 Example 2: Line of sight

In this Example we want to create a simple vision system. If we click on a tile we want to highlight all those tiles which are visible. There will be 2 rules:   
1) there is a limited vision range  
2) certain tiles block vision to anything behind them

To decide which tiles are visible we are going to use the following approach:
We draw a ring around the center tile and then draw lines to each tile of the ring, stopping each line after it hit a vision blocker.
Every tile which at least one line reaches is considered visible.
As we use the ring just as a helper to create the lines we want to also get those ring tiles which are out of the map bounds, so we will use HexGrid.GetTiles.Ring which does operate on the infinite plane.

Before we continue we need to talk a bit about symmetry. If you take a look at that picture:

![](LineLeftRight.png)
you can see that sometimes there are 2 equally valid solutions to draw a line from A to B. Therefore we get more consistent results if we use both possible lines to determine our vision.
The GetLines method has a parameter which allows us to nudge the origin position a tiny bit to the left or right which results in getting either the more "left" or the more "right" line. This also works for all diagonal lines.

We once again create the variables we need:

\code{.cs}
[SerializeField] private int mapRadius = 11; // the mapSize, can be set in inspector 
[SerializeField] private GameObject tilePrefab = null; // the prefab we use for each Tile -> use TilePrefab.prefab
[SerializeField] private GameObject tileVisionMarker = null; // the prefab we use for each Tile -> use TilePrefab.prefab
[SerializeField] private GameObject edgeVisionBorder = null;
[SerializeField] private List<Material> materials = null; // the materials we want to assign to the tiles for visualisation purposes -> set size to 4 in inspector and add TileMat1 to TileMat4
private HexMap<int> hexMap; // our map. For this example we create a map where an integer represents the data of each tile 
private HexMouse hexMouse = null; // the HexMouse component we add to keep track of the mouse position
private GameObject[] tileObjects; // this will contain all the GameObjects for visualisation purposes, their array index corresponds with the index of our Tiles
private List<GameObject> visionMarkers; //we will use this to display the border of the vision range
\endcode

Next step is one again creating the map and initializing mouse and camera. We now put map initialisation in its own method.

\code{.cs}
void Start ()
{
	hexMap = new HexMap<int>(HexMapBuilder.CreateHexagonalShapedMap(mapRadius), null); //creates a HexMap using one of the pre-defined shapes in the static MapBuilder Class            
	hexMouse = gameObject.AddComponent<HexMouse>(); //we attach the HexMouse script to the same gameObject this script is attached to, could also attach it anywhere else
	hexMouse.Init(hexMap); //initializes the HexMouse 
	
	InitMap();
	SetupCamera(); //set camera settings so that the map is captured by it
	
}

void InitMap()
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
\endcode

Now we add our method which calculates the visible tiles:

\code{.cs}
private HashSet<Vector3Int> CalculateVisibleTiles(Vector3Int origin, int range)
{
	List<Vector3Int> ringTiles = HexGrid.GetTiles.Ring(origin, range, 1); //we use hexGrid because we want to get the whole ring as intermediate resulst even if some tiles are out of bound
	HashSet<Vector3Int> reachedTiles = new HashSet<Vector3Int>();

	foreach(var ringTile in ringTiles)
	{
		//we use 2 lines, one slightly nudged to the left, one slightly nudged to the right because for some origin->target lines there are 2 valid/mirrored solutions
		List<Tile<int>> lineA = hexMap.GetTiles.Line(origin, ringTile, true, +0.001f);
		List<Tile<int>> lineB = hexMap.GetTiles.Line(origin, ringTile, true, -0.001f);

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
\endcode

And another method with which we visualise the result:

\code{.cs}
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
\endcode

Finally we create the update method:

\code{.cs}
void Update ()
{ 
	if (!hexMouse.CursorIsOnMap) return; // if we are not on the map we won't do anything so we can return

	Vector3Int mouseTilePosition = hexMouse.TileCoord;

	if (Input.GetMouseButtonDown(0)) // change a tile when clicked on it
	{
		var visibleTiles = CalculateVisibleTiles(mouseTilePosition, 5);
		UpdateVisionMarkers(visibleTiles);
	}
}
\endcode