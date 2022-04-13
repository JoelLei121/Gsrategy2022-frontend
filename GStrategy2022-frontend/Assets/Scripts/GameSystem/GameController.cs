using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    
    public CommandLoader commandLoader;
    public bool commandIsDone;

    // public PlayerStatus[] players;
    public GameObject[] players = new GameObject[2];
    public int playerNum;

    //public Map map; // waiting for map data development
    public int mapSize = 20;
    // public GameObject playerPrefab;
    public HexGrid map;
    public Initialize initialize;
    public GameUI UI;

    public bool isInstantiated = false;

    void Start()
    {
        initialize.Run();
        map.init();
        // After initialize done
        commandLoader.GetCommandFromDocument();
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
            p.transform.position = map.GetUnitPosition(x, z);
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
