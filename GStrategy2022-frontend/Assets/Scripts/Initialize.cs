using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

[Serializable]
class Map
{
    public int[] size;
    public int[,] barriers;
    public int[,] resources;

}


[Serializable]
class InitialState
{
    public Map map;
    public PlayerInitialize[] players;
}

public class Initialize : MonoBehaviour
{
    public string path = "Assets/Resources/Init.json";
    public GameController gameController;

    void Awake()
    {

    }

    public void Run()
    {
        Debug.Log("waiting for init");
        String initialJson = File.ReadAllText(path);
        InitialState state = JsonUtility.FromJson<InitialState>(initialJson);
        //Initialize Map

        //Initialize Players
        gameController.playerNum = state.players.Length;
        gameController.players = new GameObject[gameController.playerNum];

        for(int i = 0; i < gameController.playerNum; i++)
        {
            GameObject player = Instantiate<GameObject>(gameController.playerPrefab);
            gameController.players[i] = player;
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            playerStatus.init(state.players[i]);
        }
        Debug.Log("Players are ready.");
        gameController.isInstantiated = true;
        Response<GameState> tmp = new Response<GameState>();
        tmp.list = new List<GameState>();
        tmp.list.Add(new GameState());
        Debug.Log(JsonUtility.ToJson(tmp));
    }
}
