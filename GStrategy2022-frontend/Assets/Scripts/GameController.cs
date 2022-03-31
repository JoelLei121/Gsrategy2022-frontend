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

    public bool isInstantiated = false;

    void Start()
    {
        // After initialize done
        // commandLoader.GetCommandFromDocument();
        commandIsDone = false;
        int cnt = 0;
        while(!isInstantiated && cnt < 1000)
        {
            Debug.Log("waiting for init");
            cnt++;
        }
        if(cnt < 1000)
        {
            Debug.Log("Data is read, game start");
            GameStart();
        }
        else
        {
            Debug.Log("ERROR");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //unnecessary
    }

    async void GameStart()
    {
        int i = 0;
        foreach(GameObject p in players)
        {
            // p.transform.position = map.GetMapUnit(i,0).GetGloPos();
            p.transform.Translate(0, 0, i);
            i+=10;
        }
        Debug.Log("Players are placed");
        while(!commandIsDone)
            commandLoader.LoadCommand();
    }

    public PlayerStatus GetPlayerStatus(int id)
    {
        return players[id].GetComponent<PlayerStatus>();
    }


}
