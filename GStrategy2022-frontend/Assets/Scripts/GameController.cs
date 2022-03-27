using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public CommandLoader commandLoader;
    public bool commandIsDone;

    public PlayerStatus[] players;
    public int playerNum = 4;

    //public Map map; // waiting for map data development
    public int mapSize = 20;

    void Start()
    {
        commandLoader.GetCommandFromDocument();
        commandIsDone = false;
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        //unnecessary
    }

    void GameStart()
    {
        while(!commandIsDone)
            commandLoader.LoadCommand();
    }


}
