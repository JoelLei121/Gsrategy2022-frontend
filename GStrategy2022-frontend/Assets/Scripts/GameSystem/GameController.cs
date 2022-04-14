using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;


[Serializable]
public class Map
{
    public int[] size;
    public List<int[]> Barrier;
    public List<int[]> Resource;

}

[Serializable]
public class InitialState
{
    public Map map;
    public PlayerInitialize[] players;
}

[Serializable]
public class GameState
{
    public int ActivePlayerId;
    public int[] ActivePos;
    public String CurrentEvent;
    public int[] MapSize = { 10, 10, 10 };
    public int Round;
    public int[] VictimId;
    public int? WinnerId;
    public float Exp;

}

public class Response<T>
{
    public List<T> list;
    public InitialState InitialState;
}


public class GameController : MonoBehaviour
{
    
    public CommandLoader commandLoader;
    public bool commandIsDone;
    public GameObject cam;
    public GameObject ocean;
    public GameObject[] players = new GameObject[2];
    public int playerNum;

    //public Map map; // waiting for map data development
    public int mapSize = 20;
    // public GameObject playerPrefab;
    public HexGrid map;
    public Initialize initialize;
    public GameUI UI;
    public bool isInstantiated = false;

    [DllImport("__Internal")]
    private static extern String ReadGameHistory();
    private String gameHistory;


    void Start()
    {
        gameHistory = File.ReadAllText("Assets\\Resources\\Play.json");
        if (gameHistory == null)
        {
            Debug.Log("Empty String.");
            Application.Quit();
        }
        Response<GameState> response = JsonConvert.DeserializeObject<Response<GameState>>(gameHistory);
        Debug.Log(JsonConvert.SerializeObject(response));

        initialize.Run(response.InitialState);
        map.init(response.InitialState.map);
        Vector3 camPos = map.GetUnitPosition(response.InitialState.map.size[0] - 1, response.InitialState.map.size[0] - 1);
        camPos.y = -0.5f;
        ocean.transform.position = camPos;
        camPos.y = (float)(2.5* (float)response.InitialState.map.size[0]);
        cam.transform.position = camPos;
       
        // After initialize done
        List<GameState> gameStates = response.list;

        if (gameStates != null)
            Debug.Log("Game process read successfully.");
        commandLoader.GetCommandFromDocument(gameStates);

        commandIsDone = false; 
        Debug.Log("Data is read, game start");
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        //unnecessary
    }

    void GameStart()
    {
        foreach(GameObject p in players)
        {
            PlayerStatus pStatus = p.GetComponent<PlayerStatus>();
            int x = pStatus.pos[0];
            int z = pStatus.pos[2];
            p.transform.position = map.GetUnitPosition(x,z);
        }
        Debug.Log("Players are placed");
        StartCoroutine(commandLoader.LoadCommand());
        // if isDone, stop coroutines
    }

    public PlayerStatus GetPlayerStatus(int id)
    {
        return players[id].GetComponent<PlayerStatus>();
    }


}
