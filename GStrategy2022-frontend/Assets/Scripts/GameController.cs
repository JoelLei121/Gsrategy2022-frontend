using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public CommandLoader commandLoader;
    public bool commandIsDone;

    // public PlayerStatus[] players;
    public GameObject[] players;
    public int playerNum;

    //public Map map; // waiting for map data development
    public int mapSize = 20;
    public GameObject playerPrefab;
    public HexGrid map;
    public Initialize initialize;

    public bool isInstantiated = false;

    void Start()
    {
        initialize.Run();
        map.init();
        // After initialize done
        // commandLoader.GetCommandFromDocument();
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
        int i = 0;
        foreach(GameObject p in players)
        {
            p.transform.position = map.GetUnitTransform(i, i);
            i++;
        }
        Debug.Log("Players are placed");
        // while(!commandIsDone)
        //     commandLoader.LoadCommand();
    }

    public PlayerStatus GetPlayerStatus(int id)
    {
        return players[id].GetComponent<PlayerStatus>();
    }


}
