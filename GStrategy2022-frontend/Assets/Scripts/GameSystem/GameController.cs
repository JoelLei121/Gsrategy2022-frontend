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
    public List<int[]> barriers;
    public List<int[]> resources;

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
    public int MinesLeft;
    public int Round;
    public string UpgradeType;
    public int[] VictimId;
    public int? WinnerId;
    public float Exp;
    public int BoundaryHurt = 10;

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
    public GameObject mainCamera, overallCamera;
    public GameObject ocean;
    public GameObject[] players = new GameObject[2];
    public int playerNum;

    //public Map map; // waiting for map data development
    public int mapSize = 20;
    // public GameObject playerPrefab;
    public HexGrid map;
    public Initialize initialize;
    public GameUI redUI,blueUI;
    public OverlayUI overlayUI;
    public AllUI allUI,UI;
    public RecordUI record;
    public float playSpeed = 1f;

    [DllImport("__Internal")]
    private static extern String ReadGameHistory();
    private String gameHistory;
    public string gameJson = "Assets\\Resources\\Play.json";


    void Start()
    {
        //gameHistory = ReadGameHistory();
        gameHistory = File.ReadAllText(gameJson);
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
        overallCamera.transform.position = camPos;
        camPos.y = (float)(2.5* (float)response.InitialState.map.size[0]) + 5f;
        camPos.x = 20f;
        mainCamera.transform.position = camPos;
       
        // After initialize done
        List<GameState> gameStates = response.list;

        if (gameStates != null)
            Debug.Log("Game process read successfully.");
        commandLoader.GetCommandFromDocument(gameStates);

        commandIsDone = false; 
        Debug.Log("Data is read, game start");
        GameStart();
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

    public void SpeedUp(bool flag)
    {
        playSpeed = flag ? 2f : 1f;
        return;
    }

}
