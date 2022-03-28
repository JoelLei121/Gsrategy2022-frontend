using UnityEngine;
using System.IO;
using System;

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
    public Player[] players;
}

public class Initialize : MonoBehaviour
{
    public string path = "Assets/Resources/Init.json";
    public GameController gameController;

    void Start()
    {
        String initialJson = File.ReadAllText(path);
        InitialState state = JsonUtility.FromJson<InitialState>(initialJson);
        //Initialize Map

        //Initialize Players
        gameController.playerNum = state.players.Length;
        gameController.players = new PlayerStatus[gameController.players.Length];
        foreach (Player p in state.players)
        {
            gameController.players[p.id] = new PlayerStatus(p);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
